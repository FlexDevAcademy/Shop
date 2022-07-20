using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    public class ShoppingBagController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;
        public ShoppingBagController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ShoppingBag
        public async Task<IActionResult> Index()
        {
            //var user = await _userManager.GetUserAsync(HttpContext.User);
            //var profile = _context.Profiles.FirstOrDefault(p => p.Email == user.Email);
            //var userProfile = await _context.Profiles
            //    .Include(p => p.ShoppingBag)
            //    .ThenInclude(s => s.Items)
            //    .FirstAsync(s => s.Id == profile.Id);
            var userProfile = await FindUserProfileWithShoppingBag(HttpContext);
            
            return View(userProfile.ShoppingBag);
        }

        public async Task<IActionResult> DeleteFromShoppingBag(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProfile = await FindUserProfileWithShoppingBag(HttpContext);
            var itemInShoppingBag = userProfile.ShoppingBag.Items.FirstOrDefault(p => p.Id == id);

            if(itemInShoppingBag == null) { 
                return NotFound(); 
            }

            userProfile.ShoppingBag.Items
                .Remove(itemInShoppingBag);
            _context.SaveChanges();

            return Ok();
        }

        public async Task<IActionResult> AddToShoppingBag(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProfile = await FindUserProfileWithShoppingBag(HttpContext);
            var itemToAdd = _context.Items.FirstOrDefault(p => p.Id == id);

            if (itemToAdd == null)
            {
                return NotFound();
            }

            userProfile.ShoppingBag.Items
                .Add(itemToAdd);
            _context.SaveChanges();

            return Ok();
        }

        // GET: ShoppingBag/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingBag = await _context.ShoppingBag
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingBag == null)
            {
                return NotFound();
            }

            return View(shoppingBag);
        }

        // GET: ShoppingBag/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ShoppingBag/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProfileId")] ShoppingBag shoppingBag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shoppingBag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shoppingBag);
        }

        // GET: ShoppingBag/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingBag = await _context.ShoppingBag.FindAsync(id);
            if (shoppingBag == null)
            {
                return NotFound();
            }
            return View(shoppingBag);
        }

        // POST: ShoppingBag/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProfileId")] ShoppingBag shoppingBag)
        {
            if (id != shoppingBag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingBag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingBagExists(shoppingBag.Id))
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
            return View(shoppingBag);
        }

        

        // GET: ShoppingBag/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingBag = await _context.ShoppingBag
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingBag == null)
            {
                return NotFound();
            }

            return View(shoppingBag);
        }

        // POST: ShoppingBag/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shoppingBag = await _context.ShoppingBag.FindAsync(id);
            _context.ShoppingBag.Remove(shoppingBag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShoppingBagExists(int id)
        {
            return _context.ShoppingBag.Any(e => e.Id == id);
        }

        private async Task<Profile> FindUserProfileWithShoppingBag(HttpContext httpContext)
        {
            var user = await _userManager.GetUserAsync(httpContext.User);
            var profile = _context.Profiles.FirstOrDefault(p => p.Email == user.Email);
            var userProfile = await _context.Profiles
                .Include(p => p.ShoppingBag)
                .ThenInclude(s => s.Items)
                .FirstAsync(s => s.Id == profile.Id);
            return userProfile;
        }
    }
}
