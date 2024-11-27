using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Attachment;
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
        private readonly IFileUploadService _fileUploadService;

        public AttachmentService(IAttachmentRepository attachmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileUploadService fileUploadService)
        {
            _attachmentRepository = attachmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileUploadService = fileUploadService;
        }

        public async Task<bool> DeleteAttachmentAsync(int attachmentId)
        {
            bool result = await _attachmentRepository.DeleteAttachmentAsync(attachmentId);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<bool> DeleteAttachmentAsync(int sourceId, SourceType sourceType)
        {
            bool result = await _attachmentRepository.DeleteAttachmentAsync(sourceId, sourceType);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }


        public async Task<List<AttachmentDto>> GetAttachmentsAsync(int sourceId, SourceType sourceType, Modules module,
            int attachmentTypeId = 0, string reference = null, bool isDownloadLink = true)
       {
            var entity = await _attachmentRepository.GetAttachmentsAsync(sourceId,
                reference,
                sourceType.ToString(),
                attachmentTypeId);
            var result = _mapper.Map<List<AttachmentDto>>(entity);

            if (isDownloadLink)
            {
                SetDownloadLink(result, module);
            }
            else
            {
                SetGetFileLink(result, module);
            }

            return result;
        }

        public async Task<AttachmentDto> GetAttachmentById(int attachmentId, bool getLink = false,
            Modules module = default, bool isDownloadLink = true)
        {
            var result = _mapper.Map<AttachmentDto>(await _attachmentRepository.GetAttachmentByIdAsync(attachmentId));

            if (!getLink) return result;
            if (isDownloadLink)
            {
                SetDownloadLink(result, module);
            }
            else
            {
                SetGetFileLink(result, module);
            }

            return result;
        }

        public async Task SaveAttachmentAsync(AttachmentSaveModel attachment, SourceType sourceType, int sourceId)
        {
            attachment.SourceId = sourceId;
            attachment.SourceType = sourceType.ToString();
            await _attachmentRepository.SaveAttachmentAsync(attachment);
        }

        public async Task SaveAttachmentAsync(List<AttachmentSaveModel> attachments,
            SourceType sourceType,
            int sourceId)
        {
            foreach (var attachment in attachments)
            {
                if (attachment.Type == 2)
                {
                    await DeleteAttachmentAsync(attachment.AttachmentId);
                    continue;
                }

                if (attachment.AttachmentId > 0) continue;

                await SaveAttachmentAsync(attachment, sourceType, sourceId);
            }
        }


        public async Task SaveAttachmentAsync2(List<AttachmentSaveModel> attachments,
        SourceType sourceType,
        int sourceId)
        {
            foreach (var attachment in attachments)
            {
                if (attachment.Type == 2 && attachment.AttachmentId > 0)
                {
                    await DeleteAttachmentAsync(attachment.AttachmentId);
                    continue;
                }

                if (attachment.AttachmentId > 0) continue;

                await SaveAttachmentAsync(attachment, sourceType, sourceId);
            }
        }

        private void SetDownloadLink(AttachmentDto attachmentDto, Modules module)
        {
            attachmentDto.FileLink = _fileUploadService.GetDownloadFileLink(attachmentDto.FileLink, module);
        }

        private void SetDownloadLink(List<AttachmentDto> attachmentList, Modules module)
        {
            attachmentList.ForEach(x => SetDownloadLink(x, module));
        }

        private void SetGetFileLink(AttachmentDto attachmentDto, Modules module)
        {
            attachmentDto.FileLink = _fileUploadService.GetFileLink(attachmentDto.FileLink, module);
        }

        private void SetGetFileLink(List<AttachmentDto> attachmentList, Modules module)
        {
            attachmentList.ForEach(x => SetGetFileLink(x, module));
        }
    }
}