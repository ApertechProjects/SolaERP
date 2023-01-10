using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.Procedure;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlProcedureRepository : IProcedureRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlProcedureRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> AddAsync(Procedure entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Procedure>> GetAllAsync()
        {
            var result = await Task.Run(() =>
            {
                List<Procedure> procedures = new List<Procedure>();
                using (var command = _unitOfWork.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM dbo.VW_Procedures_List";
                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        procedures.Add(reader.GetByEntityStructure<Procedure>());
                    }

                    return procedures;
                }
            });
            return result;
        }

        public Task<Procedure> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Procedure entity)
        {
            throw new NotImplementedException();
        }
    }
}
