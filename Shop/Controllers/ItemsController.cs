﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    [Authorize]
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ItemsController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var profile = _context.Profiles.FirstOrDefault(p => p.Email == user.Email);
            return View(await _context.Items.Where(i => i.ProfileId != profile.Id).ToListAsync());
        }

        public async Task<IActionResult> UserItems()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var profile = _context.Profiles.FirstOrDefault(p => p.Email == user.Email);
            return View(await _context.Items.Where(i => i.ProfileId == profile.Id).ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var profile = _context.Profiles.FirstOrDefault(p => p.Email == user.Email);

            ItemDetailsViewModel itemDetailsViewModel = new ItemDetailsViewModel()
            {
                Id = item.Id,
                Description = item.Description,
                Image = item.Image,
                Name = item.Name,
                Price = item.Price,
                Show = item.ProfileId != profile.Id,
            };
            return View(itemDetailsViewModel);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Price,Image")] ItemViewModel itemViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var profile = _context.Profiles.FirstOrDefault(p => p.Email == user.Email);
                
                Item item = new Item()
                {
                    Name = itemViewModel.Name,
                    Description = itemViewModel.Description,
                    Price = itemViewModel.Price,
                    Image = itemViewModel.Image.FileName,
                };
                item.ProfileId = profile.Id;
                _context.Add(item);

                await _context.SaveChangesAsync();
                string projectRootPath = _hostingEnvironment.WebRootPath + "/Images/";
                bool exists = System.IO.Directory.Exists(projectRootPath+item.Id.ToString());

                if (!exists)
                    System.IO.Directory.CreateDirectory(projectRootPath + item.Id.ToString());
                var filePath = projectRootPath + $"{item.Id}/{itemViewModel.Image.FileName}";

                using (var stream = System.IO.File.Create(filePath))
                {
                    await itemViewModel.Image.CopyToAsync(stream);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(itemViewModel);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image,Description,Price,ProfileId")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
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
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
}
