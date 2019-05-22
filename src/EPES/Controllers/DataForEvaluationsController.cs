using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using EPES.Data;
using EPES.Models;
using EPES.ViewModels;

namespace EPES.Controllers
{
    [Authorize]
    public class DataForEvaluationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DataForEvaluationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: DataForEvaluations
        public async Task<IActionResult> Index(int yearPoint = 0)
        {
            var user = await _userManager.GetUserAsync(User);
            var officeCode = user.OfficeId;
            var viewModel = new DataForEvaluationViewModel();

            DateTime yearForQuery;
            if (yearPoint == 0)
            {
                yearForQuery = new DateTime(DateTime.Now.Year, 1, 1);
            }
            else
            {
                yearForQuery = new DateTime(DateTime.Now.AddYears(yearPoint).Year, 1, 1);
            }

            if (officeCode == "00013000")
            {
                viewModel.pointA = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.A) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                viewModel.pointB = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.B) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                viewModel.pointC = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.C) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
            }
            else
            {

                if (officeCode.Substring(0, 2) == "00")
                {
                    viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == officeCode && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.AuditOffice.Code == officeCode && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == officeCode && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                }
                else
                {
                    // List Plan B All Office
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();

                    if (officeCode.Substring(2, 6) == "000000")
                    {
                        viewModel.pointC = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Where(p => (p.Plan == TypeOfPlan.C && p.OwnerOffice == null && p.Year == yearForQuery) || (p.Plan == TypeOfPlan.C && p.OwnerOffice.Code.StartsWith(officeCode.Substring(0, 2)) && p.Year == yearForQuery)).ToListAsync();
                    }
                    else
                    {
                        viewModel.pointC = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Where(p => (p.Plan == TypeOfPlan.C && p.OwnerOffice == null && p.Year == yearForQuery) || (p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == officeCode && p.Year == yearForQuery)).ToListAsync();
                    }
                }
            }
            viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();

            return View(viewModel);
        }

        // GET: DataForEvaluations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataForEvaluation = await _context.DataForEvaluations
                .Include(d => d.Office)
                .Include(d => d.PointOfEvaluation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataForEvaluation == null)
            {
                return NotFound();
            }

            return View(dataForEvaluation);
        }

        // GET: DataForEvaluations/Createint 
        public async Task<IActionResult> Create(int poeid)
        {
            DataForEvaluationViewModel viewModel = new DataForEvaluationViewModel();
            viewModel.Point = await _context.PointOfEvaluations.FindAsync(poeid);
            //viewModel.Offices = await _context.Offices.Where(o => o.Code != "00000000" && o.Code.Substring(5, 3) == "000").ToListAsync();
            //ViewBag.poeid = poeid;
            return View(viewModel);
        }

        // POST: DataForEvaluations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int poeid,int ownerofficeid, int? expext1, int? expext2, int? expext3, int? expext4,int? expext5, int? expext6, int? expext7, int? expext8, int? expext9, int? expext10, int? expext11, int? expext12)
        {
            var dataForEvaluation = new List<DataForEvaluation>();
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                //dataForEvaluation.UpdateUserId = user.Id;
                //dataForEvaluation.PointOfEvaluationId = poeid;



                //_context.Add(dataForEvaluation);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dataForEvaluation);
        }

        // GET: DataForEvaluations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataForEvaluation = await _context.DataForEvaluations.FindAsync(id);
            if (dataForEvaluation == null)
            {
                return NotFound();
            }
            ViewData["OfficeId"] = new SelectList(_context.Offices, "Id", "Id", dataForEvaluation.OfficeId);
            ViewData["PointOfEvaluationId"] = new SelectList(_context.PointOfEvaluations, "Id", "Id", dataForEvaluation.PointOfEvaluationId);
            return View(dataForEvaluation);
        }

        // POST: DataForEvaluations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Expect,Result,OldResult,Month,AuditComment,Approve,CommentApprove,PointOfEvaluationId,OfficeId")] DataForEvaluation dataForEvaluation)
        {
            if (id != dataForEvaluation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dataForEvaluation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataForEvaluationExists(dataForEvaluation.Id))
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
            ViewData["OfficeId"] = new SelectList(_context.Offices, "Id", "Id", dataForEvaluation.OfficeId);
            ViewData["PointOfEvaluationId"] = new SelectList(_context.PointOfEvaluations, "Id", "Id", dataForEvaluation.PointOfEvaluationId);
            return View(dataForEvaluation);
        }

        // GET: DataForEvaluations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataForEvaluation = await _context.DataForEvaluations
                .Include(d => d.Office)
                .Include(d => d.PointOfEvaluation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataForEvaluation == null)
            {
                return NotFound();
            }

            return View(dataForEvaluation);
        }

        // POST: DataForEvaluations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dataForEvaluation = await _context.DataForEvaluations.FindAsync(id);
            _context.DataForEvaluations.Remove(dataForEvaluation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DataForEvaluationExists(int id)
        {
            return _context.DataForEvaluations.Any(e => e.Id == id);
        }

        private void PopulateOfficesDropDownList(object selectedOffice = null)
        {
            var officesQuery = from d in _context.Offices
                               where (d.Code.Substring(5, 3) == "000")
                               orderby d.Id
                               select d;
            ViewBag.OfficeID = new SelectList(officesQuery.AsNoTracking(), "Id", "Name", selectedOffice);
        }

        private void PopulateAuditOfficesDropDownList(object selectedOffice = null)
        {
            var officesQuery = from d in _context.Offices
                               where (d.Code != "00000000" && d.Code.Substring(5, 3) == "000")
                               orderby d.Id
                               select d;
            ViewBag.AuditOfficeID = new SelectList(officesQuery.AsNoTracking(), "Id", "Name", selectedOffice);
        }
    }
}
