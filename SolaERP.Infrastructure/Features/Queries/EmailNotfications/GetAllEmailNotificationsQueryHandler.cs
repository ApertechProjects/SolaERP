using MediatR;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.Infrastructure.Features.Queries.EmailNotfications
{
    public class GetAllEmailNotificationsQueryHandler : IRequestHandler<GetAllEmailNotificationsQueryRequest, GetAllEmailNotificationsQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailNotficationRepository _repository;

        public GetAllEmailNotificationsQueryHandler(IUnitOfWork unitOfWork, IEmailNotficationRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<GetAllEmailNotificationsQueryResponse> Handle(GetAllEmailNotificationsQueryRequest request, CancellationToken cancellationToken)
        {
            var emails = await _repository.GetAllEmailNotficationsAsync();

            if (emails.Count == 0)
                emails = null;

            return new()
            {
                EmailNotfications = emails,
            };
        }
    }
}
