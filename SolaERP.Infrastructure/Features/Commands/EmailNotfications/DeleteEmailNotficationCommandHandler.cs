using MediatR;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.Infrastructure.Features.Commands.EmailNotfications
{
    public class DeleteEmailNotficationCommandHandler : IRequestHandler<DeleteEmailNotficationRequest, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailNotficationRepository _repository;

        public DeleteEmailNotficationCommandHandler(IEmailNotficationRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteEmailNotficationRequest request, CancellationToken cancellationToken)
        {
            var isDeleted = await _repository.DeleteAsync(request.Id);

            await _unitOfWork.SaveChangesAsync();
            return isDeleted;
        }
    }
}
