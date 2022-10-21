using BibliotecaLog.Data;
using BibliotecaLog.Repository.Interface;
using BibliotecaLog.Models;

namespace BibliotecaLog.Repository
{
    public class StudentRepository : Repository<StudentViewModel>, IStudentRepository
    {
        public StudentRepository(DataContext context) : base(context)
        {
        }
    }
}
