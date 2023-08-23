using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Attachment;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;

namespace SolaERP.Persistence.Services
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

        public async Task<ApiResponse<string>> DeleteAttachmentAsync(int attachmentId)
        {
            bool result = await _attachmentRepository.DeleteAttachmentAsync(attachmentId);
            await _unitOfWork.SaveChangesAsync();

            return result ? ApiResponse<string>.Success("Operation is succesfull", 200) : ApiResponse<string>.Fail("Operation failed", 400);
        }

        public async Task<ApiResponse<string>> DeleteAttachmentAsync(int sourceId, SourceType sourceType)
        {
            bool result = await _attachmentRepository.DeleteAttachmentAsync(sourceId, sourceType);
            await _unitOfWork.SaveChangesAsync();

            return result ? ApiResponse<string>.Success("Operation is succesfull", 200) : ApiResponse<string>.Fail("Operation failed", 400);
        }

        public async Task<ApiResponse<List<AttachmentDto>>> GetAttachmentsAsync(int sourceId, string reference, string sourceType)
        {
            var entity = await _attachmentRepository.GetAttachmentsAsync(sourceId, reference, sourceType);
            var result = _mapper.Map<List<AttachmentDto>>(entity);

            return ApiResponse<List<AttachmentDto>>.Success(result, 200);
        }

        public async Task<ApiResponse<List<string>>> GetAttachmentsAsync(int sourceId, int sourceType)
        {
            var entity = await _attachmentRepository.GetAttachmentsAsync(sourceId, sourceType);

            return ApiResponse<List<string>>.Success(entity, 200);
        }

        public async Task<ApiResponse<List<AttachmentWithFileDto>>> GetAttachmentWithFilesAsync(int attachmentId)
        {
            var entity = await _attachmentRepository.GetAttachmentsWithFileDataAsync(attachmentId);
            var result = _mapper.Map<List<AttachmentWithFileDto>>(entity);
            for (int i = 0; i < result.Count; i++)
            {
                result[i].Preview = $"data:{result[i].ExtensionType};base64," + result[i].FileData;
            }
            return ApiResponse<List<AttachmentWithFileDto>>.Success(result, 200);
        }

        public async Task<ApiResponse<string>> SaveAttachmentAsync(AttachmentSaveModel model)
        {
            bool result = await _attachmentRepository.SaveAttachmentAsync(model);
            await _unitOfWork.SaveChangesAsync();

            return result ? ApiResponse<string>.Success("Operation is succesfull", 200) : ApiResponse<string>.Fail("Operation failed", 400);
        }
    }
}
