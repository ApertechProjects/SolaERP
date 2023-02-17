using Microsoft.AspNetCore.Http;
using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.Menu;
using SolaERP.Infrastructure.Entities.User;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data;
using System.Data.Common;

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
            List<MenuWithPrivilages> userMenus = new List<MenuWithPrivilages>();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC dbo.SP_UserMenu_Load @userId";
                command.Parameters.AddWithValue(command, "@userId", userId);

                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    userMenus.Add(reader.GetByEntityStructure<MenuWithPrivilages>());

                return userMenus;
            }
        }

        public async Task<List<GroupMenu>> GetGroupMenusByGroupIdAsync(int groupId)
        {
            List<GroupMenu> menus = new();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SP_GroupMenus_Load";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue(command, "@GroupId", groupId);

                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                    menus.Add(reader.GetByEntityStructure<GroupMenu>());

                return menus;
            }
        }

        public Task<bool> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Menu entity)
        {
            throw new NotImplementedException();
        }

        public async Task<AdditionalPrivilegeAccess> GetAdditionalPrivilegeAccessAsync(int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "exec dbo.SP_AdditionalPrivilegesAccess @UserId";
                command.Parameters.AddWithValue(command, "@UserId", userId);
                using var reader = await command.ExecuteReaderAsync();

                AdditionalPrivilegeAccess user = new AdditionalPrivilegeAccess();
                if (reader.Read())
                {
                    user = reader.GetByEntityStructure<AdditionalPrivilegeAccess>();
                }
                return user;
            }
        }
    }
}
