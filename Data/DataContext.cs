using BibliotecaLog.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace BibliotecaLog.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<AuthorViewModel> Authors { get; set; }
        public DbSet<BookViewModel> Books { get; set; }
        public DbSet<BookLoan> Loans { get; set; }
        public DbSet<StudentViewModel> Students { get; set; }
    }
}
