using InfoSystem.Core.DataAbstraction;
using InfoSystem.Web.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem.Data.Repositories
{
    public class DriverRepository : RepositoryBase, IDriverRepository
    {
        public DriverRepository(ApplicationDbContext applicationDbContext )
            : base(applicationDbContext)
        {

        }
        public async Task<int> Create(DriverCreateData driver)
        {
            var entity = new Entities.Driver()
            {
                Car = driver.Car,
                CoDriverName = driver.CoDriverName
            };
            Db.Driver.Add(entity);
            await Db.SaveChangesAsync();
            return entity.DriverId;
        }

        public Task DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<DriverListing>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<DriverDetail> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
