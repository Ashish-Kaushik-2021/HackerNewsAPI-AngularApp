using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;

namespace demoAPI.Interfaces
{
    public interface IHackerNewsRepository
    {
        Task<HttpResponseMessage> TopStoriesAsync();
        Task<HttpResponseMessage> GetStoryByIdAsync(int id);

    }
}
