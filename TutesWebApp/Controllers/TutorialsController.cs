using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TutesWebApp.Data;
using TutesWebApp.Models;

namespace TutesWebApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class TutorialsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public TutorialsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this._environment = env;
        }

        // GET: Tutorials
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tutorials.Include(t => t.Course);
            return View(await applicationDbContext.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> ViewTutorials()
        {
            var applicationDbContext = _context.Tutorials.Include(t => t.Course);
            return View(await applicationDbContext.ToListAsync());
        }


        // GET: Tutorials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorials = await _context.Tutorials
                .Include(t => t.Course)
                .FirstOrDefaultAsync(m => m.TutorialID == id);
            if (tutorials == null)
            {
                return NotFound();
            }

            return View(tutorials);
        }

        [AllowAnonymous]
        public async Task<IActionResult> TutorialDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorials = await _context.Tutorials
                .Include(t => t.Course)
                .FirstOrDefaultAsync(m => m.TutorialID == id);
            if (tutorials == null)
            {
                return NotFound();
            }
            else
            {
                var topics = from s in _context.Topics
                             where s.TutorialID == id
                                select s;
                ViewBag.TutorialTopics = topics.OrderBy(s => s.TopicID);
            }

            return View(tutorials);
        }



        // GET: Tutorials/Create
        public IActionResult Create()
        {
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "CourseName");
            return View();
        }

        // POST: Tutorials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TutorialID,TutorialTitle,FileUpload,Description,Duration,CourseID,Online")] Tutorials tutorials)
        {
            using (var memoryStream = new MemoryStream())
            {
                await tutorials.FileUpload.FormFile.CopyToAsync(memoryStream);

                string photoname = tutorials.FileUpload.FormFile.FileName;
                int photosize = (int)tutorials.FileUpload.FormFile.Length;
                string phototype = tutorials.FileUpload.FormFile.ContentType;
                tutorials.PhotoName = photoname;
                tutorials.PhotoType = phototype;
                tutorials.ExtName = Path.GetExtension(photoname);
                if (!".jpg.jpeg.png.gif.bmp".Contains(tutorials.ExtName.ToLower()))
                {
                    ModelState.AddModelError("FileUpload.FormFile", "Invalid Format of Image Given.");
                }
                else
                {
                    ModelState.Remove("PhotoName");
                    ModelState.Remove("PhotoType");
                    ModelState.Remove("ExtName");
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(tutorials);
                await _context.SaveChangesAsync();
                var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsRootFolder))
                {
                    Directory.CreateDirectory(uploadsRootFolder);
                }
                string filename = tutorials.TutorialID + tutorials.ExtName;
                var filePath = Path.Combine(uploadsRootFolder, filename);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await tutorials.FileUpload.FormFile.CopyToAsync(fileStream).ConfigureAwait(false);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "CourseName", tutorials.CourseID);
            return View(tutorials);
        }

        // GET: Tutorials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorials = await _context.Tutorials.FindAsync(id);
            if (tutorials == null)
            {
                return NotFound();
            }
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "CourseName", tutorials.CourseID);
            return View(tutorials);
        }

        // POST: Tutorials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TutorialID,TutorialTitle,PhotoName,ExtName,PhotoType,Description,Duration,CourseID,Online")] Tutorials tutorials)
        {
            if (id != tutorials.TutorialID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tutorials);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TutorialsExists(tutorials.TutorialID))
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
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "CourseName", tutorials.CourseID);
            return View(tutorials);
        }

        // GET: Tutorials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorials = await _context.Tutorials
                .Include(t => t.Course)
                .FirstOrDefaultAsync(m => m.TutorialID == id);
            if (tutorials == null)
            {
                return NotFound();
            }

            return View(tutorials);
        }

        // POST: Tutorials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tutorials = await _context.Tutorials.FindAsync(id);
            _context.Tutorials.Remove(tutorials);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TutorialsExists(int id)
        {
            return _context.Tutorials.Any(e => e.TutorialID == id);
        }


    }
}
