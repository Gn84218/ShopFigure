using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace OnlineShop.Areas.Identity.Data
{
    public class OnlineShopUserRole 
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "角色名稱是必填的")]
        public string RoleName { get; set; }

    }
    public class OnlineShopUserViewModel
    {
        public OnlineShopUser User { get; set; }
        public string RoleName { get; set; }
    }
}
