using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Models;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    [Authorize]
    public class AttachmentController : CustomBaseController
    {
        private readonly IAttachmentService _attachmentService;

        public AttachmentController(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        [HttpGet("{attachmentId}")]
        public async Task<IActionResult> ById(int attachmentId)
          => CreateActionResult(await _attachmentService.GetAttachmentWithFilesAsync(attachmentId));

        [HttpGet]
        public async Task<IActionResult> ByType(int sourceId, string reference, string sourceType)
            => CreateActionResult(await _attachmentService.GetAttachmentsAsync(sourceId, reference, sourceType));

        [HttpPost]
        public async Task<IActionResult> Save(AttachmentSaveModel model)
            => CreateActionResult(await _attachmentService.SaveAttachmentAsync(model));

        [HttpDelete("{attachmentId}")]
        public async Task<IActionResult> Delete(int attachmentId)
            => CreateActionResult(await _attachmentService.DeleteAttachmentAsync(attachmentId));



    }
}
