using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataProtection.Web.Models;
using Microsoft.AspNetCore.DataProtection;

namespace DataProtection.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly EfDbContext _context;
        private readonly IDataProtector _dataProtector;

        public ProductsController(EfDbContext context, IDataProtectionProvider dataProtectionProvider)
        {
            _context = context;
            _dataProtector = dataProtectionProvider.CreateProtector(DataProtectorKey.Key);
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var result = await _context.Products.ToListAsync();
            var timeLimitedProtector = _dataProtector.ToTimeLimitedDataProtector();
            result.ForEach(x => x.EncrypedId = timeLimitedProtector.Protect(x.Id.ToString(),TimeSpan.FromSeconds(5)));
            return View(result);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(string encrypedId)
        {
            if (encrypedId == null)
            {
                return NotFound();
            }
            var timeLimitedProtector = _dataProtector.ToTimeLimitedDataProtector();

            var id = timeLimitedProtector.Unprotect(encrypedId);
            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == int.Parse(id));

            if (product == null)
            {
                return NotFound();
            }
            product.EncrypedId = encrypedId;

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                await _context.AddAsync(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(string encrypedId)
        {
            if (encrypedId == null)
            {
                return NotFound();
            }
            var id = int.Parse(_dataProtector.Unprotect(encrypedId));
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string? encrypedId, [Bind("Id,Name,Description")] Product product)
        {
            var id = int.Parse(_dataProtector.Unprotect(encrypedId));
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(string? encrypedId)
        {
            if (encrypedId == null)
            {
                return NotFound();
            }
            var id = int.Parse(_dataProtector.Unprotect(encrypedId));
            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string encrypedId)
        {
            var id = int.Parse(_dataProtector.Unprotect(encrypedId));
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
