using BibliotecaLog.Data;
using BibliotecaLog.Models;
using BibliotecaLog.Repository;
using BibliotecaLog.Repository.Interface;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Net;
using System.Net.WebSockets;
using System.Reflection.Emit;

namespace BibliotecaLog.Controllers
{
    public class BookLoanController : Controller
    {
        //CONFIG
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
            var loanlist = await _dbContext.Loans
                .Include(c => c.Student)
                .Include(d => d.Book)
                .AsNoTracking()
                .ToListAsync();
            return View( loanlist );
        }
        //a "Index" dos livros a emprestar
        public async Task<IActionResult> IndexLivros()
        {
            var books = await _dbContext.Books
                .Include(c => c.BookAuthor)
                .Include(l => l.BookLoan)
                .AsNoTracking()
                .ToListAsync();
            //foreach (var item in books) {
            //    foreach (var s in _dbContext.Loans)
            //    {
            //        if (item.LoanId == s.Id)
            //        {
            //            if (!item.BookLoan.Contains(s))
            //            {
            //                item.BookLoan.Add(s);
            //            }
            //        }
            //    }
            //} 
            return View(books);
        }
        // GET: BookLoanController/Edit/5
        //View do  pegar o livro emprestado
        
        public async Task<IActionResult> Create(int id, string StudentEmail)
        {
            var book = await _bookRepository.ConsultarTodos(b => b.Id == id);
            ViewData["BookId"] = new SelectList(book, "Id",
                "Title");
            var student = await _studentRepository.ConsultarTodos(n => n.Email == StudentEmail);
            ViewData["StudentID"] = new SelectList(student, "Id",
                "Email");
            return View();
        }

        // POST: BookLoanController/Edit/5
        //pegando o livro emprestado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Create(BookLoan loan)
        {
            var book = await _bookRepository.ConsultarTodos(b => b.Id == loan.BookId);
            ViewData["BookId"] = new SelectList(book, "Id",
                "Title", loan.BookId);

            var student = await _studentRepository.ConsultarTodos(n => n.Id == loan.StudentId);
            ViewData["StudentID"] = new SelectList(student, "Id",
                "Email", loan.StudentId);
            loan.Book = await _bookRepository.ConsultarUm(loan.BookId);
            loan.Book.IsBorrowed = true;
            loan.Book.LoanId = loan.Id;
            try
            {
                if (loan != null)
                {
                    await _loanRepository.Criar(new BookLoan { 
                        BookId = loan.BookId,
                        StudentId = loan.StudentId,
                        IsActive = true,
                        BorrowStart = loan.BorrowStart,
                        BorrowEnd = loan.BorrowEnd
                    });
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                RedirectToRoute(new { controller = "BookLoan", action = "Index", loan.StudentId });
            }
            return View();
        }

        public async Task<IActionResult> Devolver (int id, string StudentEmail)
        {
            var loan = await _loanRepository.ConsultarUm(id);
            var book = await _bookRepository.ConsultarTodos(b => b.Id == loan.BookId);
            ViewData["BookId"] = new SelectList(book, "Id",
                "Title");
            var student = await _studentRepository.ConsultarTodos(n => n.Email == StudentEmail);
            ViewData["StudentID"] = new SelectList(student, "Id",
                "StudentName");
            return View(loan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Devolver(BookLoan loan)
        {
            var bookl = await _bookRepository.ConsultarTodos(b => b.Id == loan.BookId);
            ViewData["BookId"] = new SelectList(bookl, "Id",
                "Title");
            var book = await _bookRepository.ConsultarUm(loan.BookId);
            loan.BorrowStart = loan.BorrowStart;
            loan.BorrowEnd = DateTime.Now;
            book.IsBorrowed = false;
            loan.IsActive = false;
            try
            {
                if (ModelState.IsValid)
                {
                    await _loanRepository.Atualizar(loan);
                    await _bookRepository.Atualizar(book);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index"});
            }
            return View();
        }
    }
}
