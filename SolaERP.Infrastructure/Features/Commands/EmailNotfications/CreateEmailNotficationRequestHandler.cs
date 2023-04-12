using MediatR;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.Infrastructure.Features.Commands.EmailNotfications
{
    public class CreateEmailNotficationRequestHandler : IRequestHandler<CreateEmailNotficationCommandRequest, bool>
    {
        private readonly IUnitOfWork _unitofWork;
        private readonly IEmailNotficationRepository _repository;

        public CreateEmailNotficationRequestHandler(IEmailNotficationRepository repository, IUnitOfWork unitofWork)
        {
            _repository = repository;
            _unitofWork = unitofWork;
        }

        public async Task<bool> Handle(CreateEmailNotficationCommandRequest request, CancellationToken cancellationToken)
        {
            var result = await _repository.CreateAsync(new()
            {
                EmailNotificationId = 0,
                Notification = request.Notfication,
                Description = request.Description,
                Check = false
            });

            await _unitofWork.SaveChangesAsync();
            return result;
        }
    }
}
