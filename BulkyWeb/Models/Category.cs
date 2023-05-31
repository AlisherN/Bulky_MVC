using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models
{
	public class Category
	{
        // [Key] // boshqa nomdagi maydon uchun primary key berish uchun.
        public int Id { get; set; } // Id yoki CategoryId bo'lsa, primary key avtomatik beriladi
        [Required]
        public string Name { get; set; }

        public int DisplayOrder { get; set; }
    }
}   
