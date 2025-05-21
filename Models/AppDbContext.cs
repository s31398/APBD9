
using Microsoft.EntityFrameworkCore;

namespace DeviceApi.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<DeviceType> DeviceTypes => Set<DeviceType>();
    public DbSet<Position> Positions => Set<Position>();
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<DeviceEmployee> DeviceEmployees => Set<DeviceEmployee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DeviceType>(e =>
        {
            e.ToTable("DeviceType");
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Position>(e =>
        {
            e.ToTable("Position");
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
            e.Property(x => x.MinExpYears);
        });

        modelBuilder.Entity<Person>(e =>
        {
            e.ToTable("Person");
            e.HasKey(x => x.Id);
            e.Property(x => x.PassportNumber).HasMaxLength(30).IsRequired();
            e.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            e.Property(x => x.MiddleName).HasMaxLength(100);
            e.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            e.Property(x => x.PhoneNumber).HasMaxLength(20).IsRequired();
            e.Property(x => x.Email).HasMaxLength(150).IsRequired();
        });

        modelBuilder.Entity<Device>(e =>
        {
            e.ToTable("Device");
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(150).IsRequired();
            e.Property(x => x.IsEnabled);
            e.Property(x => x.AdditionalProperties).HasMaxLength(8000).IsRequired();
            e.HasOne(x => x.DeviceType)
                .WithMany(d => d.Devices)
                .HasForeignKey(x => x.DeviceTypeId);
        });

        modelBuilder.Entity<Employee>(e =>
        {
            e.ToTable("Employee");
            e.HasKey(x => x.Id);
            e.Property(x => x.Salary).HasColumnType("decimal(18,2)");
            e.Property(x => x.HireDate);
            e.HasOne(x => x.Position)
                .WithMany(p => p.Employees)
                .HasForeignKey(x => x.PositionId);
            e.HasOne(x => x.Person)
                .WithOne(p => p.Employee)
                .HasForeignKey<Employee>(x => x.PersonId);
        });

        modelBuilder.Entity<DeviceEmployee>(e =>
        {
            e.ToTable("DeviceEmployee");
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Device)
                .WithMany(d => d.DeviceEmployees)
                .HasForeignKey(x => x.DeviceId);
            e.HasOne(x => x.Employee)
                .WithMany(emp => emp.DeviceEmployees)
                .HasForeignKey(x => x.EmployeeId);
        });
    }
}
