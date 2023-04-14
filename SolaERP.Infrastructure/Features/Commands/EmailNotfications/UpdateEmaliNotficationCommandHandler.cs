using MediatR;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.Infrastructure.Features.Commands.EmailNotfications
{
    public class UpdateEmaliNotficationCommandHandler : IRequestHandler<UpdateEmailNotficationRequest, bool>
    {
        private readonly IUnitOfWork _unitofWork;
        private readonly IEmailNotficationRepository _repository;

        public UpdateEmaliNotficationCommandHandler(IEmailNotficationRepository repository, IUnitOfWork unitofWork)
        {
            _repository = repository;
            _unitofWork = unitofWork;
        }

        public async Task<bool> Handle(UpdateEmailNotficationRequest request, CancellationToken cancellationToken)
        {
            var result = await _repository.UpdateAsync(new()
            {
                EmailNotificationId = request.EmailNotficationId,
                Notification = request.Notfication,
                Description = request.Description,
                Check = false
            });

            await _unitofWork.SaveChangesAsync();
            return result;
        }
    }
}
