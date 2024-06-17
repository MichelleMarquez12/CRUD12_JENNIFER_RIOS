using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportManager.Core.Transports;

namespace TransportManager.ApplicationServices.Transport
{
    public interface IJourneyAppService
    {
        Task<IEnumerable<JourneyDto>> GetAllAsync();
        Task<JourneyDto> GetByIdAsync(int id);
        Task<JourneyDto> InsertAsync(JourneyDto journey);
        Task<JourneyDto> EditAsync(JourneyDto journey);
        Task<int> DeleteAsync(int id);
    }
}
