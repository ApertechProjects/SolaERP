using SolaERP.DataAccess.Abstract;
using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Entities.Auth;

namespace SolaERP.DataAccess.DataAcces.Implementation
{
    public class SqlUserRepository : IUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SqlUserRepository(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }


        public Task AddAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAllAsync()
        {
            using (var command = _unitOfWork.CreateCommand())
            {
                command.CommandText = "Select * from Config.AppUser";
                var reader = command.ExecuteReader();

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

        public Task<User> GetById(int id)
        {
            throw new NotImplementedException();
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
