using BibliotecaLog.Data;
using BibliotecaLog.Models;
using BibliotecaLog.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaLog.Controllers
{
    public class BookController : Controller
    {
        //config
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly DataContext _dbContext;
        public BookController(IBookRepository bookRep, IAuthorRepository authorRep
               , ILoanRepository loanRepository, DataContext db)
        {
            _authorRepository = authorRep;
            _bookRepository = bookRep;
            _loanRepository = loanRepository;
            _dbContext = db;
        }
        // GET: BookController
        public async Task<IActionResult> Index()
        {
            var books = await _dbContext.Books
                .Include(c => c.BookAuthor)
                .AsNoTracking()
                .ToListAsync();                
            return View(books);
        }

        // GET: BookController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var book = await _bookRepository.ConsultarUm(id);
            if (book == null)//valida se existe
            {
                return NotFound();
            }
            //recupera repositório de livros com a fk do autor
            book.BookAuthor = await _authorRepository.ConsultarUm(book.AuthorId);
            return View(book);
        }

        // GET: BookController/Create
        public IActionResult Create()
        {
            var authorList = _authorRepository.ConsultarTodos().Result.ToList();
            ViewData["AuthorID"] = new SelectList(authorList, "Id", "Name");
            return View();
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel Book)
        {
            if (Book != null)
                {
                    Book.BookAuthor = await _authorRepository.ConsultarUm(Book.AuthorId);
                    await _bookRepository.Criar(Book);
                    return RedirectToAction(nameof(Index));
                }
            var authorList = _authorRepository.ConsultarTodos().Result.ToList();
            ViewData["AuthorID"] = new SelectList(authorList, "Id",
                "Name", Book.AuthorId);
            return View(Book);
            
        }

        // GET: BookController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _bookRepository.ConsultarUm(id);
            if (book == null)
            {
                return NotFound();
            }
            book.BookAuthor = await _authorRepository.ConsultarUm(book.AuthorId);
            var authorList = _authorRepository.ConsultarTodos().Result.ToList();
            ViewData["AuthorID"] = new SelectList(authorList, "Id",
                "Name", book.AuthorId);
            return View(book);
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookViewModel Book)
        {
            if (Book != null)
                try
                {
                    Book.BookAuthor = await _authorRepository.ConsultarUm(Book.AuthorId);
                    await _bookRepository.Atualizar(Book);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            var authorList = _authorRepository.ConsultarTodos().Result.ToList();
            ViewData["AuthorID"] = new SelectList(authorList, "Id",
                "Name", Book.AuthorId);
            return View(Book);
        }

        // GET: BookController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookRepository.ConsultarUm(id);
            if (book == null)
            {
                return NotFound();
            }
            book.BookAuthor = await _authorRepository.ConsultarUm(book.AuthorId);
            return View(book);
        }

        // POST: BookController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var delBook = await _bookRepository
                    .ConsultarUm(id);
                if (delBook == null) return NotFound();
                if (delBook.BookLoan != null) 
                { 
                    var loan = await _loanRepository.ConsultarUm((int)delBook.LoanId);
                    loan.BorrowEnd = DateTime.Now;
                    delBook.BookLoan = null;
                }
                await _bookRepository.Excluir(delBook);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
