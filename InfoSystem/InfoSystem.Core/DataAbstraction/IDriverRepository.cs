using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using InfoSystem.Core.Drivers;

namespace InfoSystem.Core.DataAbstraction
{
    public interface IDriverRepository
    {
        Task<List<DriverListing>> GetAll(); //royd2lit Driver na GetAllDriverResult a getDriverDetail
        Task<DriverDetail> GetById(int id);
        Task DeleteById(int id);
        Task<int> Create(DriverCreateData driver);
    }

    public class DriverCreateData
    {
        public string Name { get; set; }
        public string CoDriverName { get; set; }
        public string Car { get; set; }
    }
    public class DriverDetail
    {
        public int DriverId { get; set; }
        public string Name { get; set; }
        public string CoDriverName { get; set; }
        public string Car { get; set; }
    }
    public class DriverListing
    {
        public int DriverId { get; set; }
        public string Name { get; set; }
        public string CoDriverName { get; set; }
        public string Car { get; set; }
    }

}
