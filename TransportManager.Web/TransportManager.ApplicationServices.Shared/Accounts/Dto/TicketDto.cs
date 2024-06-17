using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportManager.Core.Transports
{
    public class TicketDto
    {
        public int Id { get; set; }
        public int Journeys_Id { get; set; }
        public int Passengers_Id { get; set; }
        public int Seat { get; set; }
    }
}
