using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.DataAccess.DataAcces.SqlServer
{
    public class SqlUserRepository : IUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlUserRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddAsync(User entity)
        {
            string query = "Exec SP_User_insert @FullName,@StatusId,@UserName,@Email ,@EmailConfirmed ,@PasswordHash,@UserTypeId,@UserToken";
            var result = await Task.Run(() =>
            {
                using (var command = _unitOfWork.CreateCommand())
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

                    var value = command.ExecuteNonQuery();
                    return value == 0 || value == -1 ? false : true;
                }
            });
            return result;
        }
        public async Task<User> GetByUserId(int userId)
        {
            var result = await Task.Run(() =>
            {
                User user = null;
                using (var command = _unitOfWork.CreateCommand())
                {
                    command.CommandText = "Select * from Config.AppUser where Id = @Id";
                    command.Parameters.AddWithValue(command, "@Id", userId);

                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        user = reader.GetByEntityStructure<User>();
                    }
                    return user;
                }
            });
            return result;
        }
        public async Task<List<User>> GetAllAsync()
        {
            var result = await Task.Run(() =>
            {
                using (var command = _unitOfWork.CreateCommand())
                {
                    command.CommandText = "Select * from Config.AppUser where IsDeleted = 0";
                    using var reader = command.ExecuteReader();

                    List<User> users = new List<User>();
                    while (reader.Read())
                    {
                        users.Add(reader.GetByEntityStructure<User>());
                    }
                    return users;
                }
            });
            return result;
        }
        public async Task<User> GetByEmailAsync(string email)
        {
            var result = await Task.Run(() =>
            {
                User user = null;
                using (var command = _unitOfWork.CreateCommand())
                {
                    command.CommandText = "EXEC SP_GET_USER_BY_EMAIL @Email";
                    command.Parameters.AddWithValue(command, "@Email", email);

                    using var reader = command.ExecuteReader();

                    if (reader.Read())
                        user = reader.GetByEntityStructure<User>();

                    return user;
                }
            });
            return result;

        }
        public async Task<User> GetByIdAsync(int id)
        {
            var result = await Task.Run(() =>
            {
                User user = null;
                using (var command = _unitOfWork.CreateCommand())
                {
                    command.CommandText = "EXEC SP_GETUSER_BY_NAME_OR_ID @Id";
                    command.Parameters.AddWithValue(command, "@Id", id);

                    using var reader = command.ExecuteReader();

                    if (reader.Read())
                        user = reader.GetByEntityStructure<User>();

                    return user;
                }
            });
            return result;
        }
        public async Task<User> GetByUserNameAsync(string userName)
        {
            var result = await Task.Run(() =>
            {
                User user = null;
                using (var command = _unitOfWork.CreateCommand())
                {
                    command.CommandText = "EXEC SP_GETUSER_BY_NAME_OR_ID NULL,@UserName";
                    command.Parameters.AddWithValue(command, "@UserName", userName);

                    using var reader = command.ExecuteReader();
                    if (reader.Read())
                        user = reader.GetByEntityStructure<User>();

                    return user;
                }
            });
            return result;
        }
        public async Task<User> GetLastInsertedUserAsync()
        {
            var result = await Task.Run(() =>
            {
                using (var command = _unitOfWork.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM GET_LAST_INSERTED_USER";
                    using var reader = command.ExecuteReader();

                    User user = new User();
                    if (reader.Read())
                    {
                        user = reader.GetByEntityStructure<User>();
                    }
                    return user;
                }
            });
            return result;
        }
        public bool Remove(int Id)
        {
            using (var command = _unitOfWork.CreateCommand())
            {
                command.CommandText = "SP_DELETE_USER @Id";
                command.Parameters.AddWithValue(command, "@Id", Id);
                var value = command.ExecuteNonQuery();

                return value == 0 || value == -1 ? false : true;
            }
        }
        public void Update(User entity)
        {
            string query = "Exec [dbo].[SP_UserData_U] @UserId,@FullName,@Position,@PhoneNumber,@Photo";
            using (var command = _unitOfWork.CreateCommand())
            {
                command.Parameters.AddWithValue(command, "@UserId", entity.Id);
                command.Parameters.AddWithValue(command, "@FullName", entity.FullName);
                command.Parameters.AddWithValue(command, "@Position", entity.Position);
                command.Parameters.AddWithValue(command, "@PhoneNumber", entity.PhoneNumber);
                command.Parameters.AddWithValue(command, "@Photo", entity.Photo);
                command.CommandText = query;

                command.ExecuteNonQuery();
            }
        }
    }
}
