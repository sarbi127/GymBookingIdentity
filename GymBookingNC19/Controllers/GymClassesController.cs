using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymBookingNC19.Core.Models;
using GymBookingNC19.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using GymBookingNC19.Core.ViewModels;

namespace GymBookingNC19.Controllers
{
    [Authorize]
    public class GymClassesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public GymClassesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        // GET: GymClasses
        [AllowAnonymous]
        public async Task<IActionResult> Index(IndexViewModel vm = null)
        {

            var model = new IndexViewModel();

            if (vm.History)
            {
                List<GymClass> gym = await GetHistoryAsync();

                model = new IndexViewModel { GymClasses = gym };
                return View(model);
            }

            List<GymClass> gymclasses = await GetAllWithUsers();

            var model2 = new IndexViewModel { GymClasses = gymclasses };

            return View(model2);
        }

        private async Task<List<GymClass>> GetAllWithUsers()
        {
            var gymclasses = await _context.GymClasses
                .Include(g => g.AttendingMembers)
                .ThenInclude(a => a.ApplicationUser)
                //  .IgnoreQueryFilters()
                .ToListAsync();
            return gymclasses;
        }

        private async Task<List<GymClass>> GetHistoryAsync()
        {
            return await _context.GymClasses
           .Include(g => g.AttendingMembers)
           .ThenInclude(a => a.ApplicationUser)
           .IgnoreQueryFilters()
           .Where(g => g.StartDate < DateTime.Now)
           .ToListAsync();
        }

        [Authorize(Roles = "Member")]



        public async Task<IActionResult> GetBookings()



        {

            var userId = userManager.GetUserId(User);
            List<GymClass> model = await GetAllBookingsAsync(userId);

            return View(model);

        }

        private async Task<List<GymClass>> GetAllBookingsAsync(string userId)
        {
            return await _context.ApplicationUserGymClasses
                .Where(ag => ag.ApplicationUserId == userId)
                .IgnoreQueryFilters()
                .Select(ag => ag.GymClass)
                .ToListAsync();
        }

        [Authorize(Roles ="Member")]
        public async Task<IActionResult> BookingToogle(int? id)
        {
            if (id == null) return NotFound();

            //Hämta den inloggade användarens id
            // var userId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var userId = userManager.GetUserId(User);
            GymClass currentGymClass = await GetWithMembersAsync(id);

            //Är den aktuella inloggade användaren bokad på passet?
            var attending = currentGymClass.AttendingMembers
                .FirstOrDefault(u => u.ApplicationUserId == userId);

            //Om inte, boka användaren på passet
            if (attending == null)
            {
                var book = new ApplicationUserGymClass
                {
                    ApplicationUserId = userId,
                    GymClassId = currentGymClass.Id
                };

                _context.ApplicationUserGymClasses.Add(book);
                _context.SaveChanges();
            }

            //Annars avboka
            else
            {
                _context.ApplicationUserGymClasses.Remove(attending);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));

        }

        private async Task<GymClass> GetWithMembersAsync(int? id)
        {

            //Hämta aktuellt gympass
            //Todo: Remove butten in ui if passs history!!!
            return await _context.GymClasses
                .Include(a => a.AttendingMembers)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        // GET: GymClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClasses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        // GET: GymClasses/Create
        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: GymClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,Duration,Description")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gymClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gymClass);
        }

        // GET: GymClasses/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClasses.FindAsync(id);
            if (gymClass == null)
            {
                return NotFound();
            }
            return View(gymClass);
        }

        // POST: GymClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,Duration,Description")] GymClass gymClass)
        {
            if (id != gymClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gymClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymClassExists(gymClass.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gymClass);
        }

        // GET: GymClasses/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GymClass gymClass = await GetAsync(id);

            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        private async Task<GymClass> GetAsync(int? id)
        {
            return await _context.GymClasses
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        // POST: GymClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gymClass = await GetAsync(id);

            _context.GymClasses.Remove(gymClass);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GymClassExists(int id)
        {
            return GetAny(id);
        }

        private bool GetAny(int id)
        {
            return _context.GymClasses.Any(e => e.Id == id);
        }
    }
}
