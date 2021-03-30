using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AnimalChip.Data;
using AnimalChip.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace AnimalChip.Controllers
{

    //[Authorize]
    public class AnimalsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMemoryCache memoryCache;

        public AnimalsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IMemoryCache memoryCache)
        {
            _context = context;
            _userManager = userManager;

            this._context = context;
            this.memoryCache = memoryCache;


        }

        [Authorize(Roles = "Administrator")]
        // GET: Animals
        public async Task<IActionResult> Index()
        {

            List<Animal> animals;

            var stopwatch = new Stopwatch();
            stopwatch.Start();


            if (!memoryCache.TryGetValue("Animals", out animals))
            {

                memoryCache.Set("Animals", _context.Animal.ToList());
            }

            animals = memoryCache.Get("Animals") as List<Animal>;

            stopwatch.Stop();

            ViewBag.TotalTime = stopwatch.Elapsed;
            ViewBag.TotalRows = animals.Count;

            // return View(await _context.Animal.ToListAsync());

            return View(animals);

        }
        [Authorize]
        // GET: Animals IndexYourAnimal
        public async Task<IActionResult> IndexYourAnimal()
        {
            // var user = await _userManager.GetUserAsync(HttpContext.User);
            //var email = user.Email;
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            stopwatch.Stop();

            ViewBag.TotalTime = stopwatch.Elapsed;

            return View("Index", await _context.Animal.Where(j => j.Email.Equals(userEmail)).ToListAsync());




        }

        // GET: Animals/Search
        public async Task<IActionResult> Search()
        {
            return View();
        }

        // GET: Animals/AdvancedSearch
        public async Task<IActionResult> AdvancedSearch()
        {
            return View();
        }

        [HttpPost]
        // POST: Animals/ShowResults
        public async Task<IActionResult> ShowResults(String SearchPhrase)
        {
            if (SearchPhrase == null)
            {
                TempData["MessageNotNumeric"] = $"{("Nothing entred.")}";
                return RedirectToAction(nameof(Search));
            }
            else if (SearchPhrase.All(char.IsDigit))
            {
                TempData["Message"] = $"{("#") + SearchPhrase}";
                return View("IndexSearch", await _context.Animal.Where(j => j.Chip.Equals(SearchPhrase)).ToListAsync());
            }
            TempData["MessageNotNumeric"] = $"{("Did you try to put not a numeric Chip? Please use only digits.")}";
            return RedirectToAction(nameof(Search));

        }

        // POST: Animals/ShowResultsAdvanced
        public async Task<IActionResult> ShowResultsAdvanced(String SearchPhrase)
        {

            if (SearchPhrase == null)
            {
                TempData["MessageNotNumeric"] = $"{("Nothing entred.")}";
                return RedirectToAction(nameof(Search));
            }
            else if (SearchPhrase.All(char.IsDigit))
            {
                TempData["Message"] = $"{("#") + SearchPhrase}";
                return View("IndexAdvancedSearch", await _context.Animal.Where(j => j.Chip.Equals(SearchPhrase)).ToListAsync());

            }
            TempData["MessageNotNumeric"] = $"{("Did you try to put not a numeric Chip? Please use only digits.")}";
            return RedirectToAction(nameof(Search));
        }

        [Authorize]
        // GET: Animals/ DetailsAdvacedSearch
        public async Task<IActionResult> DetailsAdvacedSearch(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            var animal = await _context.Animal
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animal == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            return View(animal);
        }

        [Authorize]
        // GET: Animals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            var animal = await _context.Animal
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animal == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            return View(animal);
        }

        [Authorize]
        // GET: Animals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Animals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Kind,Breed,Chip,BirthDate,ModifiedTime,Contact,Email")] Animal animal)
        {

            //var user = await _userManager.GetUserAsync(HttpContext.User);

            //var email = user.Email;
            try
            {
                Animal existingAnimal = await _context.Animal.SingleOrDefaultAsync(m => m.Chip == animal.Chip);

                if (existingAnimal != null)
                {
                    // The employee already exists.
                    // Do whatever you need to do - This is just an example.
                    ModelState.AddModelError(string.Empty, "Animal with given chip already exist. Please use another number.");

                }
                else if (ModelState.IsValid)
                {
                    var userEmail = User.FindFirstValue(ClaimTypes.Email);
                    animal.Email = userEmail;
                    animal.ModifiedTime = DateTime.Now;
                    _context.Add(animal);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = $"Animal: {animal.Name} was created";
                    return RedirectToAction(nameof(IndexYourAnimal));
                }
                return View(animal);

            }
            catch (Exception)
            {

                ModelState.AddModelError(string.Empty, "Animal with given chip already exist. Please use another number.");
                return View(animal);
            }


        }


        // GET: Animals/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            var animal = await _context.Animal.FindAsync(id);
            if (animal == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            TempData["Message"] = $"Animal: {animal.Name} was edited";

            return View(animal);
        }


        // POST: Animals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Kind,Breed,Chip,BirthDate,ModifiedTime,Contact")] Animal animal)
        {
            if (id != animal.Id)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(animal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimalExists(animal.Id))
                    {
                        Response.StatusCode = 404;
                        return View("NotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(animal);
        }

        // GET: Animals/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            var animal = await _context.Animal
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animal == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
            TempData["Message"] = $"Animal: {animal.Name}, was deleted";

            return View(animal);
        }

        // POST: Animals/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var animal = await _context.Animal.FindAsync(id);
            _context.Animal.Remove(animal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnimalExists(int id)
        {
            return _context.Animal.Any(e => e.Id == id);
        }
    }
}
