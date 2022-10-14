using IRFestival.Api.Common;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IRFestival.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        private BlobUtility BlobUtility { get; }

        public PicturesController(BlobUtility blobUtility)
        {
            BlobUtility = blobUtility;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(string[]))]
        public async Task<ActionResult> GetAllPictureUrls()
        {
            var container = BlobUtility.GetPicturesContainer();
            var result = container.GetBlobs().Select(blob => BlobUtility.GetSasUri(container, blob.Name)).ToArray();

            return Ok(result);
        }

        [HttpPost]
        public void PostPicture(IFormFile file)
        {
        }
    }
}
