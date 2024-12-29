using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using OnlineShop.Areas.Identity.Data;
namespace OnlineShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<OnlineShopContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("OnlineShopContext") ?? throw new InvalidOperationException("Connection string 'OnlineShopContext' not found.")));

            builder.Services.AddDbContext<OnlineShopUserContext>(options =>
                  options.UseSqlServer(builder.Configuration.GetConnectionString("OnlineShopUserContextConnection")));

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseSqlServer(builder.Configuration.GetConnectionString("OnlineShopUserContextConnection")));

            builder.Services.AddDefaultIdentity<OnlineShopUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequiredLength = 4;             //密碼長度
                options.Password.RequireLowercase = false;       //包含小寫英文
                options.Password.RequireUppercase = false;       //包含大寫英文
                options.Password.RequireNonAlphanumeric = false; //包含符號
                options.Password.RequireDigit = false;           //包含數字
             })
                .AddRoles<IdentityRole>() //角色
                .AddEntityFrameworkStores<OnlineShopUserContext>();

           
            //builder.Services.AddDistributedMemoryCache(); // 使用內存緩存


            //啟用 Session
            builder.Services.AddSession();


            // 添加 Razor Pages 支持
            builder.Services.AddRazorPages();


            // 註冊 MVC 與 Razor 頁面服務
            builder.Services.AddControllersWithViews();
            //自訂編碼器
            builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs }));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            

            app.UseHttpsRedirection();//確保通訊加密
            app.UseStaticFiles();//啟用靜態檔案的提供功能 圖 CSS JS檔

            app.UseRouting();//啟用路由系統
            app.UseAuthentication();//身份驗證功能
            app.UseAuthorization();//啟用授權功能

            app.MapRazorPages();//啟用 Razor Pages 路由支持  自動匹配 專案中位於 Pages 資料夾中的 Razor Pages 檔案（.cshtml）
            
            app.UseSession();//f啟用Session

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
              name: "Products",
             pattern: "{controller=Products}/{action=Index}/{id?}");
            app.MapControllerRoute(
             name: "User",
            pattern: "{controller=User}/{action=RoleEdit}/{id?}");

            app.Run();
        }
    }
}