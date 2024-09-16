using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Validator
{
    public class DueDiligenceValidator
    {
        private readonly ISupplierEvaluationRepository _repository;

        public DueDiligenceValidator(ISupplierEvaluationRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> ValidateDueDiligenceAsync(List<DueDiligenceChildSaveDto> allChilds)
        {
            StringBuilder resultItems = new StringBuilder();
            var mandatoryCheckForDueDiligence = await _repository.GetDueDiligenceMandatoryDatas();
            //List<DueDiligenceChildSaveDto> allChilds = dueDiligence
            //    .Where(x => x.Childs != null)         // Ensure the Childs list is not null
            //    .SelectMany(x => x.Childs)
            //    .ToList();

            foreach (var item in allChilds)
            {
                bool isMandatory = mandatoryCheckForDueDiligence
                    .Where(x => x.DueDiligenceDesignId == item.DesignId)
                    .Select(x => x.IsMandatory)
                    .FirstOrDefault();

                if (IsFieldInvalid(item, isMandatory))
                {
                    resultItems.Append(item.LineNo + ",");
                }
            }
            if (resultItems.Length > 0)
            {
                string res = resultItems.ToString().TrimEnd(',');
                return res;
            }
            return null;
        }

        public async Task<bool> ValidateGridDatas(List<DueDiligenceChildSaveDto> allChilds)
        {
            foreach (var item in allChilds)
            {
                if (item.Question.Contains("3 fiscal years"))
                {
                    var emptyData = item.GridDatas.Where(x => string.IsNullOrEmpty(x.Column1) ||
                                                              string.IsNullOrEmpty(x.Column2) ||
                                                              string.IsNullOrEmpty(x.Column3) ||
                                                              string.IsNullOrWhiteSpace(x.Column4));
                    if (emptyData.Any())
                    {
                        return false;
                    }
                }
            }
            
            return true;
        }

        private bool IsFieldInvalid(DueDiligenceChildSaveDto item, bool isMandatory)
        {
            if (item.HasRadioBox)
                return false;

            return string.IsNullOrEmpty(item.TextboxValue) &&
                   (item.GridDatas == null || item.GridDatas.Count == 0) &&
                   item.DateTimeValue == null &&
                   (item.Attachments == null || item.Attachments.Count == 0) &&
                   isMandatory;
        }
    }
}
