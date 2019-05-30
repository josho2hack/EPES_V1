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

            viewModel.yearPoint = yearPoint;
            return View(viewModel);
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexPost(int yearPoint, int selectoffice = 1)
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
                if (selectoffice == 1)
                {
                    viewModel.pointA = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.A) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.B) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.C) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                }
                else
                {
                    viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && (p.OwnerOfficeId == selectoffice) && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.B) && (p.OwnerOfficeId == selectoffice || p.AuditOfficeId == selectoffice || p.OwnerOfficeId == null) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.C) && (p.OwnerOfficeId == selectoffice || p.AuditOfficeId == selectoffice) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                }
                ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name",selectoffice);
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

            viewModel.yearPoint = yearPoint;
            return View(viewModel);
        }

        // GET: DataForEvaluations/Details/5
        public async Task<IActionResult> Details(int? poeid,int yearPoint)
        {
            if (poeid == null)
            {
                return NotFound();
            }

            DataForEvaluationViewModel viewModel = new DataForEvaluationViewModel();
            viewModel.Point = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Where(p => p.Id == poeid).FirstOrDefaultAsync();
            if (viewModel.Point.OwnerOfficeId == null || viewModel.Point.OwnerOfficeId == 1)
            {
                return RedirectToAction(nameof(AllDetails), new { poeid = poeid, yearPoint = yearPoint, ownerofficeid = viewModel.Point.OwnerOfficeId });
            }

            viewModel.expect1 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 1).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect2 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 2).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect3 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 3).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect4 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 4).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect5 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 5).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect6 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 6).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect7 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 7).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect8 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 8).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect9 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 9).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect10 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 10).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect11 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 11).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect12 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 12).Select(d => d.Expect).FirstOrDefaultAsync();

            viewModel.yearPoint = yearPoint;
            return View(viewModel);
        }

        public async Task<IActionResult> AllDetails(int poeid, int yearPoint, int? ownerofficeid)
        {
            DataForEvaluationViewModel viewModel = new DataForEvaluationViewModel();
            viewModel.Point = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Where(p => p.Id == poeid).FirstOrDefaultAsync();

            if (ownerofficeid == null)
            {
                ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) != "000" && d.Code.Substring(5, 3) == "000"), "Id", "Name");
            }
            else
            {
                ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name");
            }

            viewModel.yearPoint = yearPoint;
            return View(viewModel);
        }

        [HttpPost, ActionName("AllDetails")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AllDetailsPost(int poeid, int yearPoint, int? ownerofficeid)
        {
            DataForEvaluationViewModel viewModel = new DataForEvaluationViewModel();
            viewModel.Point = await _context.PointOfEvaluations.Where(p => p.Id == poeid).FirstOrDefaultAsync();

            if (ownerofficeid == null)
            {
                ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) != "000" && d.Code.Substring(5, 3) == "000"), "Id", "Name");
            }
            else
            {
                ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", ownerofficeid);
            }

            viewModel.expect1 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 1).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect2 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 2).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect3 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 3).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect4 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 4).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect5 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 5).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect6 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 6).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect7 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 7).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect8 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 8).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect9 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 9).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect10 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 10).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect11 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 11).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect12 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 12).Select(d => d.Expect).FirstOrDefaultAsync();

            //viewModel.Office = await _context.Offices.Where(o => o.Id == ownerofficeid).FirstOrDefaultAsync();
            viewModel.yearPoint = yearPoint;
            return View(viewModel);
        }

        // GET: DataForEvaluations/Edit/5
        public async Task<IActionResult> Edit(int? poeid,int yearPoint)
        {
            if (poeid == null)
            {
                return NotFound();
            }

            DataForEvaluationViewModel viewModel = new DataForEvaluationViewModel();
            viewModel.Point = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Where(p => p.Id == poeid).FirstOrDefaultAsync();
            if (viewModel.Point.OwnerOfficeId == null || viewModel.Point.OwnerOfficeId == 1)
            {
                return RedirectToAction(nameof(AllEdit), new { poeid = poeid, yearPoint = yearPoint, ownerofficeid = viewModel.Point.OwnerOfficeId });
                //return View("AllEdit");
            }

            viewModel.expect1 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 1).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect2 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 2).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect3 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 3).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect4 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 4).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect5 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 5).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect6 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 6).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect7 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 7).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect8 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 8).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect9 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 9).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect10 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 10).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect11 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 11).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect12 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 12).Select(d => d.Expect).FirstOrDefaultAsync();

            viewModel.yearPoint = yearPoint;
            return View(viewModel);
        }

        public async Task<IActionResult> AllEdit(int poeid, int yearPoint ,int? ownerofficeid)
        {
            DataForEvaluationViewModel viewModel = new DataForEvaluationViewModel();
            viewModel.Point = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Where(p => p.Id == poeid).FirstOrDefaultAsync();

            if (ownerofficeid == null)
            {
                ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) != "000" && d.Code.Substring(5, 3) == "000"), "Id", "Name");
            }
            else
            {
                ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name");
            }

            viewModel.yearPoint = yearPoint;
            return View(viewModel);
        }

        [HttpPost, ActionName("AllEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AllEditPost(int poeid, int yearPoint, int? ownerofficeid)
        {
            DataForEvaluationViewModel viewModel = new DataForEvaluationViewModel();
            viewModel.Point = await _context.PointOfEvaluations.Where(p => p.Id == poeid).FirstOrDefaultAsync();

            if (ownerofficeid == null)
            {
                ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) != "000" && d.Code.Substring(5, 3) == "000"), "Id", "Name");
            }
            else
            {
                ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name",ownerofficeid);
            }

            viewModel.expect1 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 1).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect2 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 2).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect3 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 3).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect4 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 4).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect5 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 5).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect6 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 6).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect7 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 7).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect8 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 8).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect9 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 9).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect10 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 10).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect11 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 11).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect12 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 12).Select(d => d.Expect).FirstOrDefaultAsync();

            viewModel.Office = await _context.Offices.Where(o => o.Id == ownerofficeid).FirstOrDefaultAsync();
            viewModel.yearPoint = yearPoint;
            return View(viewModel);
        }

        // POST: DataForEvaluations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost([Bind("yearPoint,poeid,expect1,expect2,expect3,expect4,expect5,expect6,expect7,expect8,expect9,expect10,expect11,expect12")] DataForEvaluationViewModel dataView,int? ownerofficeid)
        {
            dataView.Point = await _context.PointOfEvaluations.Where(p => p.Id == dataView.poeid).FirstOrDefaultAsync();
            Office office;
            if (ownerofficeid != null)
            {
                office = await _context.Offices.Where(o => o.Id == ownerofficeid).FirstOrDefaultAsync();
            }
            else
            {
                office = await _context.Offices.Where(o => o.Id == dataView.Point.OwnerOfficeId).FirstOrDefaultAsync();
            }
            
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                await SaveExpect(dataView.Point.Id, office.Id, 10, dataView.expect10, user.Id);
                await SaveExpect(dataView.Point.Id, office.Id, 11, dataView.expect11, user.Id);
                await SaveExpect(dataView.Point.Id, office.Id, 12, dataView.expect12, user.Id);
                await SaveExpect(dataView.Point.Id, office.Id, 1, dataView.expect1, user.Id);
                await SaveExpect(dataView.Point.Id, office.Id, 2, dataView.expect2, user.Id);
                await SaveExpect(dataView.Point.Id, office.Id, 3, dataView.expect3, user.Id);
                await SaveExpect(dataView.Point.Id, office.Id, 4, dataView.expect4, user.Id);
                await SaveExpect(dataView.Point.Id, office.Id, 5, dataView.expect5, user.Id);
                await SaveExpect(dataView.Point.Id, office.Id, 6, dataView.expect6, user.Id);
                await SaveExpect(dataView.Point.Id, office.Id, 7, dataView.expect7, user.Id);
                await SaveExpect(dataView.Point.Id, office.Id, 8, dataView.expect8, user.Id);
                await SaveExpect(dataView.Point.Id, office.Id, 9, dataView.expect9, user.Id);

                if (dataView.Point.OwnerOfficeId != null)
                {
                    return RedirectToAction(nameof(Index), new { yearPoint = dataView.yearPoint });
                }
                return RedirectToAction(nameof(Edit), new { poeid = dataView.poeid, yearPoint = dataView.yearPoint });
            }
            return View(dataView);
        }

        private bool DataForEvaluationExists(int id)
        {
            return _context.DataForEvaluations.Any(e => e.Id == id);
        }

        public async Task SaveExpect(int poeid, int ownerofficeid, int month, decimal? expect, string userid)
        {
            if (expect != null)
            {
                DataForEvaluation dataForEvaluation;
                dataForEvaluation = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == month).FirstOrDefaultAsync();
                if (dataForEvaluation != null)
                {
                    dataForEvaluation.UpdateUserId = userid;
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
                    dataForEvaluation = new DataForEvaluation();
                    dataForEvaluation.UpdateUserId = userid;
                    dataForEvaluation.PointOfEvaluationId = poeid;
                    dataForEvaluation.OfficeId = ownerofficeid;
                    dataForEvaluation.Month = month;
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
