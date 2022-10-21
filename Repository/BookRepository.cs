using BibliotecaLog.Data;
using BibliotecaLog.Repository.Interface;
using BibliotecaLog.Models;

namespace BibliotecaLog.Repository
{
    public class BookRepository : Repository<BookViewModel>, IBookRepository
    {
        public BookRepository(DataContext context) : base(context)
        {
        }
    }
}
