using InfoSystem.Web.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem.Data.Repositories
{
    public abstract class RepositoryBase
    {
        protected ApplicationDbContext Db;
        protected RepositoryBase(ApplicationDbContext applicationDbContext)
        {
            Db = applicationDbContext;
        }

    }
}
