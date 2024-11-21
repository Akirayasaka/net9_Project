using Microsoft.EntityFrameworkCore;
using net9_WebAPI.Models;

namespace net9_WebAPI.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public required DbSet<Villa> Villas { get; set; }
    }
}
