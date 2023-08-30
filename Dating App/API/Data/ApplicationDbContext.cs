using DatingApplication.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DatingApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, int, 
                 IdentityUserClaim<int>, AppUserRole, 
                 IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<LikeUser> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Connection> Connections { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<LikeUser>().HasKey(k => new { k.SourceUSerId, k.TargetUserId });

            modelBuilder.Entity<LikeUser>()
               .HasOne(s => s.SourceUser)
               .WithMany(l => l.LikedUsers)
               .HasForeignKey(l => l.SourceUSerId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LikeUser>()
              .HasOne(s => s.TargetUser)
              .WithMany(l => l.LikedByUsers)
              .HasForeignKey(l => l.TargetUserId)
              .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne(s => s.Sender)
                .WithMany(ms => ms.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(r=> r.Reciver)
                .WithMany(mr=> mr.MessagesRecived)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<AppUser>()
                .HasMany(ur => ur.UserRole)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId)
                .IsRequired();

            modelBuilder.Entity<AppRole>()
                .HasMany(ur => ur.UserRole)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId)
                .IsRequired();

        }
    }
}
