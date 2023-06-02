using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bulky.Models
{
	public class Category
	{
        // [Key] // boshqa nomdagi maydon uchun primary key berish uchun.
        public int Id { get; set; } // Id yoki CategoryId bo'lsa, primary key avtomatik beriladi
        [Required]
        [MaxLength(30, ErrorMessage = "Ko'pi bilan 30 ta belgi")]
        [MinLength(2, ErrorMessage = "Kamida 2 ta belgi")]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "1 dan 100 gacha son qabul qilinadi")]
        public int DisplayOrder { get; set; }
    }
}   
