using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPES.Data;
using EPES.Models;
using EPES.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> OwnerScoreReport(string selectoffice, int month = 0, int yearPoint = 0)
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

            rmodel.p = await _context.PointOfEvaluations.Where(p => p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.ScoreDrafts).Include(p => p.Scores).OrderBy(p => p.Point).ToListAsync();

            //var rmodel = await (from mp in _context.PointOfEvaluations
            //                    join mo in _context.Offices on mp.OwnerOfficeId equals mo.Id
            //                    join msd in _context.ScoreDrafts on mo.Id equals msd.OfficeId
            //                    join ms in _context.Scores on mo.Id equals ms.OfficeId
            //                    join md in _context.DataForEvaluations on mp.Id equals md.PointOfEvaluationId
            //                    where md.Month == month
            //                    select new ReportViewModel { p = mp, Score = ms.ScoreValue, ScoreDraft = msd.ScoreValue }
            //                ).ToListAsync();

            List<Object> list = new List<object>();
            if (m < 10)
            {
                for (int i = 10; i <= 12; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") });
                }
                for (int i = 1; i <= m; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") });
                }
            }
            else
            {
                for (int i = 10; i <= m; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") });
                }
            }

            ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", user.OfficeId);

            ViewBag.Month = new SelectList(list, "Value", "Month", m);
            ViewBag.selectoffice = selectoffice;
            rmodel.month = m;
            rmodel.yearPoint = yearPoint;

            rmodel.month = month;
            return View(rmodel);
        }

        [HttpPost, ActionName("OwnerScoreReport")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OwnerScoreReportPost(string selectoffice, int month = 0, int yearPoint = 0)
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
                rmodel.p = await _context.PointOfEvaluations.Where(p => p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.ScoreDrafts).Include(p => p.Scores).OrderBy(p => p.Point).ToListAsync();
            }
            else
            {
                rmodel.p = await _context.PointOfEvaluations.Where(p => p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.ScoreDrafts).Include(p => p.Scores).OrderBy(p => p.Point).ToListAsync();
            }

            //var rmodel = await (from mp in _context.PointOfEvaluations
            //                    join mo in _context.Offices on mp.OwnerOfficeId equals mo.Id
            //                    join msd in _context.ScoreDrafts on mo.Id equals msd.OfficeId
            //                    join ms in _context.Scores on mo.Id equals ms.OfficeId
            //                    join md in _context.DataForEvaluations on mp.Id equals md.PointOfEvaluationId
            //                    where md.Month == month
            //                    select new ReportViewModel { p = mp, Score = ms.ScoreValue, ScoreDraft = msd.ScoreValue }
            //                ).ToListAsync();

            List<Object> list = new List<object>();
            if (m < 10)
            {
                for (int i = 10; i <= 12; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") });
                }
                for (int i = 1; i <= m; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") });
                }
            }
            else
            {
                for (int i = 10; i <= m; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") });
                }
            }

            ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", user.OfficeId);

            ViewBag.Month = new SelectList(list, "Value", "Month", m);
            ViewBag.selectoffice = selectoffice;
            rmodel.month = m;
            rmodel.yearPoint = yearPoint;

            rmodel.month = month;
            return View(rmodel);
        }
    }
}