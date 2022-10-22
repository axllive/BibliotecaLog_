using BibliotecaLog.Data;
using BibliotecaLog.Models;
using BibliotecaLog.Repository;
using BibliotecaLog.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaLog.Controllers
{
    public class AuthorController : Controller
    {
        //config
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;

        public AuthorController(IBookRepository bookRep, IAuthorRepository authorRep)
        {
            _authorRepository = authorRep;
            _bookRepository = bookRep;
        }
        // GET: AuthorController Index - lista todos os autores
        public async Task<IActionResult> Index()
        {
            return View(await _authorRepository.ConsultarTodos() );
        }

        // GET: AuthorController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            //consulta autor no contexto
            var author = await _authorRepository.ConsultarUm(id);
            if (author == null)//valida se existe
            {
                return NotFound();
            }
            //recupera repositório de livros com a fk do autor
            var bookList = await _bookRepository.
                ConsultarTodos(x => x.AuthorId == id);
            foreach (var item in bookList)
            {   //vincula ao autor se já não estiver vinculado
                if (!author.AuthorBooks.Contains(item))
                {
                    author.AuthorBooks.Add(item);
                    }
            }
            return View(author);
        }

        // GET: AuthorController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AuthorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorViewModel author)
        {
            if (ModelState.IsValid)
            {
                await _authorRepository.Criar(author);
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: AuthorController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _authorRepository.ConsultarUm(id);
            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }

        // POST: AuthorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AuthorViewModel EditAuthor)
        {
            if (ModelState.IsValid)
                try
                {
                    await _authorRepository.Atualizar(EditAuthor);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            return View();
        }

        // GET: AuthorController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _authorRepository.ConsultarUm(id);
            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }

        // POST: AuthorController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) { 
            try
            {
                var delAuthor = await _authorRepository
                    .ConsultarUm(id);
                if (delAuthor == null) return NotFound();
                await _authorRepository.Excluir(delAuthor);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
