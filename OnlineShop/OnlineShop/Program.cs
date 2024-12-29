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
                options.Password.RequiredLength = 4;             //�K�X����
                options.Password.RequireLowercase = false;       //�]�t�p�g�^��
                options.Password.RequireUppercase = false;       //�]�t�j�g�^��
                options.Password.RequireNonAlphanumeric = false; //�]�t�Ÿ�
                options.Password.RequireDigit = false;           //�]�t�Ʀr
             })
                .AddRoles<IdentityRole>() //����
                .AddEntityFrameworkStores<OnlineShopUserContext>();

           
            //builder.Services.AddDistributedMemoryCache(); // �ϥΤ��s�w�s


            //�ҥ� Session
            builder.Services.AddSession();


            // �K�[ Razor Pages ���
            builder.Services.AddRazorPages();


            // ���U MVC �P Razor �����A��
            builder.Services.AddControllersWithViews();
            //�ۭq�s�X��
            builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs }));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            

            app.UseHttpsRedirection();//�T�O�q�T�[�K
            app.UseStaticFiles();//�ҥ��R�A�ɮת����ѥ\�� �� CSS JS��

            app.UseRouting();//�ҥθ��Ѩt��
            app.UseAuthentication();//�������ҥ\��
            app.UseAuthorization();//�ҥα��v�\��

            app.MapRazorPages();//�ҥ� Razor Pages ���Ѥ��  �۰ʤǰt �M�פ���� Pages ��Ƨ����� Razor Pages �ɮס].cshtml�^
            
            app.UseSession();//f�ҥ�Session

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