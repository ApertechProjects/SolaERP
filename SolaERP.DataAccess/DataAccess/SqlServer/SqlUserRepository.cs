using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Repositories;
using SolaERP.Infrastructure.UnitOfWork;
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

            string query = @"Exec [dbo].[SP_User_insert] @FullName,@StatusId,@UserName,@Email ,@EmailConfirmed ,@PasswordHash,@UserTypeId,@UserToken";

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
                command.Parameters.AddWithValue(command, "@UserToken", entity.UserToken);

                var result = command.ExecuteNonQuery() == 0 ? false : true;
                command.Connection.Close();
                return result;
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
                command.Connection.Close();
                return users;
            }
        }
        public User GetByEmail(string email)
        {
            User user = new User();
            using (var command = _unitOfWork.CreateCommand())
            {
                command.CommandText = "EXEC SP_GETUSER_BY_EMAIL @Email";
                IDbDataParameter dbDataParameter = command.CreateParameter();
                dbDataParameter.ParameterName = "@Email";
                dbDataParameter.Value = email;
                command.Parameters.Add(dbDataParameter);

                using var reader = command.ExecuteReader();

                if (reader.Read())
                    user = reader.GetByEntityStructure<User>();

                command.Connection.Close();
                return user;
            }
        }
        public async Task<User> GetByIdAsync(int id)
        {
            User user = null;
            return await Task.Run(() =>
            {
                using (var command = _unitOfWork.CreateCommand())
                {
                    command.CommandText = "EXEC SP_GETUSER_BY_NAME_OR_ID @Id";
                    IDbDataParameter dbDataParameter = command.CreateParameter();
                    dbDataParameter.ParameterName = "@Id";
                    dbDataParameter.Value = id;
                    command.Parameters.Add(dbDataParameter);

                    using var reader = command.ExecuteReader();

                    if (reader.Read())
                        user = reader.GetByEntityStructure<User>();

                    command.Connection.Close();
                    return user;
                }
            });
        }
        public async Task<User> GetByUserNameAsync(string userName)
        {
            User user = null;
            return await Task.Run(() =>
            {
                using (var command = _unitOfWork.CreateCommand())
                {
                    command.CommandText = "EXEC SP_GETUSER_BY_NAME_OR_ID NULL,@UserName";
                    IDbDataParameter dbDataParameter = command.CreateParameter();
                    dbDataParameter.ParameterName = "@UserName";
                    dbDataParameter.Value = userName;
                    command.Parameters.Add(dbDataParameter);

                    using var reader = command.ExecuteReader();

                    if (reader.Read())
                        user = reader.GetByEntityStructure<User>();

                    command.Connection.Close();
                    return user;
                }

            });
        }
        public void Remove(User entity)
        {
            using (var command = _unitOfWork.CreateCommand())
            {
                command.CommandText = "Delete from Config.AppUser Where Id = @Id";
                IDbDataParameter dbDataParameter = command.CreateParameter();
                dbDataParameter.ParameterName = "@Id";
                dbDataParameter.Value = entity.Id;
                command.Parameters.Add(dbDataParameter);

                command.ExecuteNonQuery();
                command.Connection.Close();
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
                command.Connection.Close();
            }
        }

    }
}
