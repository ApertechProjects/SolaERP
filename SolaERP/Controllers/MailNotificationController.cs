using MediatR;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Controllers;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Features.Commands.EmailNotifications;
using SolaERP.Infrastructure.Features.Queries.EmailNotifications;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MailNotificationController : CustomBaseController
    {
        private readonly IMediator _mediator;

        public MailNotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllEmailNotificationsQueryRequest request)
            => CreateActionResult(ApiResponse<GetAllEmailNotificationsQueryResponse>.Success(await _mediator.Send(request), 200));

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmailNotificationCommandRequest request)
        {
            var result = await _mediator.Send(request);
            return result ? Ok(ApiResponse<bool>.Success(200)) : BadRequest(ApiResponse<bool>.Fail("Something went wrong Notification is not created", 400));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateEmailNotificationRequest request)
        {
            var result = await _mediator.Send(request);
            return result ? Ok(ApiResponse<bool>.Success(200)) : BadRequest(ApiResponse<bool>.Fail("Something went wrong Notification is not updated", 400));
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(DeleteEmailNotificationRequest request)
        {
            var result = await _mediator.Send(request);
            return result ? Ok(ApiResponse<bool>.Success(200)) : BadRequest(ApiResponse<bool>.Fail("Something went wrong Notification is not deleted", 400));
        }

    }
}
