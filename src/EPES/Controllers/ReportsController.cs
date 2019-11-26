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
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> OwnerScoreReport()
        {
            DateTime yearForQuery;
            if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
            {
                yearForQuery = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);
            }
            else
            {
                yearForQuery = new DateTime(DateTime.Now.Year, 1, 1);
            }

            int m;
            if (DateTime.Now.Month == 1)
            {
                m = 12;
            }
            else
            {
                m = DateTime.Now.Month - 1;
            }


            var user = await _userManager.GetUserAsync(User);
            var rmodel = new ReportViewModel();

            var selectoffice = user.OfficeId;

            rmodel.p = await _context.PointOfEvaluations.Where(p => p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Scores).OrderBy(p => p.Point).ToListAsync();

            //var rmodel = await (from mp in _context.PointOfEvaluations
            //                    join mo in _context.Offices on mp.OwnerOfficeId equals mo.Id
            //                    join msd in _context.ScoreDrafts on mo.Id equals msd.OfficeId
            //                    join ms in _context.Scores on mo.Id equals ms.OfficeId
            //                    join md in _context.DataForEvaluations on mp.Id equals md.PointOfEvaluationId
            //                    where md.Month == month
            //                    select new ReportViewModel { p = mp, Score = ms.ScoreValue, ScoreDraft = msd.ScoreValue }
            //                ).ToListAsync();

            List<Object> list = new List<object>();
            if (User.IsInRole("Admin"))
            {
                for (int i = 1; i <= 12; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") });
                }
            }
            else
            {
                if (DateTime.Now.Month - 1 < 10)
                {
                    for (int i = 10; i <= 12; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") });
                    }
                    for (int i = 1; i <= DateTime.Now.Month - 1; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") });
                    }
                }
                else
                {
                    for (int i = 10; i <= DateTime.Now.Month - 1; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") });
                    }
                }
            }
            if (User.IsInRole("Admin"))
            {

                ViewBag.OfficeCode = new SelectList(await _context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000").ToListAsync(), "Code", "Name", selectoffice);
            }
            else if (User.IsInRole("Manager"))
            {
                if (user.OfficeId.Substring(0, 3) == "000")
                {
                    ViewBag.OfficeCode = new SelectList(await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => new { Code = b.OwnerOffice.Code, Name = b.OwnerOffice.Name }).Distinct().ToListAsync(), "Code", "Name", selectoffice);
                }
                else
                {
                    ViewBag.OfficeCode = new SelectList(await _context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000").ToListAsync(), "Code", "Name", selectoffice);
                }
            }
            else
            {
                ViewBag.OfficeCode = new SelectList(await _context.Offices.Where(d => d.Code == user.OfficeId).ToListAsync(), "Code", "Name", selectoffice);
            }

            ViewBag.Month = new SelectList(list, "Value", "Month", m);
            ViewBag.selectoffice = selectoffice;
            rmodel.month = m;

            return View(rmodel);
        }

        [HttpPost, ActionName("OwnerScoreReport")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OwnerScoreReportPost(string selectoffice, int month, int yearPoint)
        {
            DateTime yearForQuery;
            if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
            {
                yearForQuery = new DateTime(DateTime.Now.AddYears(1 + yearPoint).Year, 1, 1);
            }
            else
            {
                yearForQuery = new DateTime(DateTime.Now.AddYears(yearPoint).Year, 1, 1);
            }

            int m;
            if (month == 0)
            {
                if (DateTime.Now.Month == 1)
                {
                    m = 12;
                }
                else
                {
                    m = DateTime.Now.Month - 1;
                }
            }
            else
            {
                m = month;
            }

            var user = await _userManager.GetUserAsync(User);
            var rmodel = new ReportViewModel();

            if (String.IsNullOrEmpty(selectoffice))
            {
                selectoffice = user.OfficeId;
            }

            rmodel.p = await _context.PointOfEvaluations.Where(p => p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Scores).OrderBy(p => p.Point).ToListAsync();

            //var rmodel = await (from mp in _context.PointOfEvaluations
            //                    join mo in _context.Offices on mp.OwnerOfficeId equals mo.Id
            //                    join msd in _context.ScoreDrafts on mo.Id equals msd.OfficeId
            //                    join ms in _context.Scores on mo.Id equals ms.OfficeId
            //                    join md in _context.DataForEvaluations on mp.Id equals md.PointOfEvaluationId
            //                    where md.Month == month
            //                    select new ReportViewModel { p = mp, Score = ms.ScoreValue, ScoreDraft = msd.ScoreValue }
            //                ).ToListAsync();

            List<Object> list = new List<object>();
            if (User.IsInRole("Admin"))
            {
                for (int i = 1; i <= 12; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") });
                }
            }
            else
            {
                if (DateTime.Now.Month - 1 < 10)
                {
                    for (int i = 10; i <= 12; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") });
                    }
                    for (int i = 1; i <= DateTime.Now.Month - 1; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") });
                    }
                }
                else
                {
                    for (int i = 10; i <= DateTime.Now.Month - 1; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") });
                    }
                }
            }

            if (User.IsInRole("Admin"))
            {

                ViewBag.OfficeCode = new SelectList(await _context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000").ToListAsync(), "Code", "Name", selectoffice);
            }
            else if (User.IsInRole("Manager"))
            {
                if (user.OfficeId.Substring(0, 3) == "000")
                {
                    ViewBag.OfficeCode = new SelectList(await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => new { Code = b.OwnerOffice.Code, Name = b.OwnerOffice.Name }).Distinct().ToListAsync(), "Code", "Name", selectoffice);
                }
                else
                {
                    ViewBag.OfficeCode = new SelectList(await _context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000").ToListAsync(), "Code", "Name", selectoffice);
                }
            }
            else
            {
                ViewBag.OfficeCode = new SelectList(await _context.Offices.Where(d => d.Code == user.OfficeId).ToListAsync(), "Code", "Name", selectoffice);
            }

            ViewBag.Month = new SelectList(list, "Value", "Month", m);
            ViewBag.selectoffice = selectoffice;
            rmodel.month = m;
            rmodel.yearPoint = yearPoint;

            return View(rmodel);
        }
    }
}