using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.Buyer;
using SolaERP.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlBuyerRepository : IBuyerRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlBuyerRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> AddAsync(Buyer entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Buyer>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Buyer>> GetBuyerByUserTokenAsync(int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "exec dbo.SP_UNI_Buyer_List";
                using var reader = await command.ExecuteReaderAsync();

                List<Buyer> status = new List<Buyer>();

                while (reader.Read())
                {
                    status.Add(reader.GetByEntityStructure<Buyer>());
                }
                return status;
            }
        }

        public Task<Buyer> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Buyer entity)
        {
            throw new NotImplementedException();
        }
    }
}
