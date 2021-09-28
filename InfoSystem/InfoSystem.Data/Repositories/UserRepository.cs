using InfoSystem.Core.DataAbstraction;
using InfoSystem.Core.Users;
using InfoSystem.Data.Entities;
using InfoSystem.Web.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem.Data.Repositories
{
    public class UserRepository : RepositoryBase, IUserService
    {
        public UserRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
        }

        public async Task ChangePassword(string id, string hashNewPassword)
        {
            var user = await Db.User
                .Where(u => u.Id == id)
                .SingleOrDefaultAsync();
            user.PasswordHash = hashNewPassword;
            Db.User.Update(user);
            await Db.SaveChangesAsync();
        }

        public Task<string> CreateUser(string username, string password)
        {
            //var user = new User
            throw new NotImplementedException();
        }

        public async Task<string> CreateUser(UserCreation user)
        {
            var u = new User()
            {
                UserName = user.Username,
                Email = user.Email,
                Note = user.Note,
                PasswordHash = user.Password,
                PhoneNumber = user.PhoneNumber,
                RedactionId = user.RedactionId,
            };
            Db.User.Add(u);
            await Db.SaveChangesAsync();
            return u.Id;
        }

        public async Task<UserLogin> GetUserById(string id)
        {
            var user = await Db.User
                .Where(u => u.Id == id)
                .Select(u => new UserLogin()
                {
                    Id = u.Id,
                    Password = u.PasswordHash,
                    Username = u.UserName,
                })
                .SingleOrDefaultAsync();
            return user;
        }

        public async Task<UserLogin> GetUserByUserName(string username)
        {
            var user = await Db.User
                .Where(u => u.UserName.ToLower() == username.Trim().ToLower())
                .Select(u => new UserLogin()
                {
                    Id = u.Id,
                    Password = u.PasswordHash,
                    Username = u.UserName,
                })
                .SingleOrDefaultAsync();
            return user;
        }


        public async Task UpdateGoogleDriveDirection(string id, string dir)
        {
            var user = await Db.User
                .Where(u => u.Id == id)
                .SingleOrDefaultAsync();
            user.GoogleDriveDirectoryId = dir;
            Db.User.Update(user);
            await Db.SaveChangesAsync();
        }
    }
}
