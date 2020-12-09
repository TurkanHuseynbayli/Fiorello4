using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloBack.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="nese yaz"),StringLength(30,ErrorMessage ="30 dan kece bilmezsen")]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedTime { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
