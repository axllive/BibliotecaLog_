using BibliotecaLog.Models;
using System.Linq.Expressions;

namespace BibliotecaLog.Repository.Interface
{
    public interface IRepository<T> : IDisposable where T : Root
    {
        Task Criar(T entity);
        Task<T> ConsultarUm(int Id);
        Task<List<T>> ConsultarTodos();
        Task<IEnumerable<T>> ConsultarTodos(Expression<Func<T, bool>> predicate); 
        Task Excluir(T entity);
        Task<T> Atualizar(T entity);
        Task<int> SaveChanges();

    }
}
