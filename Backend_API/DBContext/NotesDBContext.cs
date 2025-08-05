using Backend_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend_API.DBContext;

public class NotesDBContext : DbContext, INotesDBContext
{
    public NotesDBContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Note> Notes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Folder> Folders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Folder>()
            .HasMany(f => f.Notes)
            .WithOne(n => n.Folder)
            .HasForeignKey(n => n.FolderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Folders)
            .WithOne(f => f.User)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}