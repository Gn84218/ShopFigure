using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using OnlineShop.Areas.Identity.Data;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<OnlineShopUser> _userManager;
        private readonly OnlineShopContext _context;
        public HomeController(ILogger<HomeController> logger, OnlineShopContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // 查詢已付款的訂單項目及其相關的產品名稱
            var reportData = _context.OrderItem
                .Where(oi => _context.Order
                    .Any(o => o.Id == oi.OrderId && o.isPaid)) // 選擇已付款的訂單項目
                .Select(oi => new
                {
                    oi.ProductId,
                    oi.Amount,
                    oi.SubTotal,
                    ProductName = _context.Product
                        .Where(p => p.Id == oi.ProductId)
                        .Select(p => p.Name)
                        .FirstOrDefault() // 獲取產品名稱
                })
                .ToList();

            // 準備要傳遞給 View 的數據，這裡假設我們需要將結果放入 ViewBag 中
            ViewBag.ReportData = JsonSerializer.Serialize(reportData);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            // 查詢已付款的訂單項目及其相關的產品名稱
            var reportData = _context.OrderItem
                .Where(oi => _context.Order
                    .Any(o => o.Id == oi.OrderId && o.isPaid)) // 選擇已付款的訂單項目
                .Select(oi => new
                {
                    oi.ProductId,
                    oi.Amount,
                    oi.SubTotal,
                    ProductName = _context.Product
                        .Where(p => p.Id == oi.ProductId)
                        .Select(p => p.Name)
                        .FirstOrDefault() // 獲取產品名稱
                })
                .ToList();

            // 準備要傳遞給 View 的數據，這裡假設我們需要將結果放入 ViewBag 中
            ViewBag.ReportData = JsonSerializer.Serialize(reportData);

            return View();

        }
    }
}
