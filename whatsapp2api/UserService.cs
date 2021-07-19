using System;
using whatsapp2api.Models;

namespace whatsapp2api
{
    public interface IUserService
    {
        bool Authenticate(string email, string password);
        UserEntity Update(UserEntity updatedUser);
    }

    public class UserService : IUserService
    {
        public bool Authenticate(string email, string password)
        {
            throw new NotImplementedException();
        }

        public UserEntity Update(UserEntity updatedUser)
        {
            throw new NotImplementedException();
        }
    }
}