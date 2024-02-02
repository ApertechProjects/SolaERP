using SolaERP.Application.Dtos.Payment;
using SolaERP.Application.Dtos.Vendors;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Entities.Vendors;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IVendorRepository
    {
        Task<int> AddBankDetailsAsync(int userId, VendorBankDetail bankDetail);
        Task<int> UpdateBankDetailsAsync(int userId, VendorBankDetail bankDetail);
        Task<int> DeleteBankDetailsAsync(int userId, int id);
        Task<int> AddAsync(int userId, Vendor vendor);
        Task<int> UpdateAsync(int userId, Vendor vendor);
        Task<int> DeleteAsync(int userId, int id);
        Task<VendorInfo> GetByTaxAsync(string taxId);
        Task<bool> ChangeStatusAsync(int vendorId, int status, int sequence, string comment, int userId);
        Task<List<VendorWFA>> GetWFAAsync(int userId, VendorFilter filter);
        Task<List<VendorAll>> GetAll(int userId, VendorAllCommandRequest request);
        Task<List<VendorApproved>> GetApprovedAsync(int userId);
        Task<List<VendorWFA>> GetHeldAsync(int userId, VendorFilter filter);
        Task<List<VendorDraft>> GetDraftAsync(int userId, VendorFilter filter);
        Task<List<VendorInfo>> Vendors(int userId);
        Task<VendorLoad> GetHeader(int vendorId);
        Task<List<VendorWFA>> GetRejectedAsync(int userId, VendorFilter filter);
        Task<bool> ApproveAsync(VendorApproveModel model);
        Task<bool> SendToApprove(VendorSendToApproveRequest request);
        Task<string> GetVendorLogo(int vendorId);
        Task<bool> HasVendorName(string vendorName, int userId);
        Task<string> GetVendorNameByUserId(int userId);
        Task<List<VendorRFQListDto>> GetVendorRFQList(string vendorCode, int userId);
        Task<bool> RFQVendorResponseChangeStatus(int rfqMainId, int status, string vendorCode);
        Task<int> GetRevisionVendorIdByVendorCode(string vendorCode);
        Task<int> GetRevisionNumberByVendorCode(string vendorCode);
        Task<bool> TransferToIntegration(CreateVendorRequest request);
    }
}