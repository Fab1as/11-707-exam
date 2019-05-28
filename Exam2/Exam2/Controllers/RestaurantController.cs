using Exam2.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam2.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RestaurantController(ApplicationDbContext context)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        //[Authorize]
        public IActionResult GetDishesFromRestaurant(int? restaurantId)
        {
            if (restaurantId == null)
            {
                return NotFound();
            }

            var dishes = _context.Dishes.Where(x => x.RestaurantId == restaurantId);
            var restaurant = _context.Restaurants.Where(x => x.Id == restaurantId).FirstOrDefault();
            ViewData["CategoryName"] = restaurant.Name;
            ViewData["CategoryId"] = restaurant.Id;
            return View(dishes);
        }


    }
}
