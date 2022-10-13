using IRFestival.Api.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace IRFestival.Api.Controllers
{
    public class ArticlesController : Controller
    {

        private CosmosClient _cosmosClient { get; set; }
        private Container _websiteArticlesContainer { get; set; }

        public ArticlesController(IConfiguration configuration)
        {
            _cosmosClient = new CosmosClient(configuration.GetConnectionString("CosmosConnection"));
            _websiteArticlesContainer = _cosmosClient.GetContainer("IRFestivalArticles", "WebsiteArticles");
        }
        [HttpPost]
        public async Task<ActionResult> CreateItemAsync()
        {
            var dummyArticle = new Article()
            {
                Date = DateTime.Now,
                Message = "Test Message",
                Status = "Status",
                Tag = "Tag",
                Title = "Title",
                ImagePath = "ImagePath"
            };
            await _websiteArticlesContainer.CreateItemAsync(dummyArticle);
            return Ok(dummyArticle);
        }
    }
}
