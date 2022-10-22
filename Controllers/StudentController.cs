using BibliotecaLog.Data;
using BibliotecaLog.Models;
using BibliotecaLog.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaLog.Controllers
{
    public class StudentController : Controller
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly DataContext _dbContext;
        public StudentController(IAuthorRepository authorRepository,
            IStudentRepository studentRepository, IBookRepository bookRepository
            , ILoanRepository loanRepository, DataContext db)
        {
            _authorRepository = authorRepository;
            _studentRepository = studentRepository;
            _bookRepository = bookRepository;
            _loanRepository = loanRepository;
            _dbContext = db;
        }
        // GET: StudentController
        public async Task<IActionResult> Index()
        {
            var list = await _dbContext.Students
                .Include(l => l.Loans)
                .AsNoTracking()
                .ToListAsync();
            return View(list);
        }

        // GET: StudentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StudentController/Create
        public ActionResult Create(string email)
        {
            var student = new StudentViewModel();
            student.Email = email;
            return View(student);
        }

        // POST: StudentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentViewModel student)
        {
            try
            {
                if (student != null)
                {
                    Random r = new Random();
                    student.EnrollmentNumber = r.Next(1, 10000);
                    await _studentRepository.Criar(student);
                    return RedirectToAction(nameof(Index));
                }
                else
                    return View(student);
            }
            catch
            {
                return View();
            }
        }

        // GET: StudentController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _studentRepository.ConsultarUm(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: StudentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StudentViewModel student)
        {
            
            try
            {
                if (student == null)
                {
                    return NotFound();
                }
                await _studentRepository.Atualizar(student);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StudentController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _studentRepository.ConsultarUm(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: StudentController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var student = await _studentRepository
                    .ConsultarUm(id);
                if (student == null) return NotFound();
               
                if (student.Loans != null)
                {
                    var loans = student.Loans.ToList();
                    if (loans.Any())
                        foreach (var item in loans)
                        {
                            await _loanRepository.Excluir(item);
                        }

                }
                
                await _studentRepository.Excluir(student);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
