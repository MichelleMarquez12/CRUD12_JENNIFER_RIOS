using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TransportManager.Core.Transport;

namespace TransportManager.DataAccess
{
    public class TransportManagerContext : IdentityDbContext
    {
        public DbSet<Journey> Journeys { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public TransportManagerContext(DbContextOptions<TransportManagerContext> options) : base(options)
        {

        }

    }
}
