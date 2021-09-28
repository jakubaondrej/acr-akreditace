using InfoSystem.Core.DataAbstraction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem.Core.Sports
{
    public class SportService
    {
        private ISportRepository _sportRepository;
        public SportService(ISportRepository sportRepository)
        {
            _sportRepository = sportRepository;
        }

        public async Task<int> Create(SportCreateData data)
        {
            var existSport = await _sportRepository.GetSportByName(data.Name);
            if (existSport != null)
            {
                throw new Exception($"Sport {nameof(data.Name)} '{data.Name}' already exists!");
            }
            return await _sportRepository.CreateSport(data);
        }

        public async Task<int> Edit(int id, SportCreateData data)
        {
            var existSport = await _sportRepository.GetSportById(id);
            if (existSport == null)
            {
                throw new Exception($"Sport does not exist!");
            }
            return await _sportRepository.EditSport(id, data);
        }

        public async Task<SportDetail> GetSportById(int id)
        {
            return await _sportRepository.GetSportById(id);
        }

        public async Task<List<SportListing>> GetAllSports()
        {
            return await _sportRepository.GetAllSports();
        }

        public async Task Delete(int id)
        {
            await _sportRepository.Delete(id);
        }

        public async Task<List<SportListing>> GetSportsBySeasonId(int seasonId)
        {
            return await _sportRepository.GetSportsBySeasonId(seasonId);
        }
    }
}
