using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Entities.Vendors;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlVendorRepository : IVendorRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlVendorRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> AddAsync(Vendors entity)
        {
            throw new NotImplementedException();
        }


        public Task<List<Vendors>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Vendors> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<VendorInfo> GetVendorByTaxIdAsync(string taxId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "exec SP_VendorListByTaxId @TaxId";
                command.Parameters.AddWithValue(command, "@TaxId", taxId);
                using var reader = await command.ExecuteReaderAsync();
                VendorInfo result = new();

                while (reader.Read()) result = reader.GetByEntityStructure<VendorInfo>();

                return result;
            }
        }

        public async Task<List<Vendors>> GetVendorDrafts(int userId, int businessUnitId)
        {
            List<Vendors> vendorDraftList = new List<Vendors>();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC dbo.SP_VendorDraft @UserID,@BusinessUnitId";

                command.Parameters.AddWithValue(command, "@UserID", userId);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);

                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    vendorDraftList.Add(GetVendorDarftsFromReader(reader));
                }

                return vendorDraftList;
            }

        }

        public Task<List<Vendors>> GetVendorWFA(int userId, int businessUnitId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Vendors entity)
        {
            throw new NotImplementedException();
        }

        private Vendors GetVendorDarftsFromReader(IDataReader reader)
        {
            return new Vendors
            {
                VendorId = reader.Get<int>("VendorId"),
                VendorCode = reader.Get<string>("VendorCode"),
                VendorName = reader.Get<string>("VendorName"),
                TaxId = reader.Get<string>("TaxId"),
                Location = reader.Get<string>("CompanyLocation"),
                Website = reader.Get<string>("CompanyWebsite"),
                RepresentedProducts = reader.Get<string>("RepresentedProducts"),
                RepresentedCompanies = reader.Get<string>("RepresentedCompanies"),
                PaymentTerms = reader.Get<string>("PaymentTermsCode"),
                CreditDays = reader.Get<int>("CreditDays"),
                V60DaysPayment = reader.Get<int>("AgreeWithDefaultDays"),
                Country = reader.Get<string>("CountryCode"),
                Status = reader.Get<int>("StatusId"),
                StatusName = reader.Get<string>("StatusName"),
                ApproveStatusName = reader.Get<string>("ApproveStatusName"),
                UserId = reader.Get<int>("Id"),
                UserStatusName = reader.Get<string>("StatusName"),
                AppUser = new User
                {
                    Id = reader.Get<int>("Id"),
                    FullName = reader.Get<string>("Fullname"),
                    StatusId = reader.Get<int>("StatusId"),
                    Position = reader.Get<string>("Position"),
                    Sessions = reader.Get<int>("Sessions"),
                    LastActivity = reader.Get<DateTime>("LastActivity"),
                    UserName = reader.Get<string>("UserName"),
                },
            };
        }

    }
}
