using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Areas.Identity.Data;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    [Authorize(Roles = "Administrator")]//驗證 管理員身分使用
    public class UserController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager; //類似JAVA中 final
        private readonly UserManager<OnlineShopUser> _userManager;
        

        public UserController(RoleManager<IdentityRole> roleManager,
                              UserManager<OnlineShopUser> userManager)
        {
            this._roleManager = roleManager; // 宣告roleManager
            this._userManager = userManager;
        }

        //角色列表顯示
        [HttpGet]
        public IActionResult RoleList()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }
       
        //角色創建
        [HttpGet]
        public IActionResult RoleCreate()
        {
            return View();
        }        
        [HttpPost]
        public async Task<IActionResult> RoleCreate(OnlineShopUserRole userRole)
        {
            var roleExist = await _roleManager.RoleExistsAsync(userRole.RoleName); //判斷角色是否已存在
            if (!roleExist)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(userRole.RoleName));
            }
            return View();
        }
       
        //角色改名
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);//根據id查找

            if (role == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.users = await _userManager.GetUsersInRoleAsync(role.Name);
            }
            return View(role);
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(IdentityRole role)
        {
            if (role == null)
            {
                return NotFound();
            }
            else
            {
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }

        public async Task<IActionResult> ListUsers()
        {
            //創建一個集合(使用者訊息和角色)
            List<OnlineShopUserViewModel> userViewModels = new List<OnlineShopUserViewModel>();
            //取出數據所有使用者名
            var AllUsers=_userManager.Users.ToList();
            //歷遍所有使用者名
            foreach (var user in AllUsers)
            {
                //集合裡加入 使用者名,綁定角色名
                userViewModels.Add(new OnlineShopUserViewModel
                {
                    User = user,
                    //第一個參數 指定連接時使用的分隔符
                    RoleName = string.Join("",await _userManager.GetRolesAsync(user))
                });
            }
            return View(userViewModels);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user =await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        { 
            var user=await _userManager.FindByIdAsync(id); 
            var userDelete = await _userManager.DeleteAsync(user);
            //如果刪除成功
            if (userDelete.Succeeded)
            {
                //定向到ListUsers()
                return RedirectToAction("ListUsers");
            }
            foreach (var error in userDelete.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View("ListUsers");
        }
    }
}
