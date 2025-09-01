using Backend_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend_API.DBContext;

public interface INotesDBContext
{
    DbSet<Note> Notes { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<Folder> Folders { get; set; }
    DbSet<DummyModel> DummyModels { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}