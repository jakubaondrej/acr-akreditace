using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem.Core.DataAbstraction
{
    public interface IUserAccessRequestRepository
    {
        Task<UserAccessRequestDetail> GetUserAccessRequestByEmail(string email);
        Task<UserAccessRequestDetail> GetUserAccessRequestByUsername(string username);
        Task<int> CreateUserAccessRequest(UserAccessRequestCreateData data);
        Task<UserAccessRequestDetail> GetUserAccessRequestById(int id);
        Task<IEnumerable<UserAccessRequestListing>> GetAllUserAccessRequests();
        Task RemoveUserAccessRequestsById(int id);
    }
    public class UserAccessRequestCreateData
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string CallNumber { get; set; }
        public string Redaction { get; set; }
        public string Note { get; set; }
    }

    public class UserAccessRequestListing
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
    public class UserAccessRequestDetail
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string CallNumber { get; set; }
        public string Redaction { get; set; }
        public string Note { get; set; }
    }
}
