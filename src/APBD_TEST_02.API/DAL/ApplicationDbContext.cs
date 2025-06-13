using APBD_TEST_02.API.Models;
using APBD_TEST_02.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_TEST_02.API.DAL;

public class ApplicationDbContext : DbContext
{

    private readonly IConfiguration _configuration;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<Language> Languages { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Record> Records { get; set; }
    public DbSet<TaskTodo> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Language>().HasKey(l => l.Id);
        modelBuilder.Entity<Student>().HasKey(s => s.Id);
        modelBuilder.Entity<TaskTodo>().HasKey(t => t.Id);
        modelBuilder.Entity<Record>().HasKey(r => r.Id);

        modelBuilder.Entity<Record>()
            .HasOne(r => r.Language)
            .WithMany(l => l.Records)
            .HasForeignKey(r => r.LanguageId);

        modelBuilder.Entity<Record>()
            .HasOne(r => r.Student)
            .WithMany(s => s.Records)
            .HasForeignKey(r => r.StudentId);

        modelBuilder.Entity<Record>()
            .HasOne(r => r.TaskTodo)
            .WithMany(t => t.Records)
            .HasForeignKey(r => r.TaskId);
    }
}