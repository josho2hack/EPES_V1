using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using EPES.Data;
using EPES.Models;
using EPES.ViewModels;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EPES.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportsApiController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportsApiController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetHQScore(DataSourceLoadOptions loadOptions, int yearPoint = 0,int lastMonth = 0)
        {
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
                if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
                {
                    yearForQuery = new DateTime(DateTime.Now.AddYears(1 + yearPoint).Year, 1, 1);
                }
                else
                {
                    yearForQuery = new DateTime(DateTime.Now.AddYears(yearPoint).Year, 1, 1);
                }
            }

            int month;
            if (lastMonth == 0)
            {
                month = DateTime.Now.Month;
            }
            else
            {
                month = lastMonth;
            }

            var user = await _userManager.GetUserAsync(User);

            var scoretemp = _context.ScoreDrafts.Where(sd => sd.PointOfEvaluation.Year == yearForQuery &&
                                                    sd.LastMonth == month &&
                                                    sd.Office.Code.Substring(0, 3) != "00000000" &&
                                                    !sd.PointOfEvaluation.HasSub);


            var scoreDrafts = scoretemp
                                                    .Select(i => new {
                                                    i.Id,
                                                    i.PointOfEvaluation.Plan,
                                                    //i.PointOfEvaluation.Point,
                                                    //i.PointOfEvaluation.SubPoint,
                                                    //i.PointOfEvaluation.Name,
                                                    i.Office.Code,
                                                    i.Office.Name,
                                                    i.ScoreValue,
                                                    i.ScoreApprove,
                                                    i.LastMonth,
                                                    i.PointOfEvaluation.Year.Year
                                                    });

            return Json(await DataSourceLoader.LoadAsync(scoreDrafts, loadOptions));
        }
    }
}