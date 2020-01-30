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

            viewModel.selectoffice = selectoffice;
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

        // GET: PointOfEvaluations/Details/5
        public async Task<IActionResult> Details(string selectoffice, int? id, int yearPoint = 0)
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
            //dataView.SubPoint = 0;
            int roundid = 0;
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
                        roundid = dataView.Round.Id;
                        if (dataView.roundNumber == 2)
                        {
                            dataView.Round2.PointOfEvaluationId = dataView.point.Id;
                            dataView.Round2.RoundNumber = 2;
                            _context.Add(dataView.Round2);
                            roundid = dataView.Round2.Id;
                        }
                    }
                    else if (dataView.point.Unit == UnitOfPoint.ระดับ)
                    {
                        dataView.LRound.PointOfEvaluationId = dataView.point.Id;
                        dataView.LRound.RoundNumber = 1;
                        _context.Add(dataView.LRound);
                        roundid = dataView.LRound.Id;
                        if (dataView.roundNumber == 2)
                        {
                            dataView.LRound2.PointOfEvaluationId = dataView.point.Id;
                            dataView.LRound2.RoundNumber = 2;
                            _context.Add(dataView.LRound2);
                            roundid = dataView.LRound2.Id;
                        }
                    }
                    else if (dataView.point.Unit == UnitOfPoint.ระดับ_ร้อยละ)
                    {
                        dataView.LRRound.PointOfEvaluationId = dataView.point.Id;
                        dataView.LRRound.RoundNumber = 1;
                        _context.Add(dataView.LRRound);
                        roundid = dataView.LRRound.Id;
                        if (dataView.roundNumber == 2)
                        {
                            dataView.LRRound2.PointOfEvaluationId = dataView.point.Id;
                            dataView.LRRound2.RoundNumber = 2;
                            _context.Add(dataView.LRRound2);
                            roundid = dataView.LRRound2.Id;
                        }
                    }

                    await _context.SaveChangesAsync();
                    if (dataView.point.OwnerOffice != null)  //บันทึกเฉพาะมอบให้หน่วยงานเดียวเท่านั้น
                    {
                        await SaveExpect(roundid, dataView.point.Id, dataView.point.OwnerOffice.Id, 10, dataView.expect10, user.Id);
                        await SaveExpect(roundid, dataView.point.Id, dataView.point.OwnerOffice.Id, 11, dataView.expect11, user.Id);
                        await SaveExpect(roundid, dataView.point.Id, dataView.point.OwnerOffice.Id, 12, dataView.expect12, user.Id);
                        await SaveExpect(roundid, dataView.point.Id, dataView.point.OwnerOffice.Id, 1, dataView.expect1, user.Id);
                        await SaveExpect(roundid, dataView.point.Id, dataView.point.OwnerOffice.Id, 2, dataView.expect2, user.Id);
                        await SaveExpect(roundid, dataView.point.Id, dataView.point.OwnerOffice.Id, 3, dataView.expect3, user.Id);
                        await SaveExpect(roundid, dataView.point.Id, dataView.point.OwnerOffice.Id, 4, dataView.expect4, user.Id);
                        await SaveExpect(roundid, dataView.point.Id, dataView.point.OwnerOffice.Id, 5, dataView.expect5, user.Id);
                        await SaveExpect(roundid, dataView.point.Id, dataView.point.OwnerOffice.Id, 6, dataView.expect6, user.Id);
                        await SaveExpect(roundid, dataView.point.Id, dataView.point.OwnerOffice.Id, 7, dataView.expect7, user.Id);
                        await SaveExpect(roundid, dataView.point.Id, dataView.point.OwnerOffice.Id, 8, dataView.expect8, user.Id);
                        await SaveExpect(roundid, dataView.point.Id, dataView.point.OwnerOffice.Id, 9, dataView.expect9, user.Id);
                    }

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
            if (dataView.point == null)
            {
                return NotFound();
            }

            dataView.Round = await _context.Rounds.Where(r => r.PointOfEvaluationId == id).FirstOrDefaultAsync();
            dataView.Round2 = await _context.Rounds.Where(r => r.PointOfEvaluationId == id).FirstOrDefaultAsync();
            dataView.LRound = await _context.Rounds.Where(r => r.PointOfEvaluationId == id).FirstOrDefaultAsync();
            dataView.LRound2 = await _context.Rounds.Where(r => r.PointOfEvaluationId == id).FirstOrDefaultAsync();
            dataView.LRRound = await _context.Rounds.Where(r => r.PointOfEvaluationId == id).FirstOrDefaultAsync();
            dataView.LRRound2 = await _context.Rounds.Where(r => r.PointOfEvaluationId == id).FirstOrDefaultAsync();

            var user = await _userManager.GetUserAsync(User);
            var office = await _context.Offices.Where(o => o.Code == user.OfficeId).FirstOrDefaultAsync();
            var officeselect = await _context.Offices.Where(o => o.Code == selectoffice).FirstOrDefaultAsync();

            switch (dataView.point.Plan)
            {
                case TypeOfPlan.A:
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
                case TypeOfPlan.B:
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
                case TypeOfPlan.C:
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
                case TypeOfPlan.D:
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
            return View(dataView);
        }

        // POST: PointOfEvaluations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(string selectoffice, int? id, int yearpoint = 0)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);

            var pointOfEvaluationToUpdate = await _context.PointOfEvaluations.FirstOrDefaultAsync(p => p.Id == id);
            pointOfEvaluationToUpdate.UpdateUserId = user.Id;

            if (await TryUpdateModelAsync<PointOfEvaluation>(
                pointOfEvaluationToUpdate, "",
                p => p.Year, p => p.DetailPlan, p => p.Point, p => p.SubPoint, p => p.Plan, p => p.ExpectPlan, p => p.Ddrive, p => p.Name, p => p.Unit, p => p.Weight))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { yearPoint = yearpoint, selectoffice = selectoffice });
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "ไม่สามารถบันทึกข้อมูล. " +
                        "ลองพยายามบันทึกอีกครั้ง " +
                        "โปรดแจ้งผู้ดูแลระบบ");
                }
                //return RedirectToAction(nameof(Index), new { yearPoint = yearpoint, selectoffice = pointOfEvaluationToUpdate.AuditOfficeId });
            }
            
            var office = await _context.Offices.Where(o => o.Code == user.OfficeId).FirstOrDefaultAsync();
            var officeselect = await _context.Offices.Where(o => o.Code == selectoffice).FirstOrDefaultAsync();

            switch (pointOfEvaluationToUpdate.Plan)
            {
                case TypeOfPlan.A:
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
                case TypeOfPlan.B:
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
                case TypeOfPlan.C:
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
                case TypeOfPlan.D:
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
            ViewBag.yearPoint = yearpoint;
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
            var user = await _userManager.GetUserAsync(User);

            var pointOfEvaluation = await _context.PointOfEvaluations.FindAsync(Id);
            if (pointOfEvaluation == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.PointOfEvaluations.Remove(pointOfEvaluation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { yearPoint = yearP, selectoffice = selectoffice });
            }
            catch (DbUpdateException /*ex*/)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = Id, yearPoint = yearP, saveChangesError = true, selectoffice = selectoffice });
            }
        }

        private bool PointOfEvaluationExists(int id)
        {
            return _context.PointOfEvaluations.Any(e => e.Id == id);
        }


        public async Task SaveExpect(int roundid,int poeid, int ownerofficeid, int month, decimal expect, string userid)
        {
            DataForEvaluation dataForEvaluation;
            dataForEvaluation = await _context.DataForEvaluations.Where(d => d.RoundId == roundid && d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == month).FirstOrDefaultAsync();
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
                dataForEvaluation.RoundId = roundid;
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
        public async Task<IActionResult> Copy()
        {
            var target = await _context.Offices.Where(d => d.Code != "00000000" && d.Code != "01000000" && d.Code.Substring(0, 2) != "00" && d.Code.Substring(5, 3) == "000").ToListAsync();

            var user = await _userManager.GetUserAsync(User);
            try
            {
                PointOfEvaluation dataToCopy;
                for (int i = 1; i <= 22; i++)
                {
                    if (i == 19 || i == 20 || i == 21)
                    {
                        continue;
                    }
                    if (i == 22)
                    {
                        target = await _context.Offices.Where(d => d.Code != "00000000" && d.Code != "01000000" && d.Code.Substring(5, 3) == "000").ToListAsync();
                    }

                    var data = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Where(p => p.Point == i && p.OwnerOffice.Code == "01000000").FirstOrDefaultAsync();

                    foreach (var item in target)
                    {
                        dataToCopy = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Where(p => p.OwnerOffice.Code == item.Code && p.Name == data.Name && p.Plan == data.Plan).FirstOrDefaultAsync();
                        if (dataToCopy == null)
                        {
                            dataToCopy = new PointOfEvaluation();
                            dataToCopy.Year = data.Year;
                            dataToCopy.AuditOfficeId = data.AuditOfficeId;
                            dataToCopy.OwnerOfficeId = item.Id;
                            dataToCopy.Point = data.Point;
                            dataToCopy.SubPoint = data.SubPoint;
                            dataToCopy.DetailPlan = data.DetailPlan;
                            dataToCopy.Name = data.Name;
                            dataToCopy.Plan = data.Plan;
                            dataToCopy.Weight = data.Weight;
                            dataToCopy.Unit = data.Unit;

                            //dataToCopy.Rate1 = data.Rate1;
                            //dataToCopy.Rate2 = data.Rate2;
                            //dataToCopy.Rate3 = data.Rate3;
                            //dataToCopy.Rate4 = data.Rate4;
                            //dataToCopy.Rate5 = data.Rate5;
                            //dataToCopy.DetailRate1 = data.DetailRate1;
                            //dataToCopy.DetailRate2 = data.DetailRate2;
                            //dataToCopy.DetailRate3 = data.DetailRate3;
                            //dataToCopy.DetailRate4 = data.DetailRate4;
                            //dataToCopy.DetailRate5 = data.DetailRate5;
                            //dataToCopy.Detail2Rate1 = data.Detail2Rate1;
                            //dataToCopy.Detail2Rate2 = data.Detail2Rate2;
                            //dataToCopy.Detail2Rate3 = data.Detail2Rate3;
                            //dataToCopy.Detail2Rate4 = data.Detail2Rate4;
                            //dataToCopy.Detail2Rate5 = data.Detail2Rate5;
                            //dataToCopy.Rate1MonthStart = data.Rate1MonthStart;
                            //dataToCopy.Rate1MonthStop = data.Rate1MonthStop;
                            //dataToCopy.Rate1MonthStart2 = data.Rate1MonthStart2;
                            //dataToCopy.Rate1MonthStop2 = data.Rate1MonthStop2;
                            //dataToCopy.Rate2MonthStart = data.Rate2MonthStart;
                            //dataToCopy.Rate2MonthStop = data.Rate2MonthStop;
                            //dataToCopy.Rate2MonthStart2 = data.Rate2MonthStart2;
                            //dataToCopy.Rate2MonthStop2 = data.Rate2MonthStop2;
                            //dataToCopy.Rate3MonthStart = data.Rate3MonthStart;
                            //dataToCopy.Rate3MonthStop = data.Rate3MonthStop;
                            //dataToCopy.Rate3MonthStart2 = data.Rate3MonthStart2;
                            //dataToCopy.Rate3MonthStop2 = data.Rate3MonthStop2;
                            //dataToCopy.Rate4MonthStart = data.Rate4MonthStart;
                            //dataToCopy.Rate4MonthStop = data.Rate4MonthStop;
                            //dataToCopy.Rate4MonthStart2 = data.Rate4MonthStart2;
                            //dataToCopy.Rate4MonthStop2 = data.Rate4MonthStop2;
                            //dataToCopy.Rate5MonthStart = data.Rate5MonthStart;
                            //dataToCopy.Rate5MonthStop = data.Rate5MonthStop;
                            //dataToCopy.Rate5MonthStart2 = data.Rate5MonthStart2;
                            //dataToCopy.Rate5MonthStop2 = data.Rate5MonthStop2;
                            //dataToCopy.UpdateUserId = user.Id;

                            _context.Add(dataToCopy);
                            await _context.SaveChangesAsync();
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
    }
}
