using ImageDownloader.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageDownloader.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController: ControllerBase
    {
        private readonly ImageService _imageService;
        public ImageController(ImageService imageServeice)
        {
                _imageService = imageServeice;
        }
        [HttpPost]
        public async Task <IActionResult> DownloadAndStoreImages([FromQuery] string query, [FromQuery] int maxNum)
        {
            await _imageService.DownloadAndStoreImages(query, maxNum);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetImages([FromQuery] string query, [FromQuery] int maxNum)
        {
            var images = await _imageService.FetchImages(query, maxNum);
            return Ok(images);
        }
    }
}
