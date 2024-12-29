using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;

namespace OnlineShop.Controllers
{
    public class TestController1 : Controller
    {
        private readonly OnlineShopContext _context;

        public TestController1(OnlineShopContext context)
        {
            _context = context;
        }

        // GET: TestController1
        public async Task<IActionResult> Index()
        {
            return _context.Product != null ?
                       View(await _context.Product.ToListAsync()) :
                      Problem("Entity set 'OnlineShopContext.Product'  is null.");

        }


        // GET: TestController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TestController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TestController1/Create
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

        // GET: TestController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TestController1/Edit/5
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

        // GET: TestController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TestController1/Delete/5
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
