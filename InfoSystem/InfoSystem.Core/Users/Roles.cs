using System;
using System.Collections.Generic;
using System.Text;

namespace InfoSystem.Core.Users
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Supervisor = "Supervisor";
        public const string Redactor = "Redactor";
        public const string Organizer = "Organizer";
        public const string Paparazi = "Paparazi";

        public static string[] GetAll()
        {
            return new[]
            {
                Admin, Redactor, Supervisor, Organizer, Paparazi
            };
        }
    }
}
