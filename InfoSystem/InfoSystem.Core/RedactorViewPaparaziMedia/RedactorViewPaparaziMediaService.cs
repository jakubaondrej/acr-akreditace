using InfoSystem.Core.DataAbstraction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem.Core.RedactorViewPaparaziMedia
{
    public class RedactorViewPaparaziMediaService
    {
        private IRedactorViewPaparaziMediaRepository _redactorViewPaparaziMediaRepository;
        public RedactorViewPaparaziMediaService(IRedactorViewPaparaziMediaRepository redactorViewPaparaziMediaRepository)
        {
            _redactorViewPaparaziMediaRepository = redactorViewPaparaziMediaRepository;
        }

        public async Task CreateNew(string userId, int competitionSeasonId)
        {
            await _redactorViewPaparaziMediaRepository.CreateNew(userId, competitionSeasonId);
        }

        public async Task<List<RedactorViewPaparaziMediaListing>> GetAll()
        {
            return await _redactorViewPaparaziMediaRepository.GetAll();
        }

        public async Task ChangeStatusById(int id, string status)
        {
            await _redactorViewPaparaziMediaRepository.ChangeStatusById(id, status);
        }

        public async Task<bool> HasAcceptedRequest(int competitionSeasonId, string userId)
        {
            return await _redactorViewPaparaziMediaRepository.HasAcceptedRequest(competitionSeasonId,userId);
        }
    }
}
