using System.Text;
using Microsoft.EntityFrameworkCore;
using RoadDefect.Api.Models;

namespace RoadDefect.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Area> Areas => Set<Area>();
    public DbSet<RoadSegment> RoadSegments => Set<RoadSegment>();
    public DbSet<Defect> Defects => Set<Defect>();
    public DbSet<DefectImage> DefectImages => Set<DefectImage>();
    public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();
    public DbSet<WorkOrderUpdate> WorkOrderUpdates => Set<WorkOrderUpdate>();
    private static string HashPassword(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToHexString(hash);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Basic configuration; can be extended later
        //add for error

                    modelBuilder.Entity<Area>().HasData(
                    new Area { Id = 1, Name = "Area 1" },
                    new Area { Id = 2, Name = "Area 2" },
                    new Area { Id = 3, Name = "Area 3" }
                );

                // -----------------------------------------
                // SEED ROAD SEGMENTS
                // -----------------------------------------
                modelBuilder.Entity<RoadSegment>().HasData(
                    new RoadSegment
                    {
                        Id = 1,
                        Name = "Main Highway - Section 1",
                        AreaId = 1,
                        StartLat = 20.5937,
                        StartLng = 78.9629,
                        EndLat = 20.6000,
                        EndLng = 78.9700,
                        FunctionalClass = (RoadFunctionalClass)1,
                        TrafficImportance = (TrafficImportance)3
                    },
                    new RoadSegment
                    {
                        Id = 2,
                        Name = "City Ring Road - East",
                        AreaId = 2,
                        StartLat = 20.5000,
                        StartLng = 78.9000,
                        EndLat = 20.5100,
                        EndLng = 78.9150,
                        FunctionalClass = (RoadFunctionalClass)2,
                        TrafficImportance = (TrafficImportance)2
                    },
                    new RoadSegment
                    {
                        Id = 3,
                        Name = "Market Street Lane",
                        AreaId = 3,
                        StartLat = 20.5500,
                        StartLng = 78.9500,
                        EndLat = 20.5525,
                        EndLng = 78.9525,
                        FunctionalClass = (RoadFunctionalClass)3,
                        TrafficImportance = (TrafficImportance)1
                    }
                );

                // -----------------------------------------
                // SEED ADMIN USER
                // -----------------------------------------
                var adminPassword = HashPassword("Admin@123");

                modelBuilder.Entity<User>().HasData(
                    new User
                    {
                        Id = 99,
                        Name = "Super Admin",
                        Email = "admin@roaddefect.com",
                        Phone = "9999999999",
                        PasswordHash = adminPassword,
                        Role = UserRole.Admin,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    }
                );

        modelBuilder.Entity<WorkOrder>()
                    .HasOne(w => w.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(w => w.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<WorkOrder>()
                    .HasOne(w => w.AssignedToUser)
                    .WithMany()
                    .HasForeignKey(w => w.AssignedToUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<WorkOrder>()
                    .HasOne(w => w.Defect)
                    .WithOne(d => d.WorkOrder)
                    .HasForeignKey<WorkOrder>(w => w.DefectId)
                    .OnDelete(DeleteBehavior.Cascade);

        //end 
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Area>()
            .HasMany<RoadSegment>()
            .WithOne(r => r.Area!)
            .HasForeignKey(r => r.AreaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Area>()
            .HasMany<Defect>()
            .WithOne(d => d.Area!)
            .HasForeignKey(d => d.AreaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Defect>()
            .HasMany(d => d.Images)
            .WithOne(i => i.Defect!)
            .HasForeignKey(i => i.DefectId);

        modelBuilder.Entity<WorkOrder>()
            .HasMany(w => w.Updates)
            .WithOne(u => u.WorkOrder!)
            .HasForeignKey(u => u.WorkOrderId);
    }
}
