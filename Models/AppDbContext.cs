using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions options): base (options)
    {
        
    }
    public DbSet<Information> information { get; set; }
}