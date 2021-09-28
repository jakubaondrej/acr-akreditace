using InfoSystem.Core.DataAbstraction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem.Core.Drivers
{
    public class DriverService
    {
        private IDriverRepository _driverRepository;
        public DriverService(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task<List<DriverListing>> GetAll()
        {
            return await _driverRepository.GetAll();
        }

        public async Task<DriverDetail> GetById(int id)
        {
            return await _driverRepository.GetById(id);
        }

        public async Task<int> Create(CreateDriver driver) //how to?? .. .tak!!!!
        {
            var data = new DriverCreateData()
            {
                Car = driver.Car,
                CoDriverName = driver.CoDriverName,
                Name = driver.Name
            };
            return await _driverRepository.Create(data);
        }

        public async Task EditById(int id) //how to??
        {
            //todo
            throw new Exception();
        }

        public async Task DeleteById(int id) //how to.. todo: udělat jen nějakou proměnou stavu, nebo rovnou smazat??
        {
            await _driverRepository.DeleteById(id);
            //todo
            throw new Exception();
        }
    }
}
