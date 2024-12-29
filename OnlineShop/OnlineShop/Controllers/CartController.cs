using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Helpers;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    public class CartController : Controller
    {
        private readonly OnlineShopContext _context;
        public CartController(OnlineShopContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<CartItem> CartItems = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");

            //顯示購物總金額
            if (CartItems != null)
            {
                //購物車List 的SubTotal(小計)總和(Sum)
                ViewBag.Total = CartItems.Sum(x => x.SubTotal);
               
            }
            else { ViewBag.Total = 0; }
            return View(CartItems);
        }

        public IActionResult AddtoCart(int id)
        {
            //取得商品資料
            var product = _context.Product.Single(x => x.Id.Equals(id));
            CartItem item = new CartItem()
            {
                ProductId = product.Id,
                Product = product,
                Amount = 1,
                SubTotal = product.Price,
                imageSrc = ViewImage(product.Image) 
            };
            //!!!!!!!!判斷 Session 內有無購物車
            if (SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart") == null)
            {
                //空 建立新的購物車
                List<CartItem> cart = new List<CartItem>();
                cart.Add(item);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            //如果已存在購物車 檢查有無相同的商品 只調數量
            else
            {
                
                List<CartItem> cart  = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
                //查出購物車中商品Id
                int index= cart.FindIndex(x => x.Product.Id.Equals(id));
                //如果有商品
                if (index != -1)
                {
                    cart[index].Amount += item.Amount;//自增數量
                    cart[index].SubTotal += item.SubTotal;//增加總額
                }
                else
                {
                    cart.Add(item);
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }

            return NoContent(); // HttpStatus 204: 請求成功但不更新畫面
        }

        public IActionResult RemoveItem(int id)
        {
            //向 Session 取得商品列表
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            //查詢目標在List裡的位置 然後刪掉
            int index =cart.FindIndex(x => x.Product.Id.Equals(id));
            cart.RemoveAt(index);
            //Count集合總數量 同JAVA中size()
            if (cart.Count < 1)
            {
                SessionHelper.Remove(HttpContext.Session, "cart");
            }
            else
            {
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }

            return RedirectToAction("Index");
        }

        //圖片轉換格式
        private string ViewImage(byte[] arrayImage)
        {
            string base64String = Convert.ToBase64String(arrayImage, 0, arrayImage.Length);
            return "data:image/png;base64," + base64String;
        }


    }
}
