using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Attachment;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Models;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.Application.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentRepository _attachmentRepository;

        public AttachmentService(IAttachmentRepository attachmentRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _attachmentRepository = attachmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<AttachmentDto>>> GetAttachmentsAsync(AttachmentListGetModel model)
        {
            var entity = await _attachmentRepository.GetAttachmentListAsync(model);
            var result = _mapper.Map<List<AttachmentDto>>(entity);

            return ApiResponse<List<AttachmentDto>>.Success(result, 200);
        }

        public async Task<ApiResponse<List<AttachmentWithFileDto>>> GetAttachmentWithFilesAsync(int attachmentId)
        {
            var entity = await _attachmentRepository.GetAttachmenListWithFileDataAsync(attachmentId);
            var result = _mapper.Map<List<AttachmentWithFileDto>>(entity);

            return ApiResponse<List<AttachmentWithFileDto>>.Success(result, 200);
        }
    }
}
