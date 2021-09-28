using System;
using System.Collections.Generic;
using System.Text;

namespace InfoSystem.Core.Accreditations
{
    public static class AccreditationStates
    {
        public const string Accepted = "Accepted";
        public const string Apologized = "Apologized";
        public const string Rejected = "Rejected";
        public const string InProcess = "In process";

        public static string[] GetAll()
        {
            return new[]
            {
                Accepted, Apologized, Rejected, InProcess
            };
        }
    }
}
