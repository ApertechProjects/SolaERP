using SolaERP.Application.Dtos.SupplierEvaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Services
{
    public interface IPrequalificationService
    {
        public Task<bool> SavePrequalificationAsync(List<PrequalificationDto> prequalifications,int vendorId);
    }
}
