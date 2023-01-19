using DevExtreme.AspNet.Mvc;
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
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }


        public async Task<IActionResult> OwnerScoreReport()
        {
            var model = new ReportViewModel();
            int yearPoint = 0;

            if (DateTime.Now.Month == 10)
            {
                yearPoint = -1;
            }

            var user = await _userManager.GetUserAsync(User);
            if (User.IsInRole("Admin") || User.IsInRole("Special"))
            {
                var officeList = _context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000").ToList();
                ViewBag.OfficeCode = new SelectList(officeList, "Code", "Name", user.OfficeId);
            }
            else if (user.OfficeId.Substring(2, 6) == "000000")
            {
                var officeList = _context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000").ToList();
                officeList.AddRange(_context.UserOffices.Where(ou => ou.UserName == user.UserName).Select(slt => slt.Office).ToList());
                ViewBag.OfficeCode = new SelectList(officeList, "Code", "Name", user.OfficeId);
            }
            else
            {
                var officeList = _context.Offices.Where(d => d.Code == user.OfficeId).ToList();
                officeList.AddRange(_context.UserOffices.Where(ou => ou.UserName == user.UserName).Select(slt => slt.Office).ToList());
                ViewBag.OfficeCode = new SelectList(officeList, "Code", "Name", user.OfficeId);
            }

            model.yearPoint = yearPoint;
            return View(model);
        }

        [HttpPost, ActionName("SubScoreReport")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubScoreReport(ReportViewModel model)
        {

            if (DateTime.Now.Month == 10)
            {
                model.yearPoint = -1;
            }

            var user = await _userManager.GetUserAsync(User);
            if (User.IsInRole("Admin") || User.IsInRole("Special"))
            {
                ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", user.OfficeId);
            }
            else if (user.OfficeId.Substring(2, 6) == "000000")
            {
                var officeList = _context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000").ToList();
                officeList.AddRange(_context.UserOffices.Where(ou => ou.UserName == user.UserName).Select(slt => slt.Office).ToList());
                ViewBag.OfficeCode = new SelectList(officeList, "Code", "Name", user.OfficeId);
                //ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Code", "Name", user.OfficeId);
            }
            else
            {
                var officeList = _context.Offices.Where(d => d.Code == user.OfficeId).ToList();
                officeList.AddRange(_context.UserOffices.Where(ou => ou.UserName == user.UserName).Select(slt => slt.Office).ToList());
                ViewBag.OfficeCode = new SelectList(officeList, "Code", "Name", user.OfficeId);
                //ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code == user.OfficeId), "Code", "Name", user.OfficeId);
            }

            var of = await _context.Offices.Where(d => d.Code == model.selectoffice).FirstOrDefaultAsync();
            ViewBag.OfficeName = of.Name;

            return View(model);
        }

        public IActionResult AllScore()
        {
            var model = new ReportViewModel();
            int m;
            int yearPoint = 0;
            List<Object> list = new List<object>();

            if (DateTime.Now.Month == 10)
            {
                yearPoint = -1;
            }

            //บันทึกย้อนหลัง 1 เดือน
            if (DateTime.Now.Month == 1)
            {
                m = 12;
            }
            else
            {
                m = DateTime.Now.Month - 1;
            }

            //ดึงข้อมูล เฉพาะในปีงบประมาณ ต.ค. (10) ปีก่อน - ก.ย. (09) ปีปัจจุบัน
            if (yearPoint == 0)
            {
                if (m < 10)
                {
                    for (int i = 10; i <= 12; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(-1).Year, i, 1).ToString("yyyy") });
                    }
                    for (int i = 1; i <= m; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                    }
                }
                else
                {
                    for (int i = 10; i <= m; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                    }
                }
            }
            else
            {
                for (int i = 10; i <= 12; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(yearPoint).Year, i, 1).ToString("yyyy") });
                }
                for (int i = 1; i <= 9; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                }
            }

            ViewBag.Month = new SelectList(list, "Value", "Month", m);
            model.Month = m;
            model.yearPoint = yearPoint;

            return View(model);
        }

        [HttpPost, ActionName("AllScoreMonth")]
        [ValidateAntiForgeryToken]
        public IActionResult AllScoreMonthPost(ReportViewModel model)
        {
            int m;
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

            //ดึงข้อมูล เฉพาะในปีงบประมาณ ต.ค. (10) ปีก่อน - ก.ย. (09) ปีปัจจุบัน
            if (model.yearPoint == 0)
            {
                if (m < 10)
                {
                    for (int i = 10; i <= 12; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(-1).Year, i, 1).ToString("yyyy") });
                    }
                    for (int i = 1; i <= m; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                    }
                }
                else
                {
                    for (int i = 10; i <= m; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                    }
                }
            }
            else
            {
                for (int i = 10; i <= 12; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(model.yearPoint).Year, i, 1).ToString("yyyy") });
                }
                for (int i = 1; i <= 9; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                }
            }

            ViewBag.Month = new SelectList(list, "Value", "Month", model.Month);
            return View(model);
        }

        public IActionResult HQScore()
        {
            var model = new ReportViewModel();
            int m;
            int yearPoint = 0;
            List<Object> list = new List<object>();

            if (DateTime.Now.Month == 10)
            {
                yearPoint = -1;
            }

            //บันทึกย้อนหลัง 1 เดือน
            if (DateTime.Now.Month == 1)
            {
                m = 12;
            }
            else
            {
                m = DateTime.Now.Month - 1;
            }

            //ดึงข้อมูล เฉพาะในปีงบประมาณ ต.ค. (10) ปีก่อน - ก.ย. (09) ปีปัจจุบัน
            if (yearPoint == 0)
            {
                if (m < 10)
                {
                    for (int i = 10; i <= 12; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(-1).Year, i, 1).ToString("yyyy") });
                    }
                    for (int i = 1; i <= m; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                    }
                }
                else
                {
                    for (int i = 10; i <= m; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                    }
                }
            }
            else
            {
                for (int i = 10; i <= 12; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(yearPoint).Year, i, 1).ToString("yyyy") });
                }
                for (int i = 1; i <= 9; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                }
            }

            ViewBag.Month = new SelectList(list, "Value", "Month", m);
            model.Month = m;
            model.yearPoint = yearPoint;

            return View(model);
        }

        [HttpPost, ActionName("HQScoreMonth")]
        [ValidateAntiForgeryToken]
        public IActionResult HQScoreMonth(ReportViewModel model)
        {
            int m;
            List<Object> list = new List<object>();

            if (DateTime.Now.Month == 10)
            {
                model.yearPoint = -1;
            }

            //บันทึกย้อนหลัง 1 เดือน
            if (DateTime.Now.Month == 1)
            {
                m = 12;
            }
            else
            {
                m = DateTime.Now.Month - 1;
            }

            //ดึงข้อมูล เฉพาะในปีงบประมาณ ต.ค. (10) ปีก่อน - ก.ย. (09) ปีปัจจุบัน
            if (model.yearPoint == 0)
            {
                if (m < 10)
                {
                    for (int i = 10; i <= 12; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(-1).Year, i, 1).ToString("yyyy") });
                    }
                    for (int i = 1; i <= m; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                    }
                }
                else
                {
                    for (int i = 10; i <= m; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                    }
                }
            }
            else
            {
                for (int i = 10; i <= 12; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(model.yearPoint).Year, i, 1).ToString("yyyy") });
                }
                for (int i = 1; i <= 9; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                }
            }

            ViewBag.Month = new SelectList(list, "Value", "Month", m);
            return View(model);
        }

        public IActionResult PAKScore()
        {
            var model = new ReportViewModel();
            int m;
            int yearPoint = 0;
            List<Object> list = new List<object>();

            if (DateTime.Now.Month == 10)
            {
                yearPoint = -1;
            }

            //บันทึกย้อนหลัง 1 เดือน
            if (DateTime.Now.Month == 1)
            {
                m = 12;
            }
            else
            {
                m = DateTime.Now.Month - 1;
            }

            //ดึงข้อมูล เฉพาะในปีงบประมาณ ต.ค. (10) ปีก่อน - ก.ย. (09) ปีปัจจุบัน
            if (yearPoint == 0)
            {
                if (m < 10)
                {
                    for (int i = 10; i <= 12; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(-1).Year, i, 1).ToString("yyyy") });
                    }
                    for (int i = 1; i <= m; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                    }
                }
                else
                {
                    for (int i = 10; i <= m; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                    }
                }
            }
            else
            {
                for (int i = 10; i <= 12; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(yearPoint).Year, i, 1).ToString("yyyy") });
                }
                for (int i = 1; i <= 9; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                }
            }

            ViewBag.Month = new SelectList(list, "Value", "Month", m);
            model.Month = m;
            model.yearPoint = yearPoint;

            return View(model);
        }

        [HttpPost, ActionName("PAKScoreMonth")]
        [ValidateAntiForgeryToken]
        public IActionResult PAKScoreMonth(ReportViewModel model)
        {
            int m;
            List<Object> list = new List<object>();

            if (DateTime.Now.Month == 10)
            {
                model.yearPoint = -1;
            }

            //บันทึกย้อนหลัง 1 เดือน
            if (DateTime.Now.Month == 1)
            {
                m = 12;
            }
            else
            {
                m = DateTime.Now.Month - 1;
            }

            //ดึงข้อมูล เฉพาะในปีงบประมาณ ต.ค. (10) ปีก่อน - ก.ย. (09) ปีปัจจุบัน
            if (model.yearPoint == 0)
            {
                if (m < 10)
                {
                    for (int i = 10; i <= 12; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(-1).Year, i, 1).ToString("yyyy") });
                    }
                    for (int i = 1; i <= m; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                    }
                }
                else
                {
                    for (int i = 10; i <= m; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                    }
                }
            }
            else
            {
                for (int i = 10; i <= 12; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(model.yearPoint).Year, i, 1).ToString("yyyy") });
                }
                for (int i = 1; i <= 9; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                }
            }

            ViewBag.Month = new SelectList(list, "Value", "Month", m);
            return View(model);
        }

        public IActionResult BKKScore()
        {
            var model = new ReportViewModel();
            int m;
            int yearPoint = 0;
            List<Object> list = new List<object>();

            if (DateTime.Now.Month == 10)
            {
                yearPoint = -1;
            }

            //บันทึกย้อนหลัง 1 เดือน
            if (DateTime.Now.Month == 1)
            {
                m = 12;
            }
            else
            {
                m = DateTime.Now.Month - 1;
            }

            //ดึงข้อมูล เฉพาะในปีงบประมาณ ต.ค. (10) ปีก่อน - ก.ย. (09) ปีปัจจุบัน
            if (yearPoint == 0)
            {
                if (m < 10)
                {
                    for (int i = 10; i <= 12; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(-1).Year, i, 1).ToString("yyyy") });
                    }
                    for (int i = 1; i <= m; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                    }
                }
                else
                {
                    for (int i = 10; i <= m; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                    }
                }
            }
            else
            {
                for (int i = 10; i <= 12; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(yearPoint).Year, i, 1).ToString("yyyy") });
                }
                for (int i = 1; i <= 9; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                }
            }

            ViewBag.Month = new SelectList(list, "Value", "Month", m);
            model.Month = m;
            model.yearPoint = yearPoint;

            return View(model);
        }

        [HttpPost, ActionName("BKKScoreMonth")]
        [ValidateAntiForgeryToken]
        public IActionResult BKKScore(ReportViewModel model)
        {
            int m;
            List<Object> list = new List<object>();

            if (DateTime.Now.Month == 10)
            {
                model.yearPoint = -1;
            }

            //บันทึกย้อนหลัง 1 เดือน
            if (DateTime.Now.Month == 1)
            {
                m = 12;
            }
            else
            {
                m = DateTime.Now.Month - 1;
            }

            //ดึงข้อมูล เฉพาะในปีงบประมาณ ต.ค. (10) ปีก่อน - ก.ย. (09) ปีปัจจุบัน
            if (model.yearPoint == 0)
            {
                if (m < 10)
                {
                    for (int i = 10; i <= 12; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(-1).Year, i, 1).ToString("yyyy") });
                    }
                    for (int i = 1; i <= m; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                    }
                }
                else
                {
                    for (int i = 10; i <= m; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                    }
                }
            }
            else
            {
                for (int i = 10; i <= 12; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(model.yearPoint).Year, i, 1).ToString("yyyy") });
                }
                for (int i = 1; i <= 9; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                }
            }

            ViewBag.Month = new SelectList(list, "Value", "Month", m);
            return View(model);
        }

        public IActionResult NBKKScore()
        {
            var model = new ReportViewModel();
            int m;
            int yearPoint = 0;
            List<Object> list = new List<object>();

            if (DateTime.Now.Month == 10)
            {
                yearPoint = -1;
            }

            //บันทึกย้อนหลัง 1 เดือน
            if (DateTime.Now.Month == 1)
            {
                m = 12;
            }
            else
            {
                m = DateTime.Now.Month - 1;
            }

            //ดึงข้อมูล เฉพาะในปีงบประมาณ ต.ค. (10) ปีก่อน - ก.ย. (09) ปีปัจจุบัน
            if (yearPoint == 0)
            {
                if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
                {
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
                if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
                {
                    for (int i = 10; i <= 12; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(yearPoint).Year, i, 1).ToString("yyyy") });
                    }
                    for (int i = 1; i <= 9; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(1 + yearPoint).Year, i, 1).ToString("yyyy") });
                    }
                }
                else
                {
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

            ViewBag.Month = new SelectList(list, "Value", "Month", m);
            model.Month = m;
            model.yearPoint = yearPoint;

            return View(model);
        }

        [HttpPost, ActionName("NBKKScoreMonth")]
        [ValidateAntiForgeryToken]
        public IActionResult NBKKScoreMonth(ReportViewModel model)
        {
            int m;
            List<Object> list = new List<object>();

            if (DateTime.Now.Month == 10)
            {
                model.yearPoint = -1;
            }

            //บันทึกย้อนหลัง 1 เดือน
            if (DateTime.Now.Month == 1)
            {
                m = 12;
            }
            else
            {
                m = DateTime.Now.Month - 1;
            }

            //ดึงข้อมูล เฉพาะในปีงบประมาณ ต.ค. (10) ปีก่อน - ก.ย. (09) ปีปัจจุบัน
            if (model.yearPoint == 0)
            {
                if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
                {
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
                if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
                {
                    for (int i = 10; i <= 12; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(model.yearPoint).Year, i, 1).ToString("yyyy") });
                    }
                    for (int i = 1; i <= 9; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(1 + model.yearPoint).Year, i, 1).ToString("yyyy") });
                    }
                }
                else
                {
                    //m = 9;
                    for (int i = 10; i <= 12; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(-1 + model.yearPoint).Year, i, 1).ToString("yyyy") });
                    }
                    for (int i = 1; i <= 9; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(model.yearPoint).Year, i, 1).ToString("yyyy") });
                    }
                }
            }

            ViewBag.Month = new SelectList(list, "Value", "Month", m);
            return View(model);
        }

        public IActionResult STScore()
        {
            var model = new ReportViewModel();
            int m;
            int yearPoint = 0;
            List<Object> list = new List<object>();

            if (DateTime.Now.Month == 10)
            {
                yearPoint = -1;
            }

            //บันทึกย้อนหลัง 1 เดือน
            if (DateTime.Now.Month == 1)
            {
                m = 12;
            }
            else
            {
                m = DateTime.Now.Month - 1;
            }

            //ดึงข้อมูล เฉพาะในปีงบประมาณ ต.ค. (10) ปีก่อน - ก.ย. (09) ปีปัจจุบัน
            if (yearPoint == 0)
            {
                if (m < 10)
                {
                    for (int i = 10; i <= 12; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(-1).Year, i, 1).ToString("yyyy") });
                    }
                    for (int i = 1; i <= m; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                    }
                }
                else
                {
                    for (int i = 10; i <= m; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(-1).Year, i, 1).ToString("yyyy") });
                    }
                }
            }
            else
            {
                for (int i = 10; i <= 12; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(-1 + yearPoint).Year, i, 1).ToString("yyyy") });
                }
                for (int i = 1; i <= 9; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(yearPoint).Year, i, 1).ToString("yyyy") });
                }
            }

            ViewBag.Month = new SelectList(list, "Value", "Month", m);
            model.Month = m;
            model.yearPoint = yearPoint;

            return View(model);
        }

        [HttpPost, ActionName("STScoreMonth")]
        [ValidateAntiForgeryToken]
        public IActionResult STScoreMonth(ReportViewModel model)
        {
            int m;
            List<Object> list = new List<object>();

            if (DateTime.Now.Month == 10)
            {
                model.yearPoint = -1;
            }

            //บันทึกย้อนหลัง 1 เดือน
            if (DateTime.Now.Month == 1)
            {
                m = 12;
            }
            else
            {
                m = DateTime.Now.Month - 1;
            }

            //ดึงข้อมูล เฉพาะในปีงบประมาณ ต.ค. (10) ปีก่อน - ก.ย. (09) ปีปัจจุบัน
            if (model.yearPoint == 0)
            {
                if (m < 10)
                {
                    for (int i = 10; i <= 12; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(-1).Year, i, 1).ToString("yyyy") });
                    }
                    for (int i = 1; i <= m; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                    }
                }
                else
                {
                    for (int i = 10; i <= m; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(-1).Year, i, 1).ToString("yyyy") });
                    }
                }
            }
            else
            {
                for (int i = 10; i <= 12; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(-1 + model.yearPoint).Year, i, 1).ToString("yyyy") });
                }
                for (int i = 1; i <= 9; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(model.yearPoint).Year, i, 1).ToString("yyyy") });
                }
            }

            ViewBag.Month = new SelectList(list, "Value", "Month", m);
            return View(model);
        }

        public IActionResult GetPakPlanB()
        {
            var model = new ReportViewModel();
            int m;
            int yearPoint = DateTime.Now.Year;

            if(DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
            {
                yearPoint += 1;
            }

            //บันทึกย้อนหลัง 1 เดือน
            if (DateTime.Now.Month == 1)
            {
                m = 12;
            }
            else
            {
                m = DateTime.Now.Month - 1;
            }

            List<object> years = new List<object>();
            for(int ii = 2019; ii <= yearPoint; ii++)
            {
                years.Add(new { value = ii, year = new DateTime(ii, 1, 1).ToString("yyyy") });
            }
            ViewBag.selectyear = new SelectList(years, "value", "year", yearPoint);


            List<Object> list = new List<object>();
            for (int i = 10; i <= 12; i++)
            {
                list.Add(new { Value = i, Month = new DateTime(yearPoint, i, 1).AddYears(-1).ToString("MMMM yyyy") });
            }
            for (int i = 1; i <= 9; i++)
            {
                list.Add(new { Value = i, Month = new DateTime(yearPoint, i, 1).ToString("MMMM yyyy") });
            }
            ViewBag.Month = new SelectList(list, "Value", "Month", m);
            model.Month = m;
            model.yearPoint = yearPoint;

            return View(model);
        }


        [HttpPost, ActionName("GetPakPlanBMonth")]
        [ValidateAntiForgeryToken]
        public IActionResult GetPakPlanBMonth(ReportViewModel model)
        {
            int m;
            int yearPoint = DateTime.Now.Year;
            int yearSelect = model.yearPoint;


            //บันทึกย้อนหลัง 1 เดือน
            if (DateTime.Now.Month == 1)
            {
                m = 12;
            }
            else
            {
                m = DateTime.Now.Month - 1;
            }

            List<object> years = new List<object>();
            for (int ii = 2019; ii <= yearPoint; ii++)
            {
                years.Add(new { value = ii, year = new DateTime(ii, 1, 1).ToString("yyyy") });
            }
            ViewBag.selectyear = new SelectList(years, "value", "year", yearSelect);


            List<Object> list = new List<object>();
            for (int i = 10; i <= 12; i++)
            {
                list.Add(new { Value = i, Month = new DateTime(yearSelect, i, 1).AddYears(-1).ToString("MMMM yyyy") });
            }
            for (int i = 1; i <= 9; i++)
            {
                list.Add(new { Value = i, Month = new DateTime(yearSelect, i, 1).ToString("MMMM yyyy") });
            }
            ViewBag.Month = new SelectList(list, "Value", "Month", m);
            model.Month = m;
            model.yearPoint = yearSelect;

            return View(model);
        }

        public async Task<IActionResult> CentralTarget(string officeCode, int yearPoint = 0, int month = 0)
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

            var years = await _context.PointOfEvaluations.Select(slt => new { value = slt.Year.Year, year = slt.Year.ToString("yyyy") }).Distinct().OrderBy(ob => ob.year).ToListAsync();
            if (!years.Any(yy => yy.value == DateTime.Now.AddYears(1).Year))
            {
                years.Add(new { value = DateTime.Now.AddYears(1).Year, year = DateTime.Now.AddYears(1).ToString("yyyy") });
            }
            ViewBag.Year = new SelectList(years, "value", "year", yearForQuery.Year);
            ViewBag.selectYear = yearForQuery.Year;


            List<object> listMonth = new List<object>();
            listMonth.Add(new { Value = 3, Month = new DateTime(yearForQuery.Year, 3, 1).ToString("MMMM") + " " + new DateTime(yearForQuery.Year, 3, 1).ToString("yyyy") });
            listMonth.Add(new { Value = 9, Month = new DateTime(yearForQuery.Year, 9, 1).ToString("MMMM") + " " + new DateTime(yearForQuery.Year, 9, 1).ToString("yyyy") });


            if (month == 0)
            {
                if (DateTime.Now.Month >= 4 || DateTime.Now.Month <= 9)
                {
                    month = 3;
                }
                else
                {
                    month = 9;
                }
            }
            ViewBag.Month = new SelectList(listMonth, "Value", "Month", month);
            ViewBag.selectMonth = month;


            if (officeCode == null)
            {
                officeCode = user.OfficeId;
            }

            if(User.IsInRole("Admin") || User.IsInRole("Special"))
            {
                ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000" && d.Code.Substring(0, 2) == "00"), "Code", "Name", officeCode);
            } else
            {
                var officeList = _context.Offices.Where(d => d.Code.Substring(0, 2) == "00" && d.Code == user.OfficeId).ToList();
                officeList.AddRange(_context.UserOffices.Where(ou => ou.UserName == user.UserName).Select(slt => slt.Office).ToList());
                ViewBag.OfficeCode = new SelectList(officeList, "Code", "Name", officeCode);
            }
            ViewBag.selectOffice = officeCode;

            return View();
        }
    }
}