using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rocky.Controllers
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
            IEnumerable<Category> list = _db.Category;
            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Rocky.Models.Category newCategory)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Add(newCategory);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(newCategory);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Delete(Category category)
        {
            IEnumerable<Category> list = _db.Category;
            Category _category = list.Where(x => x.Id == category.Id).FirstOrDefault();
            if (_category == null) return NotFound();
            _db.Category.Remove(_category);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var v = _db.Category.Find(id);

            if (v == null) return NotFound();
            return View(v);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Rocky.Models.Category newCategory)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Update(newCategory);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(newCategory);

        }
    }
}
