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

        public async Task<ApiResponse<string>> DeleteAttachmentAsync(int attachmentId)
        {
            bool result = await _attachmentRepository.DeleteAttachmentAsync(attachmentId);
            await _unitOfWork.SaveChangesAsync();

            return result ? ApiResponse<string>.Success("Operation is succesfull", 200) : ApiResponse<string>.Fail("Operation failed", 400);
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
            for (int i = 0; i < result.Count; i++)
            {
                result[i].Base64 = $"data:{result[i].ExtensionType};base64," + Convert.ToBase64String(result[i].FileData);
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
