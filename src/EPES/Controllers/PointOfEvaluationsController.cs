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
        public async Task<IActionResult> Index(string selectoffice, int yearPoint = 0)
        {
            var user = await _userManager.GetUserAsync(User);

            DateTime yearForQuery;
            if (yearPoint == 0)
            {
                if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
                {
                    yearForQuery = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);
                }
                else
                {
                    yearForQuery = new DateTime(DateTime.Now.Year, 1, 1);
                }
            }
            else
            {
                if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
                {
                    yearForQuery = new DateTime(DateTime.Now.AddYears(1 + yearPoint).Year, 1, 1);
                }
                else
                {
                    yearForQuery = new DateTime(DateTime.Now.AddYears(yearPoint).Year, 1, 1);
                }
            }

            var viewModel = new PointOfEvaluationViewModel();
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
            else  // No Admin
            {
                if (String.IsNullOrEmpty(selectoffice))
                {
                    if (user.OfficeId.Substring(0, 3) == "000")
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();

                        var officeList = await _context.PointOfEvaluations.Where(p => (p.OwnerOffice.Code == user.OfficeId) || (p.Plan == TypeOfPlan.B && (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => new { Code = b.OwnerOffice.Code, Name = b.OwnerOffice.Name }).Distinct().ToListAsync();
                        if (officeList.Count < 1)
                        {
                            officeList.Add(new { Code = user.OfficeId, Name = user.OfficeName });
                        }
                        ViewBag.OfficeCode = new SelectList(officeList, "Code", "Name", user.OfficeId);
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

                        var officeList = await _context.PointOfEvaluations.Where(p => (p.OwnerOffice.Code == user.OfficeId) || (p.Plan == TypeOfPlan.B && (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => new { Code = b.OwnerOffice.Code, Name = b.OwnerOffice.Name }).Distinct().ToListAsync();
                        if (officeList.Count < 1)
                        {
                            officeList.Add(new { Code = user.OfficeId, Name = user.OfficeName });
                        }
                        ViewBag.OfficeCode = new SelectList(officeList, "Code", "Name", selectoffice);
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

            viewModel.selectoffice = selectoffice;
            ViewBag.selectoffice = selectoffice;
            viewModel.yearPoint = yearPoint;
            return View(viewModel);
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexPost(string selectoffice, int yearPoint = 0)
        {
            var user = await _userManager.GetUserAsync(User);

            DateTime yearForQuery;
            if (yearPoint == 0)
            {
                if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
                {
                    yearForQuery = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);
                }
                else
                {
                    yearForQuery = new DateTime(DateTime.Now.Year, 1, 1);
                }
            }
            else
            {
                if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
                {
                    yearForQuery = new DateTime(DateTime.Now.AddYears(1 + yearPoint).Year, 1, 1);
                }
                else
                {
                    yearForQuery = new DateTime(DateTime.Now.AddYears(yearPoint).Year, 1, 1);
                }
            }

            var viewModel = new PointOfEvaluationViewModel();
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

                        var officeList = await _context.PointOfEvaluations.Where(p => (p.OwnerOffice.Code == user.OfficeId) || (p.Plan == TypeOfPlan.B && (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => new { Code = b.OwnerOffice.Code, Name = b.OwnerOffice.Name }).Distinct().ToListAsync();
                        if (officeList.Count < 1)
                        {
                            officeList.Add(new { Code = user.OfficeId, Name = user.OfficeName });
                        }
                        ViewBag.OfficeCode = new SelectList(officeList, "Code", "Name", user.OfficeId);
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

                        var officeList = await _context.PointOfEvaluations.Where(p => (p.OwnerOffice.Code == user.OfficeId) || (p.Plan == TypeOfPlan.B && (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => new { Code = b.OwnerOffice.Code, Name = b.OwnerOffice.Name }).Distinct().ToListAsync();
                        if (officeList.Count < 1)
                        {
                            officeList.Add(new { Code = user.OfficeId, Name = user.OfficeName });
                        }
                        ViewBag.OfficeCode = new SelectList(officeList, "Code", "Name", selectoffice);
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

            if (!User.IsInRole("Admin") && user.OfficeId != selectoffice && user.OfficeId.Substring(0, 3) == "000" && user.OfficeId != "00013000")
            {
                //return RedirectToAction(nameof(IndexAudit), new { yearPoint = yearPoint, selectoffice = selectoffice });
                return View("IndexAudit", viewModel);
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Details(string selectoffice, int? id, int yearPoint = 0)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pointOfEvaluation = await _context.PointOfEvaluations
                .Include(p => p.OwnerOffice)
                .Include(p => p.AuditOffice)
                .Include(p => p.DataForEvaluations)
                .Include(p => p.Rounds)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pointOfEvaluation == null)
            {
                return NotFound();
            }

            ViewBag.selectoffice = selectoffice;
            ViewBag.yearPoint = yearPoint;
            return View(pointOfEvaluation);
        }

        // GET: PointOfEvaluations/Create
        public async Task<IActionResult> Create(string selectoffice, int plan, int yearPoint)
        {
            var user = await _userManager.GetUserAsync(User);
            var office = await _context.Offices.Where(o => o.Code == user.OfficeId).FirstOrDefaultAsync();
            var officeselect = await _context.Offices.Where(o => o.Code == selectoffice).FirstOrDefaultAsync();

            switch (plan)
            {
                case 0:
                    ViewBag.Plan = "A";
                    ViewBag.PlanValue = 0;

                    if (User.IsInRole("Admin"))
                    {
                        if (String.IsNullOrEmpty(selectoffice))
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", office.Id);
                        }
                        else
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", officeselect.Id);
                        }
                    }
                    else
                    {
                        List<Object> list = new List<object>();
                        list.Add(new { Id = office.Id, Name = office.Name });

                        ViewBag.OfficeId = new SelectList(list, "Id", "Name", office.Id);
                    }

                    ViewBag.AuditOfficeId = ViewBag.OfficeId;
                    break;
                case 1:
                    ViewBag.Plan = "B";
                    ViewBag.PlanValue = 1;

                    if (User.IsInRole("Admin") || (User.IsInRole("Manager") && user.OfficeId.StartsWith("000")))
                    {
                        if (String.IsNullOrEmpty(selectoffice))
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", office.Id);
                        }
                        else
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", officeselect.Id);
                        }
                    }

                    if (User.IsInRole("Manager") && user.OfficeId.Substring(2, 6) == "000000")
                    {
                        if (String.IsNullOrEmpty(selectoffice))
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Id", "Name", office.Id);
                        }
                        else
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Id", "Name", officeselect.Id);
                        }
                    }

                    if (User.IsInRole("User"))
                    {
                        List<Object> list = new List<object>();
                        list.Add(new { Id = office.Id, Name = office.Name });

                        ViewBag.OfficeId = new SelectList(list, "Id", "Name", office.Id);
                    }

                    if (User.IsInRole("Admin") || User.IsInRole("User") || (User.IsInRole("Manager") && user.OfficeId.Substring(2, 6) == "000000"))
                    {
                        if (String.IsNullOrEmpty(selectoffice))
                        {
                            ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", office.Id);
                        }
                        else
                        {
                            if (selectoffice.Substring(0, 3) == "000")
                            {
                                ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", officeselect.Id);
                            }
                            else
                            {
                                ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", office.Id);
                            }
                        }
                    }
                    else
                    {
                        List<Object> list = new List<object>();
                        list.Add(new { Id = office.Id, Name = office.Name });

                        ViewBag.AuditOfficeId = new SelectList(list, "Id", "Name", office.Id);
                    }
                    break;
                case 2:
                    ViewBag.Plan = "C";
                    ViewBag.PlanValue = 2;

                    if (User.IsInRole("Admin"))
                    {
                        if (String.IsNullOrEmpty(selectoffice))
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", office.Id);
                        }
                        else
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", officeselect.Id);
                        }
                        ViewBag.AuditOfficeId = ViewBag.OfficeId;
                    }

                    var item = await _context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000").Select(d => new { Id = d.Id, Name = d.Name }).ToListAsync();

                    if (User.IsInRole("Manager") && user.OfficeId.Substring(2, 6) == "000000")
                    {
                        var selectitem = await _context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000").Select(d => new { Id = d.Id, Name = d.Name }).ToListAsync();
                        foreach (var itemAdd in selectitem)
                        {
                            item.Add(itemAdd);
                        }

                        if (String.IsNullOrEmpty(selectoffice))
                        {
                            ViewBag.OfficeId = new SelectList(selectitem, "Id", "Name", office.Id);
                            ViewBag.AuditOfficeId = new SelectList(item, "Id", "Name", office.Id);
                        }
                        else
                        {
                            ViewBag.OfficeId = new SelectList(selectitem, "Id", "Name", officeselect.Id);
                            ViewBag.AuditOfficeId = new SelectList(item, "Id", "Name", officeselect.Id);
                        }
                    }

                    if ((User.IsInRole("Manager") && user.OfficeId.Substring(0, 3) == "000") || User.IsInRole("User"))
                    {
                        List<Object> list = new List<object>();
                        list.Add(new { Id = office.Id, Name = office.Name });
                        ViewBag.OfficeId = new SelectList(list, "Id", "Name", office.Id);

                        if (User.IsInRole("Manager") && user.OfficeId.Substring(0, 3) == "000")
                        {
                            ViewBag.AuditOfficeId = ViewBag.OfficeId;
                        }
                        else
                        {
                            item.Add(new { Id = office.Id, Name = office.Name });
                            ViewBag.AuditOfficeId = new SelectList(item, "Id", "Name", office.Id);
                        }
                    }
                    break;
                case 3:
                    ViewBag.Plan = "D";
                    ViewBag.PlanValue = 3;

                    if (User.IsInRole("Admin") || (User.IsInRole("Manager") && user.OfficeId.StartsWith("000")))
                    {
                        if (String.IsNullOrEmpty(selectoffice))
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", office.Id);
                        }
                        else
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", officeselect.Id);
                        }
                    }

                    if (User.IsInRole("Manager") && user.OfficeId.Substring(2, 6) == "000000")
                    {
                        if (String.IsNullOrEmpty(selectoffice))
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Id", "Name", office.Id);
                        }
                        else
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Id", "Name", officeselect.Id);
                        }
                    }

                    if (User.IsInRole("User"))
                    {
                        List<Object> list = new List<object>();
                        list.Add(new { Id = office.Id, Name = office.Name });

                        ViewBag.OfficeId = new SelectList(list, "Id", "Name", office.Id);
                    }

                    if (User.IsInRole("Admin") || User.IsInRole("User") || (User.IsInRole("Manager") && user.OfficeId.Substring(2, 6) == "000000"))
                    {
                        if (String.IsNullOrEmpty(selectoffice))
                        {
                            ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", office.Id);
                        }
                        else
                        {
                            if (selectoffice.Substring(0, 3) == "000")
                            {
                                ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", officeselect.Id);
                            }
                            else
                            {
                                ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", office.Id);
                            }
                        }
                    }
                    else
                    {
                        List<Object> list = new List<object>();
                        list.Add(new { Id = office.Id, Name = office.Name });

                        ViewBag.AuditOfficeId = new SelectList(list, "Id", "Name", office.Id);
                    }
                    break;
            }

            ViewBag.selectoffice = selectoffice;
            ViewBag.yearPoint = yearPoint;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(/*[Bind("point.Point,point.SubPoint,point.Plan,point.DetailPlan,point.ExpectPlan,point.Ddrive,point.Name,point.Unit,point.Weight,point.OwnerOfficeId,point.AuditOfficeId,point.AutoApp,Round.Rate1,Round.Rate2,Round.Rate3,Round.Rate4,Round.Rate5,Round.DetailRate1,Round.DetailRate2,Round.DetailRate3,Round.DetailRate4,Round.DetailRate5,Round.R1MStart,Round.R1MStop,Round.R2MStart,Round.R2MStop,Round.R3MStart,Round.R3MStop,Round.R4MStart,Round.R4MStop,Round.R5MStart,Round.R5MStop,Round2.Rate1,Round2.Rate2,Round2.Rate3,Round2.Rate4,Round2.Rate5,Round2.DetailRate1,Round2.DetailRate2,Round2.DetailRate3,Round2.DetailRate4,Round2.DetailRate5,Round2.R1MStart,Round2.R1MStop,Round2.R2MStart,Round2.R2MStop,Round2.R3MStart,Round2.R3MStop,Round2.R4MStart,Round2.R4MStop,Round2.R5MStart,Round2.R5MStop,LRound.Rate1,LRound.Rate2,LRound.Rate3,LRound.Rate4,LRound.Rate5,LRound.DetailRate1,LRound.DetailRate2,LRound.DetailRate3,LRound.DetailRate4,LRound.DetailRate5,LRound.R1MStart,LRound.R1MStop,LRound.R2MStart,LRound.R2MStop,LRound.R3MStart,LRound.R3MStop,LRound.R4MStart,LRound.R4MStop,LRound.R5MStart,LRound.R5MStop,LRound2.Rate1,LRound2.Rate2,LRound2.Rate3,LRound2.Rate4,LRound2.Rate5,LRound2.DetailRate1,LRound2.DetailRate2,LRound2.DetailRate3,LRound2.DetailRate4,LRound2.DetailRate5,LRound2.R1MStart,LRound2.R1MStop,LRound2.R2MStart,LRound2.R2MStop,LRound2.R3MStart,LRound2.R3MStop,LRound2.R4MStart,LRound2.R4MStop,LRound2.R5MStart,LRound2.R5MStop,LRRound.LevelNumber,LRRound.Rate1,LRRound.Rate2,LRRound.Rate3,LRRound.Rate4,LRRound.Rate5,LRRound.DetailRate1,LRRound.DetailRate2,LRRound.DetailRate3,LRRound.DetailRate4,LRRound.DetailRate5,LRRound.R1MStart,LRRound.R1MStop,LRRound.R2MStart,LRRound.R2MStop,LRRound.R3MStart,LRRound.R3MStop,LRRound.R4MStart,LRRound.R4MStop,LRRound.R5MStart,LRRound.R5MStop,LRRound2.LevelNumber,LRRound2.Rate1,LRRound2.Rate2,LRRound2.Rate3,LRRound2.Rate4,LRRound2.Rate5,LRRound2.DetailRate1,LRRound2.DetailRate2,LRRound2.DetailRate3,LRRound2.DetailRate4,LRRound2.DetailRate5,LRRound2.R1MStart,LRRound2.R1MStop,LRRound2.R2MStart,LRRound2.R2MStop,LRRound2.R3MStart,LRRound2.R3MStop,LRRound2.R4MStart,LRRound2.R4MStop,LRRound2.R5MStart,LRRound2.R5MStop,expect1,expect2,expec3,expect4,expect5,expect6,expect7,expect8,expect9,expect10,expect11,expect12,selectoffice,yearPoint,roundNumber")] */PointOfEvaluationViewModel dataView)
        {
            var user = await _userManager.GetUserAsync(User);

            if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
            {
                dataView.point.Year = new DateTime(DateTime.Now.AddYears(dataView.yearPoint + 1).Year, 1, 1);
            }
            else
            {
                dataView.point.Year = new DateTime(DateTime.Now.AddYears(dataView.yearPoint).Year, 1, 1);
            }

            dataView.point.UpdateUserId = user.Id;
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(dataView.point);

                    if (dataView.point.Unit == UnitOfPoint.ร้อยละ)
                    {
                        dataView.Round.PointOfEvaluationId = dataView.point.Id;
                        dataView.Round.RoundNumber = 1;
                        _context.Add(dataView.Round);
                        if (dataView.roundNumber == 2)
                        {
                            dataView.Round2.PointOfEvaluationId = dataView.point.Id;
                            dataView.Round2.RoundNumber = 2;
                            _context.Add(dataView.Round2);
                        }
                    }
                    else if (dataView.point.Unit == UnitOfPoint.ระดับ)
                    {
                        dataView.LRound.PointOfEvaluationId = dataView.point.Id;
                        dataView.LRound.RoundNumber = 1;
                        _context.Add(dataView.LRound);
                        if (dataView.roundNumber == 2)
                        {
                            dataView.LRound2.PointOfEvaluationId = dataView.point.Id;
                            dataView.LRound2.RoundNumber = 2;
                            _context.Add(dataView.LRound2);
                        }
                    }
                    else if (dataView.point.Unit == UnitOfPoint.ระดับ_ร้อยละ)
                    {
                        dataView.LRRound.PointOfEvaluationId = dataView.point.Id;
                        dataView.LRRound.RoundNumber = 1;
                        _context.Add(dataView.LRRound);
                        if (dataView.roundNumber == 2)
                        {
                            dataView.LRRound2.PointOfEvaluationId = dataView.point.Id;
                            dataView.LRRound2.RoundNumber = 2;
                            _context.Add(dataView.LRRound2);
                        }
                    }

                    if (dataView.point.SubPoint == 1)
                    {
                        var pointMain = await _context.PointOfEvaluations.Where(p => p.Point == dataView.point.Point && p.SubPoint == 0 && p.Year == dataView.point.Year).FirstOrDefaultAsync();
                        if (pointMain != null)
                        {
                            pointMain.HasSub = true;
                            _context.Update(pointMain);
                        }
                    }

                    await _context.SaveChangesAsync();

                    await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 10, dataView.expect10, user.Id);
                    await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 11, dataView.expect11, user.Id);
                    await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 12, dataView.expect12, user.Id);
                    await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 1, dataView.expect1, user.Id);
                    await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 2, dataView.expect2, user.Id);
                    await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 3, dataView.expect3, user.Id);
                    await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 4, dataView.expect4, user.Id);
                    await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 5, dataView.expect5, user.Id);
                    await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 6, dataView.expect6, user.Id);
                    await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 7, dataView.expect7, user.Id);
                    await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 8, dataView.expect8, user.Id);
                    await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 9, dataView.expect9, user.Id);

                    return RedirectToAction(nameof(Index), new { yearPoint = dataView.yearPoint, selectoffice = dataView.selectoffice });
                }
            }
            catch (DbUpdateException)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "ไม่สามารถบันทึกข้อมูล. " +
                    "ลองพยายามบันทึกอีกครั้ง " +
                    "โปรดแจ้งผู้ดูแลระบบ");
            }

            var office = await _context.Offices.Where(o => o.Code == user.OfficeId).FirstOrDefaultAsync();
            var officeselect = await _context.Offices.Where(o => o.Code == dataView.selectoffice).FirstOrDefaultAsync();

            switch (dataView.point.Plan)
            {
                case TypeOfPlan.A:
                    ViewBag.Plan = "A";
                    ViewBag.PlanValue = 0;

                    if (User.IsInRole("Admin"))
                    {
                        if (String.IsNullOrEmpty(dataView.selectoffice))
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", office.Id);
                        }
                        else
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", officeselect.Id);
                        }
                    }
                    else
                    {
                        List<Object> list = new List<object>();
                        list.Add(new { Id = office.Id, Name = office.Name });

                        ViewBag.OfficeId = new SelectList(list, "Id", "Name", office.Id);
                    }

                    ViewBag.AuditOfficeId = ViewBag.OfficeId;
                    break;
                case TypeOfPlan.B:
                    ViewBag.Plan = "B";
                    ViewBag.PlanValue = 1;

                    if (User.IsInRole("Admin") || (User.IsInRole("Manager") && user.OfficeId.StartsWith("000")))
                    {
                        if (String.IsNullOrEmpty(dataView.selectoffice))
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", office.Id);
                        }
                        else
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", officeselect.Id);
                        }
                    }

                    if (User.IsInRole("Manager") && user.OfficeId.Substring(2, 6) == "000000")
                    {
                        if (String.IsNullOrEmpty(dataView.selectoffice))
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Id", "Name", office.Id);
                        }
                        else
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Id", "Name", officeselect.Id);
                        }
                    }

                    if (User.IsInRole("User"))
                    {
                        List<Object> list = new List<object>();
                        list.Add(new { Id = office.Id, Name = office.Name });

                        ViewBag.OfficeId = new SelectList(list, "Id", "Name", office.Id);
                    }

                    if (User.IsInRole("Admin") || User.IsInRole("User") || (User.IsInRole("Manager") && user.OfficeId.Substring(2, 6) == "000000"))
                    {
                        if (String.IsNullOrEmpty(dataView.selectoffice))
                        {
                            ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", office.Id);
                        }
                        else
                        {
                            if (dataView.selectoffice.Substring(0, 3) == "000")
                            {
                                ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", officeselect.Id);
                            }
                            else
                            {
                                ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", office.Id);
                            }
                        }
                    }
                    else
                    {
                        List<Object> list = new List<object>();
                        list.Add(new { Id = office.Id, Name = office.Name });

                        ViewBag.AuditOfficeId = new SelectList(list, "Id", "Name", office.Id);
                    }
                    break;
                case TypeOfPlan.C:
                    ViewBag.Plan = "C";
                    ViewBag.PlanValue = 2;

                    if (User.IsInRole("Admin"))
                    {
                        if (String.IsNullOrEmpty(dataView.selectoffice))
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", office.Id);
                        }
                        else
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", officeselect.Id);
                        }
                        ViewBag.AuditOfficeId = ViewBag.OfficeId;
                    }

                    var item = await _context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000").Select(d => new { Id = d.Id, Name = d.Name }).ToListAsync();

                    if (User.IsInRole("Manager") && user.OfficeId.Substring(2, 6) == "000000")
                    {
                        var selectitem = await _context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000").Select(d => new { Id = d.Id, Name = d.Name }).ToListAsync();
                        foreach (var itemAdd in selectitem)
                        {
                            item.Add(itemAdd);
                        }

                        if (String.IsNullOrEmpty(dataView.selectoffice))
                        {
                            ViewBag.OfficeId = new SelectList(selectitem, "Id", "Name", office.Id);
                            ViewBag.AuditOfficeId = new SelectList(item, "Id", "Name", office.Id);
                        }
                        else
                        {
                            ViewBag.OfficeId = new SelectList(selectitem, "Id", "Name", officeselect.Id);
                            ViewBag.AuditOfficeId = new SelectList(item, "Id", "Name", officeselect.Id);
                        }
                    }

                    if ((User.IsInRole("Manager") && user.OfficeId.Substring(0, 3) == "000") || User.IsInRole("User"))
                    {
                        List<Object> list = new List<object>();
                        list.Add(new { Id = office.Id, Name = office.Name });
                        ViewBag.OfficeId = new SelectList(list, "Id", "Name", office.Id);

                        if (User.IsInRole("Manager") && user.OfficeId.Substring(0, 3) == "000")
                        {
                            ViewBag.AuditOfficeId = ViewBag.OfficeId;
                        }
                        else
                        {
                            item.Add(new { Id = office.Id, Name = office.Name });
                            ViewBag.AuditOfficeId = new SelectList(item, "Id", "Name", office.Id);
                        }
                    }
                    break;
                case TypeOfPlan.D:
                    ViewBag.Plan = "D";
                    ViewBag.PlanValue = 3;

                    if (User.IsInRole("Admin") || (User.IsInRole("Manager") && user.OfficeId.StartsWith("000")))
                    {
                        if (String.IsNullOrEmpty(dataView.selectoffice))
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", office.Id);
                        }
                        else
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", officeselect.Id);
                        }
                    }

                    if (User.IsInRole("Manager") && user.OfficeId.Substring(2, 6) == "000000")
                    {
                        if (String.IsNullOrEmpty(dataView.selectoffice))
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Id", "Name", office.Id);
                        }
                        else
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Id", "Name", officeselect.Id);
                        }
                    }

                    if (User.IsInRole("User"))
                    {
                        List<Object> list = new List<object>();
                        list.Add(new { Id = office.Id, Name = office.Name });

                        ViewBag.OfficeId = new SelectList(list, "Id", "Name", office.Id);
                    }

                    if (User.IsInRole("Admin") || User.IsInRole("User") || (User.IsInRole("Manager") && user.OfficeId.Substring(2, 6) == "000000"))
                    {
                        if (String.IsNullOrEmpty(dataView.selectoffice))
                        {
                            ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", office.Id);
                        }
                        else
                        {
                            if (dataView.selectoffice.Substring(0, 3) == "000")
                            {
                                ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", officeselect.Id);
                            }
                            else
                            {
                                ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", office.Id);
                            }
                        }
                    }
                    else
                    {
                        List<Object> list = new List<object>();
                        list.Add(new { Id = office.Id, Name = office.Name });

                        ViewBag.AuditOfficeId = new SelectList(list, "Id", "Name", office.Id);
                    }
                    break;
            }

            ViewBag.selectoffice = dataView.selectoffice;
            ViewBag.yearPoint = dataView.yearPoint;
            return View(dataView);
        }

        // GET: PointOfEvaluations/Edit/5
        public async Task<IActionResult> Edit(string selectoffice, int? id, int yearPoint = 0)
        {
            if (id == null)
            {
                return NotFound();
            }

            PointOfEvaluationViewModel dataView = new PointOfEvaluationViewModel();
            dataView.point = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Where(p => p.Id == id).FirstOrDefaultAsync();

            List<Object> listsubpoint = new List<object>();
            listsubpoint.Add(new { value = 0, detail = "ไม่มี" });
            listsubpoint.Add(new { value = 1, detail = "ตัวชี้วัดย่อย 1" });
            listsubpoint.Add(new { value = 2, detail = "ตัวชี้วัดย่อย 2" });
            listsubpoint.Add(new { value = 3, detail = "ตัวชี้วัดย่อย 3" });
            listsubpoint.Add(new { value = 4, detail = "ตัวชี้วัดย่อย 4" });
            listsubpoint.Add(new { value = 5, detail = "ตัวชี้วัดย่อย 5" });
            ViewBag.SubPoint = new SelectList(listsubpoint, "value", "detail", dataView.point.SubPoint);

            if (dataView.point == null)
            {
                return NotFound();
            }

            dataView.Round = await _context.Rounds.Where(r => r.PointOfEvaluationId == id && r.RoundNumber == 1).FirstOrDefaultAsync();
            dataView.Round2 = await _context.Rounds.Where(r => r.PointOfEvaluationId == id && r.RoundNumber == 2).FirstOrDefaultAsync();
            dataView.LRound = await _context.Rounds.Where(r => r.PointOfEvaluationId == id && r.RoundNumber == 1).FirstOrDefaultAsync();
            dataView.LRound2 = await _context.Rounds.Where(r => r.PointOfEvaluationId == id && r.RoundNumber == 2).FirstOrDefaultAsync();
            dataView.LRRound = await _context.Rounds.Where(r => r.PointOfEvaluationId == id && r.RoundNumber == 1).FirstOrDefaultAsync();
            dataView.LRRound2 = await _context.Rounds.Where(r => r.PointOfEvaluationId == id && r.RoundNumber == 2).FirstOrDefaultAsync();

            dataView.expect10 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == id && d.Month == 10).Select(d => d.Expect).FirstOrDefaultAsync();
            dataView.expect11 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == id && d.Month == 11).Select(d => d.Expect).FirstOrDefaultAsync();
            dataView.expect12 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == id && d.Month == 12).Select(d => d.Expect).FirstOrDefaultAsync();
            dataView.expect1 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == id && d.Month == 1).Select(d => d.Expect).FirstOrDefaultAsync();
            dataView.expect2 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == id && d.Month == 2).Select(d => d.Expect).FirstOrDefaultAsync();
            dataView.expect3 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == id && d.Month == 3).Select(d => d.Expect).FirstOrDefaultAsync();
            dataView.expect4 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == id && d.Month == 4).Select(d => d.Expect).FirstOrDefaultAsync();
            dataView.expect5 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == id && d.Month == 5).Select(d => d.Expect).FirstOrDefaultAsync();
            dataView.expect6 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == id && d.Month == 6).Select(d => d.Expect).FirstOrDefaultAsync();
            dataView.expect7 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == id && d.Month == 7).Select(d => d.Expect).FirstOrDefaultAsync();
            dataView.expect8 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == id && d.Month == 8).Select(d => d.Expect).FirstOrDefaultAsync();
            dataView.expect9 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == id && d.Month == 9).Select(d => d.Expect).FirstOrDefaultAsync();

            var user = await _userManager.GetUserAsync(User);
            var office = await _context.Offices.Where(o => o.Code == user.OfficeId).FirstOrDefaultAsync();
            //var officeselect = await _context.Offices.Where(o => o.Code == selectoffice).FirstOrDefaultAsync();

            switch (dataView.point.Plan)
            {
                case TypeOfPlan.A:
                    ViewBag.Plan = "A";
                    ViewBag.PlanValue = 0;

                    if (User.IsInRole("Admin"))
                    {
                        ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name");
                    }
                    else
                    {
                        List<Object> list = new List<object>();
                        list.Add(new { Id = office.Id, Name = office.Name });

                        ViewBag.OfficeId = new SelectList(list, "Id", "Name");
                    }
                    ViewBag.AuditOfficeId = ViewBag.OfficeId;
                    break;

                case TypeOfPlan.B:
                    ViewBag.Plan = "B";
                    ViewBag.PlanValue = 1;

                    if (User.IsInRole("Admin") || (User.IsInRole("Manager") && user.OfficeId.StartsWith("000")))
                    {
                        ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name");
                    }
                    else if (User.IsInRole("Manager") && user.OfficeId.Substring(2, 6) == "000000")
                    {
                        ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Id", "Name");
                    }
                    else
                    {
                        List<Object> list = new List<object>();
                        list.Add(new { Id = office.Id, Name = office.Name });

                        ViewBag.OfficeId = new SelectList(list, "Id", "Name");
                    }

                    ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name");
                    break;

                case TypeOfPlan.C:
                    ViewBag.Plan = "C";
                    ViewBag.PlanValue = 2;

                    var item = await _context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000").Select(d => new { Id = d.Id, Name = d.Name }).ToListAsync();

                    if (User.IsInRole("Admin"))
                    {
                        ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name");

                        ViewBag.AuditOfficeId = ViewBag.OfficeId;
                    }
                    else if (User.IsInRole("Manager") && user.OfficeId.Substring(2, 6) == "000000")
                    {
                        var selectitem = await _context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000").Select(d => new { Id = d.Id, Name = d.Name }).ToListAsync();
                        foreach (var itemAdd in selectitem)
                        {
                            item.Add(itemAdd);
                        }
                        ViewBag.OfficeId = new SelectList(selectitem, "Id", "Name");
                        ViewBag.AuditOfficeId = new SelectList(item, "Id", "Name");
                    }
                    else
                    {
                        List<Object> list = new List<object>();
                        list.Add(new { Id = office.Id, Name = office.Name });
                        ViewBag.OfficeId = new SelectList(list, "Id", "Name");

                        if (User.IsInRole("Manager") && user.OfficeId.Substring(0, 3) == "000")
                        {
                            ViewBag.AuditOfficeId = ViewBag.OfficeId;
                        }
                        else
                        {
                            item.Add(new { Id = office.Id, Name = office.Name });
                            ViewBag.AuditOfficeId = new SelectList(item, "Id", "Name");
                        }
                    }
                    break;

                case TypeOfPlan.D:
                    ViewBag.Plan = "D";
                    ViewBag.PlanValue = 3;

                    if (User.IsInRole("Admin") || (User.IsInRole("Manager") && user.OfficeId.StartsWith("000")))
                    {
                        ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name");
                    }
                    else if (User.IsInRole("Manager") && user.OfficeId.Substring(2, 6) == "000000")
                    {
                        ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Id", "Name");
                    }
                    else
                    {
                        List<Object> list = new List<object>();
                        list.Add(new { Id = office.Id, Name = office.Name });

                        ViewBag.OfficeId = new SelectList(list, "Id", "Name");
                    }

                    ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name");

                    break;
            }

            ViewBag.selectoffice = selectoffice;
            ViewBag.yearPoint = yearPoint;
            dataView.roundNumber = dataView.point.Rounds.Count;
            return View(dataView);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(PointOfEvaluationViewModel dataView)
        {
            var user = await _userManager.GetUserAsync(User);

            var pointOfEvaluationToUpdate = await _context.PointOfEvaluations.Include(p => p.Rounds).FirstAsync(p => p.Id == dataView.point.Id);
            pointOfEvaluationToUpdate.UpdateUserId = user.Id;

            var unit = pointOfEvaluationToUpdate.Unit;
            if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
            {
                dataView.point.Year = new DateTime(DateTime.Now.AddYears(dataView.yearPoint + 1).Year, 1, 1);
            }
            else
            {
                dataView.point.Year = new DateTime(DateTime.Now.AddYears(dataView.yearPoint).Year, 1, 1);
            }

            pointOfEvaluationToUpdate.Year = dataView.point.Year;
            pointOfEvaluationToUpdate.Point = dataView.point.Point;
            pointOfEvaluationToUpdate.SubPoint = dataView.point.SubPoint;
            pointOfEvaluationToUpdate.Plan = dataView.point.Plan;
            pointOfEvaluationToUpdate.DetailPlan = dataView.point.DetailPlan;
            pointOfEvaluationToUpdate.ExpectPlan = dataView.point.ExpectPlan;
            pointOfEvaluationToUpdate.Ddrive = dataView.point.Ddrive;
            pointOfEvaluationToUpdate.Name = dataView.point.Name;
            pointOfEvaluationToUpdate.Unit = dataView.point.Unit;
            pointOfEvaluationToUpdate.Weight = dataView.point.Weight;
            pointOfEvaluationToUpdate.OwnerOfficeId = dataView.point.OwnerOfficeId;
            pointOfEvaluationToUpdate.AuditOfficeId = dataView.point.AuditOfficeId;
            pointOfEvaluationToUpdate.AutoApp = dataView.point.AutoApp;

            //if (await TryUpdateModelAsync<PointOfEvaluationViewModel>(
            //    dataView.point, "",
            //    p => p.point.Year, p => p.DetailPlan, p => p.Point, p => p.SubPoint, p => p.Plan, p => p.ExpectPlan, p => p.Ddrive, p => p.Name, p => p.Unit, p => p.Weight, p => p.AuditOfficeId, p => p.OwnerOfficeId, p=>p.AutoApp))
            //{
            try
            {
                if (pointOfEvaluationToUpdate.Rounds != null)
                {
                    foreach (var item in pointOfEvaluationToUpdate.Rounds)
                    {
                        _context.Rounds.Remove(item);
                    }
                }


                if (dataView.point.Unit == UnitOfPoint.ร้อยละ)
                {
                    dataView.Round.PointOfEvaluationId = dataView.point.Id;
                    dataView.Round.RoundNumber = 1;
                    _context.Add(dataView.Round);
                    if (dataView.roundNumber == 2)
                    {
                        dataView.Round2.PointOfEvaluationId = dataView.point.Id;
                        dataView.Round2.RoundNumber = 2;
                        _context.Add(dataView.Round2);
                    }
                }
                else if (dataView.point.Unit == UnitOfPoint.ระดับ)
                {
                    dataView.LRound.PointOfEvaluationId = dataView.point.Id;
                    dataView.LRound.RoundNumber = 1;
                    dataView.LRound.Rate1 = 1;
                    dataView.LRound.Rate2 = 2;
                    dataView.LRound.Rate3 = 3;
                    dataView.LRound.Rate4 = 4;
                    dataView.LRound.Rate5 = 5;
                    _context.Add(dataView.LRound);
                    if (dataView.roundNumber == 2)
                    {
                        dataView.LRound2.PointOfEvaluationId = dataView.point.Id;
                        dataView.LRound2.RoundNumber = 2;
                        dataView.LRound2.Rate1 = 1;
                        dataView.LRound2.Rate2 = 2;
                        dataView.LRound2.Rate3 = 3;
                        dataView.LRound2.Rate4 = 4;
                        dataView.LRound2.Rate5 = 5;
                        _context.Add(dataView.LRound2);
                    }
                }
                else if (dataView.point.Unit == UnitOfPoint.ระดับ_ร้อยละ)
                {
                    dataView.LRRound.PointOfEvaluationId = dataView.point.Id;
                    dataView.LRRound.RoundNumber = 1;
                    switch (dataView.LRRound.LevelNumber)
                    {
                        case 2:
                            dataView.LRound.Rate2 = 2;
                            break;
                        case 3:
                            dataView.LRound.Rate2 = 2;
                            dataView.LRound.Rate3 = 3;
                            break;
                        case 4:
                            dataView.LRound.Rate2 = 2;
                            dataView.LRound.Rate3 = 3;
                            dataView.LRound.Rate4 = 4;
                            break;
                        default:
                            break;
                    }
                    _context.Add(dataView.LRRound);
                    if (dataView.roundNumber == 2)
                    {
                        dataView.LRRound2.PointOfEvaluationId = dataView.point.Id;
                        dataView.LRRound2.RoundNumber = 2;
                        switch (dataView.LRRound2.LevelNumber)
                        {
                            case 2:
                                dataView.LRound2.Rate2 = 2;
                                break;
                            case 3:
                                dataView.LRound2.Rate2 = 2;
                                dataView.LRound2.Rate3 = 3;
                                break;
                            case 4:
                                dataView.LRound2.Rate2 = 2;
                                dataView.LRound2.Rate3 = 3;
                                dataView.LRound2.Rate4 = 4;
                                break;
                            default:
                                break;
                        }
                        _context.Add(dataView.LRRound2);
                    }
                }

                _context.Update(pointOfEvaluationToUpdate);
                await _context.SaveChangesAsync();

                await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 10, dataView.expect10, user.Id);
                await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 11, dataView.expect11, user.Id);
                await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 12, dataView.expect12, user.Id);
                await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 1, dataView.expect1, user.Id);
                await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 2, dataView.expect2, user.Id);
                await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 3, dataView.expect3, user.Id);
                await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 4, dataView.expect4, user.Id);
                await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 5, dataView.expect5, user.Id);
                await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 6, dataView.expect6, user.Id);
                await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 7, dataView.expect7, user.Id);
                await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 8, dataView.expect8, user.Id);
                await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 9, dataView.expect9, user.Id);

                return RedirectToAction(nameof(Index), new { yearPoint = dataView.yearPoint, selectoffice = dataView.selectoffice });
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "ไม่สามารถบันทึกข้อมูล. " +
                    "ลองพยายามบันทึกอีกครั้ง " +
                    "โปรดแจ้งผู้ดูแลระบบ");
            }
            //return RedirectToAction(nameof(Index), new { yearPoint = yearpoint, selectoffice = pointOfEvaluationToUpdate.AuditOfficeId });
            //}

            var office = await _context.Offices.Where(o => o.Code == user.OfficeId).FirstOrDefaultAsync();
            var officeselect = await _context.Offices.Where(o => o.Code == dataView.selectoffice).FirstOrDefaultAsync();

            switch (pointOfEvaluationToUpdate.Plan)
            {
                case TypeOfPlan.A:
                    ViewBag.Plan = "A";
                    ViewBag.PlanValue = 0;

                    if (User.IsInRole("Admin"))
                    {
                        if (String.IsNullOrEmpty(dataView.selectoffice))
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", office.Id);
                        }
                        else
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", officeselect.Id);
                        }
                    }
                    else
                    {
                        List<Object> list = new List<object>();
                        list.Add(new { Id = office.Id, Name = office.Name });

                        ViewBag.OfficeId = new SelectList(list, "Id", "Name", office.Id);
                    }

                    ViewBag.AuditOfficeId = ViewBag.OfficeId;
                    break;
                case TypeOfPlan.B:
                    ViewBag.Plan = "B";
                    ViewBag.PlanValue = 1;

                    if (User.IsInRole("Admin") || (User.IsInRole("Manager") && user.OfficeId.StartsWith("000")))
                    {
                        if (String.IsNullOrEmpty(dataView.selectoffice))
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", office.Id);
                        }
                        else
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", officeselect.Id);
                        }
                    }

                    if (User.IsInRole("Manager") && user.OfficeId.Substring(2, 6) == "000000")
                    {
                        if (String.IsNullOrEmpty(dataView.selectoffice))
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Id", "Name", office.Id);
                        }
                        else
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Id", "Name", officeselect.Id);
                        }
                    }

                    if (User.IsInRole("User"))
                    {
                        List<Object> list = new List<object>();
                        list.Add(new { Id = office.Id, Name = office.Name });

                        ViewBag.OfficeId = new SelectList(list, "Id", "Name", office.Id);
                    }

                    if (User.IsInRole("Admin") || User.IsInRole("User") || (User.IsInRole("Manager") && user.OfficeId.Substring(2, 6) == "000000"))
                    {
                        if (String.IsNullOrEmpty(dataView.selectoffice))
                        {
                            ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", office.Id);
                        }
                        else
                        {
                            if (dataView.selectoffice.Substring(0, 3) == "000")
                            {
                                ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", officeselect.Id);
                            }
                            else
                            {
                                ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", office.Id);
                            }
                        }
                    }
                    else
                    {
                        List<Object> list = new List<object>();
                        list.Add(new { Id = office.Id, Name = office.Name });

                        ViewBag.AuditOfficeId = new SelectList(list, "Id", "Name", office.Id);
                    }
                    break;
                case TypeOfPlan.C:
                    ViewBag.Plan = "C";
                    ViewBag.PlanValue = 2;

                    if (User.IsInRole("Admin"))
                    {
                        if (String.IsNullOrEmpty(dataView.selectoffice))
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", office.Id);
                        }
                        else
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", officeselect.Id);
                        }
                        ViewBag.AuditOfficeId = ViewBag.OfficeId;
                    }

                    var item = await _context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000").Select(d => new { Id = d.Id, Name = d.Name }).ToListAsync();

                    if (User.IsInRole("Manager") && user.OfficeId.Substring(2, 6) == "000000")
                    {
                        var selectitem = await _context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000").Select(d => new { Id = d.Id, Name = d.Name }).ToListAsync();
                        foreach (var itemAdd in selectitem)
                        {
                            item.Add(itemAdd);
                        }

                        if (String.IsNullOrEmpty(dataView.selectoffice))
                        {
                            ViewBag.OfficeId = new SelectList(selectitem, "Id", "Name", office.Id);
                            ViewBag.AuditOfficeId = new SelectList(item, "Id", "Name", office.Id);
                        }
                        else
                        {
                            ViewBag.OfficeId = new SelectList(selectitem, "Id", "Name", officeselect.Id);
                            ViewBag.AuditOfficeId = new SelectList(item, "Id", "Name", officeselect.Id);
                        }
                    }

                    if ((User.IsInRole("Manager") && user.OfficeId.Substring(0, 3) == "000") || User.IsInRole("User"))
                    {
                        List<Object> list = new List<object>();
                        list.Add(new { Id = office.Id, Name = office.Name });
                        ViewBag.OfficeId = new SelectList(list, "Id", "Name", office.Id);

                        if (User.IsInRole("Manager") && user.OfficeId.Substring(0, 3) == "000")
                        {
                            ViewBag.AuditOfficeId = ViewBag.OfficeId;
                        }
                        else
                        {
                            item.Add(new { Id = office.Id, Name = office.Name });
                            ViewBag.AuditOfficeId = new SelectList(item, "Id", "Name", office.Id);
                        }
                    }
                    break;
                case TypeOfPlan.D:
                    ViewBag.Plan = "D";
                    ViewBag.PlanValue = 3;

                    if (User.IsInRole("Admin") || (User.IsInRole("Manager") && user.OfficeId.StartsWith("000")))
                    {
                        if (String.IsNullOrEmpty(dataView.selectoffice))
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", office.Id);
                        }
                        else
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", officeselect.Id);
                        }
                    }

                    if (User.IsInRole("Manager") && user.OfficeId.Substring(2, 6) == "000000")
                    {
                        if (String.IsNullOrEmpty(dataView.selectoffice))
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Id", "Name", office.Id);
                        }
                        else
                        {
                            ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Id", "Name", officeselect.Id);
                        }
                    }

                    if (User.IsInRole("User"))
                    {
                        List<Object> list = new List<object>();
                        list.Add(new { Id = office.Id, Name = office.Name });

                        ViewBag.OfficeId = new SelectList(list, "Id", "Name", office.Id);
                    }

                    if (User.IsInRole("Admin") || User.IsInRole("User") || (User.IsInRole("Manager") && user.OfficeId.Substring(2, 6) == "000000"))
                    {
                        if (String.IsNullOrEmpty(dataView.selectoffice))
                        {
                            ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", office.Id);
                        }
                        else
                        {
                            if (dataView.selectoffice.Substring(0, 3) == "000")
                            {
                                ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", officeselect.Id);
                            }
                            else
                            {
                                ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", office.Id);
                            }
                        }
                    }
                    else
                    {
                        List<Object> list = new List<object>();
                        list.Add(new { Id = office.Id, Name = office.Name });

                        ViewBag.AuditOfficeId = new SelectList(list, "Id", "Name", office.Id);
                    }
                    break;
            }

            ViewBag.selectoffice = dataView.selectoffice;
            ViewBag.yearPoint = dataView.yearPoint;
            return View(pointOfEvaluationToUpdate);
        }

        // GET: PointOfEvaluations/Delete/5
        public async Task<IActionResult> Delete(string selectoffice, int? id, int yearPoint, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pointOfEvaluation = await _context.PointOfEvaluations
                .Include(p => p.OwnerOffice)
                .Include(p => p.AuditOffice)
                .Include(p => p.DataForEvaluations)
                .Include(p => p.Rounds)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pointOfEvaluation == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "ลบไม่ได้." +
                    "อาจมีการบันทึกเป้าหมายหรือผลการปฎิบัติแล้ว " +
                    "โปรดแจ้งผู้ดูแลระบบ";
            }

            ViewBag.selectoffice = selectoffice;
            ViewBag.yearPoint = yearPoint;
            return View(pointOfEvaluation);
        }

        // POST: PointOfEvaluations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string selectoffice, int Id, int yearP)
        {
            PointOfEvaluation poe = await _context.PointOfEvaluations
                .Include(p => p.DataForEvaluations)
                .Include(p => p.Rounds)
                .SingleAsync(p => p.Id == Id);

            //var data = await _context.DataForEvaluations
            //    .Where(d => d.PointOfEvaluationId == Id)
            //    .ToListAsync();

            //var round = await _context.Rounds
            //    .Where(r => r.PointOfEvaluationId == Id)
            //    .ToListAsync();

            foreach (var data in poe.DataForEvaluations)
            {
                _context.DataForEvaluations.Remove(data);
            }

            foreach (var round in poe.Rounds)
            {
                _context.Rounds.Remove(round);
            }

            try
            {
                _context.PointOfEvaluations.Remove(poe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { yearPoint = yearP, selectoffice = selectoffice });
            }
            catch (DbUpdateException /*ex*/)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = Id, yearPoint = yearP, saveChangesError = true, selectoffice = selectoffice });
            }
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
                    //_context.Update(dataForEvaluation);
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
                //dataForEvaluation.Approve = Approve.รอพิจารณา;
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

        [HttpGet, ActionName("CopyPak1ToPak")]
        public async Task<IActionResult> CopyPak1ToPak()
        {
            var target = await _context.Offices.Where(d => d.Code != "00000000" && d.Code != "01000000" && d.Code.Substring(2, 6) == "000000").ToListAsync();

            var user = await _userManager.GetUserAsync(User);

            try
            {
                for (int i = 1; i <= 21; i++)
                {
                    if (i == 18)
                    {
                        continue;
                    }

                    var dataPoint = await _context.PointOfEvaluations.Include(p => p.Rounds).Include(p => p.DataForEvaluations).Include(p => p.OwnerOffice).FirstOrDefaultAsync(p => p.Point == i && p.OwnerOffice.Code == "01000000");

                    var dataRounds = await _context.Rounds.Where(r => r.PointOfEvaluationId == dataPoint.Id).ToListAsync();

                    var dataForEPES = await _context.DataForEvaluations.Where(r => r.PointOfEvaluationId == dataPoint.Id).ToListAsync();

                    foreach (var item in target)
                    {
                        //if (i == 1)
                        //{
                        //    PointOfEvaluation poe = await _context.PointOfEvaluations
                        //                            .Include(p => p.DataForEvaluations)
                        //                            .Include(p => p.Rounds)
                        //                            .SingleAsync(p => p.Point == 1 && p.OwnerOfficeId == item.Id);

                        //    foreach (var data in poe.DataForEvaluations)
                        //    {
                        //        _context.DataForEvaluations.Remove(data);
                        //    }

                        //    foreach (var round in poe.Rounds)
                        //    {
                        //        _context.Rounds.Remove(round);
                        //    }

                        //    _context.PointOfEvaluations.Remove(poe);
                        //    await _context.SaveChangesAsync();
                        //}
                        var pointToCopy = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Where(p => p.OwnerOffice.Code == item.Code && p.Name == dataPoint.Name && p.Plan == dataPoint.Plan).FirstOrDefaultAsync();
                        if (pointToCopy == null)
                        {
                            pointToCopy = new PointOfEvaluation();
                            pointToCopy.AuditOfficeId = dataPoint.AuditOfficeId;
                            pointToCopy.AutoApp = dataPoint.AutoApp;
                            pointToCopy.Ddrive = dataPoint.Ddrive;
                            pointToCopy.DetailPlan = dataPoint.DetailPlan;
                            pointToCopy.ExpectPlan = dataPoint.ExpectPlan;
                            pointToCopy.HasSub = dataPoint.HasSub;
                            pointToCopy.Name = dataPoint.Name;
                            pointToCopy.OwnerOfficeId = item.Id;
                            pointToCopy.Plan = dataPoint.Plan;
                            pointToCopy.Point = dataPoint.Point;
                            pointToCopy.SubPoint = dataPoint.SubPoint;
                            pointToCopy.Unit = dataPoint.Unit;
                            pointToCopy.UpdateUserId = user.Id;
                            pointToCopy.Weight = dataPoint.Weight;
                            pointToCopy.Year = dataPoint.Year;

                            _context.PointOfEvaluations.Add(pointToCopy);
                            await _context.SaveChangesAsync();

                            foreach (var round in dataRounds)
                            {
                                var roundToCopy = new Round();
                                roundToCopy.PointOfEvaluationId = pointToCopy.Id;
                                roundToCopy.DetailRate1 = round.DetailRate1;
                                roundToCopy.DetailRate2 = round.DetailRate2;
                                roundToCopy.DetailRate3 = round.DetailRate3;
                                roundToCopy.DetailRate4 = round.DetailRate4;
                                roundToCopy.DetailRate5 = round.DetailRate5;
                                roundToCopy.LevelNumber = round.LevelNumber;
                                roundToCopy.Rate1MonthStart = round.Rate1MonthStart;
                                roundToCopy.Rate1MonthStop = round.Rate1MonthStop;
                                roundToCopy.Rate2MonthStart = round.Rate2MonthStart;
                                roundToCopy.Rate2MonthStop = round.Rate2MonthStop;
                                roundToCopy.Rate3MonthStart = round.Rate3MonthStart;
                                roundToCopy.Rate3MonthStop = round.Rate3MonthStop;
                                roundToCopy.Rate4MonthStart = round.Rate4MonthStart;
                                roundToCopy.Rate4MonthStop = round.Rate4MonthStop;
                                roundToCopy.Rate5MonthStart = round.Rate5MonthStart;
                                roundToCopy.Rate5MonthStop = round.Rate5MonthStop;
                                roundToCopy.Rate1 = round.Rate1;
                                roundToCopy.Rate2 = round.Rate2;
                                roundToCopy.Rate3 = round.Rate3;
                                roundToCopy.Rate4 = round.Rate4;
                                roundToCopy.Rate5 = round.Rate5;
                                roundToCopy.RoundNumber = round.RoundNumber;

                                _context.Rounds.Add(roundToCopy);
                                await _context.SaveChangesAsync();
                            }

                            foreach (var dataforE in dataForEPES)
                            {
                                var dataForEvaluation = new DataForEvaluation();
                                dataForEvaluation.UpdateUserId = user.Id;
                                dataForEvaluation.PointOfEvaluationId = pointToCopy.Id;
                                dataForEvaluation.OfficeId = item.Id;
                                dataForEvaluation.Month = dataforE.Month;
                                dataForEvaluation.Expect = dataforE.Expect;

                                _context.DataForEvaluations.Add(dataForEvaluation);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
                return Ok();
            }
            catch (DbUpdateException)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "ไม่สามารถบันทึกข้อมูล. " +
                    "ลองพยายามบันทึกอีกครั้ง " +
                    "โปรดแจ้งผู้ดูแลระบบ");
                return NotFound();
            }
        }

        [HttpGet, ActionName("CopyPak1ToST1")]
        public async Task<IActionResult> CopyPak1ToST1()
        {
            var target = await _context.Offices.Where(d => d.Code == "01001000").ToListAsync();

            var user = await _userManager.GetUserAsync(User);
            try
            {
                for (int i = 1; i <= 21; i++)
                {

                    if (i == 17 || i == 18)
                    {
                        continue;
                    }

                    var dataPoint = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).FirstOrDefaultAsync(p => p.Point == i && p.OwnerOffice.Code == "01000000");

                    if (!dataPoint.HasSub)
                    {
                        var dataRounds = await _context.Rounds.Where(r => r.PointOfEvaluationId == dataPoint.Id).ToListAsync();

                        var dataForEPES = await _context.DataForEvaluations.Where(r => r.PointOfEvaluationId == dataPoint.Id).ToListAsync();

                        foreach (var item in target)
                        {
                            //if (i == 1)
                            //{
                            //    PointOfEvaluation poe = await _context.PointOfEvaluations
                            //                            .Include(p => p.DataForEvaluations)
                            //                            .Include(p => p.Rounds)
                            //                            .SingleAsync(p => p.Point == 1 && p.OwnerOfficeId == item.Id);

                            //    foreach (var data in poe.DataForEvaluations)
                            //    {
                            //        _context.DataForEvaluations.Remove(data);
                            //    }

                            //    foreach (var round in poe.Rounds)
                            //    {
                            //        _context.Rounds.Remove(round);
                            //    }

                            //    _context.PointOfEvaluations.Remove(poe);
                            //    await _context.SaveChangesAsync();
                            //}
                            var pointToCopy = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Where(p => p.OwnerOffice.Code == item.Code && p.Name == dataPoint.Name && p.Plan == dataPoint.Plan).FirstOrDefaultAsync();
                            if (pointToCopy == null)
                            {
                                pointToCopy = new PointOfEvaluation();
                                pointToCopy.AuditOfficeId = dataPoint.AuditOfficeId;
                                pointToCopy.AutoApp = dataPoint.AutoApp;
                                pointToCopy.Ddrive = dataPoint.Ddrive;
                                pointToCopy.DetailPlan = dataPoint.DetailPlan;
                                pointToCopy.ExpectPlan = dataPoint.ExpectPlan;
                                pointToCopy.HasSub = dataPoint.HasSub;
                                pointToCopy.Name = dataPoint.Name;
                                pointToCopy.OwnerOfficeId = item.Id;
                                pointToCopy.Plan = dataPoint.Plan;
                                pointToCopy.Point = dataPoint.Point;
                                pointToCopy.SubPoint = dataPoint.SubPoint;
                                pointToCopy.Unit = dataPoint.Unit;
                                pointToCopy.UpdateUserId = user.Id;
                                pointToCopy.Weight = dataPoint.Weight;
                                pointToCopy.Year = dataPoint.Year;

                                _context.PointOfEvaluations.Add(pointToCopy);
                                await _context.SaveChangesAsync();

                                foreach (var round in dataRounds)
                                {
                                    var roundToCopy = new Round();
                                    roundToCopy.PointOfEvaluationId = pointToCopy.Id;
                                    roundToCopy.DetailRate1 = round.DetailRate1;
                                    roundToCopy.DetailRate2 = round.DetailRate2;
                                    roundToCopy.DetailRate3 = round.DetailRate3;
                                    roundToCopy.DetailRate4 = round.DetailRate4;
                                    roundToCopy.DetailRate5 = round.DetailRate5;
                                    roundToCopy.LevelNumber = round.LevelNumber;
                                    roundToCopy.Rate1MonthStart = round.Rate1MonthStart;
                                    roundToCopy.Rate1MonthStop = round.Rate1MonthStop;
                                    roundToCopy.Rate2MonthStart = round.Rate2MonthStart;
                                    roundToCopy.Rate2MonthStop = round.Rate2MonthStop;
                                    roundToCopy.Rate3MonthStart = round.Rate3MonthStart;
                                    roundToCopy.Rate3MonthStop = round.Rate3MonthStop;
                                    roundToCopy.Rate4MonthStart = round.Rate4MonthStart;
                                    roundToCopy.Rate4MonthStop = round.Rate4MonthStop;
                                    roundToCopy.Rate5MonthStart = round.Rate5MonthStart;
                                    roundToCopy.Rate5MonthStop = round.Rate5MonthStop;
                                    roundToCopy.Rate1 = round.Rate1;
                                    roundToCopy.Rate2 = round.Rate2;
                                    roundToCopy.Rate3 = round.Rate3;
                                    roundToCopy.Rate4 = round.Rate4;
                                    roundToCopy.Rate5 = round.Rate5;
                                    roundToCopy.RoundNumber = round.RoundNumber;

                                    _context.Rounds.Add(roundToCopy);
                                    await _context.SaveChangesAsync();
                                }

                                foreach (var dataforE in dataForEPES)
                                {
                                    var dataForEvaluation = new DataForEvaluation();
                                    dataForEvaluation.UpdateUserId = user.Id;
                                    dataForEvaluation.PointOfEvaluationId = pointToCopy.Id;
                                    dataForEvaluation.OfficeId = item.Id;
                                    dataForEvaluation.Month = dataforE.Month;
                                    dataForEvaluation.Expect = dataforE.Expect;

                                    _context.DataForEvaluations.Add(dataForEvaluation);
                                    await _context.SaveChangesAsync();
                                }
                            }
                        }
                    }
                    else
                    {
                        var dataPointHasSub = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Where(p => p.Point == i && p.OwnerOffice.Code == "01000000").ToListAsync();
                        foreach (var dataPointS in dataPointHasSub)
                        {
                            var dataRounds = await _context.Rounds.Where(r => r.PointOfEvaluationId == dataPointS.Id).ToListAsync();

                            var dataForEPES = await _context.DataForEvaluations.Where(r => r.PointOfEvaluationId == dataPointS.Id).ToListAsync();

                            foreach (var item in target)
                            {
                                var pointToCopy = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Where(p => p.OwnerOffice.Code == item.Code && p.Name == dataPointS.Name && p.Plan == dataPointS.Plan).FirstOrDefaultAsync();
                                if (pointToCopy == null)
                                {
                                    pointToCopy = new PointOfEvaluation();
                                    pointToCopy.AuditOfficeId = dataPointS.AuditOfficeId;
                                    pointToCopy.AutoApp = dataPointS.AutoApp;
                                    pointToCopy.Ddrive = dataPointS.Ddrive;
                                    pointToCopy.DetailPlan = dataPointS.DetailPlan;
                                    pointToCopy.ExpectPlan = dataPointS.ExpectPlan;
                                    pointToCopy.HasSub = dataPointS.HasSub;
                                    pointToCopy.Name = dataPointS.Name;
                                    pointToCopy.OwnerOfficeId = item.Id;
                                    pointToCopy.Plan = dataPointS.Plan;
                                    pointToCopy.Point = dataPointS.Point;
                                    pointToCopy.SubPoint = dataPointS.SubPoint;
                                    pointToCopy.Unit = dataPointS.Unit;
                                    pointToCopy.UpdateUserId = user.Id;
                                    pointToCopy.Weight = dataPointS.Weight;
                                    pointToCopy.Year = dataPointS.Year;

                                    _context.PointOfEvaluations.Add(pointToCopy);
                                    await _context.SaveChangesAsync();

                                    foreach (var round in dataRounds)
                                    {
                                        var roundToCopy = new Round();
                                        roundToCopy.PointOfEvaluationId = pointToCopy.Id;
                                        roundToCopy.DetailRate1 = round.DetailRate1;
                                        roundToCopy.DetailRate2 = round.DetailRate2;
                                        roundToCopy.DetailRate3 = round.DetailRate3;
                                        roundToCopy.DetailRate4 = round.DetailRate4;
                                        roundToCopy.DetailRate5 = round.DetailRate5;
                                        roundToCopy.LevelNumber = round.LevelNumber;
                                        roundToCopy.Rate1MonthStart = round.Rate1MonthStart;
                                        roundToCopy.Rate1MonthStop = round.Rate1MonthStop;
                                        roundToCopy.Rate2MonthStart = round.Rate2MonthStart;
                                        roundToCopy.Rate2MonthStop = round.Rate2MonthStop;
                                        roundToCopy.Rate3MonthStart = round.Rate3MonthStart;
                                        roundToCopy.Rate3MonthStop = round.Rate3MonthStop;
                                        roundToCopy.Rate4MonthStart = round.Rate4MonthStart;
                                        roundToCopy.Rate4MonthStop = round.Rate4MonthStop;
                                        roundToCopy.Rate5MonthStart = round.Rate5MonthStart;
                                        roundToCopy.Rate5MonthStop = round.Rate5MonthStop;
                                        roundToCopy.Rate1 = round.Rate1;
                                        roundToCopy.Rate2 = round.Rate2;
                                        roundToCopy.Rate3 = round.Rate3;
                                        roundToCopy.Rate4 = round.Rate4;
                                        roundToCopy.Rate5 = round.Rate5;
                                        roundToCopy.RoundNumber = round.RoundNumber;

                                        _context.Rounds.Add(roundToCopy);
                                        await _context.SaveChangesAsync();
                                    }

                                    foreach (var dataforE in dataForEPES)
                                    {
                                        var dataForEvaluation = new DataForEvaluation();
                                        dataForEvaluation.UpdateUserId = user.Id;
                                        dataForEvaluation.PointOfEvaluationId = pointToCopy.Id;
                                        dataForEvaluation.OfficeId = item.Id;
                                        dataForEvaluation.Month = dataforE.Month;
                                        dataForEvaluation.Expect = dataforE.Expect;

                                        _context.DataForEvaluations.Add(dataForEvaluation);
                                        await _context.SaveChangesAsync();
                                    }
                                }
                            }
                        }
                    }
                }
                return Ok();
            }
            catch (DbUpdateException)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "ไม่สามารถบันทึกข้อมูล. " +
                    "ลองพยายามบันทึกอีกครั้ง " +
                    "โปรดแจ้งผู้ดูแลระบบ");
                return NotFound();
            }
        }

        [HttpGet, ActionName("CopyST1ToSTAll")]
        public async Task<IActionResult> CopyST1ToSTAll()
        {
            var target = await _context.Offices.Where(d => d.Code != "00000000" && d.Code != "01001000" && d.Code.Substring(5, 3) == "000" && d.Code.Substring(0, 3) != "000" && d.Code.Substring(2, 6) != "000000").ToListAsync();

            var user = await _userManager.GetUserAsync(User);
            try
            {
                for (int i = 20; i <= 22; i++)
                {
                    var dataPoint = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).FirstOrDefaultAsync(p => p.Point == i && p.OwnerOffice.Code == "01001000");
                    if (dataPoint != null)
                    {
                        if (!dataPoint.HasSub && dataPoint.SubPoint == 0)
                        {
                            var dataRounds = await _context.Rounds.Where(r => r.PointOfEvaluationId == dataPoint.Id).ToListAsync();

                            var dataForEPES = await _context.DataForEvaluations.Where(r => r.PointOfEvaluationId == dataPoint.Id).ToListAsync();

                            foreach (var item in target)
                            {
                                //if (i == 1)
                                //{
                                //    PointOfEvaluation poe = await _context.PointOfEvaluations
                                //                            .Include(p => p.DataForEvaluations)
                                //                            .Include(p => p.Rounds)
                                //                            .SingleAsync(p => p.Point == 1 && p.OwnerOfficeId == item.Id);

                                //    foreach (var data in poe.DataForEvaluations)
                                //    {
                                //        _context.DataForEvaluations.Remove(data);
                                //    }

                                //    foreach (var round in poe.Rounds)
                                //    {
                                //        _context.Rounds.Remove(round);
                                //    }

                                //    _context.PointOfEvaluations.Remove(poe);
                                //    await _context.SaveChangesAsync();
                                //}
                                var pointToCopy = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Where(p => p.OwnerOffice.Code == item.Code && p.Name == dataPoint.Name && p.Plan == dataPoint.Plan).FirstOrDefaultAsync();
                                if (pointToCopy == null)
                                {
                                    pointToCopy = new PointOfEvaluation();
                                    pointToCopy.AuditOfficeId = dataPoint.AuditOfficeId;
                                    pointToCopy.AutoApp = dataPoint.AutoApp;
                                    pointToCopy.Ddrive = dataPoint.Ddrive;
                                    pointToCopy.DetailPlan = dataPoint.DetailPlan;
                                    pointToCopy.ExpectPlan = dataPoint.ExpectPlan;
                                    pointToCopy.HasSub = dataPoint.HasSub;
                                    pointToCopy.Name = dataPoint.Name;
                                    pointToCopy.OwnerOfficeId = item.Id;
                                    pointToCopy.Plan = dataPoint.Plan;
                                    pointToCopy.Point = dataPoint.Point;
                                    pointToCopy.SubPoint = dataPoint.SubPoint;
                                    pointToCopy.Unit = dataPoint.Unit;
                                    pointToCopy.UpdateUserId = user.Id;
                                    pointToCopy.Weight = dataPoint.Weight;
                                    pointToCopy.Year = dataPoint.Year;

                                    _context.PointOfEvaluations.Add(pointToCopy);
                                    await _context.SaveChangesAsync();

                                    foreach (var round in dataRounds)
                                    {
                                        var roundToCopy = new Round();
                                        roundToCopy.PointOfEvaluationId = pointToCopy.Id;
                                        roundToCopy.DetailRate1 = round.DetailRate1;
                                        roundToCopy.DetailRate2 = round.DetailRate2;
                                        roundToCopy.DetailRate3 = round.DetailRate3;
                                        roundToCopy.DetailRate4 = round.DetailRate4;
                                        roundToCopy.DetailRate5 = round.DetailRate5;
                                        roundToCopy.LevelNumber = round.LevelNumber;
                                        roundToCopy.Rate1MonthStart = round.Rate1MonthStart;
                                        roundToCopy.Rate1MonthStop = round.Rate1MonthStop;
                                        roundToCopy.Rate2MonthStart = round.Rate2MonthStart;
                                        roundToCopy.Rate2MonthStop = round.Rate2MonthStop;
                                        roundToCopy.Rate3MonthStart = round.Rate3MonthStart;
                                        roundToCopy.Rate3MonthStop = round.Rate3MonthStop;
                                        roundToCopy.Rate4MonthStart = round.Rate4MonthStart;
                                        roundToCopy.Rate4MonthStop = round.Rate4MonthStop;
                                        roundToCopy.Rate5MonthStart = round.Rate5MonthStart;
                                        roundToCopy.Rate5MonthStop = round.Rate5MonthStop;
                                        roundToCopy.Rate1 = round.Rate1;
                                        roundToCopy.Rate2 = round.Rate2;
                                        roundToCopy.Rate3 = round.Rate3;
                                        roundToCopy.Rate4 = round.Rate4;
                                        roundToCopy.Rate5 = round.Rate5;
                                        roundToCopy.RoundNumber = round.RoundNumber;

                                        _context.Rounds.Add(roundToCopy);
                                        //await _context.SaveChangesAsync();

                                    }

                                    foreach (var dataforE in dataForEPES)
                                    {
                                        var dataForEvaluation = new DataForEvaluation();
                                        dataForEvaluation.UpdateUserId = user.Id;
                                        dataForEvaluation.PointOfEvaluationId = pointToCopy.Id;
                                        dataForEvaluation.OfficeId = item.Id;
                                        dataForEvaluation.Month = dataforE.Month;
                                        dataForEvaluation.Expect = dataforE.Expect;

                                        _context.DataForEvaluations.Add(dataForEvaluation);
                                        //await _context.SaveChangesAsync();
                                    }
                                    await _context.SaveChangesAsync();
                                }
                            }
                        }
                        else
                        {
                            var dataPointHasSub = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Where(p => p.Point == i && p.OwnerOffice.Code == "01001000").ToListAsync();
                            foreach (var dataPointS in dataPointHasSub)
                            {
                                var dataRounds = await _context.Rounds.Where(r => r.PointOfEvaluationId == dataPointS.Id).ToListAsync();

                                var dataForEPES = await _context.DataForEvaluations.Where(r => r.PointOfEvaluationId == dataPointS.Id).ToListAsync();

                                foreach (var item in target)
                                {
                                    var pointToCopy = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Where(p => p.OwnerOffice.Code == item.Code && p.Name == dataPointS.Name && p.Plan == dataPointS.Plan).FirstOrDefaultAsync();
                                    if (pointToCopy == null)
                                    {
                                        pointToCopy = new PointOfEvaluation();
                                        pointToCopy.AuditOfficeId = dataPointS.AuditOfficeId;
                                        pointToCopy.AutoApp = dataPointS.AutoApp;
                                        pointToCopy.Ddrive = dataPointS.Ddrive;
                                        pointToCopy.DetailPlan = dataPointS.DetailPlan;
                                        pointToCopy.ExpectPlan = dataPointS.ExpectPlan;
                                        pointToCopy.HasSub = dataPointS.HasSub;
                                        pointToCopy.Name = dataPointS.Name;
                                        pointToCopy.OwnerOfficeId = item.Id;
                                        pointToCopy.Plan = dataPointS.Plan;
                                        pointToCopy.Point = dataPointS.Point;
                                        pointToCopy.SubPoint = dataPointS.SubPoint;
                                        pointToCopy.Unit = dataPointS.Unit;
                                        pointToCopy.UpdateUserId = user.Id;
                                        pointToCopy.Weight = dataPointS.Weight;
                                        pointToCopy.Year = dataPointS.Year;

                                        _context.PointOfEvaluations.Add(pointToCopy);
                                        await _context.SaveChangesAsync();

                                        foreach (var round in dataRounds)
                                        {
                                            var roundToCopy = new Round();
                                            roundToCopy.PointOfEvaluationId = pointToCopy.Id;
                                            roundToCopy.DetailRate1 = round.DetailRate1;
                                            roundToCopy.DetailRate2 = round.DetailRate2;
                                            roundToCopy.DetailRate3 = round.DetailRate3;
                                            roundToCopy.DetailRate4 = round.DetailRate4;
                                            roundToCopy.DetailRate5 = round.DetailRate5;
                                            roundToCopy.LevelNumber = round.LevelNumber;
                                            roundToCopy.Rate1MonthStart = round.Rate1MonthStart;
                                            roundToCopy.Rate1MonthStop = round.Rate1MonthStop;
                                            roundToCopy.Rate2MonthStart = round.Rate2MonthStart;
                                            roundToCopy.Rate2MonthStop = round.Rate2MonthStop;
                                            roundToCopy.Rate3MonthStart = round.Rate3MonthStart;
                                            roundToCopy.Rate3MonthStop = round.Rate3MonthStop;
                                            roundToCopy.Rate4MonthStart = round.Rate4MonthStart;
                                            roundToCopy.Rate4MonthStop = round.Rate4MonthStop;
                                            roundToCopy.Rate5MonthStart = round.Rate5MonthStart;
                                            roundToCopy.Rate5MonthStop = round.Rate5MonthStop;
                                            roundToCopy.Rate1 = round.Rate1;
                                            roundToCopy.Rate2 = round.Rate2;
                                            roundToCopy.Rate3 = round.Rate3;
                                            roundToCopy.Rate4 = round.Rate4;
                                            roundToCopy.Rate5 = round.Rate5;
                                            roundToCopy.RoundNumber = round.RoundNumber;

                                            _context.Rounds.Add(roundToCopy);
                                            //await _context.SaveChangesAsync();
                                        }

                                        foreach (var dataforE in dataForEPES)
                                        {
                                            var dataForEvaluation = new DataForEvaluation();
                                            dataForEvaluation.UpdateUserId = user.Id;
                                            dataForEvaluation.PointOfEvaluationId = pointToCopy.Id;
                                            dataForEvaluation.OfficeId = item.Id;
                                            dataForEvaluation.Month = dataforE.Month;
                                            dataForEvaluation.Expect = dataforE.Expect;

                                            _context.DataForEvaluations.Add(dataForEvaluation);
                                            //await _context.SaveChangesAsync();
                                        }
                                        await _context.SaveChangesAsync();
                                    }
                                }
                            }
                        }
                    }
                }
                return Ok();
            }
            catch (DbUpdateException)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "ไม่สามารถบันทึกข้อมูล. " +
                    "ลองพยายามบันทึกอีกครั้ง " +
                    "โปรดแจ้งผู้ดูแลระบบ");
                return NotFound();
            }
        }

        [HttpGet, ActionName("P1ST1ToPak1")]
        public async Task<IActionResult> CopyPak1ToHQ()
        {
            var target = await _context.Offices.Where(d => d.Code == "01000000").ToListAsync();

            var user = await _userManager.GetUserAsync(User);
            try
            {
                for (int i = 1; i <= 1; i++)
                {
                    var dataPoint = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).FirstOrDefaultAsync(p => p.Point == i && p.OwnerOffice.Code == "01001000");

                    var dataRounds = await _context.Rounds.Where(r => r.PointOfEvaluationId == dataPoint.Id).ToListAsync();

                    var dataForEPES = await _context.DataForEvaluations.Where(r => r.PointOfEvaluationId == dataPoint.Id).ToListAsync();

                    foreach (var item in target)
                    {
                        var pointToCopy = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Include(p => p.Rounds).Include(p => p.DataForEvaluations).Where(p => p.OwnerOffice.Code == item.Code && p.Name == dataPoint.Name && p.Plan == dataPoint.Plan).FirstOrDefaultAsync();
                        if (pointToCopy == null)
                        {
                            pointToCopy = new PointOfEvaluation();
                            pointToCopy.AuditOfficeId = dataPoint.AuditOfficeId;
                            pointToCopy.AutoApp = dataPoint.AutoApp;
                            pointToCopy.Ddrive = dataPoint.Ddrive;
                            pointToCopy.DetailPlan = dataPoint.DetailPlan;
                            pointToCopy.ExpectPlan = dataPoint.ExpectPlan;
                            pointToCopy.HasSub = dataPoint.HasSub;
                            pointToCopy.Name = dataPoint.Name;
                            pointToCopy.OwnerOfficeId = item.Id;
                            pointToCopy.Plan = dataPoint.Plan;
                            pointToCopy.Point = dataPoint.Point;
                            pointToCopy.SubPoint = dataPoint.SubPoint;
                            pointToCopy.Unit = dataPoint.Unit;
                            pointToCopy.UpdateUserId = user.Id;
                            pointToCopy.Weight = dataPoint.Weight;
                            pointToCopy.Year = dataPoint.Year;

                            _context.PointOfEvaluations.Add(pointToCopy);
                            await _context.SaveChangesAsync();

                            foreach (var round in dataRounds)
                            {
                                var roundToCopy = new Round();
                                roundToCopy.PointOfEvaluationId = pointToCopy.Id;
                                roundToCopy.DetailRate1 = round.DetailRate1;
                                roundToCopy.DetailRate2 = round.DetailRate2;
                                roundToCopy.DetailRate3 = round.DetailRate3;
                                roundToCopy.DetailRate4 = round.DetailRate4;
                                roundToCopy.DetailRate5 = round.DetailRate5;
                                roundToCopy.LevelNumber = round.LevelNumber;
                                roundToCopy.Rate1MonthStart = round.Rate1MonthStart;
                                roundToCopy.Rate1MonthStop = round.Rate1MonthStop;
                                roundToCopy.Rate2MonthStart = round.Rate2MonthStart;
                                roundToCopy.Rate2MonthStop = round.Rate2MonthStop;
                                roundToCopy.Rate3MonthStart = round.Rate3MonthStart;
                                roundToCopy.Rate3MonthStop = round.Rate3MonthStop;
                                roundToCopy.Rate4MonthStart = round.Rate4MonthStart;
                                roundToCopy.Rate4MonthStop = round.Rate4MonthStop;
                                roundToCopy.Rate5MonthStart = round.Rate5MonthStart;
                                roundToCopy.Rate5MonthStop = round.Rate5MonthStop;
                                roundToCopy.Rate1 = round.Rate1;
                                roundToCopy.Rate2 = round.Rate2;
                                roundToCopy.Rate3 = round.Rate3;
                                roundToCopy.Rate4 = round.Rate4;
                                roundToCopy.Rate5 = round.Rate5;
                                roundToCopy.RoundNumber = round.RoundNumber;

                                _context.Rounds.Add(roundToCopy);
                                await _context.SaveChangesAsync();
                            }

                            foreach (var dataforE in dataForEPES)
                            {
                                var dataForEvaluation = new DataForEvaluation();
                                dataForEvaluation.UpdateUserId = user.Id;
                                dataForEvaluation.PointOfEvaluationId = pointToCopy.Id;
                                dataForEvaluation.OfficeId = item.Id;
                                dataForEvaluation.Month = dataforE.Month;
                                dataForEvaluation.Expect = dataforE.Expect;

                                _context.DataForEvaluations.Add(dataForEvaluation);
                                await _context.SaveChangesAsync();
                            }

                        }
                        else
                        {
                            if (pointToCopy.DataForEvaluations != null)
                            {
                                foreach (var dfe in pointToCopy.DataForEvaluations)
                                {
                                    _context.DataForEvaluations.Remove(dfe);
                                    //await _context.SaveChangesAsync();
                                }
                            }

                            if (pointToCopy.Rounds != null)
                            {
                                foreach (var r in pointToCopy.Rounds)
                                {
                                    _context.Rounds.Remove(r);
                                    //await _context.SaveChangesAsync();
                                }
                            }

                            _context.PointOfEvaluations.Remove(pointToCopy);
                            await _context.SaveChangesAsync();

                            pointToCopy = new PointOfEvaluation();
                            pointToCopy.AuditOfficeId = dataPoint.AuditOfficeId;
                            pointToCopy.AutoApp = dataPoint.AutoApp;
                            pointToCopy.Ddrive = dataPoint.Ddrive;
                            pointToCopy.DetailPlan = dataPoint.DetailPlan;
                            pointToCopy.ExpectPlan = dataPoint.ExpectPlan;
                            pointToCopy.HasSub = dataPoint.HasSub;
                            pointToCopy.Name = dataPoint.Name;
                            pointToCopy.OwnerOfficeId = item.Id;
                            pointToCopy.Plan = dataPoint.Plan;
                            pointToCopy.Point = dataPoint.Point;
                            pointToCopy.SubPoint = dataPoint.SubPoint;
                            pointToCopy.Unit = dataPoint.Unit;
                            pointToCopy.UpdateUserId = user.Id;
                            pointToCopy.Weight = dataPoint.Weight;
                            pointToCopy.Year = dataPoint.Year;

                            _context.PointOfEvaluations.Add(pointToCopy);
                            await _context.SaveChangesAsync();

                            foreach (var round in dataRounds)
                            {
                                var roundToCopy = new Round();
                                roundToCopy.PointOfEvaluationId = pointToCopy.Id;
                                roundToCopy.DetailRate1 = round.DetailRate1;
                                roundToCopy.DetailRate2 = round.DetailRate2;
                                roundToCopy.DetailRate3 = round.DetailRate3;
                                roundToCopy.DetailRate4 = round.DetailRate4;
                                roundToCopy.DetailRate5 = round.DetailRate5;
                                roundToCopy.LevelNumber = round.LevelNumber;
                                roundToCopy.Rate1MonthStart = round.Rate1MonthStart;
                                roundToCopy.Rate1MonthStop = round.Rate1MonthStop;
                                roundToCopy.Rate2MonthStart = round.Rate2MonthStart;
                                roundToCopy.Rate2MonthStop = round.Rate2MonthStop;
                                roundToCopy.Rate3MonthStart = round.Rate3MonthStart;
                                roundToCopy.Rate3MonthStop = round.Rate3MonthStop;
                                roundToCopy.Rate4MonthStart = round.Rate4MonthStart;
                                roundToCopy.Rate4MonthStop = round.Rate4MonthStop;
                                roundToCopy.Rate5MonthStart = round.Rate5MonthStart;
                                roundToCopy.Rate5MonthStop = round.Rate5MonthStop;
                                roundToCopy.Rate1 = round.Rate1;
                                roundToCopy.Rate2 = round.Rate2;
                                roundToCopy.Rate3 = round.Rate3;
                                roundToCopy.Rate4 = round.Rate4;
                                roundToCopy.Rate5 = round.Rate5;
                                roundToCopy.RoundNumber = round.RoundNumber;

                                _context.Rounds.Add(roundToCopy);
                                await _context.SaveChangesAsync();
                            }

                            foreach (var dataforE in dataForEPES)
                            {
                                var dataForEvaluation = new DataForEvaluation();
                                dataForEvaluation.UpdateUserId = user.Id;
                                dataForEvaluation.PointOfEvaluationId = pointToCopy.Id;
                                dataForEvaluation.OfficeId = item.Id;
                                dataForEvaluation.Month = dataforE.Month;
                                dataForEvaluation.Expect = dataforE.Expect;

                                _context.DataForEvaluations.Add(dataForEvaluation);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
                return Ok();
            }
            catch (DbUpdateException)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "ไม่สามารถบันทึกข้อมูล. " +
                    "ลองพยายามบันทึกอีกครั้ง " +
                    "โปรดแจ้งผู้ดูแลระบบ");
                return NotFound();
            }
        }

        [HttpGet, ActionName("CopyX")]
        public async Task<IActionResult> CopyX()
        {
            var target = await _context.Offices.Where(d => d.Code != "00000000" && d.Code != "01001000" && d.Code.Substring(2, 6) != "000000" && d.Code.Substring(5, 3) == "000" && d.Code.Substring(0, 3) != "000").ToListAsync();

            var user = await _userManager.GetUserAsync(User);
            try
            {
                for (int i = 20; i <= 22; i++)
                {
                    var dataPoint = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).FirstOrDefaultAsync(p => p.Point == i && p.OwnerOffice.Code == "01001000");

                    var dataRounds = await _context.Rounds.Where(r => r.PointOfEvaluationId == dataPoint.Id).ToListAsync();

                    var dataForEPES = await _context.DataForEvaluations.Where(r => r.PointOfEvaluationId == dataPoint.Id).ToListAsync();

                    foreach (var item in target)
                    {
                        var pointToCopy = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Include(p => p.Rounds).Include(p => p.DataForEvaluations).Where(p => p.OwnerOffice.Code == item.Code && p.Name == dataPoint.Name && p.Plan == dataPoint.Plan).FirstOrDefaultAsync();
                        if (pointToCopy == null)
                        {
                            pointToCopy = new PointOfEvaluation();
                            pointToCopy.AuditOfficeId = dataPoint.AuditOfficeId;
                            pointToCopy.AutoApp = dataPoint.AutoApp;
                            pointToCopy.Ddrive = dataPoint.Ddrive;
                            pointToCopy.DetailPlan = dataPoint.DetailPlan;
                            pointToCopy.ExpectPlan = dataPoint.ExpectPlan;
                            pointToCopy.HasSub = dataPoint.HasSub;
                            pointToCopy.Name = dataPoint.Name;
                            pointToCopy.OwnerOfficeId = item.Id;
                            pointToCopy.Plan = dataPoint.Plan;
                            pointToCopy.Point = dataPoint.Point;
                            pointToCopy.SubPoint = dataPoint.SubPoint;
                            pointToCopy.Unit = dataPoint.Unit;
                            pointToCopy.UpdateUserId = user.Id;
                            pointToCopy.Weight = dataPoint.Weight;
                            pointToCopy.Year = dataPoint.Year;

                            _context.PointOfEvaluations.Add(pointToCopy);
                            await _context.SaveChangesAsync();

                            foreach (var round in dataRounds)
                            {
                                var roundToCopy = new Round();
                                roundToCopy.PointOfEvaluationId = pointToCopy.Id;
                                roundToCopy.DetailRate1 = round.DetailRate1;
                                roundToCopy.DetailRate2 = round.DetailRate2;
                                roundToCopy.DetailRate3 = round.DetailRate3;
                                roundToCopy.DetailRate4 = round.DetailRate4;
                                roundToCopy.DetailRate5 = round.DetailRate5;
                                roundToCopy.LevelNumber = round.LevelNumber;
                                roundToCopy.Rate1MonthStart = round.Rate1MonthStart;
                                roundToCopy.Rate1MonthStop = round.Rate1MonthStop;
                                roundToCopy.Rate2MonthStart = round.Rate2MonthStart;
                                roundToCopy.Rate2MonthStop = round.Rate2MonthStop;
                                roundToCopy.Rate3MonthStart = round.Rate3MonthStart;
                                roundToCopy.Rate3MonthStop = round.Rate3MonthStop;
                                roundToCopy.Rate4MonthStart = round.Rate4MonthStart;
                                roundToCopy.Rate4MonthStop = round.Rate4MonthStop;
                                roundToCopy.Rate5MonthStart = round.Rate5MonthStart;
                                roundToCopy.Rate5MonthStop = round.Rate5MonthStop;
                                roundToCopy.Rate1 = round.Rate1;
                                roundToCopy.Rate2 = round.Rate2;
                                roundToCopy.Rate3 = round.Rate3;
                                roundToCopy.Rate4 = round.Rate4;
                                roundToCopy.Rate5 = round.Rate5;
                                roundToCopy.RoundNumber = round.RoundNumber;

                                _context.Rounds.Add(roundToCopy);
                                await _context.SaveChangesAsync();
                            }

                            foreach (var dataforE in dataForEPES)
                            {
                                var dataForEvaluation = new DataForEvaluation();
                                dataForEvaluation.UpdateUserId = user.Id;
                                dataForEvaluation.PointOfEvaluationId = pointToCopy.Id;
                                dataForEvaluation.OfficeId = item.Id;
                                dataForEvaluation.Month = dataforE.Month;
                                dataForEvaluation.Expect = dataforE.Expect;

                                _context.DataForEvaluations.Add(dataForEvaluation);
                                await _context.SaveChangesAsync();
                            }

                        }
                        else
                        {
                            if (pointToCopy.DataForEvaluations != null)
                            {
                                foreach (var dfe in pointToCopy.DataForEvaluations)
                                {
                                    _context.DataForEvaluations.Remove(dfe);
                                    //await _context.SaveChangesAsync();
                                }
                            }

                            if (pointToCopy.Rounds != null)
                            {
                                foreach (var r in pointToCopy.Rounds)
                                {
                                    _context.Rounds.Remove(r);
                                    //await _context.SaveChangesAsync();
                                }
                            }

                            _context.PointOfEvaluations.Remove(pointToCopy);
                            await _context.SaveChangesAsync();

                            pointToCopy = new PointOfEvaluation();
                            pointToCopy.AuditOfficeId = dataPoint.AuditOfficeId;
                            pointToCopy.AutoApp = dataPoint.AutoApp;
                            pointToCopy.Ddrive = dataPoint.Ddrive;
                            pointToCopy.DetailPlan = dataPoint.DetailPlan;
                            pointToCopy.ExpectPlan = dataPoint.ExpectPlan;
                            pointToCopy.HasSub = dataPoint.HasSub;
                            pointToCopy.Name = dataPoint.Name;
                            pointToCopy.OwnerOfficeId = item.Id;
                            pointToCopy.Plan = dataPoint.Plan;
                            pointToCopy.Point = dataPoint.Point;
                            pointToCopy.SubPoint = dataPoint.SubPoint;
                            pointToCopy.Unit = dataPoint.Unit;
                            pointToCopy.UpdateUserId = user.Id;
                            pointToCopy.Weight = dataPoint.Weight;
                            pointToCopy.Year = dataPoint.Year;

                            _context.PointOfEvaluations.Add(pointToCopy);
                            await _context.SaveChangesAsync();

                            foreach (var round in dataRounds)
                            {
                                var roundToCopy = new Round();
                                roundToCopy.PointOfEvaluationId = pointToCopy.Id;
                                roundToCopy.DetailRate1 = round.DetailRate1;
                                roundToCopy.DetailRate2 = round.DetailRate2;
                                roundToCopy.DetailRate3 = round.DetailRate3;
                                roundToCopy.DetailRate4 = round.DetailRate4;
                                roundToCopy.DetailRate5 = round.DetailRate5;
                                roundToCopy.LevelNumber = round.LevelNumber;
                                roundToCopy.Rate1MonthStart = round.Rate1MonthStart;
                                roundToCopy.Rate1MonthStop = round.Rate1MonthStop;
                                roundToCopy.Rate2MonthStart = round.Rate2MonthStart;
                                roundToCopy.Rate2MonthStop = round.Rate2MonthStop;
                                roundToCopy.Rate3MonthStart = round.Rate3MonthStart;
                                roundToCopy.Rate3MonthStop = round.Rate3MonthStop;
                                roundToCopy.Rate4MonthStart = round.Rate4MonthStart;
                                roundToCopy.Rate4MonthStop = round.Rate4MonthStop;
                                roundToCopy.Rate5MonthStart = round.Rate5MonthStart;
                                roundToCopy.Rate5MonthStop = round.Rate5MonthStop;
                                roundToCopy.Rate1 = round.Rate1;
                                roundToCopy.Rate2 = round.Rate2;
                                roundToCopy.Rate3 = round.Rate3;
                                roundToCopy.Rate4 = round.Rate4;
                                roundToCopy.Rate5 = round.Rate5;
                                roundToCopy.RoundNumber = round.RoundNumber;

                                _context.Rounds.Add(roundToCopy);
                                await _context.SaveChangesAsync();
                            }

                            foreach (var dataforE in dataForEPES)
                            {
                                var dataForEvaluation = new DataForEvaluation();
                                dataForEvaluation.UpdateUserId = user.Id;
                                dataForEvaluation.PointOfEvaluationId = pointToCopy.Id;
                                dataForEvaluation.OfficeId = item.Id;
                                dataForEvaluation.Month = dataforE.Month;
                                dataForEvaluation.Expect = dataforE.Expect;

                                _context.DataForEvaluations.Add(dataForEvaluation);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
                return Ok();
            }
            catch (DbUpdateException)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "ไม่สามารถบันทึกข้อมูล. " +
                    "ลองพยายามบันทึกอีกครั้ง " +
                    "โปรดแจ้งผู้ดูแลระบบ");
                return NotFound();
            }
        }

        [HttpGet, ActionName("CopySTALLX")]
        public IActionResult CopySTAllX()
        {
            return Ok();
        }
    }
}
