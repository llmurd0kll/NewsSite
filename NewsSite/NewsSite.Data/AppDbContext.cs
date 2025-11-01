using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewsSite.Domain.Entities;

namespace NewsSite.Data
    {
    public class AppDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
        {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<News> News { get; set; }

        protected override void OnModelCreating(ModelBuilder b)
            {
            base.OnModelCreating(b);

            b.Entity<News>(e =>
            {
                e.Property(x => x.TitleRu).HasMaxLength(200).IsRequired();
                e.Property(x => x.TitleEn).HasMaxLength(200).IsRequired();
                e.Property(x => x.SubtitleRu).HasMaxLength(400);
                e.Property(x => x.SubtitleEn).HasMaxLength(400);
                e.Property(x => x.ImagePath).HasMaxLength(300);
                e.HasIndex(x => x.PublishedAt);
            });
            }
        }
    }
