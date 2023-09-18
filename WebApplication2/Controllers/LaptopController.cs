using Humanizer.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using WebApplication2.Context;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class LaptopController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public LaptopController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Index()
        {
            var Data = _context.Laptops.ToList();
            return View(Data);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Laptop laptop)
        {
            try
            {

                string uniqueFileName = UploadImage(laptop);
                var data = new Laptop()
                {
                    Name = laptop.Name,
                    Email = laptop.Email,
                    FatherName = laptop.FatherName,
                    Path = uniqueFileName
                };
                _context.Laptops.Add(data);
                _context.SaveChanges();
                TempData["Success"] = "Record successfully Save!";
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

            }
            return View();
        }
        private string UploadImage(Laptop laptop)
        {
            string uniqueFileName = string.Empty;
            if (laptop.FormFile != null)
            {
                string UploadFolder = Path.Combine(_environment.WebRootPath, "Contant/Laptop/");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + laptop.FormFile.FileName;
                string filepath = Path.Combine(UploadFolder, uniqueFileName);
                using (var filestream = new FileStream(filepath, FileMode.Create))
                {
                    laptop.FormFile.CopyTo(filestream);

                }
            }
            return uniqueFileName;
        }

        public IActionResult Delete(int Id)
        {
            if (Id == 0)
            {
                return NotFound();
            }
            else
            {
                var Data = _context.Laptops.Where(x => x.Id == Id).SingleOrDefault();
                if (Data != null)
                {
                    string deleteFromfolder = Path.Combine(_environment.WebRootPath, "Contant/Laptop/");
                    string CurrentImage = Path.Combine(Directory.GetCurrentDirectory(), deleteFromfolder, Data.Path);
                    if (CurrentImage != null)
                    {
                        if (System.IO.File.Exists(CurrentImage))
                        {
                            System.IO.File.Delete(CurrentImage);
                        }
                    }
                    _context.Laptops.Remove(Data);
                    _context.SaveChanges();
                    TempData["Success"] = "Record Deleted!";
                }
            }
            return RedirectToAction("Index");


        }

        public IActionResult Details(int Id)
        {
            if (Id == 0)
            {
                return NotFound();
            }

            var Data = _context.Laptops.Where(x => x.Id == Id).SingleOrDefault();
            return View(Data);
        }
        [HttpGet]
        public IActionResult Edit(int Id)
        {

            var Data = _context.Laptops.Where(x => x.Id == Id).SingleOrDefault();
            if (Data != null)
            {

                return View(Data);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Edit(Laptop model)
        {
           // ModelState.Remove("FormFile");
            if (ModelState.IsValid) { }
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _context.Laptops.Where(e => e.Id == model.Id).SingleOrDefault();
                    string uniquefileName = string.Empty;
                    if (model.FormFile != null)
                    {
                        if (data.Path != null)
                        {
                            string filepath = Path.Combine(_environment.WebRootPath, "Contant/Laptop", data.Path);
                            if (System.IO.File.Exists(filepath))
                            {
                                System.IO.File.Delete(filepath);
                            }
                        }
                        uniquefileName = UploadImage(model);
                    }

                    data.Name = model.Name;
                    data.FatherName=model.FatherName;
                    data.Email=model.Email;
                    if(model.FormFile!= null)
                    {
                        data.Path = uniquefileName;
                    }
                    _context.Laptops.Update(data);
                    _context.SaveChanges();
                    TempData["success"] = "Record Updated Successfully!";
                   return RedirectToAction("Index");
 
                }
                else
                {
                    return View("model");
                }


            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return RedirectToAction("Index");   
        }
    }
}
