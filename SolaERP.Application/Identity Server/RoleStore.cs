using Microsoft.AspNetCore.Identity;
using SolaERP.Infrastructure.Entities.Auth;

namespace SolaERP.Application.Identity_Server
{
    public class RoleStore : IRoleStore<User>
    {


        #region RoleStore Implementation
        public Task CreateAsync(User role)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> CreateAsync(User role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(User role)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(User role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByIdAsync(string roleId)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByNameAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedRoleNameAsync(User role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(User role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleNameAsync(User role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(User role, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetRoleNameAsync(User role, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(User role)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(User role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
