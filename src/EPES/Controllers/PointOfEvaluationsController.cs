using DevExpress.Charts.Native;
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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace EPES.Controllers
{
    [Authorize]
    public class PointOfEvaluationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;

        public PointOfEvaluationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: PointOfEvaluations
        public async Task<IActionResult> Index(string selectoffice, string message, int yearPoint = 0)
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
                yearForQuery = new DateTime(yearPoint, 1, 1);
            }

            if(string.IsNullOrEmpty(selectoffice))
            {
                selectoffice = user.OfficeId;
            }

            DateTime year2023 = new DateTime(2023, 1, 1);

            var viewModel = new PointOfEvaluationViewModel();
            if (User.IsInRole("Admin") || User.IsInRole("Special"))
            {
                if(yearForQuery >= year2023)
                {
                    viewModel.pointFlagship = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.Flagship && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).OrderBy(ob => ob.Point).ThenBy(ob => ob.SubPoint).ToListAsync();
                    viewModel.pointCascade = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.Cascade && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).OrderBy(ob => ob.Point).ThenBy(ob => ob.SubPoint).ToListAsync();
                    viewModel.PointJointKPI = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.Joint_KPI && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).OrderBy(ob => ob.Point).ThenBy(ob => ob.SubPoint).ToListAsync();
                } else
                {
                    if (selectoffice.Substring(0, 3) == "000")
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    }
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                }

                ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);
            }
            else  // No Admin
            {
                List<Office> officeList;
                if(user.OfficeId.StartsWith("000"))
                {
                    officeList = await _context.PointOfEvaluations.Where(p => (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery).Select(b => b.OwnerOffice).Distinct().ToListAsync();
                    if (officeList.Count < 1)
                    {
                        officeList.AddRange(_context.Offices.Where(ofc => ofc.Code == user.OfficeId));
                    }
                }
                else if(user.OfficeId.EndsWith("000000"))
                {
                    officeList = await _context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000").ToListAsync();
                } else
                {
                    officeList = await _context.Offices.Where(ofc => ofc.Code == user.OfficeId).ToListAsync();
                }

                if(yearForQuery >= year2023)
                {
                    viewModel.pointFlagship = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.Flagship && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).OrderBy(ob => ob.Point).ThenBy(ob => ob.SubPoint).ToListAsync();
                    viewModel.pointCascade = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.Cascade && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).OrderBy(ob => ob.Point).ThenBy(ob => ob.SubPoint).ToListAsync();
                    viewModel.PointJointKPI = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.Joint_KPI && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).OrderBy(ob => ob.Point).ThenBy(ob => ob.SubPoint).ToListAsync();
                } else
                {
                    if (selectoffice.Substring(0, 3) == "000")
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    }
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                }

                var userOffices = await _context.UserOffices.Include(of => of.Office).Where(ou => ou.UserName == user.UserName).Select(slt => slt.Office).ToListAsync();
                foreach (var uo in userOffices)
                {
                    if (!officeList.Contains(uo))
                    {
                        officeList.Add(uo);
                        if (uo.Code.Substring(0, 2) != "00" && uo.Code.Substring(2, 6) == "000000")
                        {
                            var subOffices = await _context.Offices.Where(so => so.Code.Substring(0, 2) == uo.Code.Substring(0, 2) && so.Code.Substring(2, 6) != "000000" && so.Code.Substring(5, 3) == "000").ToListAsync();
                            foreach (var so in subOffices)
                            {
                                if (!officeList.Contains(so))
                                {
                                    officeList.Add(so);
                                }
                            }
                        }
                    }
                }
                ViewBag.OfficeCode = new SelectList(officeList, "Code", "Name", selectoffice);
            }

            viewModel.selectoffice = selectoffice;
            ViewBag.selectoffice = selectoffice;
            viewModel.yearPoint = yearForQuery.Year;
            ViewBag.UserOffices = await _context.UserOffices.Where(uo => uo.UserName == user.UserName).Select(slt => slt.Office.Code).ToListAsync();

            var years = await _context.PointOfEvaluations.Select(slt => new { value = slt.Year.Year, year = slt.Year.ToString("yyyy") }).Distinct().OrderBy(ob => ob.year).ToListAsync();
            if (!years.Any(yy => yy.value == DateTime.Now.AddYears(1).Year))
            {
                years.Add(new { value = DateTime.Now.AddYears(1).Year, year = DateTime.Now.AddYears(1).ToString("yyyy") });
            }
            ViewBag.selectyear = new SelectList(years, "value", "year", yearForQuery.Year);

            if(!string.IsNullOrEmpty(message))
            {
                ViewBag.message = message;
            }
            return View(viewModel);
        }

        

        
        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexPost(string selectoffice, string message, int yearPoint = 0)
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
                yearForQuery = new DateTime(yearPoint, 1, 1);
            }

            if (string.IsNullOrEmpty(selectoffice))
            {
                selectoffice = user.OfficeId;
            }

            DateTime year2023 = new DateTime(2023, 1, 1);

            var viewModel = new PointOfEvaluationViewModel();
            if (User.IsInRole("Admin") || User.IsInRole("Special"))
            {
                if (yearForQuery >= year2023)
                {
                    viewModel.pointFlagship = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.Flagship && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).OrderBy(ob => ob.Point).ThenBy(ob => ob.SubPoint).ToListAsync();
                    viewModel.pointCascade = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.Cascade && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).OrderBy(ob => ob.Point).ThenBy(ob => ob.SubPoint).ToListAsync();
                    viewModel.PointJointKPI = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.Joint_KPI && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).OrderBy(ob => ob.Point).ThenBy(ob => ob.SubPoint).ToListAsync();
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
                }

                ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);
            }
            else  // No Admin
            {
                List<Office> officeList;
                if (user.OfficeId.StartsWith("000"))
                {
                    officeList = await _context.PointOfEvaluations.Where(p => (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery).Select(b => b.OwnerOffice).Distinct().ToListAsync();
                    if (officeList.Count < 1)
                    {
                        officeList.AddRange(_context.Offices.Where(ofc => ofc.Code == user.OfficeId));
                    }
                }
                else if (user.OfficeId.EndsWith("000000"))
                {
                    officeList = await _context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000").ToListAsync();
                }
                else
                {
                    officeList = await _context.Offices.Where(ofc => ofc.Code == user.OfficeId).ToListAsync();
                }

                if (yearForQuery >= year2023)
                {
                    viewModel.pointFlagship = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.Flagship && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).OrderBy(ob => ob.Point).ThenBy(ob => ob.SubPoint).ToListAsync();
                    viewModel.pointCascade = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.Cascade && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).OrderBy(ob => ob.Point).ThenBy(ob => ob.SubPoint).ToListAsync();
                    viewModel.PointJointKPI = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.Joint_KPI && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).OrderBy(ob => ob.Point).ThenBy(ob => ob.SubPoint).ToListAsync();
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
                }

                var userOffices = await _context.UserOffices.Include(of => of.Office).Where(ou => ou.UserName == user.UserName).Select(slt => slt.Office).ToListAsync();
                foreach (var uo in userOffices)
                {
                    if (!officeList.Contains(uo))
                    {
                        officeList.Add(uo);
                        if (uo.Code.Substring(0, 2) != "00" && uo.Code.Substring(2, 6) == "000000")
                        {
                            var subOffices = await _context.Offices.Where(so => so.Code.Substring(0, 2) == uo.Code.Substring(0, 2) && so.Code.Substring(2, 6) != "000000" && so.Code.Substring(5, 3) == "000").ToListAsync();
                            foreach (var so in subOffices)
                            {
                                if (!officeList.Contains(so))
                                {
                                    officeList.Add(so);
                                }
                            }
                        }
                    }
                }
                ViewBag.OfficeCode = new SelectList(officeList, "Code", "Name", selectoffice);
            }

            viewModel.selectoffice = selectoffice;
            ViewBag.selectoffice = selectoffice;
            viewModel.yearPoint = yearForQuery.Year;
            ViewBag.UserOffices = await _context.UserOffices.Where(uo => uo.UserName == user.UserName).Select(slt => slt.Office.Code).ToListAsync();

            var years = await _context.PointOfEvaluations.Select(slt => new { value = slt.Year.Year, year = slt.Year.ToString("yyyy") }).Distinct().OrderBy(ob => ob.year).ToListAsync();
            if (!years.Any(yy => yy.value == DateTime.Now.AddYears(1).Year))
            {
                years.Add(new { value = DateTime.Now.AddYears(1).Year, year = DateTime.Now.AddYears(1).ToString("yyyy") });
            }
            ViewBag.selectyear = new SelectList(years, "value", "year", yearForQuery.Year);

            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.message = message;
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
                .Include(p => p.Theme)
                .Include(p => p.End)
                .Include(p => p.Way)
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

            var userOffices = await _context.UserOffices.Where(uo => uo.UserName == user.UserName).Select(slt => slt.Office).ToListAsync();

            List<Office> auditList = new List<Office>();
            int defaultOffice;


            if (String.IsNullOrEmpty(selectoffice))
            {
                defaultOffice = office.Id;
            }
            else
            {
                defaultOffice = officeselect.Id;
            }

            switch (plan)
            {
                case 0:
                    ViewBag.Plan = "A";
                    ViewBag.PlanValue = 0;

                    List<Object> list = new List<object>();
                    if (office.Code.Substring(0, 2) == "00")
                    {
                        list.Add(new { Id = office.Id, Name = office.Name });
                    }

                    foreach (var uo in userOffices)
                    {
                        if(uo.Code.Substring(0, 2) == "00")
                        {
                            list.Add(new { Id = uo.Id, Name = uo.Name });
                        }
                    }

                    ViewBag.AuditOfficeId = new SelectList(list, "Id", "Name", office.Id);
                    break;
                case 1:
                    ViewBag.Plan = "B";
                    ViewBag.PlanValue = 1;

                    ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", defaultOffice);
                    break;
                case 2:
                    ViewBag.Plan = "C";
                    ViewBag.PlanValue = 2;

                    if (User.IsInRole("Admin"))
                    {
                        ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", defaultOffice);
                    } else
                    {
                        var item = await _context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000").ToListAsync();

                        if (User.IsInRole("Manager") && (user.OfficeId.Substring(2, 6) == "000000"))
                        {
                            var selectitem = await _context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000").ToListAsync();
                            foreach (var itemAdd in selectitem)
                            {
                                item.Add(itemAdd);
                            }
                        }

                        if (User.IsInRole("Manager") && (userOffices.Any(uo => uo.Code.Substring(2, 6) == "000000")))
                        {
                            foreach (var uo in userOffices)
                            {
                                if (uo.Code.Substring(2, 6) == "000000")
                                {
                                    var selectitem = await _context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(uo.Code.Substring(0, 2)) && d.Code.Substring(5, 3) == "000").ToListAsync();
                                    foreach (var itemAdd in selectitem)
                                    {
                                        item.Add(itemAdd);
                                    }
                                }
                            }
                        }

                        if ((User.IsInRole("Manager") && (user.OfficeId.Substring(0, 2) == "00")) || User.IsInRole("User"))
                        {
                            item.Add(office);
                        }
                        ViewBag.AuditOfficeId = new SelectList(item, "Id", "Name", defaultOffice);
                    }

                    break;
                case 3:
                    ViewBag.Plan = "D";
                    ViewBag.PlanValue = 3;

                    if (User.IsInRole("Admin") || User.IsInRole("User") || (User.IsInRole("Manager") && (user.OfficeId.Substring(2, 6) == "000000" || userOffices.Any(uo => uo.Code.Substring(2, 6) == "000000"))))
                    {
                        auditList.AddRange(await _context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000").ToListAsync());
                    }
                    else
                    {
                        auditList.Add(office);
                    }
                    ViewBag.AuditOfficeId = new SelectList(auditList, "Id", "Name", defaultOffice);
                    break;
                case 4:
                    ViewBag.Plan = "Flagship";
                    ViewBag.PlanValue = 4;
                    if(User.IsInRole("Admin"))
                    {
                        ViewBag.AuditOfficeId = new SelectList(await _context.Offices.Where(d => (d.Code.Substring(0, 2) == "00" && d.Code != "00000000") || (d.Code.Substring(0, 2) != "00" && d.Code.Substring(5, 3) == "000")).ToListAsync(), "Id", "Name", defaultOffice);
                    } else
                    {
                        ViewBag.AuditOfficeId = new SelectList(await _context.Offices.Where(d => ((d.Code.Substring(0, 2) == "00" && d.Code != "00000000") || d.Code.Substring(0, 2) == office.Code.Substring(0, 2)) && d.Code.Substring(5, 3) == "000").ToListAsync(), "Id", "Name", defaultOffice);
                    }
                    break;
                case 5:
                    ViewBag.Plan = "Cascade";
                    ViewBag.PlanValue = 5;
                    ViewBag.AuditOfficeId = new SelectList(await _context.Offices.Where(d => d.Code.Substring(0, 2) == "00" && d.Code != "0000000").ToListAsync(), "Id", "Name", defaultOffice);
                    break;
                case 6:
                    ViewBag.Plan = "Joint KPI";
                    ViewBag.PlanValue = 6;
                    ViewBag.AuditOfficeId = new SelectList(await _context.Offices.Where(d => d.Code.Substring(0, 2) == "00" && d.Code != "00000000").ToListAsync(), "Id", "Name", defaultOffice);
                    break;
            }

            ViewBag.OfficeId = new SelectList(await _context.Offices.Where(d => d.Id == officeselect.Id).ToListAsync(), "Id", "Name", officeselect.Id);
            ViewBag.selectoffice = selectoffice;
            ViewBag.yearPoint = yearPoint;
            
            ViewBag.Theme = await _context.Theme.Where(whr => whr.IsActive).ToListAsync();

            var endDict = new Dictionary<int, List<List<string>>>();
            var end = await _context.End.Where(whr => whr.IsActive).ToListAsync();
            foreach(var ee in end)
            {
                if(!endDict.ContainsKey(ee.ThemeID))
                {
                    endDict.Add(ee.ThemeID, new List<List<string>>());
                }
                var endVal = new List<string>();
                endVal.Add(ee.Id.ToString());
                endVal.Add(ee.EndName);
                endDict[ee.ThemeID].Add(endVal);
            }
            ViewBag.End = endDict;

            var wayDict = new Dictionary<int, List<List<string>>>();
            var wayList = await _context.Way.Where(whr => whr.IsActive).ToListAsync();
            foreach(var ww in wayList)
            {
                if(!wayDict.ContainsKey(ww.EndID))
                {
                    wayDict.Add(ww.EndID, new List<List<string>>());
                }
                var wayVal = new List<string>();
                wayVal.Add(ww.Id.ToString());
                wayVal.Add(ww.WayName);
                wayDict[ww.EndID].Add(wayVal);
            }
            ViewBag.Way = wayDict;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PointOfEvaluationViewModel dataView)
        {
            var user = await _userManager.GetUserAsync(User);
            dataView.point.Year = new DateTime(dataView.yearPoint, 1, 1);
            /*
            if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
            {
                dataView.point.Year = new DateTime(DateTime.Now.AddYears(dataView.yearPoint + 1).Year, 1, 1);
            }
            else
            {
                dataView.point.Year = new DateTime(DateTime.Now.AddYears(dataView.yearPoint).Year, 1, 1);
            }
            */
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
                    else if(dataView.point.Unit == UnitOfPoint.ค่าคะแนน)
                    {
                        dataView.Score1Round1.PointOfEvaluationId = dataView.point.Id;
                        dataView.Score1Round1.RoundNumber = 1;
                        dataView.Score1Round1.LevelNumber = 1;
                        _context.Add(dataView.Score1Round1);
                        dataView.Score2Round1.PointOfEvaluationId = dataView.point.Id;
                        dataView.Score2Round1.RoundNumber = 1;
                        dataView.Score2Round1.LevelNumber = 2;
                        dataView.Score2Round1.Rate1MonthStart = dataView.Score1Round1.Rate1MonthStart;
                        dataView.Score2Round1.Rate5MonthStop = dataView.Score1Round1.Rate5MonthStop;
                        _context.Add(dataView.Score2Round1);
                        if(dataView.roundNumber == 2)
                        {
                            dataView.Score1Round2.PointOfEvaluationId = dataView.point.Id;
                            dataView.Score1Round2.RoundNumber = 2;
                            dataView.Score1Round2.LevelNumber = 1;
                            _context.Add(dataView.Score1Round2);
                            dataView.Score2Round2.PointOfEvaluationId = dataView.point.Id;
                            dataView.Score2Round2.RoundNumber = 2;
                            dataView.Score2Round2.LevelNumber = 2;
                            dataView.Score2Round2.Rate1MonthStart = dataView.Score1Round2.Rate1MonthStart;
                            dataView.Score2Round2.Rate5MonthStop = dataView.Score1Round2.Rate5MonthStop;
                            _context.Add(dataView.Score2Round2);
                        }
                    }

                    if (dataView.point.SubPoint >= 1)
                    {
                        var pointMain = await _context.PointOfEvaluations.Where(p => p.Point == dataView.point.Point && p.SubPoint == 0 && p.Year == dataView.point.Year && p.OwnerOfficeId == dataView.point.OwnerOfficeId).FirstOrDefaultAsync();
                        if (pointMain != null)
                        {
                            pointMain.HasSub = true;
                            _context.Update(pointMain);
                        }
                    }
                    var w = dataView.point.WeightAll;
                    await _context.SaveChangesAsync();

                    if (!w)
                    {
                        dataView.point.WeightAll = w;
                        _context.Update(dataView.point);
                    }

                    await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 10, dataView.expect10, user.Id, dataView.weight10);
                    await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 11, dataView.expect11, user.Id, dataView.weight11);
                    await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 12, dataView.expect12, user.Id, dataView.weight12);
                    await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 1, dataView.expect1, user.Id, dataView.weight1);
                    await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 2, dataView.expect2, user.Id, dataView.weight2);
                    await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 3, dataView.expect3, user.Id, dataView.weight3);
                    await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 4, dataView.expect4, user.Id, dataView.weight4);
                    await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 5, dataView.expect5, user.Id, dataView.weight5);
                    await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 6, dataView.expect6, user.Id, dataView.weight6);
                    await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 7, dataView.expect7, user.Id, dataView.weight7);
                    await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 8, dataView.expect8, user.Id, dataView.weight8);
                    await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 9, dataView.expect9, user.Id, dataView.weight9);

                    return RedirectToAction(nameof(Index), new { yearPoint = dataView.yearPoint, selectoffice = dataView.selectoffice });
                }
            }
            catch (DbUpdateException ex)
            {
                //Log the error (uncomment ex variable name and write a log.
                /*
                ModelState.AddModelError("", "ไม่สามารถบันทึกข้อมูล. " +
                    "ลองพยายามบันทึกอีกครั้ง " +
                    "โปรดแจ้งผู้ดูแลระบบ");
                */
                return RedirectToAction(nameof(Index), new { yearPoint = dataView.yearPoint, selectoffice = dataView.selectoffice, message = "Error\nPlease contact admin\n" + ex.InnerException });
            }

            return RedirectToAction(nameof(Index), new { yearPoint = dataView.yearPoint, selectoffice = dataView.selectoffice });
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
            dataView.Score1Round1 = await _context.Rounds.Where(r => r.PointOfEvaluationId == id && r.RoundNumber == 1 && r.LevelNumber == 1).FirstOrDefaultAsync();
            dataView.Score2Round1 = await _context.Rounds.Where(r => r.PointOfEvaluationId == id && r.RoundNumber == 1 && r.LevelNumber == 2).FirstOrDefaultAsync();
            dataView.Score1Round2 = await _context.Rounds.Where(r => r.PointOfEvaluationId == id && r.RoundNumber == 2 && r.LevelNumber == 1).FirstOrDefaultAsync();
            dataView.Score2Round2 = await _context.Rounds.Where(r => r.PointOfEvaluationId == id && r.RoundNumber == 2 && r.LevelNumber == 2).FirstOrDefaultAsync();

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

            dataView.weight10 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == id && d.Month == 10).Select(d => d.Weight).FirstOrDefaultAsync();
            dataView.weight11 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == id && d.Month == 11).Select(d => d.Weight).FirstOrDefaultAsync();
            dataView.weight12 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == id && d.Month == 12).Select(d => d.Weight).FirstOrDefaultAsync();
            dataView.weight1 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == id && d.Month == 1).Select(d => d.Weight).FirstOrDefaultAsync();
            dataView.weight2 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == id && d.Month == 2).Select(d => d.Weight).FirstOrDefaultAsync();
            dataView.weight3 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == id && d.Month == 3).Select(d => d.Weight).FirstOrDefaultAsync();
            dataView.weight4 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == id && d.Month == 4).Select(d => d.Weight).FirstOrDefaultAsync();
            dataView.weight5 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == id && d.Month == 5).Select(d => d.Weight).FirstOrDefaultAsync();
            dataView.weight6 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == id && d.Month == 6).Select(d => d.Weight).FirstOrDefaultAsync();
            dataView.weight7 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == id && d.Month == 7).Select(d => d.Weight).FirstOrDefaultAsync();
            dataView.weight8 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == id && d.Month == 8).Select(d => d.Weight).FirstOrDefaultAsync();
            dataView.weight9 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == id && d.Month == 9).Select(d => d.Weight).FirstOrDefaultAsync();

            var user = await _userManager.GetUserAsync(User);
            var office = await _context.Offices.Where(o => o.Code == user.OfficeId).FirstOrDefaultAsync();
            //var officeselect = await _context.Offices.Where(o => o.Code == selectoffice).FirstOrDefaultAsync();
            var userOffices = await _context.UserOffices.Where(uo => uo.UserName == user.UserName).Select(slt => slt.Office).ToListAsync();

            switch (dataView.point.Plan)
            {
                case TypeOfPlan.A:
                    ViewBag.Plan = "A";
                    ViewBag.PlanValue = 0;

                    List<Object> list = new List<object>();
                    if(office.Code.Substring(0, 2) == "00")
                    {
                        list.Add(new { Id = office.Id, Name = office.Name });
                    }

                    foreach (var uo in userOffices)
                    {
                        if (uo.Code.Substring(0, 2) == "00")
                        {
                            list.Add(new { Id = uo.Id, Name = uo.Name });
                        }
                    }

                    ViewBag.AuditOfficeId = new SelectList(list, "Id", "Name", office.Id);
                    break;

                case TypeOfPlan.B:
                    ViewBag.Plan = "B";
                    ViewBag.PlanValue = 1;

                    ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name");
                    break;

                case TypeOfPlan.C:
                    ViewBag.Plan = "C";
                    ViewBag.PlanValue = 2;

                    var item = await _context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000").Select(d => new { Id = d.Id, Name = d.Name }).ToListAsync();

                    if (User.IsInRole("Admin"))
                    {
                        ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name");
                    }
                    else if (User.IsInRole("Manager") && user.OfficeId.Substring(2, 6) == "000000")
                    {
                        var selectitem = await _context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000").Select(d => new { Id = d.Id, Name = d.Name }).ToListAsync();
                        foreach (var itemAdd in selectitem)
                        {
                            item.Add(itemAdd);
                        }

                        foreach (var uo in userOffices)
                        {
                            item.Add(new { uo.Id, uo.Name });
                        }
                        ViewBag.AuditOfficeId = new SelectList(item, "Id", "Name");
                    }
                    else
                    {
                        List<Object> listD = new List<object>();
                        listD.Add(new { Id = office.Id, Name = office.Name });

                        foreach (var uo in userOffices)
                        {
                            item.Add(new { uo.Id, uo.Name });
                        }
                        ViewBag.AuditOfficeId = new SelectList(item, "Id", "Name");
                    }
                    break;

                case TypeOfPlan.D:
                    ViewBag.Plan = "D";
                    ViewBag.PlanValue = 3;

                    ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name");

                    break;
                case TypeOfPlan.Flagship:
                    ViewBag.Plan = "Flagship";
                    ViewBag.PlanValue = 4;
                    if (User.IsInRole("Admin"))
                    {
                        ViewBag.AuditOfficeId = new SelectList(await _context.Offices.Where(d => (d.Code.Substring(0, 2) == "00" && d.Code != "00000000") || (d.Code.Substring(0, 2) != "00" && d.Code.Substring(5, 3) == "000")).ToListAsync(), "Id", "Name");
                    }
                    else
                    {
                        ViewBag.AuditOfficeId = new SelectList(await _context.Offices.Where(d => ((d.Code.Substring(0, 2) == "00" && d.Code != "00000000") || d.Code.Substring(0, 2) == office.Code.Substring(0, 2)) && d.Code.Substring(5, 3) == "000").ToListAsync(), "Id", "Name");
                    }
                    break;
                case TypeOfPlan.Cascade:
                    ViewBag.Plan = "Cascade";
                    ViewBag.PlanValue = 5;
                    ViewBag.AuditOfficeId = new SelectList(await _context.Offices.Where(d => d.Code.Substring(0, 2) == "00" && d.Code != "00000000").ToListAsync(), "Id", "Name");
                    break;
                case TypeOfPlan.Joint_KPI:
                    ViewBag.Plan = "Joint KPI";
                    ViewBag.PlanValue = 6;
                    ViewBag.AuditOfficeId = new SelectList(await _context.Offices.Where(d => d.Code.Substring(0, 2) == "00" && d.Code != "00000000").ToListAsync(), "Id", "Name");
                    break;
            }

            ViewBag.selectoffice = selectoffice;
            ViewBag.yearPoint = yearPoint;
            dataView.roundNumber = dataView.point.Rounds.Count;
            if(dataView.point.Unit == UnitOfPoint.ค่าคะแนน)
            {
                dataView.roundNumber = dataView.point.Rounds.Count / 2;
            }


            ViewBag.Theme = await _context.Theme.Where(whr => whr.IsActive).ToListAsync();
            var endDict = new Dictionary<int, List<List<string>>>();
            var end = await _context.End.Where(whr => whr.IsActive).ToListAsync();
            foreach (var ee in end)
            {
                if (!endDict.ContainsKey(ee.ThemeID))
                {
                    endDict.Add(ee.ThemeID, new List<List<string>>());
                }
                var endVal = new List<string>();
                endVal.Add(ee.Id.ToString());
                endVal.Add(ee.EndName);
                endDict[ee.ThemeID].Add(endVal);
            }
            ViewBag.End = endDict;

            var wayDict = new Dictionary<int, List<List<string>>>();
            var wayList = await _context.Way.Where(whr => whr.IsActive).ToListAsync();
            foreach (var ww in wayList)
            {
                if (!wayDict.ContainsKey(ww.EndID))
                {
                    wayDict.Add(ww.EndID, new List<List<string>>());
                }
                var wayVal = new List<string>();
                wayVal.Add(ww.Id.ToString());
                wayVal.Add(ww.WayName);
                wayDict[ww.EndID].Add(wayVal);
            }
            ViewBag.Way = wayDict;

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
            dataView.point.Year = new DateTime(dataView.yearPoint, 1, 1);
            /*
            if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
            {
                dataView.point.Year = new DateTime(DateTime.Now.AddYears(dataView.yearPoint + 1).Year, 1, 1);
            }
            else
            {
                dataView.point.Year = new DateTime(DateTime.Now.AddYears(dataView.yearPoint).Year, 1, 1);
            }
            */
            pointOfEvaluationToUpdate.Year = dataView.point.Year;
            pointOfEvaluationToUpdate.Point = dataView.point.Point;
            pointOfEvaluationToUpdate.SubPoint = dataView.point.SubPoint;
            pointOfEvaluationToUpdate.Plan = dataView.point.Plan;
            if(dataView.yearPoint >= 2023)
            {
                pointOfEvaluationToUpdate.ThemeId = dataView.point.ThemeId;
                pointOfEvaluationToUpdate.EndId = dataView.point.EndId;
                pointOfEvaluationToUpdate.WayId = dataView.point.WayId;
            } else
            {
                pointOfEvaluationToUpdate.ExpectPlan = dataView.point.ExpectPlan;
                pointOfEvaluationToUpdate.Ddrive = dataView.point.Ddrive;
            }
            pointOfEvaluationToUpdate.DetailPlan = dataView.point.DetailPlan;
            pointOfEvaluationToUpdate.Name = dataView.point.Name;
            pointOfEvaluationToUpdate.Unit = dataView.point.Unit;
            pointOfEvaluationToUpdate.Weight = dataView.point.Weight;
            pointOfEvaluationToUpdate.OwnerOfficeId = dataView.point.OwnerOfficeId;
            pointOfEvaluationToUpdate.AuditOfficeId = dataView.point.AuditOfficeId;
            pointOfEvaluationToUpdate.AutoApp = dataView.point.AutoApp;
            pointOfEvaluationToUpdate.FixExpect = dataView.point.FixExpect;
            pointOfEvaluationToUpdate.CalPerMonth = dataView.point.CalPerMonth;
            pointOfEvaluationToUpdate.WeightAll = dataView.point.WeightAll;
            pointOfEvaluationToUpdate.StartZero = dataView.point.StartZero;

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
                else if (dataView.point.Unit == UnitOfPoint.ค่าคะแนน)
                {
                    dataView.Score1Round1.PointOfEvaluationId = dataView.point.Id;
                    dataView.Score1Round1.RoundNumber = 1;
                    dataView.Score1Round1.LevelNumber = 1;
                    _context.Add(dataView.Score1Round1);
                    dataView.Score2Round1.PointOfEvaluationId = dataView.point.Id;
                    dataView.Score2Round1.RoundNumber = 1;
                    dataView.Score2Round1.LevelNumber = 2;
                    dataView.Score2Round1.Rate1MonthStart = dataView.Score1Round1.Rate1MonthStart;
                    dataView.Score2Round1.Rate5MonthStop = dataView.Score1Round1.Rate5MonthStop;
                    _context.Add(dataView.Score2Round1);
                    if (dataView.roundNumber == 2)
                    {
                        dataView.Score1Round2.PointOfEvaluationId = dataView.point.Id;
                        dataView.Score1Round2.RoundNumber = 2;
                        dataView.Score1Round2.LevelNumber = 1;
                        _context.Add(dataView.Score1Round2);
                        dataView.Score2Round2.PointOfEvaluationId = dataView.point.Id;
                        dataView.Score2Round2.RoundNumber = 2;
                        dataView.Score2Round2.LevelNumber = 2;
                        dataView.Score2Round2.Rate1MonthStart = dataView.Score1Round2.Rate1MonthStart;
                        dataView.Score2Round2.Rate5MonthStop = dataView.Score1Round2.Rate5MonthStop;
                        _context.Add(dataView.Score2Round2);
                    }
                }

                if (dataView.point.SubPoint >= 1)
                {
                    var pointMain = await _context.PointOfEvaluations.Where(p => p.Point == dataView.point.Point && p.SubPoint == 0 && p.Year == dataView.point.Year && p.OwnerOfficeId == dataView.point.OwnerOfficeId).FirstOrDefaultAsync();
                    if (pointMain != null)
                    {
                        pointMain.HasSub = true;
                        _context.Update(pointMain);
                    }
                }

                _context.Update(pointOfEvaluationToUpdate);
                await _context.SaveChangesAsync();

                await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 10, dataView.expect10, user.Id, dataView.weight10);
                await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 11, dataView.expect11, user.Id, dataView.weight11);
                await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 12, dataView.expect12, user.Id, dataView.weight12);
                await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 1, dataView.expect1, user.Id, dataView.weight1);
                await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 2, dataView.expect2, user.Id, dataView.weight2);
                await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 3, dataView.expect3, user.Id, dataView.weight3);
                await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 4, dataView.expect4, user.Id, dataView.weight4);
                await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 5, dataView.expect5, user.Id, dataView.weight5);
                await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 6, dataView.expect6, user.Id, dataView.weight6);
                await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 7, dataView.expect7, user.Id, dataView.weight7);
                await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 8, dataView.expect8, user.Id, dataView.weight8);
                await SaveExpect(dataView.point.Id, dataView.point.OwnerOfficeId, 9, dataView.expect9, user.Id, dataView.weight9);

                return RedirectToAction(nameof(Index), new { yearPoint = dataView.yearPoint, selectoffice = dataView.selectoffice });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                /*
                ModelState.AddModelError("", "ไม่สามารถบันทึกข้อมูล. " +
                    "ลองพยายามบันทึกอีกครั้ง " +
                    "โปรดแจ้งผู้ดูแลระบบ");
                */
                return RedirectToAction(nameof(Index), new { yearPoint = dataView.yearPoint, selectoffice = dataView.selectoffice, message = "Error\nPlease contact admin\n" + ex.InnerException });
            }
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
            catch (DbUpdateException)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = Id, yearPoint = yearP, saveChangesError = true, selectoffice = selectoffice });
            }
        }

        public async Task SaveExpect(int poeid, int ownerofficeid, int month, decimal expect, string userid,decimal weight)
        {
            DataForEvaluation dataForEvaluation;
            dataForEvaluation = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == month).FirstOrDefaultAsync();
            if (dataForEvaluation != null)
            {
                dataForEvaluation.UpdateUserId = userid;
                dataForEvaluation.Expect = expect;
                dataForEvaluation.Weight = weight;
                try
                {
                    //_context.Update(dataForEvaluation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    throw ex;
                    //Log the error (uncomment ex variable name and write a log.
                    /*
                    ModelState.AddModelError("", "ไม่สามารถบันทึกข้อมูล. " +
                        "ลองพยายามบันทึกอีกครั้ง " +
                        "โปรดแจ้งผู้ดูแลระบบ");
                    */
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
                dataForEvaluation.Weight = weight;
                //dataForEvaluation.Approve = Approve.รอพิจารณา;
                try
                {
                    _context.Add(dataForEvaluation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    throw ex;
                    //Log the error (uncomment ex variable name and write a log.
                    /*
                    ModelState.AddModelError("", "ไม่สามารถบันทึกข้อมูล. " +
                        "ลองพยายามบันทึกอีกครั้ง " +
                        "โปรดแจ้งผู้ดูแลระบบ");
                    */
                }
            }
        }

        [HttpGet, ActionName("CopyAllPak1toPak")]
        public async Task<IActionResult> CopyAllPak1toPak()
        {
            var target = await _context.Offices.Where(d => d.Code != "00000000" && d.Code != "01000000" && d.Code.Substring(2, 6) == "000000").ToListAsync();

            var user = await _userManager.GetUserAsync(User);

            var y = new DateTime(DateTime.Now.Year, 1, 1);


            if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
            {
                y = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);
            }

            var dataPoints = await _context.PointOfEvaluations.Where(p => p.OwnerOffice.Code == "01000000" && p.Year == y).ToListAsync();

            try
            {
                foreach (var dataPoint in dataPoints)
                {
                    var dataRounds = await _context.Rounds.Where(r => r.PointOfEvaluationId == dataPoint.Id).ToListAsync();

                    var dataForEPES = await _context.DataForEvaluations.Where(r => r.PointOfEvaluationId == dataPoint.Id).ToListAsync();

                    foreach (var item in target)
                    {
                        var pointToCopy = await _context.PointOfEvaluations.Where(p => p.OwnerOffice.Code == item.Code && p.Point == dataPoint.Point && p.Name == dataPoint.Name && p.Year == y).FirstOrDefaultAsync();
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
                            pointToCopy.AttachFile = dataPoint.AttachFile;

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
                return RedirectToAction(nameof(Index));
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

        [HttpGet, ActionName("CopyBDPak1toST1")]
        public async Task<IActionResult> CopyBDPak1toST1()
        {
            var target = await _context.Offices.Where(d => d.Code == "01001000").ToListAsync();

            var user = await _userManager.GetUserAsync(User);

            var y = new DateTime(DateTime.Now.Year, 1, 1);


            if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
            {
                y = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);
            }

            var dataPoints = await _context.PointOfEvaluations.Where(p => p.OwnerOffice.Code == "01000000" && p.Year == y && (p.Plan == TypeOfPlan.B || p.Plan == TypeOfPlan.D)).ToListAsync();

            try
            {
                foreach (var dataPoint in dataPoints)
                {
                    var dataRounds = await _context.Rounds.Where(r => r.PointOfEvaluationId == dataPoint.Id).ToListAsync();

                    var dataForEPES = await _context.DataForEvaluations.Where(r => r.PointOfEvaluationId == dataPoint.Id).ToListAsync();

                    foreach (var item in target)
                    {
                        var pointToCopy = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Where(p => p.OwnerOffice.Code == item.Code && p.Point == dataPoint.Point && p.Name == dataPoint.Name && p.Plan == dataPoint.Plan && p.Year == y).FirstOrDefaultAsync();
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
                            pointToCopy.AttachFile = dataPoint.AttachFile;

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
                return RedirectToAction(nameof(Index));
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

        [HttpGet, ActionName("CopyST1toST")]
        public async Task<IActionResult> CopyST1toST()
        {
            var target = await _context.Offices.Where(d => d.Code != "00000000" && d.Code != "01001000" && d.Code.Substring(5, 3) == "000" && d.Code.Substring(0, 3) != "000" && d.Code.Substring(2, 6) != "000000").ToListAsync();

            var user = await _userManager.GetUserAsync(User);

            var y = new DateTime(DateTime.Now.Year, 1, 1);


            if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
            {
                y = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);
            }

            var dataPoints = await _context.PointOfEvaluations.Where(p => p.OwnerOffice.Code == "01001000" && p.Year == y).ToListAsync();

            try
            {
                foreach (var dataPoint in dataPoints)
                {
                    if (dataPoint != null)
                    {
                        var dataRounds = await _context.Rounds.Where(r => r.PointOfEvaluationId == dataPoint.Id).ToListAsync();

                        var dataForEPES = await _context.DataForEvaluations.Where(r => r.PointOfEvaluationId == dataPoint.Id).ToListAsync();

                        foreach (var item in target)
                        {
                            var pointToCopy = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Where(p => p.OwnerOffice.Code == item.Code && p.Point == dataPoint.Point && p.Name == dataPoint.Name && p.Plan == dataPoint.Plan && p.Year == y).FirstOrDefaultAsync();
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
                                pointToCopy.AttachFile = dataPoint.AttachFile;

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
                return RedirectToAction(nameof(Index));
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

        [HttpGet, ActionName("CopyDPak1toHQ")]
        public async Task<IActionResult> CopyDPak1toHQ()
        {
            var target = await _context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(2, 6) != "000000" && d.Code.Substring(5, 3) == "000" && d.Code.Substring(0, 3) == "000").ToListAsync();

            var user = await _userManager.GetUserAsync(User);

            var y = new DateTime(DateTime.Now.Year, 1, 1);


            if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
            {
                y = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);
            }

            var dataPoints = await _context.PointOfEvaluations.Where(p => p.OwnerOffice.Code == "01000000" && p.Year == y && p.Plan == TypeOfPlan.D).ToListAsync();

            try
            {
                foreach (var dataPoint in dataPoints)
                {
                    var dataRounds = await _context.Rounds.Where(r => r.PointOfEvaluationId == dataPoint.Id).ToListAsync();

                    var dataForEPES = await _context.DataForEvaluations.Where(r => r.PointOfEvaluationId == dataPoint.Id).ToListAsync();

                    foreach (var item in target)
                    {
                        var pointToCopy = await _context.PointOfEvaluations.Where(p => p.OwnerOffice.Code == item.Code && p.Point == dataPoint.Point && p.Name == dataPoint.Name && p.Plan == dataPoint.Plan && p.Year == y).FirstOrDefaultAsync();
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
                            pointToCopy.AttachFile = dataPoint.AttachFile;

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
                return RedirectToAction(nameof(Index));
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

        [HttpGet, ActionName("CopyPoint17")]
        public async Task<IActionResult> CopyPoint17()
        {
            var user = await _userManager.GetUserAsync(User);

            var yearPoint = new DateTime(DateTime.Now.Year, 1, 1);
            
            var offices = await _context.PointOfEvaluations.Where(p => p.AutoApp == AutoApps.ผู้ประกอบการรายใหญ่ในท้องที่ && p.Year == yearPoint).Select(s => s.OwnerOffice).ToListAsync();
            try
            {
                foreach(var ofc in offices)
                {
                    var point18Exist = await _context.PointOfEvaluations.Where(p => p.Point == 18 && p.Year == yearPoint && p.OwnerOfficeId == ofc.Id).FirstOrDefaultAsync();
                    if(point18Exist == null)
                    {
                        var point17 = await _context.PointOfEvaluations.Where(p => p.AutoApp == AutoApps.ผู้ประกอบการรายใหญ่ในท้องที่ && p.Year == yearPoint && p.OwnerOffice.Code == ofc.Code).FirstOrDefaultAsync();
                        var point18 = new PointOfEvaluation();
                        point18.AuditOfficeId = point17.AuditOfficeId;
                        point18.AutoApp = point17.AutoApp;
                        point18.Ddrive = point17.Ddrive;
                        point18.DetailPlan = point17.DetailPlan;
                        point18.ExpectPlan = point17.ExpectPlan;
                        point18.HasSub = point17.HasSub;
                        point18.Name = point17.Name;
                        point18.OwnerOfficeId = point17.OwnerOfficeId;
                        point18.Plan = point17.Plan;
                        point18.Point = 18;
                        point18.SubPoint = 0;
                        point18.Unit = point17.Unit;
                        point18.UpdateUserId = user.Id;
                        point18.Weight = point17.Weight;
                        point18.Year = point17.Year;
                        point18.WeightAll = false;
                        point18.AttachFile = point17.AttachFile;

                        point17.AutoApp = AutoApps.ไม่มี;
                        point17.WeightAll = false;

                        _context.PointOfEvaluations.Add(point18);
                        await _context.SaveChangesAsync();

                        
                        var round17 = await _context.Rounds.Where(r => r.PointOfEvaluation == point17).ToListAsync();
                        foreach(var round in round17)
                        {
                            var round18 = new Round();
                            round18.PointOfEvaluationId = point18.Id;
                            round18.DetailRate1 = round.DetailRate1;
                            round18.DetailRate2 = round.DetailRate2;
                            round18.DetailRate3 = round.DetailRate3;
                            round18.DetailRate4 = round.DetailRate4;
                            round18.DetailRate5 = round.DetailRate5;
                            round18.LevelNumber = round.LevelNumber;
                            round18.Rate1MonthStart = round.Rate1MonthStart;
                            round18.Rate1MonthStop = round.Rate1MonthStop;
                            round18.Rate2MonthStart = round.Rate2MonthStart;
                            round18.Rate2MonthStop = round.Rate2MonthStop;
                            round18.Rate3MonthStart = round.Rate3MonthStart;
                            round18.Rate3MonthStop = round.Rate3MonthStop;
                            round18.Rate4MonthStart = round.Rate4MonthStart;
                            round18.Rate4MonthStop = round.Rate4MonthStop;
                            round18.Rate5MonthStart = round.Rate5MonthStart;
                            round18.Rate5MonthStop = round.Rate5MonthStop;
                            round18.Rate1 = round.Rate1;
                            round18.Rate2 = round.Rate2;
                            round18.Rate3 = round.Rate3;
                            round18.Rate4 = round.Rate4;
                            round18.Rate5 = round.Rate5;
                            round18.RoundNumber = round.RoundNumber;
                            _context.Rounds.Add(round18);
                            await _context.SaveChangesAsync();
                        }

                        for(var ii = 1; ii <= 12; ii++)
                        {
                            var dfe17 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluation == point17 && d.Month == ii).FirstOrDefaultAsync();

                            var dfe18 = new DataForEvaluation();
                            dfe18.Month = dfe17.Month;
                            dfe18.UpdateUserId = user.Id;
                            dfe18.PointOfEvaluationId = point18.Id;
                            dfe18.OfficeId = ofc.Id;
                            
                            if(ii >= 4 && ii <= 9)
                            {
                                dfe17.Weight = 0;
                                dfe18.Weight = point17.Weight;
                                if(ii == 4)
                                {
                                    dfe17.Expect = 0;
                                    dfe17.Result = 0;
                                    dfe17.ResultLevelRate = 0;
                                    dfe17.Approve = null;

                                    dfe18.AttachFile = dfe17.AttachFile;
                                    dfe17.AttachFile = null;
                                }
                            } else
                            {
                                dfe18.Weight = 0;
                                dfe17.Weight = point17.Weight;
                            }
                            _context.DataForEvaluations.Add(dfe18);
                            await _context.SaveChangesAsync();
                        }

                    }
                }
                return RedirectToAction(nameof(Index));
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


        [HttpGet, ActionName("CopyCascadeJointAPIFromPak1toAll")]
        public async Task<IActionResult> CopyCascadeJointAPIFromPak1toAll()
        {
            var target = await _context.Offices.Where(d => d.Code != "00000000" && d.Code != "01000000" && d.Code.Substring(5, 3) == "000" && d.Code.Substring(0, 3) != "000").ToListAsync();

            var user = await _userManager.GetUserAsync(User);

            var y = new DateTime(DateTime.Now.Year, 1, 1);


            if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
            {
                y = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);
            }

            var dataPoints = await _context.PointOfEvaluations.Where(p => p.OwnerOffice.Code == "01000000" && p.Year == y && (p.Plan == TypeOfPlan.Cascade || p.Plan == TypeOfPlan.Joint_KPI) && p.Point == 15).ToListAsync();

            try
            {
                foreach (var dataPoint in dataPoints)
                {
                    if (dataPoint != null)
                    {
                        var dataRounds = await _context.Rounds.Where(r => r.PointOfEvaluationId == dataPoint.Id).ToListAsync();

                        var dataForEPES = await _context.DataForEvaluations.Where(r => r.PointOfEvaluationId == dataPoint.Id).ToListAsync();

                        foreach (var item in target)
                        {
                            var pointToCopy = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Where(p => p.OwnerOffice.Code == item.Code && p.Point == dataPoint.Point && p.SubPoint == dataPoint.SubPoint && p.Plan == dataPoint.Plan && p.Year == y).FirstOrDefaultAsync();
                            if (pointToCopy == null)
                            {
                                pointToCopy = new PointOfEvaluation();
                                pointToCopy.AuditOfficeId = dataPoint.AuditOfficeId;
                                pointToCopy.AutoApp = dataPoint.AutoApp;
                                //pointToCopy.Ddrive = dataPoint.Ddrive;
                                pointToCopy.DetailPlan = dataPoint.DetailPlan;
                                //pointToCopy.ExpectPlan = dataPoint.ExpectPlan;
                                pointToCopy.ThemeId = dataPoint.ThemeId;
                                pointToCopy.EndId = dataPoint.EndId;
                                pointToCopy.WayId = dataPoint.WayId;
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
                                pointToCopy.AttachFile = dataPoint.AttachFile;
                                pointToCopy.WeightAll = dataPoint.WeightAll;

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
                                    //dataForEvaluation.Expect = dataforE.Expect;
                                    dataForEvaluation.Weight = dataforE.Weight;
                                    dataForEvaluation.Approve = dataforE.Approve;


                                    _context.DataForEvaluations.Add(dataForEvaluation);
                                    //await _context.SaveChangesAsync();
                                }
                                await _context.SaveChangesAsync();
                            } else
                            {
                                if(pointToCopy.ThemeId == null && pointToCopy.EndId == null && pointToCopy.WayId == null)
                                {
                                    pointToCopy.ThemeId = dataPoint.ThemeId;
                                    pointToCopy.EndId = dataPoint.EndId;
                                    pointToCopy.WayId = dataPoint.WayId;
                                    await _context.SaveChangesAsync();
                                }
                            }
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
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

        public async Task CopyPoint2023ByPoint(DateTime yearForQuery, List<PointOfEvaluation> points, List<Office> targetOffices)
        {
            var user = await _userManager.GetUserAsync(User);
            try
            {
                foreach (var dataPoint in points)
                {
                    if (dataPoint != null)
                    {
                        var dataRounds = await _context.Rounds.Where(r => r.PointOfEvaluationId == dataPoint.Id).ToListAsync();

                        var dataForEPES = await _context.DataForEvaluations.Where(r => r.PointOfEvaluationId == dataPoint.Id).ToListAsync();

                        foreach (var item in targetOffices)
                        {
                            var pointToCopy = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Where(p => p.OwnerOffice.Code == item.Code && p.Point == dataPoint.Point && p.SubPoint == dataPoint.SubPoint && p.Year == yearForQuery).FirstOrDefaultAsync();
                            if (pointToCopy == null)
                            {
                                pointToCopy = new PointOfEvaluation();
                                pointToCopy.AuditOfficeId = dataPoint.AuditOfficeId;
                                pointToCopy.AutoApp = dataPoint.AutoApp;
                                pointToCopy.DetailPlan = dataPoint.DetailPlan;
                                pointToCopy.ThemeId = dataPoint.ThemeId;
                                pointToCopy.EndId = dataPoint.EndId;
                                pointToCopy.WayId = dataPoint.WayId;
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
                                pointToCopy.AttachFile = dataPoint.AttachFile;
                                pointToCopy.WeightAll = dataPoint.WeightAll;

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
                                    //dataForEvaluation.Expect = dataforE.Expect;
                                    dataForEvaluation.Weight = dataforE.Weight;
                                    dataForEvaluation.Approve = dataforE.Approve;


                                    _context.DataForEvaluations.Add(dataForEvaluation);
                                    await _context.SaveChangesAsync();
                                }
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }                
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
        }

        public async Task CopyPoint2023ByName(DateTime yearForQuery, List<PointOfEvaluation> points, List<Office> targetOffices)
        {
            var user = await _userManager.GetUserAsync(User);
            try
            {
                foreach (var dataPoint in points)
                {
                    if (dataPoint != null)
                    {
                        var dataRounds = await _context.Rounds.Where(r => r.PointOfEvaluationId == dataPoint.Id).ToListAsync();

                        var dataForEPES = await _context.DataForEvaluations.Where(r => r.PointOfEvaluationId == dataPoint.Id).ToListAsync();

                        foreach (var item in targetOffices)
                        {
                            var pointToCopy = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Where(p => p.OwnerOffice.Code == item.Code && p.Name == dataPoint.Name && p.Year == yearForQuery).FirstOrDefaultAsync();
                            if (pointToCopy == null)
                            {
                                pointToCopy = new PointOfEvaluation();
                                pointToCopy.AuditOfficeId = dataPoint.AuditOfficeId;
                                pointToCopy.AutoApp = dataPoint.AutoApp;
                                pointToCopy.DetailPlan = dataPoint.DetailPlan;
                                pointToCopy.ThemeId = dataPoint.ThemeId;
                                pointToCopy.EndId = dataPoint.EndId;
                                pointToCopy.WayId = dataPoint.WayId;
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
                                pointToCopy.AttachFile = dataPoint.AttachFile;
                                pointToCopy.WeightAll = dataPoint.WeightAll;

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
                                    //dataForEvaluation.Expect = dataforE.Expect;
                                    dataForEvaluation.Weight = dataforE.Weight;
                                    dataForEvaluation.Approve = dataforE.Approve;


                                    _context.DataForEvaluations.Add(dataForEvaluation);
                                    await _context.SaveChangesAsync();
                                }
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
        }

        [HttpGet, ActionName("CopyCascadeFromPak1ToAll")]
        public async Task<IActionResult> CopyCascadeFromPak1ToAll()
        {
            var y = new DateTime(DateTime.Now.Year, 1, 1);
            if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
            {
                y = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);
            }

            var target = await _context.Offices.Where(d => d.Code != "00000000" && d.Code != "01000000" && d.Code.Substring(5, 3) == "000" && d.Code.Substring(0, 3) != "000").ToListAsync();
            var dataPoints = await _context.PointOfEvaluations.Where(p => p.OwnerOffice.Code == "01000000" && p.Year == y && (p.Plan == TypeOfPlan.Cascade)).ToListAsync();
            try
            {
                await CopyPoint2023ByPoint(y, dataPoints, target);
                return RedirectToAction(nameof(Index));
            } catch (Exception ex)
            {
                return RedirectToAction(nameof(Index), new { selectoffice = "01000000", message = ex.InnerException });
            }
        }

        [HttpGet, ActionName("CopyCascadeFromPak1ToPaks")]
        public async Task<IActionResult> CopyCascadeFromPak1ToPaks()
        {
            var y = new DateTime(DateTime.Now.Year, 1, 1);
            if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
            {
                y = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);
            }

            var target = await _context.Offices.Where(d => d.Code != "00000000" && d.Code != "01000000" && d.Code.Substring(2, 6) == "000000" && d.Code.Substring(0, 3) != "000").ToListAsync();
            var dataPoints = await _context.PointOfEvaluations.Where(p => p.OwnerOffice.Code == "01000000" && p.Year == y && (p.Plan == TypeOfPlan.Cascade)).ToListAsync();
            try
            {
                await CopyPoint2023ByPoint(y, dataPoints, target);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return RedirectToAction(nameof(Index), new { selectoffice = "01000000", message = ex.InnerException });
            }
        }

        [HttpGet, ActionName("CopyCascadeFromST1ToSTs")]
        public async Task<IActionResult> CopyCascadeFromST1ToSTs()
        {
            var y = new DateTime(DateTime.Now.Year, 1, 1);
            if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
            {
                y = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);
            }

            var target = await _context.Offices.Where(d => d.Code != "00000000" && d.Code != "01001000" && d.Code.Substring(5, 3) == "000" && d.Code.Substring(2, 6) != "000000" && d.Code.Substring(0, 3) != "000").ToListAsync();
            var dataPoints = await _context.PointOfEvaluations.Where(p => p.OwnerOffice.Code == "01001000" && p.Year == y && (p.Plan == TypeOfPlan.Cascade)).ToListAsync();
            try
            {
                await CopyPoint2023ByPoint(y, dataPoints, target);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index), new { selectoffice = "01001000", message = ex.InnerException });
            }
        }

        [HttpGet, ActionName("CopyJointKPIFromPak1ToAll")]
        public async Task<IActionResult> CopyJointKPIFromPak1ToAll()
        {
            var y = new DateTime(DateTime.Now.Year, 1, 1);
            if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
            {
                y = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);
            }

            var target = await _context.Offices.Where(d => d.Code != "00000000" && d.Code != "01000000" && d.Code.Substring(5, 3) == "000" && d.Code.Substring(0, 3) != "000").ToListAsync();
            var dataPoints = await _context.PointOfEvaluations.Where(p => p.OwnerOffice.Code == "01000000" && p.Year == y && (p.Plan == TypeOfPlan.Joint_KPI)).ToListAsync();
            try
            {
                await CopyPoint2023ByPoint(y, dataPoints, target);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index), new { selectoffice = "01000000", message = ex.InnerException });
            }
        }

        [HttpGet, ActionName("CopyJointKPIFromPak1ToCentral")]
        public async Task<IActionResult> CopyJointKPIFromPak1ToCentral()
        {
            var y = new DateTime(DateTime.Now.Year, 1, 1);
            if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
            {
                y = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);
            }

            var target = await _context.Offices.Where(d => d.Code != "00000000" && d.Code != "01000000" && d.Code.Substring(0, 2) == "00").ToListAsync();
            var dataPoints = await _context.PointOfEvaluations.Where(p => p.OwnerOffice.Code == "01000000" && p.Year == y && (p.Plan == TypeOfPlan.Joint_KPI)).ToListAsync();
            try
            {
                await CopyPoint2023ByName(y, dataPoints, target);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index), new { selectoffice = "01000000", message = ex.InnerException });
            }
        }


        [HttpPost]
        public async Task<IActionResult> FileSelection(IFormFile attachFile, int pid, string selectoffice, int yearPoint)
        {
            // Learn to use the entire functionality of the dxFileUploader widget.
            // http://js.devexpress.com/Documentation/Guide/UI_Widgets/UI_Widgets_-_Deep_Dive/dxFileUploader/

            if (attachFile != null)
            {
                await SaveFile(attachFile, pid);
            }

            return RedirectToAction(nameof(Index), new { selectoffice = selectoffice, yearPoint = yearPoint});
        }
        public async Task<IActionResult> FileDelete(int pid, string selectoffice, int yearPoint)
        {
            var de = await _context.PointOfEvaluations.FirstAsync(d => d.Id == pid);
            if (de != null)
            {
                var user = await _userManager.GetUserAsync(User);
                //de.UpdateUserId = user.Id;
                de.AttachFile = null;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { selectoffice = selectoffice, yearPoint = yearPoint});
        }
        public async Task SaveFile(IFormFile file, int pid)
        {
            //var result = Path.GetFileName(file.FileName);
            //var uniqueFile = Guid.NewGuid().ToString() + "_" + result;
            var uniqueFile = Guid.NewGuid().ToString() + ".pdf";
            try
            {
                var path = Path.Combine(_hostingEnvironment.WebRootPath, "attach_files");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                using (var fileStream = System.IO.File.Create(Path.Combine(path, uniqueFile)))
                {
                    await file.CopyToAsync(fileStream);
                }

                var user = await _userManager.GetUserAsync(User);
                var de = await _context.PointOfEvaluations.FirstAsync(d => d.Id == pid);
                if (de != null)
                {
                    //de.UpdateUserId = user.Id;
                    de.AttachFile = uniqueFile;
                    await _context.SaveChangesAsync();
                }
            }
            catch
            {
                Response.StatusCode = 400;
            }
        }

        public async Task<IActionResult> CopyPoint(string selectoffice, string message)
        {
            var user = await _userManager.GetUserAsync(User);

            DateTime yearForQuery;
            if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
            {
                yearForQuery = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);
            }
            else
            {
                yearForQuery = new DateTime(DateTime.Now.Year, 1, 1);
            }

            if (string.IsNullOrEmpty(selectoffice))
            {
                selectoffice = user.OfficeId;
            }

            ResultViewModel pvViewModel = new ResultViewModel();
            pvViewModel.pointA = await _context.PointOfEvaluations.Where(pv => pv.Year == yearForQuery && pv.OwnerOffice.Code == selectoffice).OrderBy(ob => ob.Point).ThenBy(ob => ob.SubPoint).ToListAsync();
            pvViewModel.yearPoint = yearForQuery.Year;
            pvViewModel.selectoffice = selectoffice;
            ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);

            if(!String.IsNullOrEmpty(message))
            {
                ViewBag.message = message;
            }
            return View(pvViewModel);
        }

        [HttpPost, ActionName("CopyPoint")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CopyPointPost(string selectoffice, string message)
        {
            var user = await _userManager.GetUserAsync(User);

            DateTime yearForQuery;
            if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
            {
                yearForQuery = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);
            }
            else
            {
                yearForQuery = new DateTime(DateTime.Now.Year, 1, 1);
            }

            if (string.IsNullOrEmpty(selectoffice))
            {
                selectoffice = user.OfficeId;
            }

            ResultViewModel pvViewModel = new ResultViewModel();
            pvViewModel.pointA = await _context.PointOfEvaluations.Where(pv => pv.Year == yearForQuery && pv.OwnerOffice.Code == selectoffice).OrderBy(ob => ob.Point).ThenBy(ob => ob.SubPoint).ToListAsync();
            pvViewModel.yearPoint = yearForQuery.Year;
            pvViewModel.selectoffice = selectoffice;
            ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);

            if (!String.IsNullOrEmpty(message))
            {
                ViewBag.message = message;
            }
            return View(pvViewModel);
        }

        public async Task<IActionResult> CopyingPoint(ResultViewModel dataView)
        {
            List<Office> destOffices = new List<Office>() ;
            if (dataView.destoffice == 0) // ทุกหน่วยงาน
            {
                destOffices = await _context.Offices.Where(ofc => ofc.Code != "00000000" && ofc.Code.Substring(5, 3) == "000").ToListAsync();
            } else if (dataView.destoffice == 1) // ส่วนกลาง กอง กลุ่ม ศูนย์
            {
                destOffices = await _context.Offices.Where(ofc => ofc.Code.Substring(0, 2) == "00" && ofc.Code != "00000000").ToListAsync();
            } else if(dataView.destoffice == 2) // ตส.
            {
                destOffices = await _context.Offices.Where(ofc => ofc.Code == "00003000").ToListAsync();
            } else if(dataView.destoffice == 3) // ภญ.
            {
                destOffices = await _context.Offices.Where(ofc => ofc.Code == "00009000").ToListAsync();
            } else if (dataView.destoffice == 4) // ภาค 1 - 12
            {
                destOffices = await _context.Offices.Where(ofc => ofc.Code.Substring(0, 2) != "00" && ofc.Code.Substring(2, 6) == "000000").ToListAsync();
            } else if (dataView.destoffice == 5) // ภาค 1 - 3
            {
                destOffices = await _context.Offices.Where(ofc => (ofc.Code.Substring(0, 2) == "01" || ofc.Code.Substring(0, 2) == "02" || ofc.Code.Substring(0, 2) == "03") && ofc.Code.Substring(2, 6) == "000000").ToListAsync();
            } else if(dataView.destoffice == 6) // ภาค 4 - 12
            {
                destOffices = await _context.Offices.Where(ofc => (ofc.Code.Substring(0, 2) != "00" && ofc.Code.Substring(0, 2) != "01" && ofc.Code.Substring(0, 2) != "02" && ofc.Code.Substring(0, 2) != "03") && ofc.Code.Substring(2, 6) == "000000").ToListAsync();
            } else if(dataView.destoffice == 7) // สท.ทั่วประเทศ
            {
                destOffices = await _context.Offices.Where(ofc => ofc.Code.Substring(0, 2) != "00" && ofc.Code.Substring(2, 6) != "000000" && ofc.Code.Substring(5, 3) == "000").ToListAsync();
            } else if(dataView.destoffice == 8) // สท. กทม.
            {
                destOffices = await _context.Offices.Where(ofc => (ofc.Code.Substring(0, 2) == "01" || ofc.Code.Substring(0, 2) == "02" || ofc.Code.Substring(0, 2) == "03") && ofc.Code.Substring(5, 3) == "000" && ofc.Code.Substring(2, 6) != "000000").ToListAsync();
            } else if(dataView.destoffice == 9) // สท. ต่างจังหวัด
            {
                destOffices = await _context.Offices.Where(ofc => (ofc.Code.Substring(0, 2) != "00" && ofc.Code.Substring(0, 2) != "01" && ofc.Code.Substring(0, 2) != "02" && ofc.Code.Substring(0, 2) != "03") && ofc.Code.Substring(5, 3) == "000" && ofc.Code.Substring(2, 6) != "000000").ToListAsync();
            }
            var message = "";
            var pointNum = 0;
            if(destOffices.Count > 0)
            {
                foreach(PointOfEvaluation point in dataView.pointA)
                {
                    if(point.isSelectedToCopy)
                    {
                        try
                        {
                            var dataPoint = await _context.PointOfEvaluations.Where(p => p.Id == point.Id).ToListAsync();
                            // await CopyPoint2023(dataPoint, destOffices);
                            pointNum += 1;
                        }
                        catch (DbUpdateException ex)
                        {
                            message = "Error\nPlease contact admin\n" + ex.InnerException;
                            pointNum = -1;
                        }
                    }
                }
                if(pointNum != -1)
                {
                    message = "คัดลอก " + pointNum + " ตัวชี้วัดไปยัง " + destOffices.Count + " หน่วยงานสำเร็จ";
                }
            }
            return RedirectToAction(nameof(CopyPoint), new { selectoffice = dataView.selectoffice, message = message });
        }
    }
}
