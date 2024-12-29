using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public   class Product
    {

        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }
        
        public int Price { get; set; }

        public int Stock { get; set; }

        public byte[] Image { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<Comment> Comments { get; set; }
    }

    public  class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }

    public class Comment
    {
        public int Id { get; set; }
        public string UserName { get; set; }  //留言者
        public string Content { get; set; }   //留言內容
        public DateTime Time { get; set; }    //留言時間
        public int ProductID { get; set; }    //商品編號
    }
}
