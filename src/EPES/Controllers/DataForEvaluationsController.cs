using EPES.Data;
using EPES.Models;
using EPES.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

            DateTime yearForQuery = new DateTime(DateTime.Now.AddYears(yearPoint).Year, 1, 1);
            //if (yearPoint == 0)
            //{
            //    yearForQuery = new DateTime(DateTime.Now.Year, 1, 1);
            //}
            //else
            //{
            //    yearForQuery = new DateTime(DateTime.Now.AddYears(yearPoint).Year, 1, 1);
            //}

            if (User.IsInRole("Admin"))
            {
                if (String.IsNullOrEmpty(selectoffice))
                {
                    viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
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
        public async Task<IActionResult> Details(string selectoffice, int poeid, int yearPoint)
        {
            DataForEvaluationViewModel viewModel = new DataForEvaluationViewModel();
            viewModel.Point = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Where(p => p.Id == poeid).FirstOrDefaultAsync();
            if (viewModel.Point == null)
            {
                return NotFound();
            }

            viewModel.expect1 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 1).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect2 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 2).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect3 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 3).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect4 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 4).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect5 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 5).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect6 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 6).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect7 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 7).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect8 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 8).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect9 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 9).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect10 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 10).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect11 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 11).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect12 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 12).Select(d => d.Expect).FirstOrDefaultAsync();

            ViewBag.selectoffice = selectoffice;
            viewModel.yearPoint = yearPoint;
            return View(viewModel);
        }

        // GET: DataForEvaluations/Edit/5
        public async Task<IActionResult> Edit(string selectoffice, int poeid, int yearPoint)
        {
            DataForEvaluationViewModel viewModel = new DataForEvaluationViewModel();
            viewModel.Point = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Where(p => p.Id == poeid).FirstOrDefaultAsync();

            if (viewModel.Point == null)
            {
                return NotFound();
            }

            viewModel.expect1 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 1).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect2 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 2).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect3 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 3).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect4 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 4).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect5 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 5).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect6 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 6).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect7 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 7).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect8 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 8).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect9 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 9).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect10 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 10).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect11 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 11).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect12 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 12).Select(d => d.Expect).FirstOrDefaultAsync();

            List<Object> list = new List<object>();
            list.Add(new { Value = "", Detail = "เลือกระดับผลปฏิบัติราชการ" });
            list.Add(new { Value = "1", Detail = "1." + viewModel.Point.DetailRate1 });
            list.Add(new { Value = "2", Detail = "2." + viewModel.Point.DetailRate2 });
            list.Add(new { Value = "3", Detail = "3." + viewModel.Point.DetailRate3 });
            list.Add(new { Value = "4", Detail = "4." + viewModel.Point.DetailRate4 });
            list.Add(new { Value = "5", Detail = "5." + viewModel.Point.DetailRate5 });
            ViewBag.SelectLevel10 = new SelectList(list, "Value", "Detail", viewModel.expect10.ToString("N0"));
            ViewBag.SelectLevel11 = new SelectList(list, "Value", "Detail", viewModel.expect11.ToString("N0"));
            ViewBag.SelectLevel12 = new SelectList(list, "Value", "Detail", viewModel.expect12.ToString("N0"));
            ViewBag.SelectLevel1 = new SelectList(list, "Value", "Detail", viewModel.expect1.ToString("N0"));
            ViewBag.SelectLevel2 = new SelectList(list, "Value", "Detail", viewModel.expect2.ToString("N0"));
            ViewBag.SelectLevel3 = new SelectList(list, "Value", "Detail", viewModel.expect3.ToString("N0"));


            List<Object> list2 = new List<object>();
            list2.Add(new { Value = "", Detail = "เลือกระดับผลปฏิบัติราชการ" });
            list2.Add(new { Value = "1", Detail = "1." + viewModel.Point.Detail2Rate1 });
            list2.Add(new { Value = "2", Detail = "2." + viewModel.Point.Detail2Rate2 });
            list2.Add(new { Value = "3", Detail = "3." + viewModel.Point.Detail2Rate3 });
            list2.Add(new { Value = "4", Detail = "4." + viewModel.Point.Detail2Rate4 });
            list2.Add(new { Value = "5", Detail = "5." + viewModel.Point.Detail2Rate5 });
            ViewBag.SelectLevel4 = new SelectList(list2, "Value", "Detail", viewModel.expect4.ToString("N0"));
            ViewBag.SelectLevel5 = new SelectList(list2, "Value", "Detail", viewModel.expect5.ToString("N0"));
            ViewBag.SelectLevel6 = new SelectList(list2, "Value", "Detail", viewModel.expect6.ToString("N0"));
            ViewBag.SelectLevel7 = new SelectList(list2, "Value", "Detail", viewModel.expect7.ToString("N0"));
            ViewBag.SelectLevel8 = new SelectList(list2, "Value", "Detail", viewModel.expect8.ToString("N0"));
            ViewBag.SelectLevel9 = new SelectList(list2, "Value", "Detail", viewModel.expect9.ToString("N0"));
            ViewBag.SelectLevelx4 = new SelectList(list, "Value", "Detail", viewModel.expect4.ToString("N0"));
            ViewBag.SelectLevelx5 = new SelectList(list, "Value", "Detail", viewModel.expect5.ToString("N0"));
            ViewBag.SelectLevelx6 = new SelectList(list, "Value", "Detail", viewModel.expect6.ToString("N0"));
            ViewBag.SelectLevelx7 = new SelectList(list, "Value", "Detail", viewModel.expect7.ToString("N0"));
            ViewBag.SelectLevelx8 = new SelectList(list, "Value", "Detail", viewModel.expect8.ToString("N0"));
            ViewBag.SelectLevelx9 = new SelectList(list, "Value", "Detail", viewModel.expect9.ToString("N0"));

            ViewBag.selectoffice = selectoffice;
            viewModel.poeid = poeid;
            viewModel.yearPoint = yearPoint;
            return View(viewModel);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost([Bind("yearPoint,poeid,expect1,expect2,expect3,expect4,expect5,expect6,expect7,expect8,expect9,expect10,expect11,expect12")] DataForEvaluationViewModel viewModel, string selectoffice)
        {
            viewModel.Point = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Where(p => p.Id == viewModel.poeid).FirstOrDefaultAsync();

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                await SaveExpect(viewModel.Point.Id, viewModel.Point.OwnerOffice.Id, 10, viewModel.expect10, user.Id);
                await SaveExpect(viewModel.Point.Id, viewModel.Point.OwnerOffice.Id, 11, viewModel.expect11, user.Id);
                await SaveExpect(viewModel.Point.Id, viewModel.Point.OwnerOffice.Id, 12, viewModel.expect12, user.Id);
                await SaveExpect(viewModel.Point.Id, viewModel.Point.OwnerOffice.Id, 1, viewModel.expect1, user.Id);
                await SaveExpect(viewModel.Point.Id, viewModel.Point.OwnerOffice.Id, 2, viewModel.expect2, user.Id);
                await SaveExpect(viewModel.Point.Id, viewModel.Point.OwnerOffice.Id, 3, viewModel.expect3, user.Id);
                await SaveExpect(viewModel.Point.Id, viewModel.Point.OwnerOffice.Id, 4, viewModel.expect4, user.Id);
                await SaveExpect(viewModel.Point.Id, viewModel.Point.OwnerOffice.Id, 5, viewModel.expect5, user.Id);
                await SaveExpect(viewModel.Point.Id, viewModel.Point.OwnerOffice.Id, 6, viewModel.expect6, user.Id);
                await SaveExpect(viewModel.Point.Id, viewModel.Point.OwnerOffice.Id, 7, viewModel.expect7, user.Id);
                await SaveExpect(viewModel.Point.Id, viewModel.Point.OwnerOffice.Id, 8, viewModel.expect8, user.Id);
                await SaveExpect(viewModel.Point.Id, viewModel.Point.OwnerOffice.Id, 9, viewModel.expect9, user.Id);

                return RedirectToAction(nameof(Index), new { yearPoint = viewModel.yearPoint, selectoffice = selectoffice });
            }

            viewModel.expect1 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == viewModel.poeid && d.Month == 1).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect2 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == viewModel.poeid && d.Month == 2).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect3 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == viewModel.poeid && d.Month == 3).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect4 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == viewModel.poeid && d.Month == 4).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect5 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == viewModel.poeid && d.Month == 5).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect6 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == viewModel.poeid && d.Month == 6).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect7 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == viewModel.poeid && d.Month == 7).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect8 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == viewModel.poeid && d.Month == 8).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect9 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == viewModel.poeid && d.Month == 9).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect10 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == viewModel.poeid && d.Month == 10).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect11 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == viewModel.poeid && d.Month == 11).Select(d => d.Expect).FirstOrDefaultAsync();
            viewModel.expect12 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == viewModel.poeid && d.Month == 12).Select(d => d.Expect).FirstOrDefaultAsync();

            List<Object> list = new List<object>();
            list.Add(new { Value = "", Detail = "เลือกระดับผลปฏิบัติราชการ" });
            list.Add(new { Value = "1", Detail = "1." + viewModel.Point.DetailRate1 });
            list.Add(new { Value = "2", Detail = "2." + viewModel.Point.DetailRate2 });
            list.Add(new { Value = "3", Detail = "3." + viewModel.Point.DetailRate3 });
            list.Add(new { Value = "4", Detail = "4." + viewModel.Point.DetailRate4 });
            list.Add(new { Value = "5", Detail = "5." + viewModel.Point.DetailRate5 });
            ViewBag.SelectLevel = new SelectList(list, "Value", "Detail");
            ViewBag.SelectLevel10 = new SelectList(list, "Value", "Detail", viewModel.expect10.ToString("N0"));
            ViewBag.SelectLevel11 = new SelectList(list, "Value", "Detail", viewModel.expect11.ToString("N0"));
            ViewBag.SelectLevel12 = new SelectList(list, "Value", "Detail", viewModel.expect12.ToString("N0"));
            ViewBag.SelectLevel1 = new SelectList(list, "Value", "Detail", viewModel.expect1.ToString("N0"));
            ViewBag.SelectLevel2 = new SelectList(list, "Value", "Detail", viewModel.expect2.ToString("N0"));
            ViewBag.SelectLevel3 = new SelectList(list, "Value", "Detail", viewModel.expect3.ToString("N0"));


            List<Object> list2 = new List<object>();
            list2.Add(new { Value = "", Detail = "เลือกระดับผลปฏิบัติราชการ" });
            list2.Add(new { Value = "1", Detail = "1." + viewModel.Point.Detail2Rate1 });
            list2.Add(new { Value = "2", Detail = "2." + viewModel.Point.Detail2Rate2 });
            list2.Add(new { Value = "3", Detail = "3." + viewModel.Point.Detail2Rate3 });
            list2.Add(new { Value = "4", Detail = "4." + viewModel.Point.Detail2Rate4 });
            list2.Add(new { Value = "5", Detail = "5." + viewModel.Point.Detail2Rate5 });
            ViewBag.SelectLevel4 = new SelectList(list2, "Value", "Detail", viewModel.expect4.ToString("N0"));
            ViewBag.SelectLevel5 = new SelectList(list2, "Value", "Detail", viewModel.expect5.ToString("N0"));
            ViewBag.SelectLevel6 = new SelectList(list2, "Value", "Detail", viewModel.expect6.ToString("N0"));
            ViewBag.SelectLevel7 = new SelectList(list2, "Value", "Detail", viewModel.expect7.ToString("N0"));
            ViewBag.SelectLevel8 = new SelectList(list2, "Value", "Detail", viewModel.expect8.ToString("N0"));
            ViewBag.SelectLevel9 = new SelectList(list2, "Value", "Detail", viewModel.expect9.ToString("N0"));

            ViewBag.selectoffice = selectoffice;
            return View(viewModel);
        }

        private bool DataForEvaluationExists(int id)
        {
            return _context.DataForEvaluations.Any(e => e.Id == id);
        }

        public async Task SaveExpect(int poeid, int ownerofficeid, int month, decimal expect, string userid)
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
        }
    }
}
