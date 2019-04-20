using System;
using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IAuthRepository
    {
        // register user 
        Task<User> Registeruser(User user, string password);

        //login user to API
        Task<User> Login(string username, string password);

        // user exists 
        Task<Boolean> UserExists(string username);
    }
}
