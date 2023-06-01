﻿using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ApplicationDbContext _db;

		public CategoryController(ApplicationDbContext db) 
		{
			_db = db;
		}

		public IActionResult Index()
		{
			List<Category> categoryList = _db.categories.ToList();
			return View(categoryList);
		}

		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
        public IActionResult Create(Category category)
        {
			_db.categories.Add(category);
			_db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}