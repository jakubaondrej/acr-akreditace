using InfoSystem.Core.DataAbstraction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem.Core.Accreditations
{
    public class AccreditationService
    {
        private readonly IAccreditationRepository _accreditationRepository;

        public AccreditationService(IAccreditationRepository accreditationRepository)
        {
            _accreditationRepository = accreditationRepository;
        }

        public async Task<List<AccreditationListing>> GetAll()
        {
            return await _accreditationRepository.GetAllAccreditations();
        }

        public async Task<AccreditationDetails> GetById(int id)
        {
            return await _accreditationRepository.GetAccreditationById(id);
        }

        public async Task<List<AccreditationListing>> GetAccreditationsByCompetitionSeasonId(int competitionSeasonId)
        {
            return await _accreditationRepository.GetAccreditationsByCompetitionSeasonId(competitionSeasonId);
        }

        public async Task CreateAccreditation(AccreditationCreateData data)
        {
            var exist = await _accreditationRepository.GetAccreditationByCompetitionSeasonIdAndUserId(data.CompetitionSeasonId,data.UserId);
            if (exist != null)
            {
                throw new Exception($"Accreditation already exists! UserName = {exist.User.Username}, State = {exist.State}");
            }
            await _accreditationRepository.CreateAccreditation(data);
        }

        public async Task<AccreditationDetails> GetAccreditationByCompetitionSeasonIdAndUserId(int competitionSeasonId, string userId)
        {
            return await _accreditationRepository.GetAccreditationByCompetitionSeasonIdAndUserId(competitionSeasonId,userId);
        }

        public async Task AcceptAccreditationById(int id)
        {
            await _accreditationRepository.AcceptAccreditationById(id);
        }

        public async Task EditAccreditation(int id, AccreditationEditation data)
        {
            await _accreditationRepository.EditAccreditation(id,data);
        }
    }
}
