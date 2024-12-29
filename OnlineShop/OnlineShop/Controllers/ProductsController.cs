using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using OnlineShop.Data;
using OnlineShop.Models;
using static System.Net.Mime.MediaTypeNames;

namespace OnlineShop.Controllers
{
    
    public class ProductsController : Controller
    {
        private readonly OnlineShopContext _context;

        public ProductsController(OnlineShopContext context)
        {
            _context = context;
            
        }

        // GET: Products
        public async Task<IActionResult> Index(int? cId)
        {
            List<DetailViewModel> dvm = new List<DetailViewModel>();
            List<Product> products = new List<Product>();

            //判斷如果有傳入類別編號，就篩選那個類別的商品出來
            if (cId != null)
            {
                //查詢對應類別  如果找不到符合Single拋出異常 
                var result = _context.Category.Single(x => x.Id.Equals(cId));
                //查詢該類別下的商品  .Query()取得查詢ABLE物件。
                products = _context.Entry(result).Collection(x => x.Products).Query().ToList();
            }
            else
            {
                //查詢所有商品（無類別過濾）Include包含
                products = _context.Product.Include(p => p.Category).ToList();
            }

            //把取出來的資料加入ViewModel  
            foreach (var product in products)
            {
                DetailViewModel item = new DetailViewModel
                {
                    product = product,
                    imgsrc = ViewImage(product.Image)
                };
                dvm.Add(item);
            }
            ViewBag.count = dvm.Count;

            return View(dvm);


        }

        // GET: Products/Details/5
        [Authorize(Roles = "Administrator")]//驗證 管理員身分使用
        public async Task<IActionResult> Details(int? id)
        {
            DetailViewModel dvm = new DetailViewModel();  //建立一個 ViewModel
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }
            //查出所有留言內容
            await _context.Product.Include(c => c.Comments).FirstOrDefaultAsync(m => m.Id == id);
            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                dvm.product = product;
                if (product.Image != null)
                {
                    dvm.imgsrc = ViewImage(product.Image);
                }
            }

            return View(dvm);
        }
        private string ViewImage(byte[] arrayImage)
        {
            // 二進位圖檔轉字串
            string base64String = Convert.ToBase64String(arrayImage, 0, arrayImage.Length);
            return "data:image/png;base64," + base64String;
        }

        // GET: Products/Create
        [Authorize(Roles = "Administrator")]//驗證 管理員身分使用
        public IActionResult Create()
        {   
            //下拉選單
            ViewData["Categories"] = new SelectList(_context.Set<Category>(), "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]//驗證 管理員身分使用
        public async Task<IActionResult> Create( Product product, IFormFile Image)
        {
              
                if (Image != null)//圖片不為空 轉換成陣列
                {
                    using (var ms = new MemoryStream())
                    {
                        Image.CopyTo(ms);
                        product.Image = ms.ToArray();
                    }
                }
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
    
           // ViewData["Categories"] = new SelectList(_context.Set<Category>(), "Id", "Name", product.CategoryId);
           // return View(product);

        }


        // GET: Products/Edit/5
        [Authorize(Roles = "Administrator")]//驗證 管理員身分使用
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]//驗證 管理員身分使用
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Administrator")]//驗證 管理員身分使用
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [Authorize(Roles = "Administrator")]//驗證 管理員身分使用
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'OnlineShopContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Product?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // GET: Products/CreateCategory
        public IActionResult CreateCategory()
        {
            return View();
        }
        // POST: Products/CreateCategory
        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            _context.Category.Add(category);
            await _context.SaveChangesAsync();
            return View();
        }

        [Authorize(Roles = "Administrator")]//驗證 管理員身分使用
        public async Task<IActionResult> Test()
        {
            var products = await _context.Product
                                   .Include(p => p.Category) // 加載關聯的 Category 資料
                                   .ToListAsync();



            return products != null ?
               View(products) :
               Problem("Entity set 'OnlineShopContext.Product'  is null.");


        }

        //留言板
        [HttpPost]
        [Authorize]  //一定要登入才能留言
        public async Task<IActionResult> AddComment(int Id,string myComment)
        {
            var comment = new Comment()
            {
                ProductID = Id,
                Content = myComment,
                UserName = HttpContext.User.Identity.Name,//取得登入中的帳號
                Time = DateTime.Now//取得當下時間
            };
            _context.Add(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = Id });
        }
    }
}
