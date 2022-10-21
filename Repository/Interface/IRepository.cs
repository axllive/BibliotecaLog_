using BibliotecaLog.Models;

namespace BibliotecaLog.Repository.Interface
{
    public interface IRepository<T> : IDisposable where T : Root
    {
        Task<T> Criar(T entity);
        Task<T> ConsultarUm(int Id);
        Task<List<T>> ConsultarTodos();
        Task Excluir(T entity);
        Task<T> Atualizar(T entity);
        Task<int> SaveChanges();

    }
}
