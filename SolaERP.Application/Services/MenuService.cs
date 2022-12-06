using SolaERP.Application.Utils;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Entities.Menu;

namespace SolaERP.Application.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _repository;

        public MenuService(IMenuRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Menu>> GetUserMenusWithChildAsync()
        {
            var menus = await _repository.GetUserMenusAsync(Kernel.CurrentUserId);



            throw new NotImplementedException();


        }
    }
}
