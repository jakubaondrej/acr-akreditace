using System;
using System.Collections.Generic;
using System.Text;

namespace InfoSystem.Core.Users
{
    public class UserCreation
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public string UserRole { get; set; }
        public int RedactionId { get; set; }
    }
}
