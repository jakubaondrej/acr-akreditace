using InfoSystem.Core.DataAbstraction;
using InfoSystem.Core.Redactions;
using InfoSystem.Data.Entities;
using InfoSystem.Web.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem.Data.Repositories
{
    public class RedactionRepository : RepositoryBase, IRedactionRepository
    {
        public RedactionRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {

        }
        public async Task<int> CreateRedaction(RedactionCreateData redaction)
        {
            var data = new Redaction()
            {
                Note = redaction.Note,
                Link = redaction.Link,
                GeneralEditor = redaction.GeneralEditor,
                Name = redaction.Name,
                GeneralEditorEmail = redaction.GeneralEditorEmail,
                GeneralEditorCallNumber = redaction.GeneralEditorCallNumber
            };
            Db.Redaction.Add(data);
            await Db.SaveChangesAsync();
            return data.RedactionId;
        }

        public async Task Delete(int id)
        {
            var redaction = await Db.Redaction.Where(r=>r.RedactionId == id)
               .SingleOrDefaultAsync();
            Db.Redaction.Remove(redaction);
            await Db.SaveChangesAsync();
        }

        public async Task<int> EditRedaction(int id, RedactionDetail redaction)
        {
            var data = new Redaction()
            {
                RedactionId = id,
                Note = redaction.Note,
                Link = redaction.Link,
                GeneralEditor = redaction.GeneralEditor,
                Name = redaction.Name,
                GeneralEditorEmail = redaction.GeneralEditorEmail,
                GeneralEditorCallNumber = redaction.GeneralEditorCallNumber
            };
            Db.Redaction.Update(data);
            return await Db.SaveChangesAsync();
        }

        public async Task<List<RedactionListing>> GetAllRedactions()
        {
            var list = await Db.Redaction
                .Select(r=>new RedactionListing() 
                {
                    Id = r.RedactionId,
                    Name = r.Name
                })
                .ToListAsync();
            return list;
        }

        public async Task<RedactionDetail> GetRedactionById(int id)
        {
            return await Db.Redaction
                .Where(r => r.RedactionId == id)
                .Select(r => new RedactionDetail()
                {
                    Name = r.Name,
                    GeneralEditor = r.GeneralEditor,
                    GeneralEditorCallNumber = r.GeneralEditorCallNumber,
                    GeneralEditorEmail = r.GeneralEditorEmail,
                    Id = r.RedactionId,
                    Link = r.Link,
                    Note = r.Note
                })
                .SingleOrDefaultAsync();
        }

        public async Task<RedactionDetail> GetRedactionByName(string name)
        {
            var redaction = await Db.Redaction
                .Where(r => r.Name.ToLower() == name.Trim().ToLower())
                .Select(r=> new RedactionDetail()
                {
                    Name = r.Name,
                    GeneralEditor = r.GeneralEditor,
                    GeneralEditorCallNumber = r.GeneralEditorCallNumber,
                    GeneralEditorEmail = r.GeneralEditorEmail,
                    Id = r.RedactionId,
                    Link = r.Link,
                    Note=r.Note
                })
                .SingleOrDefaultAsync();

            return redaction;
        }
    }
}
