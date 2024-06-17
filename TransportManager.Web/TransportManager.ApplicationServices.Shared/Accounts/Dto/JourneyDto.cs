using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportManager.Core.Transports
{
    public class JourneyDto
    {
        public int Id { get; set; }
        public int DestinationId { get; set; }
        public int OriginId { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
    }
}
