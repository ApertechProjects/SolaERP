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
    public async Task<IActionResult> GetRequestAttachments(int sourceId)
    {
        var attachments = await _attachmentService.GetAttachmentsAsync(sourceId, SourceType.REQ, Modules.Request);
        return CreateActionResult(ApiResponse<List<AttachmentDto>>.Success(attachments));
    }

    [HttpGet]
    public async Task<IActionResult> GetRFQAttachments(int sourceId)
    {
        var attachments = await _attachmentService.GetAttachmentsAsync(sourceId, SourceType.RFQ, Modules.Rfqs);
        return CreateActionResult(ApiResponse<List<AttachmentDto>>.Success(attachments));
    }

    [HttpGet]
    public async Task<IActionResult> GetBidAttachments(int sourceId)
    {
        var attachments = await _attachmentService.GetAttachmentsAsync(sourceId, SourceType.BID, Modules.Bid);
        return CreateActionResult(ApiResponse<List<AttachmentDto>>.Success(attachments));
    }

    [HttpGet]
    public async Task<IActionResult> GetBidCommercialAttachments(int sourceId)
    {
        var attachments = await _attachmentService.GetAttachmentsAsync(sourceId, SourceType.BID_COMM, Modules.Bid);
        return CreateActionResult(ApiResponse<List<AttachmentDto>>.Success(attachments));
    }

    [HttpGet]
    public async Task<IActionResult> GetBidComparisonAttachments(int sourceId)
    {
        var attachments = await _attachmentService.GetAttachmentsAsync(sourceId, SourceType.BID_COMP, Modules.BidComparison);
        return CreateActionResult(ApiResponse<List<AttachmentDto>>.Success(attachments));
    }


    [HttpGet]
    public async Task<IActionResult> GetOrderAttachments(int sourceId)
    {
        var attachments = await _attachmentService.GetAttachmentsAsync(sourceId, SourceType.ORDER, Modules.Orders);
        return CreateActionResult(ApiResponse<List<AttachmentDto>>.Success(attachments));
    }

    [HttpGet]
    public async Task<IActionResult> GetPaymentAttachments(int sourceId)
    {
        var attachments = await _attachmentService.GetAttachmentsAsync(sourceId, SourceType.PYMDC, Modules.Payment);
        return CreateActionResult(ApiResponse<List<AttachmentDto>>.Success(attachments));
    }

}