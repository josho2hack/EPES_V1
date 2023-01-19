using EPES.Data;
using EPES.Models;
using EPES.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EPES.Controllers
{
    [Authorize]
    public class ResultsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ResultsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<IActionResult> IndexMonth(string selectoffice, string message, int month = 0, int yearPoint = 0)
        {
            var user = await _userManager.GetUserAsync(User);
            var viewModel = new ResultViewModel();
            int m;
            DateTime yearForQuery;
            List<Object> list = new List<object>();

            //บันทึกย้อนหลัง 1 เดือน
            if(month == 0)
            {
                if (DateTime.Now.Month == 1)
                {
                    m = 12;
                }
                else
                {
                    m = DateTime.Now.Month - 1;
                }
            } else
            {
                m = month;
            }


            //ดึงข้อมูล เฉพาะในปีงบประมาณ ต.ค. (10) ปีก่อน - ก.ย. (09) ปีปัจจุบัน
            if (yearPoint == 0)
            {
                if (DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
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
            viewModel.yearPoint = yearForQuery.Year;

            for (int i = 10; i <= 12; i++)
            {
                list.Add(new { Value = i, Month = new DateTime(yearForQuery.Year, i, 1).AddYears(-1).ToString("MMMM yyyy") });
            }
            for (int i = 1; i <= 9; i++)
            {
                list.Add(new { Value = i, Month = new DateTime(yearForQuery.Year, i, 1).ToString("MMMM yyyy") });
            }

            if (string.IsNullOrEmpty(selectoffice))
            {
                selectoffice = user.OfficeId;
            }

            DateTime year2023 = new DateTime(2023, 1, 1);
            if(yearForQuery >= year2023)
            {
                viewModel.pointFlagship = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.Flagship && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).OrderBy(ob => ob.Point).ThenBy(ob => ob.SubPoint).ToListAsync();
                viewModel.pointCascade = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.Cascade && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).OrderBy(ob => ob.Point).ThenBy(ob => ob.SubPoint).ToListAsync();
                viewModel.pointJointKPI = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.Joint_KPI && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).OrderBy(ob => ob.Point).ThenBy(ob => ob.SubPoint).ToListAsync();
            } else
            {
                if (selectoffice.Substring(0, 3) == "000")
                {
                    viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).ToListAsync();
                }
                viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).ToListAsync();
                viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).ToListAsync();
                viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).ToListAsync();
            }

            if (User.IsInRole("Admin") || User.IsInRole("Special"))
            {
                ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);
            }
            else // No Admin
            {
                List<Office> officeList = new List<Office>();
                if (selectoffice.Substring(0, 3) == "000") // HQ Office
                {
                    officeList = await _context.PointOfEvaluations.Where(p => ((p.OwnerOffice.Code == selectoffice) || (p.AuditOffice.Code == user.OfficeId)) && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => b.OwnerOffice).Distinct().ToListAsync();
                    if (officeList.Count < 1)
                    {
                        officeList.AddRange(_context.Offices.Where(ofc => ofc.Code == user.OfficeId));
                    }
                }
                else // Pak or ST
                {
                    officeList = await _context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000").ToListAsync();
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

            ViewBag.Month = new SelectList(list, "Value", "Month", m);
            ViewBag.selectoffice = selectoffice;
            viewModel.month = m;

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

        [HttpPost, ActionName("IndexMonth")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexMonthPost(string selectoffice, string message, ResultViewModel model, int yearPoint)
        {
            var user = await _userManager.GetUserAsync(User);
            var viewModel = new ResultViewModel();
            int m;
            DateTime yearForQuery;
            List<Object> list = new List<object>();

            //บันทึกย้อนหลัง 1 เดือน
            if(model.month == 0)
            {
                if (DateTime.Now.Month == 1)
                {
                    m = 12;
                }
                else
                {
                    m = DateTime.Now.Month - 1;
                }
            } else
            {
                m = model.month;
            }


            //ดึงข้อมูล เฉพาะในปีงบประมาณ ต.ค. (10) ปีก่อน - ก.ย. (09) ปีปัจจุบัน
            if (yearPoint == 0)
            {
                if (DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
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

            viewModel.yearPoint = yearForQuery.Year;

            for (int i = 10; i <= 12; i++)
            {
                list.Add(new { Value = i, Month = new DateTime(yearForQuery.Year, i, 1).AddYears(-1).ToString("MMMM yyyy") });
            }
            for (int i = 1; i <= 9; i++)
            {
                list.Add(new { Value = i, Month = new DateTime(yearForQuery.Year, i, 1).ToString("MMMM yyyy") });
            }

            if (string.IsNullOrEmpty(selectoffice))
            {
                selectoffice = user.OfficeId;
            }

            DateTime year2023 = new DateTime(2023, 1, 1);

            if (yearForQuery >= year2023)
            {
                viewModel.pointFlagship = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.Flagship && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).OrderBy(ob => ob.Point).ThenBy(ob => ob.SubPoint).ToListAsync();
                viewModel.pointCascade = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.Cascade && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).OrderBy(ob => ob.Point).ThenBy(ob => ob.SubPoint).ToListAsync();
                viewModel.pointJointKPI = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.Joint_KPI && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).OrderBy(ob => ob.Point).ThenBy(ob => ob.SubPoint).ToListAsync();
            }
            else
            {
                if (selectoffice.Substring(0, 3) == "000")
                {
                    viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).ToListAsync();
                }
                viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).ToListAsync();
                viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).ToListAsync();
                viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).ToListAsync();
            }

            if (User.IsInRole("Admin") || User.IsInRole("Special"))
            {
                ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);
            }
            else // No Admin
            {
                List<Office> officeList = new List<Office>();
                if (selectoffice.Substring(0, 3) == "000") // HQ Office
                {
                    officeList = await _context.PointOfEvaluations.Where(p => ((p.OwnerOffice.Code == selectoffice) || (p.AuditOffice.Code == user.OfficeId)) && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => b.OwnerOffice).Distinct().ToListAsync();
                    if (officeList.Count < 1)
                    {
                        officeList.AddRange(_context.Offices.Where(ofc => ofc.Code == user.OfficeId));
                    }
                }
                else // Pak or ST
                {
                    officeList = await _context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000").ToListAsync();
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

            ViewBag.Month = new SelectList(list, "Value", "Month", m);
            ViewBag.selectoffice = selectoffice;
            viewModel.month = m;

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
            /*
                var user = await _userManager.GetUserAsync(User);
                var viewModel = new ResultViewModel();
                int m;
                DateTime yearForQuery;
                List<Object> list = new List<object>();

                //บันทึกย้อนหลัง 1 เดือน
                if (DateTime.Now.Month == 1)
                {
                    m = 12;
                }
                else
                {
                    m = DateTime.Now.Month - 1;
                }

                viewModel.yearPoint = yearPoint;

                //ดึงข้อมูล เฉพาะในปีงบประมาณ ต.ค. (10) ปีก่อน - ก.ย. (09) ปีปัจจุบัน
                if (yearPoint == 0)
                {
                    if (DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
                    {
                        yearForQuery = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);
                        for (int i = 10; i <= 12; i++)
                        {
                            list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                        }
                        for (int i = 1; i <= 9; i++)
                        {
                            list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(1).Year, i, 1).ToString("yyyy") });
                        }
                    }
                    else
                    {
                        yearForQuery = new DateTime(DateTime.Now.Year, 1, 1);
                        for (int i = 10; i <= 12; i++)
                        {
                            list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(-1).Year, i, 1).ToString("yyyy") });
                        }
                        for (int i = 1; i <= 9; i++)
                        {
                            list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                        }
                    }
                }
                else
                {
                    if (DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
                    {
                        yearForQuery = new DateTime(DateTime.Now.AddYears(1 + yearPoint).Year, 1, 1);
                        for (int i = 10; i <= 12; i++)
                        {
                            list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(yearPoint).Year, i, 1).ToString("yyyy") });
                        }
                        for (int i = 1; i <= 9; i++)
                        {
                            list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(1 + yearPoint).Year, i, 1).ToString("yyyy") });
                        }
                    }
                    else if (DateTime.Now.Month == 10)
                    {
                        yearForQuery = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);
                        for (int i = 10; i <= 12; i++)
                        {
                            list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                        }
                        for (int i = 1; i <= 9; i++)
                        {
                            list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(1).Year, i, 1).ToString("yyyy") });
                        }
                    }
                    else
                    {
                        yearForQuery = new DateTime(DateTime.Now.AddYears(yearPoint).Year, 1, 1);
                        //m = 9;
                        for (int i = 10; i <= 12; i++)
                        {
                            list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(-1 + yearPoint).Year, i, 1).ToString("yyyy") });
                        }
                        for (int i = 1; i <= 9; i++)
                        {
                            list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(yearPoint).Year, i, 1).ToString("yyyy") });
                        }
                    }
                }

                if (User.IsInRole("Admin") || User.IsInRole("Special"))
                {
                    if (String.IsNullOrEmpty(selectoffice))
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).ToListAsync();
                        viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).ToListAsync();
                        viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).ToListAsync();
                        viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).ToListAsync();

                        ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", user.OfficeId);
                    }
                    else
                    {
                        if (selectoffice.Substring(0, 3) == "000")
                        {
                            viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).ToListAsync();
                        }
                        viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).ToListAsync();
                        viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).ToListAsync();
                        viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).ToListAsync();

                        ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);
                    }
                }
                else // No Admin
                {
                    List<Office> officeList;
                    string defaultOffice;
                    if (String.IsNullOrEmpty(selectoffice))
                    {
                        defaultOffice = user.OfficeId;
                        if (user.OfficeId.Substring(0, 3) == "000") // HQ Office
                        {
                            viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).ToListAsync();

                            // var officeList = await _context.PointOfEvaluations.Where(p => (p.OwnerOffice.Code == user.OfficeId) || (p.Plan == TypeOfPlan.B && (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => new { Code = b.OwnerOffice.Code, Name = b.OwnerOffice.Name }).Distinct().ToListAsync();

                            officeList = await _context.PointOfEvaluations.Where(p => (p.OwnerOffice.Code == user.OfficeId) || (p.Plan == TypeOfPlan.B && (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => b.OwnerOffice).Distinct().ToListAsync();

                            if (officeList.Count < 1)
                            {
                                //officeList.Add(new { Code = user.OfficeId, Name = user.OfficeName });
                                officeList.AddRange(_context.Offices.Where(ofc => ofc.Code == user.OfficeId));
                            }

                            //ViewBag.OfficeCode = new SelectList(officeList, "Code", "Name", user.OfficeId);
                        }
                        else // Pak or ST
                        {
                            //ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Code", "Name", user.OfficeId);
                            officeList = await _context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000").ToListAsync();
                        }

                        viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).ToListAsync();
                        viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).ToListAsync();
                        viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).ToListAsync();
                    }
                    else
                    {
                        defaultOffice = selectoffice;
                        if (user.OfficeId.Substring(0, 3) == "000") // HQ Office
                        {
                            viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).ToListAsync();

                            //var officeList = await _context.PointOfEvaluations.Where(p => (p.OwnerOffice.Code == user.OfficeId) || (p.Plan == TypeOfPlan.B && (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => new { Code = b.OwnerOffice.Code, Name = b.OwnerOffice.Name }).Distinct().ToListAsync();

                            officeList = await _context.PointOfEvaluations.Where(p => (p.OwnerOffice.Code == user.OfficeId) || (p.Plan == TypeOfPlan.B && (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => b.OwnerOffice).Distinct().ToListAsync();

                            if (officeList.Count < 1)
                            {
                                //officeList.Add(new { Code = user.OfficeId, Name = user.OfficeName });
                                officeList.AddRange(_context.Offices.Where(ofc => ofc.Code == user.OfficeId));
                            }

                            //ViewBag.OfficeCode = new SelectList(officeList, "Code", "Name", selectoffice);
                        }
                        else // Pak or ST
                        {
                            officeList = await _context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000").ToListAsync();
                            //ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);
                        }

                        viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).ToListAsync();
                        viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).ToListAsync();
                        viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.Rounds).ToListAsync();
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
                    ViewBag.OfficeCode = new SelectList(officeList, "Code", "Name", defaultOffice);
                }

                List<int> pointHasSub = new List<int>();

                if (viewModel.pointA != null)
                {
                    foreach (var point in viewModel.pointA)
                    {
                        if (point.SubPoint == 1)
                        {
                            pointHasSub.Add(point.Point);
                        }
                    }
                }

                if (viewModel.pointB != null)
                {
                    foreach (var point in viewModel.pointB)
                    {
                        if (point.SubPoint == 1)
                        {
                            pointHasSub.Add(point.Point);
                        }
                    }
                }

                foreach (var point in viewModel.pointC)
                {
                    if (point.SubPoint == 1)
                    {
                        pointHasSub.Add(point.Point);
                    }
                }

                foreach (var point in viewModel.pointD)
                {
                    if (point.SubPoint == 1)
                    {
                        pointHasSub.Add(point.Point);
                    }
                }

                ViewData["pointHasSub"] = pointHasSub.ToArray();

                ViewBag.Month = new SelectList(list, "Value", "Month", model.month);
                ViewBag.selectoffice = selectoffice;
                viewModel.yearPoint = model.yearPoint;
                viewModel.month = model.month;

                return View(viewModel);
            */
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMonth(string selectoffice, List<UpdateDataViewModel> UpdateData, int yearPoint, int month, string Update)
        {
            //ViewBag.msg1 = "hello1";
            var user = await _userManager.GetUserAsync(User);
            if (Update == "บันทึก")
            {
                if (UpdateData != null)
                {
                    foreach (var item in UpdateData)
                    {

                        if (!item.hasSub)
                        {
                            int previousMonth = month - 1;
                            if(month == 1)
                            {
                                previousMonth = 12;
                            }
                            else if(month == 10)
                            {
                                previousMonth = 10;
                            }

                            //var dde = await _context.DataForEvaluations.Where(d => d.Id == item.Id).Include(d => d.PointOfEvaluation).FirstOrDefaultAsync();
                            var previousFile = _context.DataForEvaluations.Where(de => de.PointOfEvaluationId == item.poeid && (de.PointOfEvaluation.Unit == UnitOfPoint.ระดับ || de.PointOfEvaluation.Unit == UnitOfPoint.ระดับ_ร้อยละ) && de.Month == previousMonth && de.Result == item.Result && de.ResultLevelRate == item.ResultLevelRate && de.Completed == item.Completed).Select(ii => ii.AttachFile).FirstOrDefault();

                            if (item.Id != null)
                            {
                                var de = await _context.DataForEvaluations.Where(d => d.Id == item.Id).Include(d => d.PointOfEvaluation).FirstOrDefaultAsync();
                                if ((de.Approve == null || de.Approve == Approve.รอพิจารณา) && de.PointOfEvaluation.Weight != 0)
                                {
                                    de.Result = item.Result;
                                    de.ResultLevelRate = item.ResultLevelRate;
                                    de.UpdateUserId = user.Id;
                                    de.Completed = item.Completed;
                                    de.TimeUpdate = DateTime.Now;
                                    de.Approve = Approve.รอพิจารณา;

                                    if(previousFile != null && de.AttachFile == null)
                                    {
                                        de.AttachFile = previousFile;
                                    }

                                    try
                                    {
                                        var err = await _context.SaveChangesAsync();
                                    }
                                    catch (DbUpdateException ex)
                                    {
                                        //Log the error (uncomment ex variable name and write a log.
                                        /*
                                        ModelState.AddModelError("", "ไม่สามารถบันทึกข้อมูล. " +
                                            "ลองพยายามบันทึกอีกครั้ง " +
                                            "โปรดแจ้งผู้ดูแลระบบ");
                                        */
                                        return RedirectToAction(nameof(IndexMonth), new { selectoffice = selectoffice, isUpdate = true, yearPoint = yearPoint, month = month, message = "Error\nPlease contact admin\n" + ex.InnerException });
                                    }
                                }
                            }
                            else
                            {
                                var de = new DataForEvaluation();
                                de.Result = item.Result;
                                de.ResultLevelRate = item.ResultLevelRate;
                                de.UpdateUserId = user.Id;
                                de.PointOfEvaluationId = item.poeid;
                                de.OfficeId = item.officeid;
                                de.Month = month;
                                de.Completed = item.Completed;
                                de.TimeUpdate = DateTime.Now;
                                de.Approve = Approve.รอพิจารณา;

                                if(previousFile != null)
                                {
                                    de.AttachFile = previousFile;
                                }

                                try
                                {
                                    _context.Add(de);
                                    await _context.SaveChangesAsync();
                                }
                                catch (DbUpdateException ex)
                                {
                                    //Log the error (uncomment ex variable name and write a log.
                                    /*
                                    ModelState.AddModelError("", "ไม่สามารถบันทึกข้อมูล. " +
                                        "ลองพยายามบันทึกอีกครั้ง " +
                                        "โปรดแจ้งผู้ดูแลระบบ");
                                    */
                                    return RedirectToAction(nameof(IndexMonth), new { selectoffice = selectoffice, isUpdate = true, yearPoint = yearPoint, month = month, message = "Error\nPlease contact admin\n" + ex.InnerException });
                                }
                            }
                        }

                    }
                }
            }

            return RedirectToAction(nameof(IndexMonth), new { selectoffice = selectoffice, isUpdate = true, yearPoint = yearPoint, month = month });
        }

        [HttpPost]
        public async Task<IActionResult> FileSelection(IFormFile attachFile, int deid, string selectoffice, int yearPoint, int month)
        {
            // Learn to use the entire functionality of the dxFileUploader widget.
            // http://js.devexpress.com/Documentation/Guide/UI_Widgets/UI_Widgets_-_Deep_Dive/dxFileUploader/

            if (attachFile != null)
            {
                try
                {
                    await SaveFile(attachFile, deid);
                } catch (DbUpdateException ex)
                {
                    return RedirectToAction(nameof(IndexMonth), new { selectoffice = selectoffice, isUpdate = true, yearPoint = yearPoint, month = month, message = "Error\nPlease contact admin\n" + ex.InnerException });
                }
            }

            return RedirectToAction(nameof(IndexMonth), new { selectoffice = selectoffice, yearPoint = yearPoint, month = month });
        }
        public async Task<IActionResult> FileDelete(int deid, string selectoffice, int yearPoint, int month)
        {
            var de = await _context.DataForEvaluations.FirstAsync(d => d.Id == deid);
            if (de != null)
            {
                var user = await _userManager.GetUserAsync(User);
                de.UpdateUserId = user.Id;
                de.AttachFile = null;
                try
                {
                    await _context.SaveChangesAsync();
                } catch (DbUpdateException ex)
                {
                    return RedirectToAction(nameof(IndexMonth), new { selectoffice = selectoffice, isUpdate = true, yearPoint = yearPoint, month = month, message = "Error\nPlease contact admin\n" + ex.InnerException });
                }
            }
            return RedirectToAction(nameof(IndexMonth), new { selectoffice = selectoffice, yearPoint = yearPoint, month = month });
        }
        public async Task SaveFile(IFormFile file, int deid)
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
                var de = await _context.DataForEvaluations.FirstAsync(d => d.Id == deid);
                if (de != null)
                {
                    de.UpdateUserId = user.Id;
                    de.AttachFile = uniqueFile;
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
        }
    }
}