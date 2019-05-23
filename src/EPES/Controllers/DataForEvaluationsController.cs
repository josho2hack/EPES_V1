using EPES.Data;
using EPES.Models;
using EPES.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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

            return View(viewModel);
        }

        // POST: DataForEvaluations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("poeid,ownerofficeid,expect1,expect2,expect3,expect4,expect5,expect6,expect7, expect8, expect9,expect10, expect11, expect12")] DataForEvaluationViewModel dataView)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                SaveExpect(dataView.poeid, dataView.ownerofficeid, dataView.expect1, user.Id);
                SaveExpect(dataView.poeid, dataView.ownerofficeid, dataView.expect2, user.Id);
                SaveExpect(dataView.poeid, dataView.ownerofficeid, dataView.expect3, user.Id);
                SaveExpect(dataView.poeid, dataView.ownerofficeid, dataView.expect4, user.Id);
                SaveExpect(dataView.poeid, dataView.ownerofficeid, dataView.expect5, user.Id);
                SaveExpect(dataView.poeid, dataView.ownerofficeid, dataView.expect6, user.Id);
                SaveExpect(dataView.poeid, dataView.ownerofficeid, dataView.expect7, user.Id);
                SaveExpect(dataView.poeid, dataView.ownerofficeid, dataView.expect8, user.Id);
                SaveExpect(dataView.poeid, dataView.ownerofficeid, dataView.expect9, user.Id);
                SaveExpect(dataView.poeid, dataView.ownerofficeid, dataView.expect10, user.Id);
                SaveExpect(dataView.poeid, dataView.ownerofficeid, dataView.expect11, user.Id);
                SaveExpect(dataView.poeid, dataView.ownerofficeid, dataView.expect12, user.Id);

                return RedirectToAction(nameof(Index));
            }
            return View(dataView);
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

        public async void SaveExpect(int poeid, int ownerofficeid, decimal? expect, string userid)
        {
            if (expect != null)
            {
                DataForEvaluation dataForEvaluation = new DataForEvaluation();
                dataForEvaluation = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 1).SingleAsync();
                if (dataForEvaluation != null)
                {
                    dataForEvaluation.UpdateUserId = userid;
                    dataForEvaluation.PointOfEvaluationId = poeid;
                    dataForEvaluation.OfficeId = ownerofficeid;
                    dataForEvaluation.Expect = expect;
                    try
                    {
                        _context.Update(dataForEvaluation);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateException)
                    {
                        //Log the error (uncomment ex variable name and write a log.
                        ModelState.AddModelError("", "ไม่สามารถบันทึกข้อมูล. " +
                            "ลองพยายามบันทึกอีกครั้ง " +
                            "โปรดแจ้งผู้ดูแลระบบ");
                    }
                }
                else
                {
                    dataForEvaluation.UpdateUserId = userid;
                    dataForEvaluation.PointOfEvaluationId = poeid;
                    dataForEvaluation.OfficeId = ownerofficeid;
                    dataForEvaluation.Expect = expect;
                    try
                    {
                        _context.Add(dataForEvaluation);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateException)
                    {
                        //Log the error (uncomment ex variable name and write a log.
                        ModelState.AddModelError("", "ไม่สามารถบันทึกข้อมูล. " +
                            "ลองพยายามบันทึกอีกครั้ง " +
                            "โปรดแจ้งผู้ดูแลระบบ");
                    }
                }
            }// Expect 1
        }
    }
}
