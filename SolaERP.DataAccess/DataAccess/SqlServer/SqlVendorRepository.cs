using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Entities.Vendors;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlVendorRepository : IVendorRepository, ILoggableCrudOperations<Vendor>
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlVendorRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> AddAsync(Vendor entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddAsync(Vendor entity, int userId = 0)
        {
            throw new NotImplementedException();
        }

        public Task<List<Vendor>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Vendor> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Vendor>> GetVendorDrafts(int userId, int businessUnitId)
        {
            List<Vendor> vendorDraftList = new List<Vendor>();
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

        public Task<List<Vendor>> GetVendorWFA(int userId, int businessUnitId)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(Vendor entity)
        {
            throw new NotImplementedException();
        }

        private Vendor GetVendorDarftsFromReader(IDataReader reader)
        {
            return new Vendor
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
