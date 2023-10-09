using demoAPI.Interfaces;

namespace demoAPI.repository
{
    public class HackerNewsRepository : IHackerNewsRepository
    {
        private static HttpClient client = new HttpClient();

        public HackerNewsRepository()
        {

        }

        public async Task<HttpResponseMessage> TopStoriesAsync()
        {
            return await client.GetAsync("https://hacker-news.firebaseio.com/v0/beststories.json");
        }

        public async Task<HttpResponseMessage> GetStoryByIdAsync(int id)
        {
            return await client.GetAsync(string.Format("https://hacker-news.firebaseio.com/v0/item/{0}.json", id));
        }

    }
}
