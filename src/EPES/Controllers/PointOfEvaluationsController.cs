using EPES.Data;
using EPES.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPES.Controllers
{
    public class PointOfEvaluationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PointOfEvaluationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PointOfEvaluations
        public async Task<IActionResult> Index()
        {
            return View(await _context.PointOfEvaluations.ToListAsync());
        }

        // GET: PointOfEvaluations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pointOfEvaluation = await _context.PointOfEvaluations
                .Include(p => p.OwnerOffice)
                .Include(p => p.AuditOffice)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pointOfEvaluation == null)
            {
                return NotFound();
            }

            return View(pointOfEvaluation);
        }

        // GET: PointOfEvaluations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PointOfEvaluations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Year,Point,SubPoint,Plan,Name,Unit,Weight,Rate1,Rate2,Rate3,Rate4,Rate5")] PointOfEvaluation pointOfEvaluation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(pointOfEvaluation);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "ไม่สามารถบันทึกข้อมูล. " +
                    "ลองพยายามบันทึกอีกครั้ง " +
                    "โปรดแจ้งผู้ดูแลระบบ");
            }
            return View(pointOfEvaluation);
        }

        // GET: PointOfEvaluations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pointOfEvaluation = await _context.PointOfEvaluations.FindAsync(id);
            if (pointOfEvaluation == null)
            {
                return NotFound();
            }
            return View(pointOfEvaluation);
        }

        // POST: PointOfEvaluations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)//, [Bind("Id,Year,Point,SubPoint,Plan,Name,Unit,Weight,Rate1,Rate2,Rate3,Rate4,Rate5")] PointOfEvaluation pointOfEvaluation)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pointOfEvaluationToUpdate = await _context.PointOfEvaluations.FirstOrDefaultAsync(p => p.Id == id);

            if (await TryUpdateModelAsync<PointOfEvaluation>(
                pointOfEvaluationToUpdate, "",
                p => p.Year, p => p.Point, p => p.SubPoint, p => p.Plan, p => p.Name, p => p.Unit, p => p.Weight,
                p => p.Rate1, p => p.Rate2, p => p.Rate3, p => p.Rate4, p => p.Rate5))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "ไม่สามารถบันทึกข้อมูล. " +
                        "ลองพยายามบันทึกอีกครั้ง " +
                        "โปรดแจ้งผู้ดูแลระบบ");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pointOfEvaluationToUpdate);
        }

        // GET: PointOfEvaluations/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pointOfEvaluation = await _context.PointOfEvaluations
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pointOfEvaluation == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "ลบไม่ได้." +
                    "ลองพยายามอีกครั้ง " +
                    "โปรดแจ้งผู้ดูแลระบบ";
            }

            return View(pointOfEvaluation);
        }

        // POST: PointOfEvaluations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pointOfEvaluation = await _context.PointOfEvaluations.FindAsync(id);
            if (pointOfEvaluation == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.PointOfEvaluations.Remove(pointOfEvaluation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool PointOfEvaluationExists(int id)
        {
            return _context.PointOfEvaluations.Any(e => e.Id == id);
        }
    }
}
