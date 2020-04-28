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
            var user = await _userManager.GetUserAsync(User);
            ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Code", "Name", user.OfficeId);
            return View();
        }

        [HttpPost, ActionName("SubScoreReport")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubScoreReport(string selectoffice, int yearPoint)
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);

            var of = await _context.Offices.Where(d => d.Code == selectoffice).FirstOrDefaultAsync();
            ViewBag.selectoffice = selectoffice;
            ViewBag.OfficeName = of.Name;
            ViewBag.yearPoint = yearPoint;
            return View();
        }

        public IActionResult AllScore()
        {
            return View();
        }

        public IActionResult HQScore()
        {
            return View();
        }

        public IActionResult PAKScore()
        {
            return View();
        }

        public IActionResult BKKScore()
        {
            return View();
        }

        public IActionResult NBKKScore()
        {
            return View();
        }

        public IActionResult STScore()
        {
            return View();
        }
    }
}