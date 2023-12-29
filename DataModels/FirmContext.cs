using Microsoft.EntityFrameworkCore;


namespace DataModels;


public class FirmContext : DbContext{

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Construction>().Property("Name").HasMaxLength(255).IsRequired();
        modelBuilder.Entity<Construction>().Property("ExpectedPrice").HasPrecision(19, 8);
        modelBuilder.Entity<Project>().Property("Price").HasPrecision(19, 8);
        modelBuilder.Entity<Organization>().Property("CountryCode").HasMaxLength(16);
        modelBuilder.Entity<Organization>().Property("VATNumber").HasMaxLength(16);
        modelBuilder.Entity<Organization>().Property("Name").HasMaxLength(255).IsRequired();

    }
    
    public FirmContext()
    {
        
    }

    public FirmContext(DbContextOptions<FirmContext> options) : base(options)
    {
        
    }

    public DbSet<Construction> Constructions {get; set;}
    public DbSet<Organization> Organizations {get; set;}
    public DbSet<Project> Projects {get; set;}
}