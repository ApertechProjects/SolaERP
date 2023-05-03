using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.Groups;
using SolaERP.Application.Entities.User;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

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
                    user = reader.GetByEntityStructure<User>("InActive", "RefreshToken", "RefreshTokenEndDate");

                return user;
            }
        }

        public async Task<User> GetUserByEmailCode(string token)
        {
            User user = null;
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT * FROM Config.AppUser Where EmailVerificationToken = @EmailVerTok";
                command.Parameters.AddWithValue(command, "@EmailVerTok", token == null ? DBNull.Value : token);

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
        public async Task<int> GetIdentityNameAsIntAsync(string name)
        {
            int userId = Convert.ToInt32(name);
            return userId;
        }

        public async Task<string> GetUserNameByTokenAsync(string finderToken)
        {
            string userName = string.Empty;
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT FullName FROM CONFIG.APPUSER WHERE USERTOKEN = @USERTOKEN";
                command.Parameters.AddWithValue(command, "@USERTOKEN", finderToken == null ? DBNull.Value : finderToken);

                using var reader = await command.ExecuteReaderAsync();
                if (reader.Read())
                    userName = reader.Get<string>("FullName");
            }
            return null;
        }

        #endregion
        //
        #region DML Operations 
        public async Task<int> SaveUserAsync(User entity)
        {
            string query = @"SET NOCOUNT OFF Exec SP_AppUser_IUD @Id,@FullName,@ChangePassword,@Theme,@UserName,@Email,
                           @PasswordHash,@PhoneNumber ,@UserTypeId,@VendorId,@UserToken,@Gender,@Buyer,@Description,@ERPUser,@UserPhoto,@SignaturePhoto,@Inactive,@ChangeUserId,@NewId output";
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = query;
                command.Parameters.AddWithValue(command, "@Id", entity.Id);
                command.Parameters.AddWithValue(command, "@FullName", entity.FullName);
                command.Parameters.AddWithValue(command, "@ChangePassword", entity.ChangePassword);
                command.Parameters.AddWithValue(command, "@Theme", entity.Theme);
                command.Parameters.AddWithValue(command, "@UserName", entity.UserName);
                command.Parameters.AddWithValue(command, "@Email", entity.Email);
                command.Parameters.AddWithValue(command, "@PasswordHash", entity.PasswordHash);
                command.Parameters.AddWithValue(command, "@PhoneNumber", entity.PhoneNumber);
                command.Parameters.AddWithValue(command, "@UserTypeId", entity.UserTypeId);
                command.Parameters.AddWithValue(command, "@VendorId", entity.VendorId);
                command.Parameters.AddWithValue(command, "@Gender", entity.Gender);
                command.Parameters.AddWithValue(command, "@Buyer", entity.Buyer);
                command.Parameters.AddWithValue(command, "@Description", entity.Description);
                command.Parameters.AddWithValue(command, "@ERPUser", entity.ERPUser);
                command.Parameters.AddWithValue(command, "@UserToken", entity.UserToken.ToString());
                command.Parameters.AddWithValue(command, "@UserPhoto", entity.UserPhoto);
                command.Parameters.AddWithValue(command, "@SignaturePhoto", entity.SignaturePhoto);
                command.Parameters.AddWithValue(command, "@Inactive", entity.InActive);
                command.Parameters.AddWithValue(command, "@ChangeUserId", entity.UserId);
                command.Parameters.AddOutPutParameter(command, "@NewId");
                await command.ExecuteNonQueryAsync();
                var result = Convert.ToInt32(command.Parameters["@NewId"].Value);
                return result;
            }
        }

        public async Task<int> RegisterUserAsync(User entity)
        {
            string query = "SET NOCOUNT OFF Exec SP_AppUser_IUD @Id,@FullName,@ChangePassword,@Theme,@UserName,@Email,@PasswordHash,@PhoneNumber ,@UserTypeId,@VendorId,@UserToken,@Gender,@Buyer,@Description,@ERPUser,NULL,NULL,0,0,@NewId output";
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = query;
                command.Parameters.AddWithValue(command, "@Id", entity.Id);
                command.Parameters.AddWithValue(command, "@FullName", entity.FullName);
                command.Parameters.AddWithValue(command, "@ChangePassword", entity.ChangePassword);
                command.Parameters.AddWithValue(command, "@Theme", entity.Theme);
                command.Parameters.AddWithValue(command, "@UserName", entity.UserName);
                command.Parameters.AddWithValue(command, "@Email", entity.Email);
                command.Parameters.AddWithValue(command, "@PasswordHash", entity.PasswordHash);
                command.Parameters.AddWithValue(command, "@PhoneNumber", entity.PhoneNumber);
                command.Parameters.AddWithValue(command, "@UserTypeId", entity.UserTypeId);
                command.Parameters.AddWithValue(command, "@VendorId", entity.VendorId);
                command.Parameters.AddWithValue(command, "@Gender", entity.Gender);
                command.Parameters.AddWithValue(command, "@Buyer", entity.Buyer);
                command.Parameters.AddWithValue(command, "@Description", entity.Description);
                command.Parameters.AddWithValue(command, "@ERPUser", entity.ERPUser);
                command.Parameters.AddWithValue(command, "@UserToken", entity.UserToken.ToString());
                command.Parameters.AddOutPutParameter(command, "@NewId");
                await command.ExecuteNonQueryAsync();
                var result = Convert.ToInt32(command.Parameters["@NewId"].Value);
                return result;
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
                command.Parameters.AddWithValue(command, "@UserId", entity.UserId);
                command.Parameters.AddWithValue(command, "@FullName", entity.FullName);
                command.Parameters.AddWithValue(command, "@PhoneNumber", entity.PhoneNumber);
                command.Parameters.AddWithValue(command, "@PasswordHash", entity.PasswordHash);
                command.CommandText = query;
                //TODO: Handle Procedure Convert Error When Updateing User
                await command.ExecuteNonQueryAsync();
            }
        }
        public async Task<bool> UpdateUserTokenAsync(int userId, string token, DateTime expirationDate, int refreshTokenLifeTime)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SP_UPDATE_USER_REFRESH_TOKEN";//@USERID,@USERTOKEN";

                command.Parameters.AddWithValue(command, "@USERID", userId);
                command.Parameters.AddWithValue(command, "@USERTOKEN", token);
                command.Parameters.AddWithValue(command, "@EXPIRATION_DATE", expirationDate.AddSeconds(refreshTokenLifeTime));
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

        public async Task<List<ActiveUser>> GetActiveUsersWithoutCurrentUserAsync(int userId)
        {
            List<ActiveUser> activeUser = new List<ActiveUser>();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "Select * from [dbo].[FN_GET_USER_WITHOUT_CURRENT_USER](@UserId)";
                command.Parameters.AddWithValue(command, "@UserId", userId);
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
            return new() { UserId = reader.Get<int>("Id"), FullName = reader.Get<string>("FullName") };
        }

        public async Task<(int, List<UserMain>)> GetUserWFAAsync(int userId, int userStatus, int userType, int page, int limit)
        {
            List<UserMain> users = new List<UserMain>();
            int totalDataCount = 0;

            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "exec SP_UsersWFA @UserType, @UserStatus, @UserId, @Limit, @Page, @TotalDataCount OUTPUT";

                command.Parameters.AddWithValue(command, "@UserType", userType is -1 ? "%" : string.Join(',', userType));
                command.Parameters.AddWithValue(command, "@UserStatus", userStatus is -1 ? "%" : string.Join(',', userStatus));
                command.Parameters.AddWithValue(command, "@Limit", limit);
                command.Parameters.AddWithValue(command, "@Page", page);
                command.Parameters.AddWithValue(command, "@UserId", userId);
                var totalDataCountParam = new SqlParameter("@TotalDataCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(totalDataCountParam);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        users.Add(reader.GetByEntityStructure<UserMain>());
                    }
                }

                totalDataCount = (int)totalDataCountParam.Value;
            }
            return (totalDataCount, users);
        }

        public async Task<(int, List<UserMain>)> GetUserAllAsync(int userId, int userStatus, int userType, int page, int limit)
        {
            List<UserMain> users = new List<UserMain>();
            int totalDataCount = 0;
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "exec SP_UsersAll @UserType,@UserStatus,@UserId,@Limit,@Page, @TotalDataCount OUTPUT";

                command.Parameters.AddWithValue(command, "@UserType", userType is -1 ? "%" : string.Join(',', userType));
                command.Parameters.AddWithValue(command, "@UserStatus", userStatus is -1 ? "%" : string.Join(',', userStatus));
                command.Parameters.AddWithValue(command, "@Limit", limit);
                command.Parameters.AddWithValue(command, "@Page", page);
                command.Parameters.AddWithValue(command, "@UserId", userId);

                var totalDataCountParam = new SqlParameter("@TotalDataCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(totalDataCountParam);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        users.Add(reader.GetByEntityStructure<UserMain>());
                    }
                }

                totalDataCount = (int)totalDataCountParam.Value;
            }
            return (totalDataCount, users);
        }

        public async Task<(int, List<UserMain>)> GetUserCompanyAsync(int userId, int userStatus, int page, int limit)
        {
            List<UserMain> users = new List<UserMain>();
            int totalDataCount = 0;
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "exec SP_UsersCompany @UserStatus,@UserId,@Limit,@Page, @TotalDataCount OUTPUT";
                command.Parameters.AddWithValue(command, "@UserStatus", userStatus is -1 ? "%" : string.Join(',', userStatus));
                command.Parameters.AddWithValue(command, "@Limit", limit);
                command.Parameters.AddWithValue(command, "@Page", page);
                command.Parameters.AddWithValue(command, "@UserId", userId);

                var totalDataCountParam = new SqlParameter("@TotalDataCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(totalDataCountParam);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        users.Add(reader.GetByEntityStructure<UserMain>());
                    }
                }

                totalDataCount = (int)totalDataCountParam.Value;
            }
            return (totalDataCount, users);
        }

        public async Task<(int, List<UserMain>)> GetUserVendorAsync(int userId, int userStatus, int page, int limit)
        {
            List<UserMain> users = new List<UserMain>();
            int totalDataCount = 0;
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "exec SP_UsersVendor @UserStatus,@UserId,@Limit,@Page, @TotalDataCount OUTPUT";
                command.Parameters.AddWithValue(command, "@UserStatus", userStatus is -1 ? "%" : string.Join(',', userStatus));
                command.Parameters.AddWithValue(command, "@Limit", limit);
                command.Parameters.AddWithValue(command, "@Page", page);
                command.Parameters.AddWithValue(command, "@UserId", userId);
                var totalDataCountParam = new SqlParameter("@TotalDataCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(totalDataCountParam);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        users.Add(reader.GetByEntityStructure<UserMain>());
                    }
                }

                totalDataCount = (int)totalDataCountParam.Value;
            }
            return (totalDataCount, users);
        }

        public async Task<bool> UserChangeStatusAsync(int userId, UserChangeStatusModel model)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_UserApprove @Id,@Sequence,@ApproveStatus,@UserId,@Comment";

                command.Parameters.AddWithValue(command, "@Id", model.Id);
                command.Parameters.AddWithValue(command, "@Sequence", model.Sequence);
                command.Parameters.AddWithValue(command, "@ApproveStatus", model.ApproveStatus);
                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@Comment", model.Comment);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }


        public async Task<UserLoad> GetUserInfoAsync(int userId)
        {
            UserLoad user = new UserLoad();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "exec SP_UserLoad @Id";
                command.Parameters.AddWithValue(command, "@Id", userId);
                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    user = reader.GetByEntityStructure<UserLoad>();
                }
                return user;
            }
        }

        public async Task<List<ERPUser>> GetERPUser()
        {
            List<ERPUser> user = new List<ERPUser>();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "select * from VW_ERPUserList ";
                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    user.Add(reader.GetByEntityStructure<ERPUser>());
                }
                return user;
            }
        }

        public async Task<bool> ChangeUserPasswordAsync(ChangeUserPasswordModel passwordModel)
        {
            string query = "Exec [dbo].[SP_UserPassword_Change] @Id,@PasswordHash";
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.Parameters.AddWithValue(command, "@Id", passwordModel.UserId);
                command.Parameters.AddWithValue(command, "@PasswordHash", passwordModel.Password);
                command.CommandText = query;

                var value = await command.ExecuteNonQueryAsync();
                return value > 0;
            }
        }

        public async Task<bool> CheckTokenAsync(Guid authToken)
        {
            string query = "SET NOCOUNT OFF select [dbo].[SF_CheckToken1] (@UserToken) as 'ReturnInfo'";
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = query;
                command.Parameters.AddWithValue(command, "@UserToken", authToken);
                using var reader = await command.ExecuteReaderAsync();
                if (reader.Read())
                {
                    return reader.Get<bool>("ReturnInfo");
                }
                return false;
            }
        }

        public async Task<bool> SetUserEmailCode(string token, int id)
        {
            string query = "UPDATE Config.AppUser Set EmailVerificationToken = @token where id = @id";
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = query;
                command.Parameters.AddWithValue(command, "@token", token);
                command.Parameters.AddWithValue(command, "@id", id);
                var value = await command.ExecuteNonQueryAsync();
                return value > 0;
            }
        }

        public async Task<List<UsersByGroup>> GetUsersByGroupIdAsync(int groupId)
        {
            List<UsersByGroup> users = new List<UsersByGroup>();
            string query = "Exec [dbo].[SP_GroupUsers_Load] @groupId";
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.Parameters.AddWithValue(command, "@groupId", groupId);
                command.CommandText = query;

                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    users.Add(reader.GetByEntityStructure<UsersByGroup>());
                }
                return users;
            }
        }

        public async Task<bool> UpdateSessionAsync(int userId, int sessionCom)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_UserSession_U @UserId,@Command";

                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@Command", sessionCom);
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> UserChangeStatusAsync(int userId, DataTable data)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_UserChangeStatusBulk @dataTable,@userId";
                command.Parameters.AddWithValue(command, "@userId", userId);

                command.Parameters.Add("@dataTable", SqlDbType.Structured).Value = data;
                command.Parameters["@dataTable"].TypeName = "UserChangeStatus";
                var value = await command.ExecuteNonQueryAsync();
                return value > 0;
            }
        }

        public async Task<bool> AddGroupToUserAsync(DataTable data, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_UsersGroupsBulk_I @UserId,@GroupId";
                command.Parameters.AddWithValue(command, "@UserId", userId);

                command.Parameters.Add("@GroupId", SqlDbType.Structured).Value = data;
                command.Parameters["@GroupId"].TypeName = "SingleIdItems";
                var value = await command.ExecuteNonQueryAsync();
                return value > 0;
            }
        }

        public async Task<bool> DeleteGroupFromUserAsync(DataTable data, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_UsersGroupBulk_D @UserId,@GroupId";
                command.Parameters.AddWithValue(command, "@UserId", userId);

                command.Parameters.Add("@GroupId", SqlDbType.Structured).Value = data;
                command.Parameters["@GroupId"].TypeName = "SingleIdItems";
                var value = await command.ExecuteNonQueryAsync();
                return value > 0;
            }
        }

        public Task CheckRefreshTokenIsValid(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddAsync(User entity)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
