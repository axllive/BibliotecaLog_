using BibliotecaLog.Data;
using BibliotecaLog.Repository.Interface;
using BibliotecaLog.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BibliotecaLog.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : Root
    {
        public readonly DataContext _context;
        //construtor do repository contendo o contexto principal
        public Repository(DataContext context)
        {
            _context = context;
        }
        //CREATE
        public virtual async Task Criar(T entity)
        {
            await _context.AddAsync(entity);
            await SaveChanges();
        }
        //READ
        public async Task<T> ConsultarUm(int Id)
        {
            //tratar nullabe element
            return await _context.Set<T>().FindAsync(Id);
        }
        public async Task<List<T>> ConsultarTodos()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<T>> ConsultarTodos(Expression<Func<T, bool>> predicate)
        {
           return await _context.Set<T>().AsNoTracking().Where(predicate).ToListAsync(); 
        }
        //UPDATE
        public virtual async Task<T> Atualizar(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await SaveChanges();
            return entity;
        }
        //DELETE
        public virtual async Task Excluir(T entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
            await SaveChanges();
        }
        //------------------------------------------------------------------------
        public void Dispose()
        {
            _context?.Dispose();
        }
        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }

        
    }
}