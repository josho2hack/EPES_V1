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

        public IActionResult OwnerScoreReport()
        {
            return View();
        }

        [HttpPost, ActionName("OwnerScoreReport")]
        [ValidateAntiForgeryToken]
        public IActionResult OwnerScoreReportPost(string selectoffice, int month, int yearPoint)
        {
            return View();
        }

        public IActionResult HQScore()
        {
            return View();
        }
    }
}