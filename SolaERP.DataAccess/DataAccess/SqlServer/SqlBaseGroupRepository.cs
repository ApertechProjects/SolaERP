using SolaERP.Application.Entities.Groups;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public abstract class SqlBaseGroupRepository
    {
        protected abstract Task<bool> SaveEmailNotificationAsync(GroupEmailNotification entity);
    }
}
