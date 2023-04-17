using MediatR;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.Infrastructure.Features.Commands.EmailNotifications
{
    public class DeleteEmailNotificationCommandHandler : IRequestHandler<DeleteEmailNotificationRequest, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailNotificationRepository _repository;

        public DeleteEmailNotificationCommandHandler(IEmailNotificationRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteEmailNotificationRequest request, CancellationToken cancellationToken)
        {
            var isDeleted = await _repository.DeleteAsync(request.Id);

            await _unitOfWork.SaveChangesAsync();
            return isDeleted;
        }
    }
}
