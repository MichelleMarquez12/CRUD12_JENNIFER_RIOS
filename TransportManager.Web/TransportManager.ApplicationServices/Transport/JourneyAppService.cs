using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportManager.Core.Transports;
using TransportManager.DataAccess.Repositories;

namespace TransportManager.ApplicationServices.Transport
{
    public class JourneyAppService : IJourneyAppService
    {
        private readonly IRepository<int, JourneyDto> _repository;

        public JourneyAppService(IRepository<int, JourneyDto> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<JourneyDto>> GetAllAsync()
        {
            return await Task.FromResult(_repository.GetAll().ToList());
        }

        public Task<JourneyDto> GetByIdAsync(int id)
        {
            return _repository.GetAsync(id);
        }

        public Task<JourneyDto> InsertAsync(JourneyDto journey)
        {
            return _repository.AddAsync(journey);
        }

        public Task<JourneyDto> EditAsync(JourneyDto journey)
        {
            return _repository.UpdateAsync(journey);
        }

        public async Task<int> DeleteAsync(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                return 1; // Éxito
            }
            catch
            {
                return 0; // No encontrado
            }
        }
    }
}
