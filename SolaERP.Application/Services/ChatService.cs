using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Chat;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Entities.Chat;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ChatService(IChatRepository chatRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _chatRepository = chatRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<User> GetSenderAsync(string finderToken)
        {
            var senderId = await _userRepository.GetUserIdByTokenAsync(finderToken);
            var sender = await _userRepository.GetByIdAsync(senderId);
            return sender;
        }

        public async Task<List<ChatHistoryDto>> LoadChatHistoryAsync(int chatId)
        {
            var chatHistory = await _chatRepository.LoadChatHistoryAsync(chatId);
            var chatHistoryList = _mapper.Map<List<ChatHistoryDto>>(chatHistory);

            return chatHistoryList;
        }

        public async Task SaveChatHistoryAsync(ChatHistoryDto chatHistory)
        {
            var chatHistoryEntity = _mapper.Map<ChatHistory>(chatHistory);
            await _chatRepository.SaveChatHistoryAsync(chatHistoryEntity);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
