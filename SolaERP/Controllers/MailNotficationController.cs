using MediatR;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Controllers;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Features.Commands.EmailNotfications;
using SolaERP.Infrastructure.Features.Queries.EmailNotfications;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MailNotficationController : CustomBaseController
    {
        private readonly IMediator _mediator;

        public MailNotficationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllEmailNotificationsQueryRequest request)
            => CreateActionResult(ApiResponse<GetAllEmailNotificationsQueryResponse>.Success(await _mediator.Send(request), 200));

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmailNotficationCommandRequest request)
        {
            var result = await _mediator.Send(request);
            return result ? Ok(ApiResponse<bool>.Success(204)) : BadRequest(ApiResponse<bool>.Fail("Something went wrong notfication is not created", 500));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateEmailNotficationRequest request)
        {
            var result = await _mediator.Send(request);
            return result ? Ok(ApiResponse<bool>.Success(204)) : BadRequest(ApiResponse<bool>.Fail("Something went wrong notfication is not updated", 500));
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(DeleteEmailNotficationRequest request)
        {
            var result = await _mediator.Send(request);
            return result ? Ok(ApiResponse<bool>.Success(204)) : BadRequest(ApiResponse<bool>.Fail("Something went wrong notfication is not deleted", 500));
        }

    }
}
