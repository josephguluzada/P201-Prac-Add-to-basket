using EduHomePrac.Models;
using EduHomePrac.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomePrac.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;

        public HomeController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Course> courses = _context.Courses.ToList();


            return View(courses);
        }

        public IActionResult Detail(int id)
        {
            Course course = _context.Courses.FirstOrDefault(x => x.Id == id);

            if (course == null) return NotFound();

            return PartialView("_CoursePartial", course);
        }

        public IActionResult AddToBasket(int id)
        {
            Course course = _context.Courses.FirstOrDefault(x => x.Id == id);

            if (course == null) return NotFound();

            BasketItemVM basketItemVM = null;

            List<BasketItemVM> books = new List<BasketItemVM>();
            string booksStr;

            if (HttpContext.Request.Cookies["Books"] != null)
            {
                booksStr = HttpContext.Request.Cookies["Books"];

                books = JsonConvert.DeserializeObject<List<BasketItemVM>>(booksStr);

                basketItemVM = books.FirstOrDefault(x => x.CourseId == id);
            }

            if (basketItemVM == null)
            {
                basketItemVM = new BasketItemVM
                {
                    CourseId = course.Id,
                    Title = course.Title,
                    Desc = course.Desc,
                    Image = course.Image,
                    Count = 1
                };

                books.Add(basketItemVM);
            }
            else
            {
                basketItemVM.Count++;
            }

            booksStr = JsonConvert.SerializeObject(books);

            HttpContext.Response.Cookies.Append("Books", booksStr);

            return RedirectToAction("index", "home");

        }

       

        

    }
}
