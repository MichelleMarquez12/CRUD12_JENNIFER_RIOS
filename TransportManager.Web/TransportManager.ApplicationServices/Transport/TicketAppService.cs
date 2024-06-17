using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportManager.Core.Transports;
using TransportManager.DataAccess.Repositories;

namespace TransportManager.ApplicationServices.Transport
{
    public class TicketAppService : ITicketAppService
    {
        private readonly IRepository<int, TicketDto> _repository;

        public TicketAppService(IRepository<int, TicketDto> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TicketDto>> GetAllAsync()
        {
            return await Task.FromResult(_repository.GetAll().ToList());
        }

        public Task<TicketDto> GetByIdAsync(int id)
        {
            return _repository.GetAsync(id);
        }

        public Task<TicketDto> InsertAsync(TicketDto ticket)
        {
            return _repository.AddAsync(ticket);
        }

        public Task<TicketDto> EditAsync(TicketDto ticket)
        {
            return _repository.UpdateAsync(ticket);
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
