using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using TutesWebApp.Data;
using TutesWebApp.Models;

namespace TutesWebApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class CourseInfoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseInfoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CourseInfoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Courses.ToListAsync());
        }

        // GET: CourseInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseInfo = await _context.Courses
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (courseInfo == null)
            {
                return NotFound();
            }

            return View(courseInfo);
        }

        // GET: CourseInfoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CourseInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseID,CourseName")] CourseInfo courseInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(courseInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(courseInfo);
        }

        // GET: CourseInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseInfo = await _context.Courses.FindAsync(id);
            if (courseInfo == null)
            {
                return NotFound();
            }
            return View(courseInfo);
        }

        // POST: CourseInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseID,CourseName")] CourseInfo courseInfo)
        {
            if (id != courseInfo.CourseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseInfoExists(courseInfo.CourseID))
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
            return View(courseInfo);
        }

        // GET: CourseInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseInfo = await _context.Courses
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (courseInfo == null)
            {
                return NotFound();
            }

            return View(courseInfo);
        }

        // POST: CourseInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courseInfo = await _context.Courses.FindAsync(id);
            _context.Courses.Remove(courseInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseInfoExists(int id)
        {
            return _context.Courses.Any(e => e.CourseID == id);
        }
    }
}
