using InfoSystem.Core.DataAbstraction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem.Core.Competitions
{
    public class CompetitionService
    {
        ICompetitionRepository _competitionRepository;
        public CompetitionService(ICompetitionRepository competitionRepository)
        {
            _competitionRepository = competitionRepository;
        }

        public async Task<List<CompetitionListing>> GetAll()
        {
            return await _competitionRepository.GetAllCompetitions();
        }
        
        public async Task<CompetitionDetails> GetById(int id)
        {
            return await _competitionRepository.GetCompetitionById(id);
        }

        public async Task<int> Create(CompetitionCreateData data) 
        {
            var existCompetition = await _competitionRepository.GetCompetitionByName(data.Name);
            if (existCompetition != null)
            {
                throw new Exception($"Competition {nameof(data.Name)} '{data.Name}' already exists!");
            }
            return await _competitionRepository.CreateCompetition(data);
        }

        public async Task<int> Edit(int id, CompetitionCreateData data) 
        {
            var existCompetition = await _competitionRepository.GetCompetitionById(id);
            if (existCompetition == null)
            {
                throw new Exception($"Competition does not exist!");
            }
            return await _competitionRepository.EditCompetition(id, data);
        }

        public async Task DeleteById(int id) 
        {
            await _competitionRepository.DeleteById(id);
        }

        public async Task<CompetitionSelection> GetCompetitionSelectionByChampionshipId(int id)
        {
            return await _competitionRepository.GetCompetitionSelectionByChampionshipId(id);
        }
    }
}
