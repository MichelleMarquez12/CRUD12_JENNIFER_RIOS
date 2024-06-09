using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportManager.Core.Transport
{
    public class Ticket
    {
        public int Id { get; set; }
        public int JourneyId { get; set; }
        public int PassengerId { get; set; }
        public int Seat { get; set; }
    }
}
