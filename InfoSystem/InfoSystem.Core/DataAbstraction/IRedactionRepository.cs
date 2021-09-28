using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using InfoSystem.Core.Redactions;

namespace InfoSystem.Core.DataAbstraction
{
    public interface IRedactionRepository
    {
        Task<RedactionDetail> GetRedactionByName(string name);
        Task<int> CreateRedaction(RedactionCreateData redaction);
        Task<List<RedactionListing>> GetAllRedactions();
        Task<RedactionDetail> GetRedactionById(int id);
        Task<int> EditRedaction(int id, RedactionDetail redaction);
        Task Delete(int id);
    }

    public class RedactionDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GeneralEditor { get; set; }
        public string GeneralEditorCallNumber { get; set; }
        public string GeneralEditorEmail { get; set; }
        public string Note { get; set; }
        public string Link { get; set; }
    }

    public class RedactionListing
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
