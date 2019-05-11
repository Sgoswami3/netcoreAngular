using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using DatingApp.API.Models;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
    {
        private readonly DataContext _context;

        public Seed(DataContext context)

        {
            this._context = context;
        }

        public void Seeduser()
        {
            var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);
            foreach (var user in users)
            {
                byte[] passwordHash, passwordSalt;

                CreatePasswordHash("passowrd", out passwordHash, out passwordSalt);
                user.passwordHash = passwordHash;
                user.passwordSalt = passwordSalt;
                user.username = user.username.ToLower();
                _context.Users.Add(user);
            }

            _context.SaveChanges();
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
    }
}
