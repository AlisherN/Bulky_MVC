using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private IUnitOfWork _db;

        public ProductController(IUnitOfWork db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Product> products = _db.Product.GetAll().ToList();

            return View(products);
        }

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> categories = _db.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });
            //ViewBag.CategoryList = categories;
            //ViewData["CategoryList"] = categories;
            ProductVM productVM = new()
            {
                CategoryList = categories,
                Product = new Product()
            };
            return View(productVM);
        }

        [HttpPost]
        public IActionResult Create(ProductVM vm)
        {
            if (vm.Product.Title == null || vm.Product.Title.ToLower() == "test")
            {
                ModelState.AddModelError("", "Berilgan nom mahsulot uchun mos emas!");
            }

            if (ModelState.IsValid)
            {
                _db.Product.Add(vm.Product);
                _db.Save();
                TempData["success"] = "Product created successfully!";
                return RedirectToAction("Index");
            } else
            { // salashda xatolik bo'lganda qytib create viewni yangi model bilan ochish
                vm.CategoryList = _db.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                });
                return View(vm);
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            Product? product = _db.Product.Get(u => u.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _db.Product.Update(product);
                _db.Save();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteProduct(int? id)
        {
            Product product = _db.Product.Get(u =>u.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _db.Product.Remove(product);
            _db.Save();
            TempData["success"] = "Product removed successfully";

            return RedirectToAction("Index");
        }
    }
}
