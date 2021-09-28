using InfoSystem.Core.DataAbstraction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem.Core.Seasons
{
    public class SeasonService
    {
        private ISeasonRepository _seasonRepository;
        public SeasonService(ISeasonRepository seasonRepository)
        {
            _seasonRepository = seasonRepository;
        }

        public async Task<int> Create(SeasonCreateData data)
        {
            var existSeason = await _seasonRepository.GetSeasonByYear(data.Year);
            if (existSeason != null)
            {
                throw new Exception($"Season {nameof(data.Year)} '{data.Year}' already exists!");
            }
            return await _seasonRepository.CreateSeason(data);
        }

        public async Task<int> Edit(int id, SeasonCreateData data)
        {
            var existSeason = await _seasonRepository.GetSeasonById(id);
            if (existSeason == null)
            {
                throw new Exception($"Season does not exist!");
            }
            return await _seasonRepository.EditSeason(id, data);
        }

        public async Task<SeasonDetails> GetSeasonById(int id)
        {
            return await _seasonRepository.GetSeasonById(id);
        }

        public async Task<List<SeasonListing>> GetAllSeasons()
        {
            return await _seasonRepository.GetAllSeasons();
        }

        public async Task Delete(int id)
        {
            await _seasonRepository.Delete(id);
        }

        public async Task<SeasonDetails> GetSeasonByYear(int year)
        {
            return await _seasonRepository.GetSeasonByYear(year);
        }

    }
}
