using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.SupplierEvaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Validator
{
    public class PrequalificationValidator
    {
        private readonly ISupplierEvaluationRepository _repository;

        public PrequalificationValidator(ISupplierEvaluationRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> ValidateDueDiligenceAsync(List<PrequalificationDesignListDto> prequalification)
        {
            StringBuilder resultItems = new StringBuilder();
            var mandatoryCheckForDueDiligence = await _repository.GetDueDiligenceMandatoryDatas();
            List<PrequalificationChildSaveDto> allChilds = prequalification
                    .Where(x => x.Prequalifications != null)   
                    .SelectMany(x => x.Prequalifications)      
                    .Where(p => p.Childs != null)              
                    .SelectMany(p => p.Childs)                 
                    .ToList();

            foreach (var item in allChilds)
            {
                bool isMandatory = mandatoryCheckForDueDiligence
                    .Where(x => x.DueDiligenceDesignId == item.DesignId)
                    .Select(x => x.IsMandatory)
                    .FirstOrDefault();

                if (IsFieldInvalid(item, isMandatory))
                {
                    string res = resultItems.ToString().TrimEnd(',');
                    return res;
                }
            }
            if (resultItems.Length > 0)
            {
                string res = resultItems.ToString().TrimEnd(',');
                return res;
            }
            return null;
        }

        private bool IsFieldInvalid(PrequalificationChildSaveDto item, bool isMandatory)
        {
            // Check for invalid field conditions
            return string.IsNullOrEmpty(item.TextboxValue) && (item.GridDatas == null || item.GridDatas.Count == 0) && isMandatory;
        }
    }
}
