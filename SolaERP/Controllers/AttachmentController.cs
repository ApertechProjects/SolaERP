using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AttachmentController : CustomBaseController
    {
        private readonly IAttachmentService _attachmentService;

        public AttachmentController(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        [HttpGet("{attachmentId}")]
        public async Task<IActionResult> GetAttachmentsWithFilesAsync(int attachmentId)
          => CreateActionResult(await _attachmentService.GetAttachmentWithFilesAsync(attachmentId));

        [HttpPost]
        public async Task<IActionResult> GetAttachmentsAsync(AttachmentListGetModel model)
            => CreateActionResult(await _attachmentService.GetAttachmentsAsync(model));

        [HttpDelete("{attachmentId}")]
        public async Task<IActionResult> DeleteAttachmentAsync(int attachmentId)
            => CreateActionResult(await _attachmentService.DeleteAttachmentAsync(attachmentId));

        [HttpPut]
        public async Task<IActionResult> SaveAttachmentAsync(AttachmentSaveModel model)
            => CreateActionResult(await _attachmentService.SaveAttachmentAsync(model));
    }
}
