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

}