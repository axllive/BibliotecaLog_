using BibliotecaLog.Data;
using BibliotecaLog.Repository.Interface;
using BibliotecaLog.Models;

namespace BibliotecaLog.Repository
{
    public class AuthorRepository : Repository<AuthorViewModel>, IAuthorRepository
    {
        public AuthorRepository(DataContext context) : base(context)
        {
        }
    }
}
