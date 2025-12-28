using Microsoft.EntityFrameworkCore;
using CisApi.Infrastructure.MySQL.Entities;

namespace CisApi.Infrastructure.MySQL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<TopicEntity> Topics { get; set; }
        public DbSet<IdeaEntity> Ideas { get; set; }
        public DbSet<IdeaVotesEntity> IdeaVotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // IMPORTANT: users exists and is managed by another API.
            // Excludes the users table from EF migrations (do not create/alter it).
            modelBuilder.Entity<UserEntity>().ToTable("users", t => t.ExcludeFromMigrations());

            // tables that EF should create/manage
            modelBuilder.Entity<TopicEntity>().ToTable("topics");
            modelBuilder.Entity<IdeaEntity>().ToTable("ideas");
            modelBuilder.Entity<IdeaVotesEntity>().ToTable("idea_votes");

            // composite key for votes
            modelBuilder.Entity<IdeaVotesEntity>().HasKey(iv => new { iv.IdeaId, iv.UserId });

            // relationships
            modelBuilder.Entity<TopicEntity>()
                .HasMany(t => t.Ideas)
                .WithOne(i => i.Topic)
                .HasForeignKey(i => i.TopicId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TopicEntity>()
                .HasOne(t => t.CreatedBy)
                .WithMany(u => u.TopicsCreated)
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IdeaEntity>()
                .HasOne(i => i.CreatedBy)
                .WithMany(u => u.IdeasCreated)
                .HasForeignKey(i => i.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IdeaEntity>()
                .HasMany(i => i.Votes)
                .WithOne(v => v.Idea)
                .HasForeignKey(v => v.IdeaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<IdeaVotesEntity>()
                .HasOne(iv => iv.User)
                .WithMany(u => u.Votes)
                .HasForeignKey(iv => iv.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Useful indexes
            modelBuilder.Entity<IdeaEntity>()
                .HasIndex(i => i.TopicId)
                .HasDatabaseName("idx_ideas_topic");
        }
    }
}
