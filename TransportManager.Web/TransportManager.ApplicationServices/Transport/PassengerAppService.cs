using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportManager.Core.Transports;
using TransportManager.DataAccess.Repositories;

namespace TransportManager.ApplicationServices.Transport
{
    public class PassengerAppService : IPassengerAppService
    {
        private readonly IRepository<int, PassengerDto> _repository;

        public PassengerAppService(IRepository<int, PassengerDto> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PassengerDto>> GetAllAsync()
        {
            return await Task.FromResult(_repository.GetAll().ToList());
        }

        public Task<PassengerDto> GetByIdAsync(int id)
        {
            return _repository.GetAsync(id);
        }

        public Task<PassengerDto> InsertAsync(PassengerDto passenger)
        {
            return _repository.AddAsync(passenger);
        }

        public Task<PassengerDto> EditAsync(PassengerDto passenger)
        {
            return _repository.UpdateAsync(passenger);
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
