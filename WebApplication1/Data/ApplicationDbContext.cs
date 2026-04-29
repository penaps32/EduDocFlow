using EduDocFlow.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace EduDocFlow.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();
        public DbSet<DocumentType> DocumentTypes => Set<DocumentType>();
        public DbSet<DocumentRequest> DocumentRequests => Set<DocumentRequest>();
        public DbSet<RequestComment> RequestComments => Set<RequestComment>();
        public DbSet<RequestStatusHistory> RequestStatusHistoryItems => Set<RequestStatusHistory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(x => x.Email).IsUnique();

                entity.Property(x => x.Role)
                    .HasConversion<string>();
            });

            modelBuilder.Entity<StudentProfile>(entity =>
            {
                entity.HasOne(x => x.User)
                    .WithOne(x => x.StudentProfile)
                    .HasForeignKey<StudentProfile>(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DocumentType>(entity =>
            {
                entity.HasIndex(x => x.Code).IsUnique();

                entity.Property(x => x.Category)
                    .HasConversion<string>();

                entity.Property(x => x.TemplateKind)
                    .HasConversion<string>();
            });

            modelBuilder.Entity<DocumentRequest>(entity =>
            {
                entity.HasIndex(x => x.Number).IsUnique();

                entity.Property(x => x.Status)
                    .HasConversion<string>();

                entity.HasOne(x => x.Student)
                    .WithMany(x => x.CreatedRequests)
                    .HasForeignKey(x => x.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.AssignedEmployee)
                    .WithMany(x => x.AssignedRequests)
                    .HasForeignKey(x => x.AssignedEmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.DocumentType)
                    .WithMany()
                    .HasForeignKey(x => x.DocumentTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RequestComment>(entity =>
            {
                entity.HasOne(x => x.DocumentRequest)
                    .WithMany(x => x.Comments)
                    .HasForeignKey(x => x.DocumentRequestId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Author)
                    .WithMany(x => x.Comments)
                    .HasForeignKey(x => x.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RequestStatusHistory>(entity =>
            {
                entity.Property(x => x.OldStatus)
                    .HasConversion<string>();

                entity.Property(x => x.NewStatus)
                    .HasConversion<string>();

                entity.HasOne(x => x.DocumentRequest)
                    .WithMany(x => x.StatusHistoryItems)
                    .HasForeignKey(x => x.DocumentRequestId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.ChangedByUser)
                    .WithMany(x => x.StatusHistoryItems)
                    .HasForeignKey(x => x.ChangedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}