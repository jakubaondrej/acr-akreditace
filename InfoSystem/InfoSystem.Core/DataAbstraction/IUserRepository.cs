using InfoSystem.Core.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem.Core.DataAbstraction
{
    public interface IUserService
    {
        Task<UserLogin> GetUserByUserName(string username);
        Task<string> CreateUser(string username, string password);
        Task<string> CreateUser(UserCreation user);
        Task UpdateGoogleDriveDirection(string id, string dir);
        Task<UserLogin> GetUserById(string id);
        Task ChangePassword(string id, string hashNewPassword);
    }
    public class UserLogin
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }
    }

    public class UserListing
    {
        public string Id { get; set; }
        public string Username { get; set; }
    }

    public class UserChangePassword
    {
        public string Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
