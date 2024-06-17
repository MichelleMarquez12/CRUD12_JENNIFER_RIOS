using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TransportManager.Web.Data
{
    public class JwtDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public JwtDbContext(DbContextOptions<JwtDbContext> options) : base(options)
        {

        }
    }
}
