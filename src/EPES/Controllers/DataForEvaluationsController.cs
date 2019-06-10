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
        public async Task<IActionResult> Index(string selectoffice, int yearPoint = 0)
        {
            var user = await _userManager.GetUserAsync(User);
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

            if (User.IsInRole("Admin"))
            {
                if (String.IsNullOrEmpty(selectoffice))
                {
                    viewModel.pointA = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.A) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.B) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.C) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", user.OfficeId);
                }
                else
                {
                    if (selectoffice.Substring(0, 3) == "000")
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    }
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);
                }
            }
            else
            {
                if (String.IsNullOrEmpty(selectoffice))
                {
                    if (user.OfficeId.Substring(0, 3) == "000")
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();

                        ViewBag.OfficeCode = new SelectList(await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => new { Code = b.OwnerOffice.Code, Name = b.OwnerOffice.Name }).Distinct().ToListAsync(), "Code", "Name", user.OfficeId);
                    }
                    else
                    {
                        ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Code", "Name", user.OfficeId);
                    }

                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                }
                else
                {
                    if (user.OfficeId.Substring(0, 3) == "000")
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();

                        ViewBag.OfficeCode = new SelectList(await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => new { Code = b.OwnerOffice.Code, Name = b.OwnerOffice.Name }).Distinct().ToListAsync(), "Code", "Name", selectoffice);
                    }
                    else
                    {
                        ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);
                    }

                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                }
            }

            ViewBag.selectoffice = selectoffice;
            viewModel.yearPoint = yearPoint;
            return View(viewModel);
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexPost(string selectoffice, int yearPoint)
        {
            var user = await _userManager.GetUserAsync(User);

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

            if (User.IsInRole("Admin"))
            {
                if (String.IsNullOrEmpty(selectoffice))
                {
                    viewModel.pointA = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.A) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.B) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.C) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", user.OfficeId);
                }
                else
                {
                    if (selectoffice.Substring(0, 3) == "000")
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    }
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);
                }
            }
            else
            {
                if (String.IsNullOrEmpty(selectoffice))
                {
                    if (user.OfficeId.Substring(0, 3) == "000")
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();

                        ViewBag.OfficeCode = new SelectList(await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => new { Code = b.OwnerOffice.Code, Name = b.OwnerOffice.Name }).Distinct().ToListAsync(), "Code", "Name", user.OfficeId);
                    }
                    else
                    {
                        ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Code", "Name", user.OfficeId);
                    }

                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                }
                else
                {
                    if (user.OfficeId.Substring(0, 3) == "000")
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();

                        ViewBag.OfficeCode = new SelectList(await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => new { Code = b.OwnerOffice.Code, Name = b.OwnerOffice.Name }).Distinct().ToListAsync(), "Code", "Name", selectoffice);
                    }
                    else
                    {
                        ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);
                    }

                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                }
            }

            ViewBag.selectoffice = selectoffice;
            viewModel.yearPoint = yearPoint;
            return View(viewModel);
        }

        // GET: DataForEvaluations/Details/5
        public async Task<IActionResult> Details(string selectoffice, int? poeid, int yearPoint)
        {
            if (poeid == null)
            {
                return NotFound();
            }

            DataForEvaluationViewModel viewModel = new DataForEvaluationViewModel();
            viewModel.Point = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Where(p => p.Id == poeid).FirstOrDefaultAsync();

            Office office;
            if (String.IsNullOrEmpty(selectoffice))
            {
                var user = await _userManager.GetUserAsync(User);
                office = await _context.Offices.Where(o => o.Code == user.OfficeId).FirstOrDefaultAsync();
            }
            else
            {
                office = await _context.Offices.Where(o => o.Code == selectoffice).FirstOrDefaultAsync();
            }

            viewModel.expect1 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == office.Id && d.Month == 1).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect2 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == office.Id && d.Month == 2).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect3 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == office.Id && d.Month == 3).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect4 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == office.Id && d.Month == 4).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect5 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == office.Id && d.Month == 5).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect6 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == office.Id && d.Month == 6).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect7 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == office.Id && d.Month == 7).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect8 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == office.Id && d.Month == 8).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect9 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == office.Id && d.Month == 9).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect10 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == office.Id && d.Month == 10).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect11 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == office.Id && d.Month == 11).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect12 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == office.Id && d.Month == 12).Select(d => d.Expect).FirstOrDefaultAsync();

            ViewBag.selectoffice = selectoffice;
            viewModel.yearPoint = yearPoint;
            viewModel.Office = office;
            return View(viewModel);
        }

        // GET: DataForEvaluations/Edit/5
        public async Task<IActionResult> Edit(string selectoffice, int? poeid, int yearPoint)
        {
            if (poeid == null)
            {
                return NotFound();
            }

            DataForEvaluationViewModel viewModel = new DataForEvaluationViewModel();
            viewModel.Point = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Where(p => p.Id == poeid).FirstOrDefaultAsync();

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

            ViewBag.selectoffice = selectoffice;
            viewModel.yearPoint = yearPoint;
            return View(viewModel);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost([Bind("yearPoint,poeid,expect1,expect2,expect3,expect4,expect5,expect6,expect7,expect8,expect9,expect10,expect11,expect12")] DataForEvaluationViewModel dataView, string selectoffice, int? ownerofficeid, int yearPoint)
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

            ViewBag.selectoffice = selectoffice;
            dataView.yearPoint = yearPoint;
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
