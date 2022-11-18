using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Repositories;
using System.Data;

namespace SolaERP.DataAccess.DataAcces.SqlServer
{
    public class SqlUserRepository : IUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SqlUserRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public bool Add(User entity)
        {
            string query = @"INSERT INTO Config.AppUser (RowIndex,FullName,NotificationEmail,ChangePassword,StatusId,Theme,ExpirationDate,Sessions,
                             LastActivity,Photo,ReturnMessage,CreatedOn,CreatedBy,UpdatedOn,UpdatedBy,UserName,NormalizedUserName,Email,EmailConfirmed,
                             PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,
                             AccessFailedCount,SyteLineUserCode,UserTypeId,CompanyId,Position,VendorId,UserToken)
                             VALUES (@RowIndex,@FullName,@NotificationEmail,@ChangePassword,@StatusId,@Theme,@ExpirationDate,@Sessions,
                             @LastActivity,@Photo,@ReturnMessage,@CreatedOn,@CreatedBy,@UpdatedOn,@UpdatedBy,@UserName,@NormalizedUserName,@Email,@EmailConfirmed,
                             @PasswordHash,@SecurityStamp,@ConcurrencyStamp,@PhoneNumber,@PhoneNumberConfirmed,@TwoFactorEnabled,@LockoutEnd,@LockoutEnabled,
                             @AccessFailedCount,@SyteLineUserCode,@UserTypeId,@CompanyId,@Position,@VendorId,@UserToken)";

            using (var command = _unitOfWork.CreateCommand())
            {
                command.Parameters.AddWithValue(command, "@RowIndex", entity.RowIndex);

                command.CommandText = query;
                return command.ExecuteNonQuery() == 0 ? false : true;
            }
        }

        public List<User> GetAllAsync()
        {
            using (var command = _unitOfWork.CreateCommand())
            {
                command.CommandText = "Select * from Config.AppUser";
                using var reader = command.ExecuteReader();

                List<User> users = new List<User>();
                while (reader.Read())
                {
                    users.Add(reader.GetByEntityStructure<User>());
                }
                return users;
            }
        }

        public User GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            User user = null;
            return await Task.Run(() =>
            {
                using (var command = _unitOfWork.CreateCommand())
                {
                    command.CommandText = "EXEC SP_GETUSER_BY_NAME_OR_ID NULL,@Id";
                    IDbDataParameter dbDataParameter = command.CreateParameter();
                    dbDataParameter.ParameterName = "@Id";
                    dbDataParameter.Value = id;
                    command.Parameters.Add(dbDataParameter);

                    var reader = command.ExecuteReader();

                    if (reader.Read())
                        user = reader.GetByEntityStructure<User>();

                    return user;
                }
            });
        }

        public User GetByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public void Remove(User entity)
        {
            throw new NotImplementedException();
        }

        public void Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
