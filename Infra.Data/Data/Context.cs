using Domain.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Data
{
    public class Context : DbContext
    {
        public Context (DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<Donation> Donation { get; set; }
    }
}
