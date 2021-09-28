using InfoSystem.Core.DataAbstraction;
using System;
using System.Threading.Tasks;

namespace InfoSystem.Core.Users
{
    public class UserService
    {
        private IUserService _userRepository;
        public UserService(IUserService userService)
        {
            _userRepository = userService;
        }
        public async Task<string> Login(string username, string password)
        {
            //todo validation (trim)
            var user = await _userRepository.GetUserByUserName(username);
            if (user == null)
                return String.Empty;

            var hashPassword = HashPassword(password);
            if (user.Password == hashPassword)
                return user.Id;
            else
                return String.Empty;
        }

        public async Task<string> Registration(string username, string password)
        {
            var user = await _userRepository.GetUserByUserName(username);
            if (user != null)
            {
                throw new Exception($"{nameof(username)} {username} already exists!");
            }
            var hashPassword = HashPassword(password);
            return await _userRepository.CreateUser(username, password);
        }

        private string HashPassword(string password)
        {
            if (String.IsNullOrEmpty(password))
                return String.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }

        public async Task<string> CreateUser(UserCreation userCreation)
        {
            var user = await _userRepository.GetUserByUserName(userCreation.Username);
            if (user != null)
            {
                throw new Exception($"{nameof(userCreation.Username)} {userCreation.Username} already exists!");
            }
            var hashPassword = HashPassword(userCreation.Password);
            userCreation.Password = hashPassword;
            return await _userRepository.CreateUser(userCreation);
        }

        public async Task UpdateGoogleDriveDirection(string id, string dir)
        {
            await _userRepository.UpdateGoogleDriveDirection(id, dir);
        }

        public async Task<bool> ChangePassword(UserChangePassword data)
        {
            var user = await _userRepository.GetUserById(data.Id);
            if (user == null)
                return false;
            var hashOldPassword = HashPassword(data.OldPassword);

            if (user.Password == hashOldPassword)
            {
                var hashNewPassword = HashPassword(data.NewPassword);
                await _userRepository.ChangePassword(user.Id, hashNewPassword);
                return true;
            }
            else
                return false;
        }
    }
}
