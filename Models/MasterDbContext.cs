using Microsoft.EntityFrameworkCore;

public class MasterDbContext : DbContext
{
    public MasterDbContext(DbContextOptions options) : base (options)
    {

    }

    public DbSet<Data> Data { get; set; }

    public DbSet<Information> Information { get; set; }
}