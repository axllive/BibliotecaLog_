using BibliotecaLog.Data;
using BibliotecaLog.Models;
using BibliotecaLog.Repository;
using BibliotecaLog.Repository.Interface;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection.Emit;

namespace BibliotecaLog.Controllers
{
    public class BookLoanController : Controller
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly DataContext _dbContext;
        public BookLoanController(IAuthorRepository authorRepository,
            IStudentRepository studentRepository, IBookRepository bookRepository
            ,ILoanRepository loanRepository, DataContext db) 
        {
            _authorRepository = authorRepository;
            _studentRepository = studentRepository;
            _bookRepository = bookRepository;
            _loanRepository = loanRepository;
            _dbContext = db; 
        }
        // GET: BookLoanController
        //A index aqui lista os empréstimos
        public async Task<IActionResult> Index()
        {
            var loanlist = _loanRepository.ConsultarTodos().Result.ToList();
            return View( loanlist );
        }

        // GET: BookLoanController/Create
        //a "create" lista os livros a emprestar
        public async Task<IActionResult> Create()
        {
            var books = await _dbContext.Books
                .Include(c => c.BookAuthor)
                .AsNoTracking()
                .ToListAsync();
            return View(books);
        }
        // GET: BookLoanController/Edit/5
        //a "edit" que vai efetivamente realizar o empréstimo do livro
        
        public async Task<IActionResult> Edit(int id, string StudentEmail)
        {
            var book = await _bookRepository.ConsultarTodos(b => b.Id == id);
            ViewData["AuthorID"] = new SelectList(book, "Id",
                "Title");
            return View();
        }

        // POST: BookLoanController/Edit/5
        //pegando o livro emprestado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Edit(BookLoan loan, int BookId, string StudentEmail)
        {
            var refBook = await _bookRepository.ConsultarUm(BookId);
            var refStudent = _studentRepository
                .ConsultarTodos(
                s => s.Email == StudentEmail)
                .Result.FirstOrDefault();
            if (loan == null ) return RedirectToRoute(new { controller = "Home", action = "Index" });
            if (loan != null)
            {
                loan.Student = refStudent;
                loan.StudentEmail = loan.StudentEmail;
                refBook.IsBorrowed = true;
                loan.Book = refBook;
                loan.BookId = refBook.Id;
                await _loanRepository.Criar(loan);
                return RedirectToRoute(new { controller = "BookLoan", action = "Index" });
            }
            else
            {
                return RedirectToRoute(new { controller = "BookLoan", action = "Index"});
            }
            var book = await _bookRepository.ConsultarTodos(b => b.Id == loan.BookId);
            ViewData["AuthorID"] = new SelectList(book, "Id",
                "Title");
            return RedirectToRoute(new { controller = "BookLoan", action = "Index" });
        }
    }
}
