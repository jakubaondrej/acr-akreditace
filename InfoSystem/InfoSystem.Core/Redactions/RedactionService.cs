using InfoSystem.Core.DataAbstraction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem.Core.Redactions
{
    public class RedactionService
    {
        private IRedactionRepository _redactionRepository;
        public RedactionService(IRedactionRepository redactionService)
        {
            _redactionRepository = redactionService;
        }

        public async Task<int> CreateRedaction(RedactionCreateData redaction)
        {
            var existR = await _redactionRepository.GetRedactionByName(redaction.Name);
            if (existR != null)
            {
                throw new Exception($"Redaction {redaction.Name} already exists!");
            }
            return await _redactionRepository.CreateRedaction(redaction);
        }

        public async Task<List<RedactionListing>> GetAll()
        {
            var list = await _redactionRepository.GetAllRedactions();
            return list;
        }

        public async Task<RedactionDetail>GetRedactionById(int id)
        {
            return await _redactionRepository.GetRedactionById(id);
        }

        public async Task EditRedaction(int id, RedactionDetail redaction)
        {
            var existR = await _redactionRepository.GetRedactionById(redaction.Id);
            if (existR == null)
            {
                throw new Exception($"Redaction {redaction.Name} does not exist!");
            }
            await _redactionRepository.EditRedaction(id, redaction);
        }

        public async Task DeleteRedaction(int id)
        {
            await _redactionRepository.Delete(id);
        }
    }
}
