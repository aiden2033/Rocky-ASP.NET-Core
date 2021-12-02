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
        public IActionResult Create(Rocky.Models.ApplicationType newApplicationType)
        {
            if (ModelState.IsValid)
            {
                _db.ApplicationType.Add(newApplicationType);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else return View(newApplicationType);

        }

        public IActionResult Delete(ApplicationType applicationType)
        {
            IEnumerable<ApplicationType> list = _db.ApplicationType;
            ApplicationType application = list.Where(x => x.Id == applicationType.Id).FirstOrDefault();
            _db.ApplicationType.Remove(application);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var v = _db.ApplicationType.Find(id);

            if (v == null) return NotFound();
            return View(v);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Rocky.Models.ApplicationType applicationType)
        {
            if (ModelState.IsValid)
            {
                _db.ApplicationType.Update(applicationType);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationType);

        }
    }
}
