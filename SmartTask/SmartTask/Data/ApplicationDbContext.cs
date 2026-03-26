using Microsoft.EntityFrameworkCore;
using SmartTask.Models;
using SmartTask.Repositories;
using Task = SmartTask.Models.Task;

namespace SmartTask.Data
{
    public class ApplicationDbContext : DbContext
    {
        public
       ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        // DbSet для каждой сущности
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskTag> TaskTags { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Настройка уникальности Email в User
            modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

            // Настройка уникальности Username в User
            modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

            // Настройка уникальности Name в рамках одного пользователя для Tag
            modelBuilder.Entity<Tag>().HasIndex(t => new { t.Name, t.OwnerId }).IsUnique();

            // Настройка связей
            modelBuilder.Entity<Project>()
            .HasOne(p => p.Owner)
            .WithMany(u => u.Projects)
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tag>()
            .HasOne(t => t.Owner)
            .WithMany(u => u.Tags)
            .HasForeignKey(t => t.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Task>()
            .HasOne(t => t.User)
            .WithMany(u => u.Tasks)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Task>()
            .HasOne(t => t.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.SetNull); // При удалении проекта задача остаётся без проекта

            modelBuilder.Entity<TaskTag>() // Настройка связи многие-ко-многим через TaskTag
            .HasOne(tt => tt.Task)
            .WithMany(t => t.TaskTags)
            .HasForeignKey(tt => tt.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskTag>()
            .HasOne(tt => tt.Tag)
            .WithMany(t => t.TaskTags)
            .HasForeignKey(tt => tt.TagId)
            .OnDelete(DeleteBehavior.Cascade);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries() 
                .Where(e => e.Entity is IHasUpdatedAt && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                ((IHasUpdatedAt)entry.Entity).UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
