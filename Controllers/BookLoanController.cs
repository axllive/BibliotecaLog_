using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaLog.Controllers
{
    public class BookLoanController : Controller
    {
        // GET: BookLoanController
        public ActionResult Index()
        {
            return View();
        }

        // GET: BookLoanController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BookLoanController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BookLoanController/Create
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

        // GET: BookLoanController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BookLoanController/Edit/5
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

        // GET: BookLoanController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BookLoanController/Delete/5
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
