using System;
using System.Collections.Generic;
using System.Text;

namespace InfoSystem.Core.Users
{
    public static class RedactorType
    {
        public const string Press = "Press";
        public const string Photo = "Photo";
        public const string TV = "TV";

        public static string[] GetAll() => new[]
            {
                Press,Photo,TV
            };
    }
}
