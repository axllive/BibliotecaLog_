using BibliotecaLog.Data;
using BibliotecaLog.Repository.Interface;
using BibliotecaLog.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaLog.Repository
{
    public class Repository<T> : IRepository<T> where T : Root
    {
        protected readonly DataContext _context;
        //construtor do repository contendo o contexto principal
        public Repository(DataContext context)
        {
            _context = context;
        }
        //CREATE
        public async Task<T> Criar(T entity)
        {
            await _context.AddAsync(entity);
            await SaveChanges();
            return entity;
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
        //UPDATE
        public async Task<T> Atualizar(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await SaveChanges();
            return entity;
        }
        //DELETE
        public async Task Excluir(T entity)
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