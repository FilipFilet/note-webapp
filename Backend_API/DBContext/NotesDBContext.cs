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
        // Setting up cascading behavior for folders, so they also delete their notes
        modelBuilder.Entity<Folder>()
            .HasMany(f => f.Notes)
            .WithOne(n => n.Folder)
            .HasForeignKey(n => n.FolderId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}