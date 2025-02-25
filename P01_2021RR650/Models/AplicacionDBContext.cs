using Microsoft.EntityFrameworkCore;

namespace P01_2021RR650.Models
{
    public class AplicacionDBContext : DbContext
    {
        public AplicacionDBContext(DbContextOptions<AplicacionDBContext> options) : base(options) 
        { 
        }
    }
}
