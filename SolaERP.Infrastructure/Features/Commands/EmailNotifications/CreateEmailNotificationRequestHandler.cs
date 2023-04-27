using MediatR;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.UnitOfWork;

namespace SolaERP.Application.Features.Commands.EmailNotifications
{
    public class CreateEmailNotificationRequestHandler : IRequestHandler<CreateEmailNotificationCommandRequest, bool>
    {
        private readonly IUnitOfWork _unitofWork;
        private readonly IEmailNotificationRepository _repository;

        public CreateEmailNotificationRequestHandler(IEmailNotificationRepository repository, IUnitOfWork unitofWork)
        {
            _repository = repository;
            _unitofWork = unitofWork;
        }

        public async Task<bool> Handle(CreateEmailNotificationCommandRequest request, CancellationToken cancellationToken)
        {
            var result = await _repository.CreateAsync(new()
            {
                EmailNotificationId = 0,
                Notification = request.Notification,
                Description = request.Description,
                Check = false
            });

            await _unitofWork.SaveChangesAsync();
            return result;
        }
    }
}
