using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models
{
	public class Category
	{
        // [Key] // boshqa nomdagi maydon uchun primary key berish uchun.
        public int Id { get; set; } // Id yoki CategoryId bo'lsa, primary key avtomatik beriladi
        [Required]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }
    }
}   
