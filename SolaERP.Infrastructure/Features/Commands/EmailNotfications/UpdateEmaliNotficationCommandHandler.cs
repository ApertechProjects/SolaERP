using MediatR;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.Infrastructure.Features.Commands.EmailNotifications
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
