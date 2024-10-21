using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ecos.Areas.Identity.Data;
using ecos.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace ecos
{
    [Authorize]
    public class ElectricityRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ElectricityRecordsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ElectricityRecords
        [Authorize]
        public async Task<IActionResult> Index()
        {
            // Sorting records by Month in descending order (newest to oldest)
            var sortedRecords = await _context.ElectricityRecords
                                              .OrderByDescending(r => r.Month)
                                              .ToListAsync();

            return View(sortedRecords);
        }


        // GET: ElectricityRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electricityRecord = await _context.ElectricityRecords
                .FirstOrDefaultAsync(m => m.Id == id);
            if (electricityRecord == null)
            {
                return NotFound();
            }

            return View(electricityRecord);
        }

        // GET: ElectricityRecords/Create
        public async Task<IActionResult> Create()
        {
            // Fetch all users from AspNetUsers
            var users = await _userManager.Users.ToListAsync();

            // Create a list of SelectListItems for the dropdown
            ViewBag.Users = new SelectList(users, "Id", "Email"); // "Id" for value, "Email" for display text

            return View();
        }


        // POST: ElectricityRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,HouseholdName,ElectricityRate,TotalBill,Month,UserId")] ElectricityRecord electricityRecord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(electricityRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If the model is invalid, pass the user list again for the dropdown
            var users = await _userManager.Users.ToListAsync();
            ViewBag.Users = new SelectList(users, "Id", "Email");

            return View(electricityRecord);
        }



        // GET: ElectricityRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electricityRecord = await _context.ElectricityRecords.FindAsync(id);
            if (electricityRecord == null)
            {
                return NotFound();
            }
            return View(electricityRecord);
        }

        // POST: ElectricityRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,HouseholdName,ElectricityRate,TotalBill,Month")] ElectricityRecord electricityRecord)
        {
            if (id != electricityRecord.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(electricityRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElectricityRecordExists(electricityRecord.Id))
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
            return View(electricityRecord);
        }

        // GET: ElectricityRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electricityRecord = await _context.ElectricityRecords
                .FirstOrDefaultAsync(m => m.Id == id);
            if (electricityRecord == null)
            {
                return NotFound();
            }

            return View(electricityRecord);
        }

        // POST: ElectricityRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var electricityRecord = await _context.ElectricityRecords.FindAsync(id);
            if (electricityRecord != null)
            {
                _context.ElectricityRecords.Remove(electricityRecord);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ElectricityRecordExists(int id)
        {
            return _context.ElectricityRecords.Any(e => e.Id == id);
        }
    }
}
