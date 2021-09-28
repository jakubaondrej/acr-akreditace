using InfoSystem.Core.DataAbstraction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem.Core.UserAccessRequests
{
    public class UserAccessRequestService
    {
        private IUserService _userRepository;
        private IUserAccessRequestRepository _uarRepository;
        public UserAccessRequestService(IUserService userService, IUserAccessRequestRepository userAccessRequestRepository)
        {
            _userRepository = userService;
            _uarRepository = userAccessRequestRepository;
        }

        public async Task<int> Create(UserAccessRequestCreateData data)
        {
            var existData = await _uarRepository.GetUserAccessRequestByEmail(data.Email);
            if (existData != null)
            {
                throw new Exception($"{nameof(data.Email)} {data.Email} is already using.");
            }
            existData = await _uarRepository.GetUserAccessRequestByUsername(data.Username);
            if (existData != null)
            {
                throw new Exception($"{nameof(data.Username)} {data.Username} is already using.");
            }
            return await _uarRepository.CreateUserAccessRequest(data);
        }

        public async Task<UserAccessRequestDetail> GetUserAccessRequestById(int id)
        {
            return await _uarRepository.GetUserAccessRequestById(id);
        }

        public async Task<IEnumerable<UserAccessRequestListing>> GetAllUserAccessRequests()
        {
            return await _uarRepository.GetAllUserAccessRequests();
        }

        public async Task RemoveUserAccessRequestsById(int id)
        {
            var existData = await _uarRepository.GetUserAccessRequestById(id);
            if (existData == null)
            {
                throw new Exception($"Object UserAccessRequests with ID = {id} does not esist ");
            }
            await _uarRepository.RemoveUserAccessRequestsById(id);
        }
    }
}
