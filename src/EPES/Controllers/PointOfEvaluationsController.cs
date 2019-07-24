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
using System.Globalization;
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
                yearForQuery = new DateTime(DateTime.Now.Year, 1, 1);
            }
            else
            {
                yearForQuery = new DateTime(DateTime.Now.AddYears(yearPoint).Year, 1, 1);
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
                yearForQuery = new DateTime(DateTime.Now.Year, 1, 1);
            }
            else
            {
                yearForQuery = new DateTime(DateTime.Now.AddYears(yearPoint).Year, 1, 1);
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

        // POST: PointOfEvaluations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DetailPlan,Point,Plan,Name,Unit,Weight,Rate1,DetailRate1,Rate2,DetailRate2,Rate3,DetailRate3,Rate4,DetailRate4,Rate5,DetailRate5,Detail2Rate1,Detail2Rate2,Detail2Rate3,Detail2Rate4,Detail2Rate5,OwnerOfficeId,AuditOfficeId,Rate1MonthStart,Rate1MonthStop,Rate1MonthStart2,Rate1MonthStop2,Rate2MonthStart,Rate2MonthStop,Rate2MonthStart2,Rate2MonthStop2,Rate3MonthStart,Rate3MonthStop,Rate3MonthStart2,Rate3MonthStop2,Rate4MonthStart,Rate4MonthStop,Rate4MonthStart2,Rate4MonthStop2,Rate5MonthStart,Rate5MonthStop,Rate5MonthStart2,Rate5MonthStop2")] PointOfEvaluation dataView, string selectoffice, int yearPoint, decimal? expect1, decimal? expect2, decimal? expect3, decimal? expect4, decimal? expect5, decimal? expect6, decimal? expect7, decimal? expect8, decimal? expect9, decimal? expect10, decimal? expect11, decimal? expect12)
        {
            var user = await _userManager.GetUserAsync(User);

            dataView.Year = new DateTime(DateTime.Now.AddYears(yearPoint).Year, 1, 1);
            dataView.UpdateUserId = user.Id;
            dataView.SubPoint = 0;

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(dataView);
                    await _context.SaveChangesAsync();
                    if (dataView.OwnerOffice != null)  //บันทึกเฉพาะมอบให้หน่วยงานเดียวเท่านั้น
                    {
                        await SaveExpect(dataView.Id, dataView.OwnerOffice.Id, 10, expect10, user.Id);
                        await SaveExpect(dataView.Id, dataView.OwnerOffice.Id, 11, expect11, user.Id);
                        await SaveExpect(dataView.Id, dataView.OwnerOffice.Id, 12, expect12, user.Id);
                        await SaveExpect(dataView.Id, dataView.OwnerOffice.Id, 1, expect1, user.Id);
                        await SaveExpect(dataView.Id, dataView.OwnerOffice.Id, 2, expect2, user.Id);
                        await SaveExpect(dataView.Id, dataView.OwnerOffice.Id, 3, expect3, user.Id);
                        await SaveExpect(dataView.Id, dataView.OwnerOffice.Id, 4, expect4, user.Id);
                        await SaveExpect(dataView.Id, dataView.OwnerOffice.Id, 5, expect5, user.Id);
                        await SaveExpect(dataView.Id, dataView.OwnerOffice.Id, 6, expect6, user.Id);
                        await SaveExpect(dataView.Id, dataView.OwnerOffice.Id, 7, expect7, user.Id);
                        await SaveExpect(dataView.Id, dataView.OwnerOffice.Id, 8, expect8, user.Id);
                        await SaveExpect(dataView.Id, dataView.OwnerOffice.Id, 9, expect9, user.Id);
                    }
                    return RedirectToAction(nameof(Index), new { yearPoint = yearPoint, selectoffice = selectoffice });
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
            var officeselect = await _context.Offices.Where(o => o.Code == selectoffice).FirstOrDefaultAsync();

            switch (dataView.Plan)
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

        // GET: PointOfEvaluations/Edit/5
        public async Task<IActionResult> Edit(string selectoffice, int? id, int yearPoint = 0)
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

            var user = await _userManager.GetUserAsync(User);
            var office = await _context.Offices.Where(o => o.Code == user.OfficeId).FirstOrDefaultAsync();
            var officeselect = await _context.Offices.Where(o => o.Code == selectoffice).FirstOrDefaultAsync();

            switch (pointOfEvaluation.Plan)
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
            return View(pointOfEvaluation);
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
                p => p.Year, p => p.DetailPlan, p => p.Point, p => p.Plan, p => p.Name, p => p.Unit, p => p.Weight,
                p => p.Rate1, p => p.DetailRate1, p => p.Rate2, p => p.DetailRate2, p => p.Rate3, p => p.DetailRate3, p => p.Rate4, p => p.DetailRate4, p => p.Rate5, p => p.DetailRate5, p => p.OwnerOfficeId, p => p.AuditOfficeId, p => p.UpdateUserId, p => p.Detail2Rate1, p => p.Detail2Rate2, p => p.Detail2Rate3, p => p.Detail2Rate4, p => p.Detail2Rate5,p => p.R1MStart,p => p.R1MStop,p => p.Rate1MonthStart2,p => p.Rate1MonthStop2, p => p.Rate2MonthStart, p => p.Rate2MonthStop, p => p.Rate2MonthStart2, p => p.Rate2MonthStop2, p => p.Rate3MonthStart, p => p.Rate3MonthStop, p => p.Rate3MonthStart2, p => p.Rate3MonthStop2, p => p.Rate4MonthStart, p => p.Rate4MonthStop, p => p.Rate4MonthStart2, p => p.Rate4MonthStop2, p => p.Rate5MonthStart, p => p.Rate5MonthStop, p => p.Rate5MonthStart2, p => p.Rate5MonthStop2))
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

        //private void PopulateOfficesDropDownList(object selectedOffice = null)
        //{
        //    var officesQuery = from d in _context.Offices
        //                       where (d.Code.Substring(5, 3) == "000")
        //                       orderby d.Id
        //                       select d;
        //    ViewBag.OfficeId = new SelectList(officesQuery.AsNoTracking(), "Id", "Name", selectedOffice);
        //}

        //private void PopulateAuditOfficesDropDownList(object selectedOffice = null)
        //{
        //    var officesQuery = from d in _context.Offices
        //                       where (d.Code != "00000000" && d.Code.Substring(5, 3) == "000")
        //                       orderby d.Id
        //                       select d;
        //    ViewBag.AuditOfficeId = new SelectList(officesQuery.AsNoTracking(), "Id", "Name", selectedOffice);
        //}

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

                            dataToCopy.Rate1 = data.Rate1;
                            dataToCopy.Rate2 = data.Rate2;
                            dataToCopy.Rate3 = data.Rate3;
                            dataToCopy.Rate4 = data.Rate4;
                            dataToCopy.Rate5 = data.Rate5;
                            dataToCopy.DetailRate1 = data.DetailRate1;
                            dataToCopy.DetailRate2 = data.DetailRate2;
                            dataToCopy.DetailRate3 = data.DetailRate3;
                            dataToCopy.DetailRate4 = data.DetailRate4;
                            dataToCopy.DetailRate5 = data.DetailRate5;
                            dataToCopy.Detail2Rate1 = data.Detail2Rate1;
                            dataToCopy.Detail2Rate2 = data.Detail2Rate2;
                            dataToCopy.Detail2Rate3 = data.Detail2Rate3;
                            dataToCopy.Detail2Rate4 = data.Detail2Rate4;
                            dataToCopy.Detail2Rate5 = data.Detail2Rate5;
                            dataToCopy.Rate1MonthStart = data.Rate1MonthStart;
                            dataToCopy.Rate1MonthStop = data.Rate1MonthStop;
                            dataToCopy.Rate1MonthStart2 = data.Rate1MonthStart2;
                            dataToCopy.Rate1MonthStop2 = data.Rate1MonthStop2;
                            dataToCopy.Rate2MonthStart = data.Rate2MonthStart;
                            dataToCopy.Rate2MonthStop = data.Rate2MonthStop;
                            dataToCopy.Rate2MonthStart2 = data.Rate2MonthStart2;
                            dataToCopy.Rate2MonthStop2 = data.Rate2MonthStop2;
                            dataToCopy.Rate3MonthStart = data.Rate3MonthStart;
                            dataToCopy.Rate3MonthStop = data.Rate3MonthStop;
                            dataToCopy.Rate3MonthStart2 = data.Rate3MonthStart2;
                            dataToCopy.Rate3MonthStop2 = data.Rate3MonthStop2;
                            dataToCopy.Rate4MonthStart = data.Rate4MonthStart;
                            dataToCopy.Rate4MonthStop = data.Rate4MonthStop;
                            dataToCopy.Rate4MonthStart2 = data.Rate4MonthStart2;
                            dataToCopy.Rate4MonthStop2 = data.Rate4MonthStop2;
                            dataToCopy.Rate5MonthStart = data.Rate5MonthStart;
                            dataToCopy.Rate5MonthStop = data.Rate5MonthStop;
                            dataToCopy.Rate5MonthStart2 = data.Rate5MonthStart2;
                            dataToCopy.Rate5MonthStop2 = data.Rate5MonthStop2;
                            dataToCopy.UpdateUserId = user.Id;

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
