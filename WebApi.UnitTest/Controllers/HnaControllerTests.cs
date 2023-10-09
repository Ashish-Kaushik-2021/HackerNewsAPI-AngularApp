using Moq;
using Moq.AutoMock;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using demoAPI.Controllers;
using demoAPI.Interfaces;
using MemoryCache.Testing.Moq;
using demoAPI.Model;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace WebApi.UnitTests
{
    [TestClass]
    public class HnaControllerTests
    {
        private AutoMocker _mock;
        private readonly Mock<IHnaRepository> _hnaRepo;
        private HnaController _hnaController;

        public HnaControllerTests()
        {
            _mock = new AutoMocker();
            _hnaController = _mock.CreateInstance<HnaController>();
            
        }

        [TestMethod]
        public async Task GetAllStories_ShouldReturnStories()
        {
            var result = await _hnaController.getHackerNewsStory("") as JsonResult;
            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public async Task GetAllStories_ShouldReturnStories_with_searchterm()
        {
            var result = await _hnaController.getHackerNewsStory("book") as JsonResult;
            List<Story> storiesResult = JsonConvert.DeserializeObject<List<Story>>(result.Data?.ToString());
            Assert.IsTrue(storiesResult.Count > 0);
        }

    }
}