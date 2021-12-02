using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using Rocky.Data;
using Rocky.Models;
using Rocky.Models.ViewModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Rocky.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> list = _db.Product.Include(x=>x.Category).Include(x=>x.ApplicationType);
            //foreach (var v in list)
            //{
            //    v.Category = _db.Category.FirstOrDefault(x => x.Id == v.CategoryId);
            //    v.ApplicationType = _db.ApplicationType.FirstOrDefault(x => x.Id == v.ApplicationTypeId );
            //}

            return View(list);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            /*IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(x => new SelectListItem
            //{
            //    Text = x.Name,
            //    Value = x.Id.ToString()
            //});
            ViewBag.CategoryDropDown = CategoryDropDown;*/

            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _db.Category.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                ApplicationTypeList = _db.ApplicationType.Select(x=> new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })

            };

            var product = new Product();
            if (id == null)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _db.Product.Find(id);
                if (productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Rocky.Models.ViewModels.ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                if (productVM.Product.Id == 0) //create
                {
                    string uploadPath = webRootPath + WC.ImagePath;
                    string filename = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (FileStream fs = new FileStream(Path.Combine(uploadPath, filename + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fs);
                    }

                    productVM.Product.Image = filename + extension;
                    _db.Product.Add(productVM.Product);
                }
                else //update
                {
                    var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(x => x.Id == productVM.Product.Id);
                    if (files.Count > 0)
                    {
                        string uploadPath = webRootPath + WC.ImagePath;
                        string filename = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);
                        var oldFile = Path.Combine(uploadPath, objFromDb.Image);

                        if (System.IO.File.Exists(oldFile)) System.IO.File.Delete(oldFile);

                        using (FileStream fs = new FileStream(Path.Combine(uploadPath, filename + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fs);
                        }
                        productVM.Product.Image = filename + extension;
                    }
                    else
                    {
                        productVM.Product.Image = objFromDb.Image;
                    }
                    _db.Product.Update(productVM.Product);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            productVM.CategorySelectList = _db.Category.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            productVM.ApplicationTypeList = _db.ApplicationType.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View(productVM);
        }


        public IActionResult Delete(Product product)
        {
            string webRootPath = _webHostEnvironment.WebRootPath+WC.ImagePath;
            IEnumerable<Product> list = _db.Product;
            Product _product = list.FirstOrDefault(x => x.Id == product.Id);
            if (_product == null) return NotFound();
            if (System.IO.File.Exists(webRootPath + _product.Image)) System.IO.File.Delete(_product.Image);
            _db.Product.Remove(_product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
