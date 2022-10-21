using BibliotecaLog.Data;
using BibliotecaLog.Repository.Interface;
using BibliotecaLog.Models;

namespace BibliotecaLog.Repository
{
    public class BookLoanRepository : Repository<BookLoan>, ILoanRepository
    {
        public BookLoanRepository(DataContext context) : base(context)
        {
        }
    }
}
