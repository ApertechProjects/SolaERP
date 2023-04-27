using MediatR;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.UnitOfWork;

namespace SolaERP.Application.Features.Queries.EmailNotifications
{
    public class GetAllEmailNotificationsQueryHandler : IRequestHandler<GetAllEmailNotificationsQueryRequest, GetAllEmailNotificationsQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailNotificationRepository _repository;

        public GetAllEmailNotificationsQueryHandler(IUnitOfWork unitOfWork, IEmailNotificationRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<GetAllEmailNotificationsQueryResponse> Handle(GetAllEmailNotificationsQueryRequest request, CancellationToken cancellationToken)
        {
            var emails = await _repository.GetAllEmailNotificationsAsync();

            if (emails.Count == 0)
                emails = null;

            return new()
            {
                EmailNotifications = emails,
            };
        }
    }
}
