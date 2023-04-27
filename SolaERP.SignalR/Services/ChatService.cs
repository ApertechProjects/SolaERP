using AutoMapper;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Entities.Chat;
using SolaERP.DataAccess.DataAcces.SqlServer;
using SolaERP.DataAccess.DataAccess.SqlServer;
using SolaERP.DataAccess.Factories;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Chat;
using SolaERP.Application.UnitOfWork;

namespace SolaERP.SignalR.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public ChatService(IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = new SqlUnitOfWork(ConnectionFactory.CreateSqlConnection(_configuration.GetConnectionString("DevelopmentConnectionString")));
            _chatRepository = new SqlChatRepository(_unitOfWork);
        }


        public async Task<List<ChatHistoryDto>> LoadChatHistoryAsync(int chatId)
        {
            var chatHistory = await _chatRepository.LoadChatHistoryAsync(chatId);
            var dto = _mapper.Map<List<ChatHistoryDto>>(chatHistory);

            return dto;
        }

        public async Task SaveChatHistoryAsync(ChatHistoryDto chatHistory)
        {
            var entity = _mapper.Map<ChatHistory>(chatHistory);
            await _chatRepository.SaveChatHistoryAsync(entity);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
