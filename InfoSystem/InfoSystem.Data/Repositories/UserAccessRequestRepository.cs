using InfoSystem.Core.DataAbstraction;
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
    public class UserAccessRequestRepository : RepositoryBase, IUserAccessRequestRepository
    {
        public UserAccessRequestRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {

        }
        public async Task<int> CreateUserAccessRequest(UserAccessRequestCreateData data)
        {
            var uar = new UserAccessRequest()
            {
                CallNumber = data.CallNumber,
                Email = data.Email,
                Note = data.Note,
                Redaction = data.Redaction,
                Username = data.Username
            };
            Db.UserAccessRequest.Add(uar);
            return await Db.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserAccessRequestListing>> GetAllUserAccessRequests()
        {
            return await Db.UserAccessRequest
                .Select(u=>new UserAccessRequestListing() 
                {
                    Username = u.Username,
                    Email = u.Email,
                    Id=u.UserAccessRequestId
                })
                .ToListAsync();
        }

        public async Task<UserAccessRequestDetail> GetUserAccessRequestByEmail(string email)
        {
            return await Db.UserAccessRequest
                .Where(u => u.Email.ToLower() == email.Trim().ToLower())
                .Select(u=> new UserAccessRequestDetail()
                {
                    Email = u.Email,
                    CallNumber=u.CallNumber,
                    Id =u.UserAccessRequestId,
                    Note =u.Note,
                    Redaction=u.Redaction,
                    Username=u.Username
                })
                .FirstOrDefaultAsync();
        }

        public async Task<UserAccessRequestDetail> GetUserAccessRequestById(int id)
        {
            return await Db.UserAccessRequest
                .Where(u => u.UserAccessRequestId == id)
                .Select(u => new UserAccessRequestDetail()
                {
                    Email = u.Email,
                    CallNumber = u.CallNumber,
                    Id = u.UserAccessRequestId,
                    Note = u.Note,
                    Redaction = u.Redaction,
                    Username = u.Username
                })
                .FirstOrDefaultAsync();
        }

        public async Task<UserAccessRequestDetail> GetUserAccessRequestByUsername(string username)
        {
            return await Db.UserAccessRequest
                .Where(u => u.Username.ToLower() == username.Trim().ToLower())
                .Select(u => new UserAccessRequestDetail()
                {
                    Email = u.Email,
                    CallNumber = u.CallNumber,
                    Id = u.UserAccessRequestId,
                    Note = u.Note,
                    Redaction = u.Redaction,
                    Username = u.Username
                })
                .FirstOrDefaultAsync();
        }

        public async Task RemoveUserAccessRequestsById(int id)
        {
            var itemToRemove = await Db.UserAccessRequest.SingleOrDefaultAsync(x => x.UserAccessRequestId == id);

            if (itemToRemove != null)
            {
                Db.UserAccessRequest.Remove(itemToRemove);
                await Db.SaveChangesAsync();
            }
        }
    }
}
