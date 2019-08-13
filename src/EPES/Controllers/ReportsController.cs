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
                yearForQuery = new DateTime(DateTime.Now.AddYears(-1).Year, 1, 1);
            }
            else
            {
                yearForQuery = new DateTime(DateTime.Now.Year, 1, 1);
            }

            var month = DateTime.Now.Month - 1;

            var user = await _userManager.GetUserAsync(User);
            //var viewModel = new PointOfEvaluationViewModel();

            //viewModel.PointList = await _context.PointOfEvaluations.Where(p => p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Scores).OrderBy(p => p.Point).ToListAsync();

            var pList = await (from p in _context.PointOfEvaluations
                               join o in _context.Offices on p.OwnerOfficeId equals o.Id
                               join sd in _context.ScoreDrafts on o.Id equals sd.OfficeId
                               join s in _context.Scores on o.Id equals s.OfficeId
                               join d in _context.DataForEvaluations on p.Id equals d.PointOfEvaluationId
                               where d.Month == month
                               select new {p,o,sd,s,d}
                               ).ToListAsync();
            return View(pList);
        }
    }
}