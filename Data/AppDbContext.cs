using CompanyApplicationApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace CompanyApplicationApi.Data;

public class AppDbContext : DbContext

{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Application> Applications { get; set; }
    public DbSet<ContactInfo> ContactInfos { get; set; }
    public DbSet<CompanyInfo> CompanyInfos { get; set; }
    public DbSet<Partners> Partners { get; set; }
    public DbSet<Address> Addresses { get; set; }

    public DbSet<City> Cities => Set<City>();
    public DbSet<District> Districts => Set<District>();
    
    public DbSet<ApiLog> ApiLogs => Set<ApiLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Application>()
            .Property(a => a.Status)
            .HasConversion<string>();
    }
}