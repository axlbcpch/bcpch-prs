using Microsoft.EntityFrameworkCore;
using PRS.Models;

namespace PRS.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Registrant> Registrants => Set<Registrant>();
    public DbSet<User> Users { get; set; }

    public DbSet<Event> Events => Set<Event>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("participant_registration");
        modelBuilder.Entity<Registrant>(e =>
        {
            e.ToTable("registrants");
            e.Property(r => r.Id).HasColumnName("id");
            e.Property(r => r.FirstName).HasColumnName("first_name");
            e.Property(r => r.LastName).HasColumnName("last_name");
            e.Property(r => r.Birthday).HasColumnName("birthday");
            e.Property(r => r.Gender).HasColumnName("gender");
            e.Property(r => r.ContactNo).HasColumnName("contact_no");
            e.Property(r => r.Email).HasColumnName("email");
            e.Property(r => r.Signature).HasColumnName("signature");
            e.Property(r => r.CreatedAt).HasColumnName("created_at");
            e.Property(r => r.Address).HasColumnName("address");
            e.Property(r => r.EventId).HasColumnName("event_id");
            // ← relationship defined here, where the FK column actually lives
            e.HasOne(r => r.Event)
            .WithMany(ev => ev.Registrants)
            .HasForeignKey(r => r.EventId)
            .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Event>(e =>
        {
            e.ToTable("events");
            e.Property(ev => ev.Id).HasColumnName("id");
            e.Property(ev => ev.Name).HasColumnName("name");
            e.Property(ev => ev.Description).HasColumnName("description");
            e.Property(ev => ev.StartDate).HasColumnName("start_date");
            e.Property(ev => ev.EndDate).HasColumnName("end_date");
            e.Property(ev => ev.Location).HasColumnName("location");
            e.Property(ev => ev.CreatedAt).HasColumnName("created_at");
            e.Property(ev => ev.CreatedBy).HasColumnName("created_by");
        });
    }
}