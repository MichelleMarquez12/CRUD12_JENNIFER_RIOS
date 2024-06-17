using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportManager.Core.Transports;

namespace TransportManager.ApplicationServices.Transport
{
    public interface ITicketAppService
    {
        Task<IEnumerable<TicketDto>> GetAllAsync();
        Task<TicketDto> GetByIdAsync(int id);
        Task<TicketDto> InsertAsync(TicketDto ticket);
        Task<TicketDto> EditAsync(TicketDto ticket);
        Task<int> DeleteAsync(int id);
    }
}
