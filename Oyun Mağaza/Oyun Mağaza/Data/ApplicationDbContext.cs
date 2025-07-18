using Microsoft.EntityFrameworkCore;
using Oyun_Mağaza.Models;

namespace Oyun_Mağaza.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<GameCategory> GameCategories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<GameTag> GameTags { get; set; }
        public DbSet<GameLanguage> GameLanguages { get; set; }
        public DbSet<SystemRequirement> SystemRequirements { get; set; }
        public DbSet<DLC> DLCs { get; set; }
        public DbSet<UserDLC> UserDLCs { get; set; }
        public DbSet<Bundle> Bundles { get; set; }
        public DbSet<UserBundle> UserBundles { get; set; }
        public DbSet<UserGame> UserGames { get; set; }
        public DbSet<GameReview> GameReviews { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }
        public DbSet<Trading> Tradings { get; set; }
        public DbSet<Screenshot> Screenshots { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Update> Updates { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<BundleDiscount> BundleDiscounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Para birimleri için hassasiyet ayarları (18,2)
            modelBuilder.Entity<Game>()
                .Property(g => g.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<DLC>()
                .Property(d => d.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Bundle>()
                .Property(b => b.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Bundle>()
                .Property(b => b.DiscountPercentage)
                .HasPrecision(5, 2);

            modelBuilder.Entity<User>()
                .Property(u => u.WalletBalance)
                .HasPrecision(18, 2);

            modelBuilder.Entity<BundleDiscount>()
                .Property(d => d.DiscountPercentage)
                .HasPrecision(5, 2);

            // İlişki konfigürasyonları
            modelBuilder.Entity<GameCategory>()
                .HasKey(gc => new { gc.GameId, gc.CategoryId });

            modelBuilder.Entity<GameTag>()
                .HasKey(gt => new { gt.GameId, gt.TagId });

            modelBuilder.Entity<UserGame>()
                .HasKey(ug => new { ug.UserId, ug.GameId });

            modelBuilder.Entity<UserDLC>()
                .HasKey(ud => new { ud.UserId, ud.DLCId });

            modelBuilder.Entity<UserBundle>()
                .HasKey(ub => new { ub.UserId, ub.BundleId });

            modelBuilder.Entity<UserAchievement>()
                .HasKey(ua => new { ua.UserId, ua.AchievementId });

            modelBuilder.Entity<WishList>()
                .HasKey(w => new { w.UserId, w.GameId });

            modelBuilder.Entity<GameLanguage>()
                .HasKey(gl => new { gl.GameId, gl.Language });

            // Trading ilişkileri
            modelBuilder.Entity<Trading>()
                .HasOne(t => t.Sender)
                .WithMany(u => u.TradingsSent)
                .HasForeignKey(t => t.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Trading>()
                .HasOne(t => t.Receiver)
                .WithMany(u => u.TradingsReceived)
                .HasForeignKey(t => t.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Trading>()
                .HasOne(t => t.GameOffered)
                .WithMany()
                .HasForeignKey(t => t.GameOfferedId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Trading>()
                .HasOne(t => t.GameRequested)
                .WithMany()
                .HasForeignKey(t => t.GameRequestedId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cascade delete davranışları
            modelBuilder.Entity<Game>()
                .HasMany(g => g.Screenshots)
                .WithOne(s => s.Game)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Game>()
                .HasMany(g => g.DLCs)
                .WithOne(d => d.Game)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Game>()
                .HasMany(g => g.Updates)
                .WithOne(u => u.Game)
                .OnDelete(DeleteBehavior.Cascade);

            // Friendship ilişkileri
            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.Friend)
                .WithMany()
                .HasForeignKey(f => f.FriendId)
                .OnDelete(DeleteBehavior.Restrict);

            // Message ilişkileri
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            // Notification ilişkileri
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasPrecision(18, 2);
        }
    }
} 