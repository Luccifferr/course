using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseSystem.Data;
using CourseSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace CourseSystem.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class GradeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GradeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Grades
        public async Task<IActionResult> Index()
        {
              return _context.Grades != null ? 
                          View(await _context.Grades.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Grades'  is null.");
        }

        // GET: Grades/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Grades == null)
            {
                return NotFound();
            }

            var grade = await _context.Grades
                .FirstOrDefaultAsync(m => m.Id == id);
            if (grade == null)
            {
                return NotFound();
            }

            return View(grade);
        }

        // GET: Grades/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Grades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Grade grade)
        {
            if (ModelState.IsValid)
            {
                grade.Id = Guid.NewGuid();
                _context.Add(grade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(grade);
        }

        // GET: Grades/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Grades == null)
            {
                return NotFound();
            }

            var grade = await _context.Grades.FindAsync(id);
            if (grade == null)
            {
                return NotFound();
            }
            return View(grade);
        }

        // POST: Grades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] Grade grade)
        {
            if (id != grade.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(grade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SinifExists(grade.Id))
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
            return View(grade);
        } 
        public async Task<IActionResult> Delete(Guid id)
        {
            if (_context.Grades == null)
            {

                return Json(new { silindimi = false, hataMesaji = "Entity set 'ApplicationDbContext.Grades'  is null." });
            }
            var category = await _context.Grades.FindAsync(id);
            if (category != null)
            {
                _context.Grades.Remove(category);
            }

            await _context.SaveChangesAsync();

            return Json(new { silindimi = true });

        }

        private bool SinifExists(Guid id)
        {
          return (_context.Grades?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
