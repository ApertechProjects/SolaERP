using Microsoft.AspNetCore.Identity;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Repositories;

namespace SolaERP.Application.Identity_Server
{
    public class UserStore : IUserStore<User>
    {
        private readonly IUserRepository _userRepository;
        public UserStore(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        #region UserStore Implementation
        public Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(int.Parse(userId));
            return user;
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUserNameAsync(normalizedUserName);
            return user;
        }

        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return await Task.Run(() => { return user.NormalizedUserName; });
        }

        public async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return await Task.Run(() => { return user.Id.ToString(); });
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return await Task.Run(() => { return user.UserName; });
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            await Task.Run(() => { user.UserName = userName; });
        }

        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
