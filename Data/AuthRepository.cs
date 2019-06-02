using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            this._context = context;
        }

        public async Task<User> Login(string username, string password)
        {

            var user = await _context.Users.Include(p => p.photos).FirstOrDefaultAsync(usr => usr.username.Equals(username));

            if (user == null)
            {
                return null;
            }

            if (!VerifyPasswordHash(password, user.passwordHash, user.passwordSalt))
            {
                return null;
            }

            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmach = new HMACSHA512(passwordSalt))
            {
              
                Encoding utf8 = Encoding.UTF8;
                var computedHash = hmach.ComputeHash(utf8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public async Task<User> Registeruser(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(password,out passwordHash, out passwordSalt);
            user.passwordHash = passwordHash;
            user.passwordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;


        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmach = new HMACSHA512())
            {
                passwordSalt = hmach.Key;
                Encoding utf8 = Encoding.UTF8;
                passwordHash = hmach.ComputeHash(utf8.GetBytes(password));
            }
            
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(x =>  x.username.Equals(username)))
            {
                return true;
            }

            return false;
        }
    }
}
