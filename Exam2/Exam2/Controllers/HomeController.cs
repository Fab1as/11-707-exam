using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Exam2.Models;
using Exam2.Data;
using Exam2.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Exam2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IActionResult> Index()
        {
            return View(_context.Restaurants);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetDishesFromRestaurant(int? restaurantId)
        {
            if (restaurantId == null)
            {
                return NotFound();
            }

            var dishes = _context.Dishes.Where(x => x.RestaurantId == restaurantId);
            var restaurant = _context.Restaurants.Where(x => x.Id == restaurantId).FirstOrDefault();
            ViewData["RestaurantName"] = restaurant.Name;
            ViewData["RestaurantId"] = restaurant.Id;
            return View(dishes);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
