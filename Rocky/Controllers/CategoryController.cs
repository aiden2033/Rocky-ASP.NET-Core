using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using System.Collections;
using System.Collections.Generic;

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
        [HttpGet]
        public IActionResult Create()
        {
            IEnumerable<Category> list = _db.Category;
            return View();
        }
    }
}
