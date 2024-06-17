using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportManager.Core.Transports;

namespace TransportManager.DataAccess
{
    public class TransportManagerContext : IdentityDbContext
    {
        public DbSet<JourneyDto> Journeys { get; set; }
        public DbSet<PassengerDto> Passengers { get; set; }
        public DbSet<TicketDto> Tickets { get; set; }
        public TransportManagerContext(DbContextOptions<TransportManagerContext> options) : base(options)
        {

        }
    }
}
