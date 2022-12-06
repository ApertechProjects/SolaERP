using Microsoft.AspNetCore.Http;
using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.Menu;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlMenuRepository : IMenuRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SqlMenuRepository(IUnitOfWork unitOfWork, IHttpContextAccessor context)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> AddAsync(Menu entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Menu>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Menu> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MenuWithPrivilages>> GetUserMenuWithPrivillagesAsync(int userId)
        {
            var result = await Task.Run(() =>
            {
                List<MenuWithPrivilages> userMenus = new List<MenuWithPrivilages>();
                using (var command = _unitOfWork.CreateCommand())
                {
                    command.CommandText = "EXEC dbo.SP_UserMenu_Load @userId";
                    command.Parameters.AddWithValue(command, "@userId", userId);

                    using var reader = command.ExecuteReader();

                    while (reader.Read())
                        userMenus.Add(reader.GetByEntityStructure<MenuWithPrivilages>());

                    return userMenus;
                }
            });
            return result;
        }

        public bool Remove(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(Menu entity)
        {
            throw new NotImplementedException();
        }
    }
}
