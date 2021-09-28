using InfoSystem.Core.DataAbstraction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem.Core.CompetitionSeasons
{
    public class CompetitionSeasonService
    {
        private readonly ICompetitionSeasonRepository _competitionSeasonRepository;

        public CompetitionSeasonService(ICompetitionSeasonRepository competitionSeasonRepository)
        {
            _competitionSeasonRepository = competitionSeasonRepository;
        }

        //public async Task<List<CompetitionSeasonListing>> GetAll()
        //{
        //    //return await _competitionSeasonRepository.GetAllCompetitionSeasons();
        //}

        public async Task<CompetitionSeasonDetails> GetById(int id)
        {
            return await _competitionSeasonRepository.GetCompetitionSeasonById(id);
        }

        public async Task<int> Create(CompetitionSeasonCreateData data)
        {
            var exist = await _competitionSeasonRepository.GetCompetitionSeasonBySeasonAndCompetitionId(data.SeasonId,data.CompetitionId);
            if (exist != null)
            {
                throw new Exception($"Season {exist.Season.Year} already exists for Competition {exist.Competition.Name}!");
            }
            return await _competitionSeasonRepository.CreateCompetitionSeason(data);
        }

        public async Task<int> Edit(int id, CompetitionSeasonCreateData data)
        {
            var exist = await _competitionSeasonRepository.GetCompetitionSeasonBySeasonAndCompetitionId(data.SeasonId, data.CompetitionId);
            if (exist == null)
            {
                throw new Exception($"Season for competition does not exist! Id = {id}");
            }
            return await _competitionSeasonRepository.EditCompetitionSeason(id, data);
        }

        public async Task DeleteById(int id)
        {
            await _competitionSeasonRepository.DeleteById(id);
        }

        public async Task<List<CompetitionSeasonListing>> GetByCompetitionId(int competitionId)
        {
            return await _competitionSeasonRepository.GetByCompetitionId(competitionId);
        }

        public async Task<CompetitionSeasonSelection> GetSelectionByCompetitionId(int competitionId)
        {
            return await _competitionSeasonRepository.GetSelectionByCompetitionId(competitionId);
        }

        public async Task<CompetitionSeasonAccreditation> GetCompetitionSeasonAccreditationById(int competitionSeasonId, string userId)
        {
            return await _competitionSeasonRepository.GetCompetitionSeasonAccreditationByCompetitionSeasonIdAndUserId(competitionSeasonId, userId);
        }

        public async Task<List<CompetitionSeasonListing>> GetNextCompetitionSeasons()
        {
            return await _competitionSeasonRepository.GetNextCompetitionSeason();
        }
    }
}
