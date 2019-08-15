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

            var month = DateTime.Now.Month - 1;

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
            rmodel.month = month;
            return View(rmodel);
        }
    }
}