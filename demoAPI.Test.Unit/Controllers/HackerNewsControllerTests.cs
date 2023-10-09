using Newtonsoft.Json;
using Moq;
using Moq.AutoMock;
using demoAPI.Controllers;
using demoAPI.Interfaces;
using demoAPI.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace demoAPI.Test.Unit.Controllers
{
    [TestFixture]
    public class HackerNewsControllerTests
    {
        private AutoMocker _mock;
        private Mock<IHackerNewsRepository> _mockHnaRepo;
        private HackerNewsController _hnaController;
        private Story _fakeStory;
        private Mock<IMemoryCache> _mockCache;
        private string cacheEntryKey = "cacheKey";
        private Story expectedCacheResult;

        [SetUp]
        public void Setup()
        {
            _mock = new AutoMocker();
            _hnaController = _mock.CreateInstance<HackerNewsController>();
            _mockHnaRepo = _mock.GetMock<IHackerNewsRepository>();
            _fakeStory = new Story
            {
                by = "samays",
                title = "test title book",
                url = "https://link.springer.com/article/10.1057/jt.2009.16"
            };

            expectedCacheResult = _fakeStory;
            _mockCache = _mock.GetMock<IMemoryCache>();

        }

        [Test]
        public async Task GetAllStories_ShouldReturnStories()
        {
            HttpContent topStoryContent = new StringContent("[37791002]");
            HttpContent storyByIdContent = new StringContent(JsonConvert.SerializeObject(_fakeStory), Encoding.UTF8, "application/json");
            _mockHnaRepo.Setup(x => x.TopStoriesAsync()).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = topStoryContent });
            _mockHnaRepo.Setup(x => x.GetStoryByIdAsync(It.IsAny<int>())).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = storyByIdContent });
            _mockCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);

            var result = await _hnaController.getHackerNewsStory("");
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult?.Value);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task GetAllStories_ShouldReturnStories_with_searchterm()
        {
            HttpContent topStoryContent = new StringContent("[37791002]");
            HttpContent storyByIdContent = new StringContent(JsonConvert.SerializeObject(_fakeStory), Encoding.UTF8, "application/json");
            _mockHnaRepo.Setup(x => x.TopStoriesAsync()).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = topStoryContent });
            _mockHnaRepo.Setup(x => x.GetStoryByIdAsync(It.IsAny<int>())).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = storyByIdContent });
            _mockCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);

            var result = await _hnaController.getHackerNewsStory("book");
            var okResult = result as OkObjectResult;
            var stories = okResult?.Value as List<Story>;

            Assert.IsNotNull(stories?.Count);
            Assert.IsTrue(stories.Count > 0);
            Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task GetAllStories_ShouldNotReturn_Stories_No_Hyperlinks()
        {
            var storyData = new Story() {
                by = "ashish",
                title = "test title book",
                url = ""
            };
            HttpContent topStoryContent = new StringContent("[37791002]");
            HttpContent storyByIdContent = new StringContent(JsonConvert.SerializeObject(storyData), Encoding.UTF8, "application/json");
            _mockHnaRepo.Setup(x => x.TopStoriesAsync()).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = topStoryContent });
            _mockHnaRepo.Setup(x => x.GetStoryByIdAsync(It.IsAny<int>())).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = storyByIdContent });
            _mockCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);

            var result = await _hnaController.getHackerNewsStory("");
            var okResult = result as OkObjectResult;
            var stories = okResult?.Value as List<Story>;

            Assert.IsNotNull(stories?.Count);
            Assert.IsTrue(stories.Count == 0);
            Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task ShouldThrowException_IfTopStoriesAPI_Fails()
        {
            HttpContent topStoryContent = new StringContent("{ INVALID DATA }");
            _mockHnaRepo.Setup(x => x.TopStoriesAsync()).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = topStoryContent });
            var result = await _hnaController.getHackerNewsStory("");
            var okResult = result as ObjectResult;
            Assert.That(okResult?.StatusCode, Is.EqualTo(500));
            Assert.That(okResult?.Value, Is.EqualTo("exception occured in API"));
            
        }

    }
}
