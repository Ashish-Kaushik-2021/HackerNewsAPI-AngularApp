using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using demoAPI.Interfaces;
using demoAPI.Model;
using System.Net;

namespace demoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HackerNewsController : ControllerBase
    {
        private readonly IHackerNewsRepository _repoHna;
        private IMemoryCache _cache;

        public HackerNewsController(IMemoryCache cache, IHackerNewsRepository repoHna)
        {
            this._cache = cache;
            this._repoHna = repoHna;
        }

        /// <summary>
        /// Retrieve hacker news top stories.
        /// </summary>
        /// <param name="searchTerm">The optional search term.</param>
        /// <returns>An list of hacker news story data object.</returns>
        [HttpGet]
        public async Task<IActionResult> getHackerNewsStory(string? searchTerm)
        {
            List<Story> stories = new List<Story>();
            List<Story> filterStories = new List<Story>();
            try
            {
                var response = await _repoHna.TopStoriesAsync();
                if (response.IsSuccessStatusCode)
                {
                    var storiesResponse = response.Content.ReadAsStringAsync().Result;
                    var bestIds = JsonConvert.DeserializeObject<List<int>>(storiesResponse);

                    if (bestIds?.Count > 0)
                    {
                        var tasks = bestIds.Select(GetStory);
                        stories = (List<Story>)(await Task.WhenAll(tasks)).ToList();
                        filterStories = stories.Where(x => x != null).ToList();
                        if (!String.IsNullOrEmpty(searchTerm))
                        {
                            var search = searchTerm.ToLower();
                            filterStories = filterStories.Where(s => s.title.ToLower().IndexOf(search) > -1 || s.by.ToLower().IndexOf(search) > -1).ToList();
                        }
                    }

                }
                return Ok(filterStories);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "exception occured in API"); ;
            }
        }

        private async Task<Story> GetStory(int storyId)
        {
            Story? story = new Story();
            var response = await _repoHna.GetStoryByIdAsync(storyId);
            if (response.IsSuccessStatusCode)
            {
                var storyResponse = response.Content.ReadAsStringAsync().Result;
                story = JsonConvert.DeserializeObject<Story>(storyResponse);
            }

            if (story != null && !string.IsNullOrWhiteSpace(story.url))
            {
                return _cache.GetOrCreate(storyId, entry => story);
            }
            else { return null; }
        }
    }
}