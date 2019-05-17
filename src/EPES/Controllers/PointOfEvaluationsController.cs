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
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPES.Controllers
{
    [Authorize]
    public class PointOfEvaluationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PointOfEvaluationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: PointOfEvaluations
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var officeCode = user.OfficeId;
            //var office = await _context.Offices.SingleAsync(o => o.Code == officeCode);
            var yearForQuery = new DateTime(DateTime.Now.Year, 1, 1);

            var viewModel = new PointOfEvaluationViewModel();
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
            viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int yearPoint = 0)
        {
            var user = await _userManager.GetUserAsync(User);
            var officeCode = user.OfficeId;
            //var office = await _context.Offices.SingleAsync(o => o.Code == officeCode);

            DateTime yearForQuery;
            if (yearPoint == 0)
            {
                yearForQuery = new DateTime(DateTime.Now.Year, 1, 1);
            }
            else
            {
                yearForQuery = new DateTime(DateTime.Now.AddYears(yearPoint).Year, 1, 1);
            }

            var viewModel = new PointOfEvaluationViewModel();
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
                    viewModel.pointA = await _context.PointOfEvaluations.Include(p => p.AuditOffice).Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == officeCode && p.Year == yearForQuery).Include(p => p.OwnerOffice).ToListAsync();
                    viewModel.pointB = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == officeCode && p.Year == yearForQuery).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == officeCode && p.Year == yearForQuery).ToListAsync();
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

            viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
            viewModel.yearPoint = yearPoint;

            return View(viewModel);
        }

        // GET: PointOfEvaluations/Details/5
        public async Task<IActionResult> Details(int? id, int yearPoint = 0)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pointOfEvaluation = await _context.PointOfEvaluations
                .Include(p => p.OwnerOffice)
                .Include(p => p.AuditOffice)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pointOfEvaluation == null)
            {
                return NotFound();
            }

            ViewBag.yearPoint = yearPoint;
            return View(pointOfEvaluation);
        }

        // GET: PointOfEvaluations/Create
        public async Task<IActionResult> Create(int? plan, int? yearPoint)
        {
            switch (plan)
            {
                case 0:
                    ViewBag.Plan = "A";
                    ViewBag.PlanValue = "0";
                    break;
                case 1:
                    ViewBag.Plan = "B";
                    ViewBag.PlanValue = "1";
                    break;
                case 2:
                    ViewBag.Plan = "C";
                    ViewBag.PlanValue = "2";
                    break;
                case 3:
                    ViewBag.Plan = "D";
                    ViewBag.PlanValue = "3";
                    break;
            }
            ViewBag.yearPoint = yearPoint;
            var user = await _userManager.GetUserAsync(User);
            var office = await _context.Offices.SingleAsync(o => o.Code == user.OfficeId);
            if (plan == 3)
            {
                PopulateOfficesDropDownList(1);
            }
            else
            {
                PopulateOfficesDropDownList();
            }
            PopulateAuditOfficesDropDownList(office.Id);
            return View();
        }

        // POST: PointOfEvaluations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Point,Plan,Name,Unit,Weight,Rate1,Rate2,Rate3,Rate4,Rate5,OwnerOfficeId,AuditOfficeId")] PointOfEvaluation pointOfEvaluation, int y, string ownerOfficeCode = null, string auditOfficeCode = null)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ownerOfficeCode != null)
            {
                pointOfEvaluation.OwnerOffice = await _context.Offices.FirstOrDefaultAsync(o => o.Code == ownerOfficeCode);
                //pointOfEvaluation.OwnerOfficeId = office.Id;
            }
            if (auditOfficeCode != null)
            {
                pointOfEvaluation.AuditOffice = await _context.Offices.FirstOrDefaultAsync(o => o.Code == auditOfficeCode);
                //pointOfEvaluation.AuditOfficeId = office.Id;
            }

            pointOfEvaluation.Year = new DateTime(DateTime.Now.AddYears(y).Year, 1, 1);
            pointOfEvaluation.UpdateUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            pointOfEvaluation.SubPoint = 0;

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(pointOfEvaluation);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { yearPoint = y });
                }
            }
            catch (DbUpdateException)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "ไม่สามารถบันทึกข้อมูล. " +
                    "ลองพยายามบันทึกอีกครั้ง " +
                    "โปรดแจ้งผู้ดูแลระบบ");
            }
            if (User.IsInRole("Admin"))
            {
                PopulateOfficesDropDownList(pointOfEvaluation.OwnerOfficeId);
                PopulateAuditOfficesDropDownList(pointOfEvaluation.AuditOfficeId);
            }
            return View(pointOfEvaluation);
        }

        // GET: PointOfEvaluations/Edit/5
        public async Task<IActionResult> Edit(int? id, int yearPoint = 0)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pointOfEvaluation = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Where(p => p.Id == id).FirstOrDefaultAsync();
            if (pointOfEvaluation == null)
            {
                return NotFound();
            }

            ViewBag.yearPoint = yearPoint;
            if (User.IsInRole("Admin"))
            {
                PopulateOfficesDropDownList();
                PopulateAuditOfficesDropDownList();
            }
            return View(pointOfEvaluation);
        }

        // POST: PointOfEvaluations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id, int yearpoint = 0)//, [Bind("Id,Year,Point,SubPoint,Plan,Name,Unit,Weight,Rate1,Rate2,Rate3,Rate4,Rate5")] PointOfEvaluation pointOfEvaluation)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);

            var pointOfEvaluationToUpdate = await _context.PointOfEvaluations.FirstOrDefaultAsync(p => p.Id == id);

            if (await TryUpdateModelAsync<PointOfEvaluation>(
                pointOfEvaluationToUpdate, "",
                p => p.Year, p => p.Point, p => p.Plan, p => p.Name, p => p.Unit, p => p.Weight,
                p => p.Rate1, p => p.Rate2, p => p.Rate3, p => p.Rate4, p => p.Rate5, p => p.OwnerOfficeId, p => p.AuditOfficeId))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { yearPoint = yearpoint });
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "ไม่สามารถบันทึกข้อมูล. " +
                        "ลองพยายามบันทึกอีกครั้ง " +
                        "โปรดแจ้งผู้ดูแลระบบ");
                }
                return RedirectToAction(nameof(Index), new { yearPoint = yearpoint });
            }
            if (User.IsInRole("Admin"))
            {
                PopulateOfficesDropDownList(pointOfEvaluationToUpdate.OwnerOfficeId);
                PopulateAuditOfficesDropDownList(pointOfEvaluationToUpdate.AuditOfficeId);
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
                .Include(p => p.OwnerOffice)
                .Include(p => p.AuditOffice)
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
        public async Task<IActionResult> DeleteConfirmed(int pointId)
        {
            var user = await _userManager.GetUserAsync(User);

            var pointOfEvaluation = await _context.PointOfEvaluations.FindAsync(pointId);
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
                return RedirectToAction(nameof(Delete), new { id = pointId, saveChangesError = true });
            }
        }

        private bool PointOfEvaluationExists(int id)
        {
            return _context.PointOfEvaluations.Any(e => e.Id == id);
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
