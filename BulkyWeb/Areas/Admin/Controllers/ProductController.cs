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

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> products = _db.Product.GetAll(includeProperties: "Category").ToList();

            return View(products);
        }

        public IActionResult Upsert(int? id)
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

            if (id == null || id == 0)
            {
                return View(productVM);
            } else
            {
                productVM.Product = _db.Product.Get(u => u.Id == id);
                return View(productVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM vm, IFormFile? file)
        {
            if (vm.Product.Title == null || vm.Product.Title.ToLower() == "test")
            {
                ModelState.AddModelError("", "Berilgan nom mahsulot uchun mos emas!");
            }

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); // random name for a file
                    string productPath = Path.Combine(wwwRootPath, @"images/product");

                    if (!string.IsNullOrEmpty(vm.Product.ImageUrl))
                    {
                        // delete the old image
                        var oldImagePath = Path.Combine(wwwRootPath, vm.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    vm.Product.ImageUrl = @"\images\product\" + fileName;
                }

                if (vm.Product.Id == 0)
                {
                    _db.Product.Add(vm.Product);
                }
                else
                {
                    _db.Product.Update(vm.Product);
                }

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
        // Tepada Upset metodi yaratilgani uchun, endi bu metod shart emas
/*        public IActionResult Edit(int? id)
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
        }*/

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
