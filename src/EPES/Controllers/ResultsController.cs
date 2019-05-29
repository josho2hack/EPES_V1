using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EPES.Data;
using EPES.Models;
using Microsoft.AspNetCore.Identity;
using EPES.ViewModels;

namespace EPES.Controllers
{
    public class ResultsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ResultsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Results
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
        public async Task<IActionResult> IndexPost(int yearPoint)
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

        // GET: Results/Details/5
        public async Task<IActionResult> Details(int? poeid, int yearPoint)
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

            viewModel.result1 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 1).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result2 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 2).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result3 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 3).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result4 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 4).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result5 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 5).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result6 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 6).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result7 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 7).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result8 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 8).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result9 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 9).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result10 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 10).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result11 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 11).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result12 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 12).Select(d => d.Result).FirstOrDefaultAsync();

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

            viewModel.result1 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 1).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result2 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 2).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result3 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 3).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result4 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 4).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result5 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 5).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result6 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 6).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result7 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 7).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result8 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 8).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result9 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 9).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result10 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 10).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result11 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 11).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result12 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 12).Select(d => d.Result).FirstOrDefaultAsync();

            //viewModel.Office = await _context.Offices.Where(o => o.Id == ownerofficeid).FirstOrDefaultAsync();
            viewModel.yearPoint = yearPoint;
            return View(viewModel);
        }

        // GET: DataForEvaluations/Edit/5
        public async Task<IActionResult> Edit(int? poeid, int yearPoint)
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

            viewModel.result1 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 1).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result2 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 2).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result3 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 3).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result4 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 4).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result5 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 5).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result6 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 6).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result7 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 7).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result8 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 8).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result9 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 9).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result10 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 10).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result11 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 11).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result12 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == viewModel.Point.OwnerOfficeId && d.Month == 12).Select(d => d.Result).FirstOrDefaultAsync();

            viewModel.yearPoint = yearPoint;
            return View(viewModel);
        }

        public async Task<IActionResult> AllEdit(int poeid, int yearPoint, int? ownerofficeid)
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
                ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", ownerofficeid);
            }

            viewModel.result1 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 1).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result2 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 2).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result3 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 3).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result4 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 4).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result5 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 5).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result6 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 6).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result7 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 7).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result8 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 8).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result9 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 9).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result10 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 10).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result11 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 11).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result12 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == 12).Select(d => d.Result).FirstOrDefaultAsync();

            viewModel.Office = await _context.Offices.Where(o => o.Id == ownerofficeid).FirstOrDefaultAsync();
            viewModel.yearPoint = yearPoint;
            return View(viewModel);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost([Bind("yearPoint,poeid,result1,result2,result3,result4,result5,result6,result7,result8,result9,result10,result11,result12")] DataForEvaluationViewModel dataView, int? ownerofficeid)
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

                await SaveResult(dataView.Point.Id, office.Id, 10, dataView.result10, user.Id);
                await SaveResult(dataView.Point.Id, office.Id, 11, dataView.result11, user.Id);
                await SaveResult(dataView.Point.Id, office.Id, 12, dataView.result12, user.Id);
                await SaveResult(dataView.Point.Id, office.Id, 1, dataView.result1, user.Id);
                await SaveResult(dataView.Point.Id, office.Id, 2, dataView.result2, user.Id);
                await SaveResult(dataView.Point.Id, office.Id, 3, dataView.result3, user.Id);
                await SaveResult(dataView.Point.Id, office.Id, 4, dataView.result4, user.Id);
                await SaveResult(dataView.Point.Id, office.Id, 5, dataView.result5, user.Id);
                await SaveResult(dataView.Point.Id, office.Id, 6, dataView.result6, user.Id);
                await SaveResult(dataView.Point.Id, office.Id, 7, dataView.result7, user.Id);
                await SaveResult(dataView.Point.Id, office.Id, 8, dataView.result8, user.Id);
                await SaveResult(dataView.Point.Id, office.Id, 9, dataView.result9, user.Id);

                if (dataView.Point.OwnerOfficeId != null)
                {
                    return RedirectToAction(nameof(Index), new { yearPoint = dataView.yearPoint });
                }
                return RedirectToAction(nameof(Edit), new { poeid = dataView.poeid, yearPoint = dataView.yearPoint });
            }
            return View(dataView);
        }

        public async Task SaveResult(int poeid, int ownerofficeid, int month, decimal? result, string userid)
        {
            if (result != null)
            {
                DataForEvaluation dataForEvaluation;
                dataForEvaluation = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == month).FirstOrDefaultAsync();
                if (dataForEvaluation != null)
                {
                    dataForEvaluation.UpdateUserId = userid;
                    dataForEvaluation.Result = result;
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
                    dataForEvaluation.Result = result;
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
            }// Result 1
        }

        private bool DataForEvaluationExists(int id)
        {
            return _context.DataForEvaluations.Any(e => e.Id == id);
        }
    }
}
