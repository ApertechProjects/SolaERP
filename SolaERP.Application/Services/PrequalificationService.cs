using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.Persistence.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Services
{
    public class PrequalificationService : IPrequalificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISupplierEvaluationRepository _repository;
        private readonly IMapper _mapper;

        public PrequalificationService(IUnitOfWork unitOfWork, ISupplierEvaluationRepository repository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<bool> SavePrequalificationAsync(List<PrequalificationDto> prequalifications, int vendorId)
        {
            List<Task<bool>> tasks = new();

            if (prequalifications is not null)
            {
                tasks.AddRange(prequalifications?.SelectMany(item =>
                {
                    var prequalificationValue = _mapper.Map<VendorPrequalificationValues>(item);
                    prequalificationValue.VendorId = vendorId;
                    prequalificationValue.DateTimeValue = prequalificationValue.DateTimeValue.ConvertDateToValidDate();

                    var tasksList = new List<Task<bool>>();
                    _repository.UpdatePrequalification(prequalificationValue); //+


                    //First delete all attachments belongs to current Prequalification_id and Vendor
                    if (item?.Attachments is not null)
                    {
                        for (int i = 0; i < item?.Attachments.Count; i++)
                        {
                            if (item.Attachments[i].Type == 2 && item.Attachments[i].AttachmentId > 0)
                            {
                                //tasksList.Add(
                                //    _attachmentRepository.DeleteAttachmentAsync(item.Attachments[i].AttachmentId));
                            }
                            else
                            {
                                var attachedFile = _mapper.Map<AttachmentSaveModel>(item.Attachments[i]);

                                attachedFile.SourceId = vendorId;
                                attachedFile.AttachmentTypeId = item.DesignId;
                                attachedFile.SourceType = SourceType.VEN_PREQ.ToString();

                                //tasksList.Add(_attachmentRepository.SaveAttachmentAsync(attachedFile));
                            }
                        }
                    }

                    if (item.HasGrid == true)
                    {
                        tasksList.AddRange(item.GridDatas.Select(gridData =>
                        {
                            var gridDatas = _mapper.Map<Application.Entities.SupplierEvaluation.PrequalificationGridData>(gridData);
                            gridDatas.PreqqualificationDesignId = item.DesignId;
                            gridDatas.VendorId = vendorId;

                            return _repository.UpdatePreGridAsync(gridDatas);
                        }));
                    }

                    return tasksList;
                }));
            }

            await Task.WhenAll(tasks);
            await _unitOfWork.SaveChangesAsync();

            return tasks.All(x => x.Result is true);
        }
    }
}
