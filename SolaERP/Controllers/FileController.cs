using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> Test()
        {
            var photo = await _fileService.DownloadPhotoWithNetworkAsBase64Async(@"\\116.203.90.202\shared folder Dev\SolaErpSRC\920e1c06-0f76-42d6-88e2-c4bbc1c058cfc#.png");
            var resizedImage = _fileService.ResizeImage(Convert.FromBase64String(photo), 50, 50);

            return Ok(Convert.ToBase64String(resizedImage));
        }
    }
}
