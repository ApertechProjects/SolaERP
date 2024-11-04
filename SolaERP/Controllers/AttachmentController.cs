using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Enums;
using SolaERP.Controllers;
using AttachmentDto = SolaERP.Application.Dtos.Attachment.AttachmentDto;

namespace SolaERP.API.Controllers;

[Route("api/[controller]/[action]")]
[Authorize]
public class AttachmentController : CustomBaseController
{
    private readonly IAttachmentService _attachmentService;

    public AttachmentController(IAttachmentService attachmentService)
    {
        _attachmentService = attachmentService;
    }

  
    [HttpGet]
    public async Task<IActionResult> GetBidAttachments(int sourceId)
    {
        var attachments = await _attachmentService.GetAttachmentsAsync(sourceId, SourceType.BID, Modules.Bid);
        return CreateActionResult(ApiResponse<List<AttachmentDto>>.Success(attachments));
    }

   
}