using IRFestival.Api.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Net;

namespace IRFestival.Api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
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
                Id = "test4",
                Date = DateTime.Now,
                Message = "Test Message",
                Status = "Unbuplished",
                Tag = "Tag",
                Title = "Title",
                SubTitle = "SubTitle",
                ImagePath = "ImagePath"
            };
            await _websiteArticlesContainer.CreateItemAsync(dummyArticle);
            return Ok(dummyArticle);
        }
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Article))]
        public async Task<IActionResult> GetAsync()
        {

            Database database = await _cosmosClient.CreateDatabaseIfNotExistsAsync("IRFestivalArticles");
            _websiteArticlesContainer = await database.CreateContainerIfNotExistsAsync("WebsiteArticles", "/tag");

            var result = new List<Article>();

            var queryDefinition = _websiteArticlesContainer.GetItemLinqQueryable<Article>()
                .Where(p => p.Status == nameof(Status.Published))
                .OrderBy(p => p.Date);

            var iterator = queryDefinition.ToFeedIterator();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                result = response.ToList();
            }

            return Ok(result);
        }
    }
}
