using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DatingApp1.Model;
using Microsoft.EntityFrameworkCore;


namespace DatingApp1.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext Context)
        {
            _context = Context;
        }
        public async Task<User> Login(string userName, string password)
        {
         var user=await  _context.Users.FirstOrDefaultAsync(u=>u.UserName==userName);
            if (user == null)
                return null;
            if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
            
               var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i=0; i<computedHash.Length;i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
                return true;

            }
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public  async Task<bool> UserExist(string userName)
        {
            if (await _context.Users.AnyAsync(x => x.UserName == userName))
                return true;
            return false;
        }

        private void CreatePasswordHash(string password,out byte[] passwordHash, out byte[] salt)
        {
            using(var hmac=new HMACSHA512())
            {
                salt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                

            }
        }
    }
}
