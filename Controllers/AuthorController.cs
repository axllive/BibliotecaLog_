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

        public AuthorController(DataContext dataContext, IAuthorRepository authorRep)
        {
            _authorRepository = authorRep;
        }
        // GET: AuthorController
        public async Task<IActionResult> Index()
        {
            return View(await _authorRepository.Authors.ToListAsync() );
        }

        // GET: AuthorController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var author = await _dataContext.Authors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }
            var BookList = await _bookRepository.ConsultarTodos();
            return View();
        }

        // GET: AuthorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuthorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthorController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AuthorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthorController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AuthorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
