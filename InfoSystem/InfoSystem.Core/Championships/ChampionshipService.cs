using InfoSystem.Core.DataAbstraction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem.Core.Championships
{
    public class ChampionshipService
    {
        private IChampionshipRepository _championchipRepository;
        public ChampionshipService(IChampionshipRepository championchipRepository)
        {
            _championchipRepository = championchipRepository;
        }

        public async Task<int> Create(ChampionshipCreateData data)
        {
            var existChampionship = await _championchipRepository.GetChampionshipByName(data.Name);
            if (existChampionship != null)
            {
                throw new Exception($"Championship {nameof(data.Name)} '{data.Name}' already exists!");
            }
            return await _championchipRepository.CreateChampionship(data);
        }

        public async Task<int> Edit(int id, ChampionshipCreateData data)
        {
            var existSeason = await _championchipRepository.GetChampionshipById(id);
            if (existSeason == null)
            {
                throw new Exception($"Championship does not exist!");
            }
            return await _championchipRepository.EditChampionship(id, data);
        }

        public async Task<ChampionshipDetails> GetChampionshipById(int id)
        {
            return await _championchipRepository.GetChampionshipById(id);
        }

        public async Task<List<ChampionshipListing>> GetAllChampionships()
        {
            return await _championchipRepository.GetAllChampionships();
        }

        public async Task Delete(int id)
        {
            await _championchipRepository.Delete(id);
        }

        public async Task<ChampionshipSelection> GetChampionshipSelectionBySportId(int id)
        {
            return await _championchipRepository.GetChampionshipSelectionBySportId(id);
        }
    }
}
