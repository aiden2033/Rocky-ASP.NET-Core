using Microsoft.AspNetCore.Mvc;
using Rocky.Models;
using Rocky.Data;
using System.Collections.Generic;
using System.Linq;

namespace Rocky.Controllers
{
    public class ApplicationTypeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ApplicationTypeController(ApplicationDbContext db)
        {
            this._db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<ApplicationType> list = _db.ApplicationType;
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Rocky.Models.ApplicationType newCategory)
        {
            _db.ApplicationType.Add(newCategory);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(ApplicationType applicationType)
        {
            IEnumerable<ApplicationType> list = _db.ApplicationType;
            ApplicationType application = list.Where(x => x.Id == applicationType.Id).FirstOrDefault();
            _db.ApplicationType.Remove(application);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
