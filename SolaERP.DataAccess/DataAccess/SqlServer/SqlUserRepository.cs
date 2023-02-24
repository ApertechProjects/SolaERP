using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Entities.User;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAcces.SqlServer
{
    public class SqlUserRepository : IUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlUserRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        #region Read Operations

        public async Task<List<User>> GetAllAsync()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT * FROM [dbo].[VW_User_List]";
                using var reader = await command.ExecuteReaderAsync();

                List<User> users = new List<User>();
                while (reader.Read())
                    users.Add(GetFromReader(reader));

                return users;
            }
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            User user = null;
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "Select * from Config.AppUser where Id = @Id";
                command.Parameters.AddWithValue(command, "@Id", userId);

                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    user = reader.GetByEntityStructure<User>();
                }
                return user;
            }
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            User user = null;
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT * FROM FN_GET_USER_BY_EMAIL (@Email)";
                command.Parameters.AddWithValue(command, "@Email", email == null ? DBNull.Value : email);

                using var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                    user = reader.GetByEntityStructure<User>();

                return user;
            }
        }
        public async Task<User> GetByIdAsync(int id)
        {
            User user = null;
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_GETUSER_BY_NAME_OR_ID @Id";
                command.Parameters.AddWithValue(command, "@Id", id);

                using var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                    user = reader.GetByEntityStructure<User>();

                return user;
            }
        }
        public async Task<User> GetUserByUsernameAsync(string userName)
        {
            User user = null;
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_GETUSER_BY_NAME_OR_ID NULL,@UserName";
                command.Parameters.AddWithValue(command, "@UserName", userName);

                using var reader = await command.ExecuteReaderAsync();
                if (reader.Read())
                    user = reader.GetByEntityStructure<User>();

                return user;
            }
        }
        public async Task<User> GetLastInsertedUserAsync()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT * FROM GET_LAST_INSERTED_USER";
                using var reader = await command.ExecuteReaderAsync();

                User user = new User();
                if (reader.Read())
                {
                    user = reader.GetByEntityStructure<User>();
                }
                return user;
            }
        }
        public async Task<int> GetUserIdByTokenAsync(string finderToken)
        {
            int userId = 0;
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT ID FROM CONFIG.APPUSER WHERE USERTOKEN = @USERTOKEN";
                command.Parameters.AddWithValue(command, "@USERTOKEN", finderToken == null ? DBNull.Value : finderToken);

                using var reader = await command.ExecuteReaderAsync();
                if (reader.Read())
                    userId = reader.Get<int>("Id");
            }
            return userId;
        }

        #endregion
        //
        #region DML Operations 

        public async Task<bool> AddAsync(User entity)
        {
            string query = "Exec SP_User_insert @FullName,@StatusId,@UserName,@Email ,@EmailConfirmed ,@PasswordHash,@UserTypeId,@UserToken";
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = query;
                command.Parameters.AddWithValue(command, "@FullName", entity.FullName);
                command.Parameters.AddWithValue(command, "@StatusId", entity.StatusId);
                command.Parameters.AddWithValue(command, "@UserName", entity.UserName);
                command.Parameters.AddWithValue(command, "@Email", entity.Email);
                command.Parameters.AddWithValue(command, "@EmailConfirmed", entity.EmailConfirmed);
                command.Parameters.AddWithValue(command, "@PasswordHash", entity.PasswordHash);
                command.Parameters.AddWithValue(command, "@UserTypeId", entity.UserTypeId);
                command.Parameters.AddWithValue(command, "@UserToken", entity.UserToken.ToString());

                var value = await command.ExecuteNonQueryAsync();
                return value > 0;
            }
        }
        public async Task<bool> RemoveAsync(int Id)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SP_DELETE_USER @Id";
                command.Parameters.AddWithValue(command, "@Id", Id);
                var value = await command.ExecuteNonQueryAsync();

                return value > 0;
            }
        }
        public async Task UpdateAsync(User entity)
        {
            string query = "Exec [dbo].[SP_UserData_U] @UserId,@FullName,@Position,@PhoneNumber,@Photo,@PasswordHash";
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.Parameters.AddWithValue(command, "@UserId", entity.Id);
                command.Parameters.AddWithValue(command, "@FullName", entity.FullName);
                command.Parameters.AddWithValue(command, "@Position", entity.Position);
                command.Parameters.AddWithValue(command, "@PhoneNumber", entity.PhoneNumber);
                command.Parameters.AddWithValue(command, "@Photo", entity.Photo);
                command.Parameters.AddWithValue(command, "@PasswordHash", entity.PasswordHash);
                command.CommandText = query;
                //TODO: Handle Procedure Convert Error When Updateing User
                await command.ExecuteNonQueryAsync();
            }
        }
        public async Task<bool> UpdateUserTokenAsync(int userId, Guid token)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SP_UPDATE_USER_TOKEN";//@USERID,@USERTOKEN";

                command.Parameters.AddWithValue(command, "@USERID", userId);
                command.Parameters.AddWithValue(command, "@USERTOKEN", token);
                command.CommandType = CommandType.StoredProcedure;

                var result = await command.ExecuteNonQueryAsync();

                return result > 0;
            }
        }
        public async Task<bool> ResetUserPasswordAsync(string email, string passwordHash)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SP_UserPassword_U";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue(command, "@EMAIL", email);
                command.Parameters.AddWithValue(command, "@PASSWORDHASH", passwordHash);

                var result = await command.ExecuteNonQueryAsync();

                return result > 0;
            }
        }

        public async Task<List<ActiveUser>> GetActiveUsersAsync()
        {
            List<ActiveUser> activeUser = new List<ActiveUser>();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "Select * from [dbo].[VW_User_List]";

                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    activeUser.Add(reader.GetByEntityStructure<ActiveUser>());
                }
                return activeUser;
            }
        }


        #endregion
        //
        #region Readers
        private User GetFromReader(IDataReader reader)
        {
            return new() { Id = reader.Get<int>("Id"), FullName = reader.Get<string>("FullName") };
        }


        #endregion
    }
}
