using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportManager.Core.Transports;

namespace TransportManager.ApplicationServices.Transport
{
    public interface IPassengerAppService
    {
        Task<IEnumerable<PassengerDto>> GetAllAsync();
        Task<PassengerDto> GetByIdAsync(int id);
        Task<PassengerDto> InsertAsync(PassengerDto passenger);
        Task<PassengerDto> EditAsync(PassengerDto passenger);
        Task<int> DeleteAsync(int id);  
    }
}
