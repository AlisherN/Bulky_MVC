using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ICategoryRepository _categoryRepository;

		public CategoryController(ICategoryRepository db) 
		{
			_categoryRepository = db;
		}

		public IActionResult Index()
		{
			List<Category> categoryList = _categoryRepository.GetAll().ToList();
			return View(categoryList);
		}

		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
        public IActionResult Create(Category category)
        {
			if (category.Name == category.DisplayOrder.ToString())
			{
				ModelState.AddModelError("name", "Name va Display Order bir xil bo'la olmaydi");
			}

			if (category.Name == null || category.Name.ToLower() == "test")
			{
				ModelState.AddModelError("", "Berilgan nom kategoriya uchun mos emas!");
			}

			if (ModelState.IsValid)
			{
                _categoryRepository.Add(category);
                _categoryRepository.Save();
				TempData["success"] = "Category created successfully!";
                return RedirectToAction("Index");
            }

			return View();
        }

		public IActionResult Edit(int? id)
		{
			if (id == null || id <= 0) 
			{
				return NotFound();
			}

            Category? category = _categoryRepository.Get(u => u.Id == id);
			//Category? category1 = _db.categories.FirstOrDefault(c => c.Name == "Thriller"); // 2-usul
			//Category? category1 = _db.categories.Where(c => c.Id == id).FirstOrDefault(); // 3-usul

			if (category == null)
			{
				NotFound();
			}

            return View(category);
		}

		[HttpPost]
		public IActionResult Edit(Category category)
		{
			if (ModelState.IsValid) 
			{
				_categoryRepository.Update(category);
				_categoryRepository.Save();
                TempData["success"] = "Category updated successfully!";
                return RedirectToAction("Index");
            }
			return View();
		}

		[HttpPost, ActionName("Delete")]
		public IActionResult DeletePost(int? id)
		{
			Category? category = _categoryRepository.Get(u => u.Id == id);
			if (category == null)
			{
				NotFound();
			}

			_categoryRepository.Remove(category);
			_categoryRepository.Save();
            TempData["success"] = "Category deleted successfully!";

            return RedirectToAction("Index");
		}
    }
}
