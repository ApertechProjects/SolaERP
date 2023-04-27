using MediatR;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.UnitOfWork;

namespace SolaERP.Application.Features.Commands.EmailNotifications
{
    public class UpdateEmaliNotificationCommandHandler : IRequestHandler<UpdateEmailNotificationRequest, bool>
    {
        private readonly IUnitOfWork _unitofWork;
        private readonly IEmailNotificationRepository _repository;

        public UpdateEmaliNotificationCommandHandler(IEmailNotificationRepository repository, IUnitOfWork unitofWork)
        {
            _repository = repository;
            _unitofWork = unitofWork;
        }

        public async Task<bool> Handle(UpdateEmailNotificationRequest request, CancellationToken cancellationToken)
        {
            var result = await _repository.UpdateAsync(new()
            {
                EmailNotificationId = request.EmailNotificationId,
                Notification = request.Notification,
                Description = request.Description,
                Check = false
            });

            await _unitofWork.SaveChangesAsync();
            return result;
        }
    }
}
