using Bookstore.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Database;

public class BookstoreContext : DbContext
{
    public DbSet<BookLike> BookLikes { get; set; }

    public BookstoreContext() { }
    public BookstoreContext(DbContextOptions<BookstoreContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
    }

    public override int SaveChanges()
    {
        return base.SaveChanges();
    }
}
