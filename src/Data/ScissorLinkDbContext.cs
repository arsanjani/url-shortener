using Microsoft.EntityFrameworkCore;
using ScissorLink.Models;

namespace ScissorLink.Data
{
    public class ScissorLinkDbContext : DbContext
    {
        public ScissorLinkDbContext(DbContextOptions<ScissorLinkDbContext> options) : base(options)
        {
        }

        public DbSet<DtoShortLink> ShortLinks { get; set; }
        public DbSet<DtoShortLinkDetail> ShortLinkDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure ShortLink entity
            modelBuilder.Entity<DtoShortLink>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.HasIndex(e => e.Token).IsUnique();
                entity.Property(e => e.IsPublish).HasDefaultValue(false);
                entity.Property(e => e.CreateAdminDate).HasDefaultValueSql("getdate()");
                
                // Configure relationships
                entity.HasMany(e => e.Details)
                      .WithOne(e => e.ShortLink)
                      .HasForeignKey(e => e.ShortLinkID)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure ShortLinkDetail entity
            modelBuilder.Entity<DtoShortLinkDetail>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.VisitDate).HasDefaultValueSql("getdate()");
            });
        }
    }
}
