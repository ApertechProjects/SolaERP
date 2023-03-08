using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AttachmentController : CustomBaseController
    {
        private readonly IAttachmentService _attachmentService;

        public AttachmentController(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        /// <summary>
        ///Retrieve attachment details with file data by ID
        /// </summary>
        /// <param name="attachmentId">ID of the attachment to retrieve.</param>

        [HttpGet("{attachmentId}")]
        public async Task<IActionResult> GetAttachmentsWithFilesAsync(int attachmentId)
          => CreateActionResult(await _attachmentService.GetAttachmentWithFilesAsync(attachmentId));

        /// <summary>
        ///Get a list of attachments based on the specified criteria.
        /// </summary>
        /// <remarks>This endpoint allows you to retrieve a list of attachments based on the specified parameters.The attachments can be filtered by source ID, source type ID, attachment type and attachment sub-type ID.</remarks>
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
