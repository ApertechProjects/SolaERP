using SolaERP.Infrastructure.Entities.Groups;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public abstract class SqlBaseGroupRepository
    {
        protected abstract Task<bool> SaveEmailNotficationAsync(GroupEmailNotfication entity);
    }
}
