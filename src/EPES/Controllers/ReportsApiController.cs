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
        public async Task<IActionResult> GetAllScore(DataSourceLoadOptions loadOptions, int yearPoint = 0, int lastMonth = 0)
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

           /*
            var scoreDrafts = _context.ScoreDrafts.Include(s => s.Office)
                                                  .Where(sd => sd.PointOfEvaluation.Year == yearForQuery &&
                                                    sd.LastMonth == month &&
                                                    sd.Office.Code != "00000000" &&
                                                    sd.PointOfEvaluation.SubPoint == 0)
                                                    //.AsEnumerable()
                                                    //.GroupBy(x => new { x.OfficeId,x.PointOfEvaluation.Plan})
                                                    .Select(i => new
                                                    {
                                                        i.Id,
                                                        i.PointOfEvaluation.Plan,
                                                        i.Office.Name,
                                                        i.Office.OfficeGroup.NameGroup,
                                                        i.ScoreValue,
                                                        i.ScoreApprove,
                                                        i.LastMonth,
                                                        i.PointOfEvaluation.Year.Year,
                                                        cal = (i.PointOfEvaluation.Weight * i.ScoreValue / 100),
                                                        calApprove = (i.PointOfEvaluation.Weight * i.ScoreApprove / 100)
                                                    });
            var scoreSum = from s in scoreDrafts
                           group s by new { s.Name,s.Plan } into g
                           select new
                           {
                               Id = g.First().Id,
                               Plan = g.Key.Plan,
                               Name = g.Key.Name,
                               NameGroup = g.First().NameGroup,
                               ScoreValue = g.Sum(d => d.ScoreValue),
                               ScoreApprove = g.Sum(d => d.ScoreApprove),
                               LastMonth = g.First().LastMonth,
                               Year = g.First().Year,
                               cal = g.Sum(d => d.cal),
                               calApprove = g.Sum(d => d.calApprove)
                           };
            */
            
            var scoreDrafts = _context.ScoreDrafts.Include(s => s.Office)
                                                  .Where(sd => sd.PointOfEvaluation.Year == yearForQuery &&
                                                    sd.LastMonth == month &&
                                                    sd.Office.Code != "00000000" &&
                                                    sd.PointOfEvaluation.SubPoint == 0)
                                                    .GroupBy(x => new { x.PointOfEvaluation.Plan, x.Office.Code })
                                                    .Select(i => new
                                                    {

                                                        Id = i.First().Id,
                                                        Plan = i.Key.Plan,
                                                        Name = i.First().Office.Name,
                                                        NameGroup = i.First().Office.OfficeGroup.NameGroup,
                                                        ScoreValue = i.Sum(a => a.ScoreValue),
                                                        ScoreApprove = i.Sum(a => a.ScoreApprove),
                                                        i.First().LastMonth,
                                                        i.First().PointOfEvaluation.Year.Year,
                                                        cal = i.Sum(a => a.PointOfEvaluation.Weight * a.ScoreValue / 100),
                                                        calApprove = i.Sum(a => a.PointOfEvaluation.Weight * a.ScoreApprove / 100)
                                                    });
           
           
            return Json(await DataSourceLoader.LoadAsync(scoreDrafts, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> GetHQScore(DataSourceLoadOptions loadOptions, int yearPoint = 0, int lastMonth = 0)
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
            /*
            var scoreDrafts = _context.ScoreDrafts.Include(s => s.Office)
                                                  .Where(sd => sd.PointOfEvaluation.Year == yearForQuery &&
                                                    sd.LastMonth == month &&
                                                    sd.Office.Code != "00000000" &&
                                                    sd.Office.Code.Substring(0, 3) == "000" &&
                                                    sd.PointOfEvaluation.SubPoint == 0)
                                                    .Select(i => new
                                                    {
                                                        i.Id,
                                                        i.PointOfEvaluation.Plan,
                                                        i.Office.Name,
                                                        i.Office.OfficeGroup.NameGroup,
                                                        i.ScoreValue,
                                                        i.ScoreApprove,
                                                        i.LastMonth,
                                                        i.PointOfEvaluation.Year.Year
                                                    });
            */

            var scoreDrafts = _context.ScoreDrafts.Include(s => s.Office)
                .Where(sd => sd.PointOfEvaluation.Year == yearForQuery && sd.LastMonth == month & sd.Office.Code != "00000000" && sd.Office.Code.Substring(0, 3) == "000" && sd.PointOfEvaluation.SubPoint == 0 && sd.ScoreValue != 0)
                .GroupBy(x => new { x.Office.Code, x.PointOfEvaluation.Plan })
                .Select(ii => new
                {
                    ii.First().Id,
                    Plan = ii.Key.Plan,
                    Name = ii.First().Office.Name,
                    ii.First().Office.OfficeGroup.NameGroup,
                    ScoreValue = ii.Average(xx => xx.ScoreValue),
                    ScoreApprove = ii.Average(xx => xx.ScoreApprove),
                    ii.First().LastMonth,
                    ii.First().PointOfEvaluation.Year.Year
                });

            return Json(await DataSourceLoader.LoadAsync(scoreDrafts, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> GetPAKScore(DataSourceLoadOptions loadOptions, int yearPoint = 0, int lastMonth = 0)
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

            var scoreDrafts = _context.ScoreDrafts.Include(s => s.Office)
                                                  .Where(sd => sd.PointOfEvaluation.Year == yearForQuery &&
                                                    sd.LastMonth == month &&
                                                    sd.Office.Code != "00000000" &&
                                                    sd.Office.Code.Substring(2, 6) == "000000" &&
                                                    sd.PointOfEvaluation.SubPoint == 0)
                                                    .Select(i => new
                                                    {
                                                        i.Id,
                                                        i.PointOfEvaluation.Plan,
                                                        i.Office.Name,
                                                        i.Office.OfficeGroup.NameGroup,
                                                        i.ScoreValue,
                                                        i.ScoreApprove,
                                                        i.LastMonth,
                                                        i.PointOfEvaluation.Year.Year
                                                    });

            return Json(await DataSourceLoader.LoadAsync(scoreDrafts, loadOptions));
        }


        [HttpGet]
        public async Task<IActionResult> GetBKKScore(DataSourceLoadOptions loadOptions, int yearPoint = 0, int lastMonth = 0)
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

            var scoreDrafts = _context.ScoreDrafts.Include(s => s.Office)
                                                  .Where(sd => sd.PointOfEvaluation.Year == yearForQuery &&
                                                    sd.LastMonth == month &&
                                                    sd.Office.Code != "00000000" &&
                                                    sd.Office.Code.Substring(0, 3) != "000" &&
                                                    sd.Office.Code.Substring(2, 6) != "000000" &&
                                                    sd.Office.Code.Substring(5, 3) == "000" &&
                                                    (sd.Office.Code.Substring(0, 2) == "01" || sd.Office.Code.Substring(0, 2) == "02"  || sd.Office.Code.Substring(0, 2) == "03") &&
                                                    sd.PointOfEvaluation.SubPoint == 0)
                                                    .Select(i => new
                                                    {
                                                        i.Id,
                                                        i.PointOfEvaluation.Plan,
                                                        i.Office.Name,
                                                        i.Office.OfficeGroup.NameGroup,
                                                        i.ScoreValue,
                                                        i.ScoreApprove,
                                                        i.LastMonth,
                                                        i.PointOfEvaluation.Year.Year
                                                    });

            return Json(await DataSourceLoader.LoadAsync(scoreDrafts, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> GetNBKKScore(DataSourceLoadOptions loadOptions, int yearPoint = 0, int lastMonth = 0)
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

            var scoreDrafts = _context.ScoreDrafts.Include(s => s.Office)
                                                  .Where(sd => sd.PointOfEvaluation.Year == yearForQuery &&
                                                    sd.LastMonth == month &&
                                                    sd.Office.Code != "00000000" &&
                                                    sd.Office.Code.Substring(0, 3) != "000" &&
                                                    sd.Office.Code.Substring(2, 6) != "000000" &&
                                                    sd.Office.Code.Substring(5, 3) == "000" &&
                                                    sd.Office.Code.Substring(0, 2) != "01" && 
                                                    sd.Office.Code.Substring(0, 2) != "02" && 
                                                    sd.Office.Code.Substring(0, 2) != "03" &&
                                                    sd.PointOfEvaluation.SubPoint == 0)
                                                    .Select(i => new
                                                    {
                                                        i.Id,
                                                        i.PointOfEvaluation.Plan,
                                                        i.Office.Name,
                                                        i.Office.OfficeGroup.NameGroup,
                                                        i.ScoreValue,
                                                        i.ScoreApprove,
                                                        i.LastMonth,
                                                        i.PointOfEvaluation.Year.Year
                                                    });

            return Json(await DataSourceLoader.LoadAsync(scoreDrafts, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> GetSTScore(DataSourceLoadOptions loadOptions, int yearPoint = 0, int lastMonth = 0)
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

            var scoreDrafts = _context.ScoreDrafts.Include(s => s.Office)
                                                  .Where(sd => sd.PointOfEvaluation.Year == yearForQuery &&
                                                    sd.LastMonth == month &&
                                                    sd.Office.Code != "00000000" &&
                                                    sd.Office.Code.Substring(0, 3) != "000" &&
                                                    sd.Office.Code.Substring(2, 6) != "000000" &&
                                                    sd.Office.Code.Substring(5, 3) == "000" &&
                                                    sd.PointOfEvaluation.SubPoint == 0)
                                                    .Select(i => new
                                                    {
                                                        i.Id,
                                                        i.PointOfEvaluation.Plan,
                                                        i.Office.Name,
                                                        i.Office.OfficeGroup.NameGroup,
                                                        i.ScoreValue,
                                                        i.ScoreApprove,
                                                        i.LastMonth,
                                                        i.PointOfEvaluation.Year.Year
                                                    });

            return Json(await DataSourceLoader.LoadAsync(scoreDrafts, loadOptions));
        }

        public async Task<object> GetDBHQScore(DataSourceLoadOptions loadOptions, int yearPoint = 0, int lastMonth = 0)
        {
            DateTime yearForQuery;
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
                if (DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
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
                month = DateTime.Now.Month - 1;
            }
            else
            {
                month = lastMonth;
            }

            var avg = _context.ScoreDrafts.Include(s => s.Office).Where(sd => sd.PointOfEvaluation.Year == yearForQuery &&
                                                        sd.LastMonth == month &&
                                                        sd.Office.Code != "00000000" &&
                                                        sd.Office.Code.Substring(0, 3) == "000" &&
                                                        sd.PointOfEvaluation.SubPoint == 0)
                                             .Select(i => new
                                             {
                                                 i.ScoreValue
                                             });
            var avgAll = await avg.DefaultIfEmpty().AverageAsync(s => s.ScoreValue);

            var avgAB = await _context.ScoreDrafts.Include(s => s.Office).Where(sd => sd.PointOfEvaluation.Year == yearForQuery &&
                                                        sd.LastMonth == month &&
                                                        sd.Office.Code != "00000000" &&
                                                        sd.Office.Code.Substring(0, 3) == "000" &&
                                                        sd.PointOfEvaluation.SubPoint == 0 &&
                                                        (sd.PointOfEvaluation.Plan == TypeOfPlan.A || sd.PointOfEvaluation.Plan == TypeOfPlan.B))
                                             .Select(i => new
                                             {
                                                 i.ScoreValue
                                             }).DefaultIfEmpty().AverageAsync(s => s.ScoreValue);

            var avgC = await _context.ScoreDrafts.Include(s => s.Office).Where(sd => sd.PointOfEvaluation.Year == yearForQuery &&
                                                        sd.LastMonth == month &&
                                                        sd.Office.Code != "00000000" &&
                                                        sd.Office.Code.Substring(0, 3) == "000" &&
                                                        sd.PointOfEvaluation.SubPoint == 0 &&
                                                        sd.PointOfEvaluation.Plan == TypeOfPlan.C)
                                             .Select(i => new
                                             {
                                                 i.ScoreValue
                                             }).DefaultIfEmpty().AverageAsync(s => s.ScoreValue);

            var avgD = await _context.ScoreDrafts.Include(s => s.Office).Where(sd => sd.PointOfEvaluation.Year == yearForQuery &&
                                                        sd.LastMonth == month &&
                                                        sd.Office.Code != "00000000" &&
                                                        sd.Office.Code.Substring(0, 3) == "000" &&
                                                        sd.PointOfEvaluation.SubPoint == 0 &&
                                                        sd.PointOfEvaluation.Plan == TypeOfPlan.D)
                                             .Select(i => new
                                             {
                                                 i.ScoreValue
                                             }).DefaultIfEmpty().AverageAsync(s => s.ScoreValue);

            return new[] { new { office = "ภาพรวม", score = avgAll }, new { office = "แผนงานระดับนโยบาย (Top-Down : A และ B) น้ำหนัก 70 %", score = avgAB }, new { office = "แผนงานที่หน่วยงานกำหนดเอง (Bottom-Up : C) น้ำหนัก 15 %", score = avgC }, new { office = "แผนงานร่วม (Joint KPI : D) น้ำหนัก 15%", score = avgD } };

            //return Json(await DataSourceLoader.LoadAsync(hqScore.AsQueryable(), loadOptions));
        }

        public async Task<object> GetDBPScore(DataSourceLoadOptions loadOptions, int yearPoint = 0, int lastMonth = 0)
        {
            DateTime yearForQuery;
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
                if (DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
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
                month = DateTime.Now.Month - 1;
            }
            else
            {
                month = lastMonth;
            }

            var avgAll = await _context.ScoreDrafts.Where(sd => sd.PointOfEvaluation.Year == yearForQuery &&
                                                        sd.LastMonth == month &&
                                                        sd.Office.Code != "00000000" &&
                                                        sd.Office.Code.Substring(0, 3) != "000" &&
                                                        sd.Office.Code.Substring(5, 3) == "000" &&
                                                        sd.PointOfEvaluation.SubPoint == 0)
                                             .Select(i => new
                                             {
                                                 i.ScoreValue
                                             }).DefaultIfEmpty().AverageAsync(s => s.ScoreValue);

            var avgB = await _context.ScoreDrafts.Where(sd => sd.PointOfEvaluation.Year == yearForQuery &&
                                                        sd.LastMonth == month &&
                                                        sd.Office.Code != "00000000" &&
                                                        sd.Office.Code.Substring(0, 3) != "000" &&
                                                        sd.Office.Code.Substring(5, 3) == "000" &&
                                                        sd.PointOfEvaluation.SubPoint == 0 &&
                                                        (sd.PointOfEvaluation.Plan == TypeOfPlan.B))
                                             .Select(i => new
                                             {
                                                 i.ScoreValue
                                             }).DefaultIfEmpty().AverageAsync(s => s.ScoreValue);

            var avgC = await _context.ScoreDrafts.Where(sd => sd.PointOfEvaluation.Year == yearForQuery &&
                                                        sd.LastMonth == month &&
                                                        sd.Office.Code != "00000000" &&
                                                        sd.Office.Code.Substring(0, 3) != "000" &&
                                                        sd.Office.Code.Substring(5, 3) == "000" &&
                                                        sd.PointOfEvaluation.SubPoint == 0 &&
                                                        sd.PointOfEvaluation.Plan == TypeOfPlan.C)
                                             .Select(i => new
                                             {
                                                 i.ScoreValue
                                             }).DefaultIfEmpty().AverageAsync(s => s.ScoreValue);

            var avgD = await _context.ScoreDrafts.Where(sd => sd.PointOfEvaluation.Year == yearForQuery &&
                                                        sd.LastMonth == month &&
                                                        sd.Office.Code != "00000000" &&
                                                        sd.Office.Code.Substring(0, 3) != "000" &&
                                                        sd.Office.Code.Substring(5, 3) == "000" &&
                                                        sd.PointOfEvaluation.SubPoint == 0 &&
                                                        sd.PointOfEvaluation.Plan == TypeOfPlan.D)
                                             .Select(i => new
                                             {
                                                 i.ScoreValue
                                             }).DefaultIfEmpty().AverageAsync(s => s.ScoreValue);

            return new[] { new { office = "ภาพรวม", score = avgAll }, new { office = "แผนงานระดับนโยบาย (Top-Down : B) น้ำหนัก 70 %", score = avgB }, new { office = "แผนงานที่หน่วยงานกำหนดเอง (Bottom-Up : C) น้ำหนัก 15 %", score = avgC }, new { office = "แผนงานร่วม (Joint KPI : D) น้ำหนัก 15%", score = avgD } };

            //return Json(await DataSourceLoader.LoadAsync(hqScore.AsQueryable(), loadOptions));
        }

        public async Task<object> GetPakPlanB(DataSourceLoadOptions loadOptions, int yearPoint = 0, int lastMonth = 0)
        {

            DateTime yearForQuery;
            if (yearPoint == 0)
            {
                if (DateTime.Now.Month == 11 || DateTime.Now.Month == 12 || DateTime.Now.Month == 10)
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
                if (DateTime.Now.Month == 11 || DateTime.Now.Month == 12 || DateTime.Now.Month == 10)
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
                month = DateTime.Now.Month - 1;
            }
            else
            {
                month = lastMonth;
            }

            var scorePak = _context.ScoreDrafts
                .Include(so => so.Office)
                .Include(sp => sp.PointOfEvaluation)
                .Where(
                    sd => sd.PointOfEvaluation.Plan == TypeOfPlan.B &&
                    sd.PointOfEvaluation.SubPoint == 0 &&
                    sd.Office.Code.Substring(0, 2) != "00" &&
                    sd.Office.Code.Substring(2, 6) == "000000" &&
                    sd.LastMonth == month &&
                    sd.PointOfEvaluation.Year == yearForQuery
                ).OrderBy(ob => ob.PointOfEvaluation.Point)
                .Select(ii => new
                    {
                        Point = ii.PointOfEvaluation.Point,
                        PointName = ii.PointOfEvaluation.Name,
                        OfficeName = ii.Office.Name.Replace("สำนักงานสรรพากรภาค", "สภ."),
                        OfficeGroup = "สภ.",
                        ii.ScoreValue,
                        ii.ScoreApprove,
                        ii.PointOfEvaluation.Year.Year
                    }
                );
            var scoreDrafts = scorePak;
            /*
            var scorePakAvg = _context.ScoreDrafts.Include(so => so.Office).Include(sp => sp.PointOfEvaluation)
                .Where(sd =>
                    sd.PointOfEvaluation.Plan == TypeOfPlan.B &&
                    sd.PointOfEvaluation.SubPoint == 0 &&
                    sd.Office.Code.Substring(0, 2) != "00" &&
                    sd.Office.Code.Substring(2, 6) == "000000" &&
                    sd.LastMonth == month &&
                    sd.PointOfEvaluation.Year == yearForQuery
                ).GroupBy(oo => new { oo.PointOfEvaluation.Point })
                .Select(ii => new
                {
                    Point = ii.Key.Point,
                    PointName = ii.First().PointOfEvaluation.Name,
                    OfficeName = "ค่าเฉลี่ยของ สภ.",
                    ScoreValue = ii.Average(jj => jj.ScoreValue),
                    ScoreApprove = ii.Average(jj => jj.ScoreApprove),
                    ii.First().PointOfEvaluation.Year.Year
                }) ;

            var scoreDrafts = scorePak.Concat(scorePakAvg);
            */
            /*
            var scoreCenter = _context.ScoreDrafts
                .Include(so => so.Office)
                .Include(sp => sp.PointOfEvaluation)
                .Where(
                    sd => sd.PointOfEvaluation.Plan == TypeOfPlan.B &&
                    sd.PointOfEvaluation.SubPoint == 0 &&
                    (sd.Office.Code == "00009000" ||
                    sd.Office.Code == "00003000") &&
                    sd.LastMonth == month &&
                    sd.PointOfEvaluation.Year == yearForQuery
                ).OrderBy(ob => ob.PointOfEvaluation.Point)
                .Select(ii => new
                {
                    Point = ii.PointOfEvaluation.Point,
                    PointName = ii.PointOfEvaluation.Name,
                    OfficeName = ii.Office.Name.Replace("สำนักงานสรรพากรภาค", "สภ."),
                    ii.ScoreValue,
                    ii.ScoreApprove,
                    ii.PointOfEvaluation.Year.Year
                }
                );

            var scoreDrafts = (scoreCenter);
            */

            if(yearForQuery.Year == 2021)
            {
                var scorePY = _context.ScoreDrafts.Include(so => so.Office).Include(sp => sp.PointOfEvaluation)
                    .Where(sd =>
                    sd.PointOfEvaluation.Plan == TypeOfPlan.B &&
                    sd.PointOfEvaluation.SubPoint == 0 &&
                    (sd.Office.Code == "00009000" || sd.Office.Code == "00003000") &&
                    sd.LastMonth == month &&
                    sd.PointOfEvaluation.Year == yearForQuery
                    );

                /*
                foreach(var item in scorePY)
                {
                    if(item.Office.Code == "00009000") // ภญ.
                    {
                        item.Office.Name = "ภญ.";
                        if (item.PointOfEvaluation.Point == 2)
                        {
                            item.PointOfEvaluation.Point = 1;
                            item.PointOfEvaluation.Name = "ร้อยละของผลการจัดเก็บภาษีเทียบกับประมาณการ";
                        }
                        else if (item.PointOfEvaluation.Point == 3)
                        {
                            item.PointOfEvaluation.Name = "ร้อยละของการแนะนำและตรวจสอบภาษีอากรของส่วนแนะนำและตรวจสอบภาษีอากร (ส่วน นต.)";
                        }
                        else if (item.PointOfEvaluation.Point == 4)
                        {
                            item.PointOfEvaluation.Name = "ร้อยละของผู้ประกอบการที่ดำเนินการแนะนำและตรวจสอบภาษีอากรแล้วเสร็จ และมีภาษีชำระ";
                        }
                        else if (item.PointOfEvaluation.Point == 5)
                        {
                            item.PointOfEvaluation.Point = 19;
                        }
                        else if (item.PointOfEvaluation.Point == 6)
                        {
                            item.PointOfEvaluation.Point = 20;
                        }
                        else if (item.PointOfEvaluation.Point == 7)
                        {
                            item.PointOfEvaluation.Point = 5;
                            item.PointOfEvaluation.Name = "ระดับความสำเร็จของร้อยละเฉลี่ยถ่วงน้ำหนักในการสอบยันใบกำกับภาษี";
                        }
                        else if(item.PointOfEvaluation.Point == 8)
                        {
                            item.PointOfEvaluation.Point = 21;
                        }
                        else if(item.PointOfEvaluation.Point == 9)
                        {
                            item.PointOfEvaluation.Point = 8;
                            item.PointOfEvaluation.Name = "ร้อยละของผลการเร่งรัดหนี้ได้เม็ดเงินเปรียบเทียบกับประมาณการ";
                        }
                        else if(item.PointOfEvaluation.Point == 10)
                        {
                            item.PointOfEvaluation.Point = 9;
                            item.PointOfEvaluation.Name = "ร้อยละของผลการจำหน่ายหนี้ภาษีอากรเปรียบเทียบกับประมาณการ";
                        }
                        else if(item.PointOfEvaluation.Point == 11)
                        {
                            item.PointOfEvaluation.Point = 10;
                            item.PointOfEvaluation.Name = "ร้อยละของผลการตรวจสอบและบันทึกการรับใบแจ้งภาษีอากรบนระบบ DMS เปรียบเทียบกับจำนวนรวมของใบแจ้งภาษีอากรที่ยังไม่ได้รับใบแจ้งการประเมิน";
                        }
                        else if(item.PointOfEvaluation.Point == 12)
                        {
                            item.PointOfEvaluation.Point = 15;
                            item.PointOfEvaluation.Name = "ร้อยละของจำนวนแบบฯ ที่ยื่นผ่านอินเทอร์เน็ต(ยกเว้น ภ.ง.ด.90,91,94)เทียบกับประมาณการจำนวนแบบที่กำหนด";
                        }
                    } else if (item.Office.Code == "00003000") // ตส.
                    {
                        item.Office.Name = "ตส.";
                        if(item.PointOfEvaluation.Point == 4)
                        {
                            item.PointOfEvaluation.Name = "ร้อยละของการแนะนำและตรวจสอบภาษีอากรของส่วนแนะนำและตรวจสอบภาษีอากร (ส่วน นต.)";
                            item.PointOfEvaluation.Point = 3;
                        }
                        else if(item.PointOfEvaluation.Point == 5)
                        {
                            item.PointOfEvaluation.Point = 22;
                        }
                        else if(item.PointOfEvaluation.Point == 6)
                        {
                            item.PointOfEvaluation.Point = 23;
                        }
                    }
                }
                */
                var scoreCenter = scorePY.Select(ii => new
                {
                    Point = ii.Office.Code == "00009000" ? ii.PointOfEvaluation.Point == 2 ? 1 : ii.PointOfEvaluation.Point == 5 ? 19 : ii.PointOfEvaluation.Point == 6 ? 20 : ii.PointOfEvaluation.Point == 7 ? 5 : ii.PointOfEvaluation.Point == 8 ? 21 : ii.PointOfEvaluation.Point == 9 ? 8 : ii.PointOfEvaluation.Point == 10 ? 9 : ii.PointOfEvaluation.Point == 11 ? 10 : ii.PointOfEvaluation.Point == 12 ? 15 : ii.PointOfEvaluation.Point : ii.PointOfEvaluation.Point == 4 ? 3 : ii.PointOfEvaluation.Point == 5 ? 22 : ii.PointOfEvaluation.Point == 6 ? 23 : ii.PointOfEvaluation.Point,
                    PointName = ii.Office.Code == "00009000" ? ii.PointOfEvaluation.Point == 2 ? "ร้อยละของผลการจัดเก็บภาษีเทียบกับประมาณการ" : ii.PointOfEvaluation.Point == 3 ? "ร้อยละของการแนะนำและตรวจสอบภาษีอากรของส่วนแนะนำและตรวจสอบภาษีอากร (ส่วน นต.)" : ii.PointOfEvaluation.Point == 4 ? "ร้อยละของผู้ประกอบการที่ดำเนินการแนะนำและตรวจสอบภาษีอากรแล้วเสร็จ และมีภาษีชำระ " : ii.PointOfEvaluation.Point == 7 ? "ระดับความสำเร็จของร้อยละเฉลี่ยถ่วงน้ำหนักในการสอบยันใบกำกับภาษี" : ii.PointOfEvaluation.Point == 9 ? "ร้อยละของผลการเร่งรัดหนี้ได้เม็ดเงินเปรียบเทียบกับประมาณการ" : ii.PointOfEvaluation.Point == 10 ? "ร้อยละของผลการจำหน่ายหนี้ภาษีอากรเปรียบเทียบกับประมาณการ" : ii.PointOfEvaluation.Point == 11 ? "ร้อยละของผลการตรวจสอบและบันทึกการรับใบแจ้งภาษีอากรบนระบบ DMS เปรียบเทียบกับจำนวนรวมของใบแจ้งภาษีอากรที่ยังไม่ได้รับใบแจ้งการประเมิน" : ii.PointOfEvaluation.Point == 12 ? "ร้อยละของจำนวนแบบฯ ที่ยื่นผ่านอินเทอร์เน็ต(ยกเว้น ภ.ง.ด.90,91,94)เทียบกับประมาณการจำนวนแบบที่กำหนด" : ii.PointOfEvaluation.Name : ii.PointOfEvaluation.Point == 4 ? "ร้อยละของการแนะนำและตรวจสอบภาษีอากรของส่วนแนะนำและตรวจสอบภาษีอากร (ส่วน นต.)" : ii.PointOfEvaluation.Name,
                    OfficeName = ii.Office.Code == "00009000" ? "ภญ." : "ตส.",
                    OfficeGroup = "ส่วนกลาง",
                    ii.ScoreValue,
                    ii.ScoreApprove,
                    ii.PointOfEvaluation.Year.Year
                });
                scoreDrafts = scoreDrafts.Concat(scoreCenter);
            } else if(yearForQuery.Year == 2022)
            {

                var scorePY = _context.ScoreDrafts.Include(so => so.Office).Include(sp => sp.PointOfEvaluation)
                    .Where(sd =>
                    sd.PointOfEvaluation.Plan == TypeOfPlan.B &&
                    sd.PointOfEvaluation.SubPoint == 0 &&
                    (sd.Office.Code == "00009000" || sd.Office.Code == "00003000") &&
                    sd.LastMonth == month &&
                    sd.PointOfEvaluation.Year == yearForQuery
                    );
                var scoreCenter = scorePY.Select(ii => new
                {
                    Point = ii.Office.Code == "00009000" ? ii.PointOfEvaluation.Point == 2 ? 1 : ii.PointOfEvaluation.Point == 3 ? 2 : ii.PointOfEvaluation.Point == 4 ? 3 : ii.PointOfEvaluation.Point == 5 ? 4 : ii.PointOfEvaluation.Point == 6 ? 17 : ii.PointOfEvaluation.Point == 9 ? 13 : ii.PointOfEvaluation.Point == 10 ? 14 : ii.PointOfEvaluation.Point == 11 ? 12 : ii.PointOfEvaluation.Point : ii.PointOfEvaluation.Point == 4 ? 2 : ii.PointOfEvaluation.Point == 5 ? 18 : ii.PointOfEvaluation.Point == 6 ? 19 : ii.PointOfEvaluation.Point == 7 ? 12 : ii.PointOfEvaluation.Point ,
                    PointName = ii.Office.Code == "00009000" ? ii.PointOfEvaluation.Point == 2 ? "ร้อยละของผลการจัดเก็บภาษีเทียบกับประมาณการ" : ii.PointOfEvaluation.Point == 3 ? "ร้อยละของการแนะนำ ตรวจสอบ และการตรวจคืนภาษีอากรผู้ประกอบการ" : ii.PointOfEvaluation.Point == 4 ? "ร้อยละของผู้ประกอบการที่ดำเนินการแนะนำและตรวจสอบภาษีอากรแล้วเสร็จ และมีภาษีชำระ" : ii.PointOfEvaluation.Point == 5 ? "ระดับความสำเร็จของร้อยละเฉลี่ยถ่วงน้ำหนักในการสอบยันใบกำกับภาษี" : ii.PointOfEvaluation.Point == 7 ? "ร้อยละของผลการเร่งรัดหนี้ได้เม็ดเงินเปรียบเทียบกับประมาณการ" : ii.PointOfEvaluation.Point == 8 ? "ร้อยละของผลการจำหน่ายหนี้ภาษีอากรเปรียบเทียบกับประมาณการ" : ii.PointOfEvaluation.Point == 9 ? "ร้อยละของจำนวนแบบฯ ที่ยื่นผ่านอินเทอร์เน็ตเทียบกับจำนวนแบบฯ ทั้งหมด " : ii.PointOfEvaluation.Point == 10 ? "ร้อยละของจำนวนแบบฯ ภ.ง.ด.1 ภ.พ.30 ภ.ง.ด.50 และ ภ.ง.ด.53  ที่ยื่นผ่านอินเทอร์เน็ต เทียบกับจำนวนแบบฯ ภ.ง.ด.1, ภ.พ.30, ภ.ง.ด.50 และ ภ.ง.ด.53 ทั้งหมด" : ii.PointOfEvaluation.Name : ii.PointOfEvaluation.Point == 4 ? "ร้อยละของการแนะนำ ตรวจสอบ และการตรวจคืนภาษีอากรผู้ประกอบการ" :ii.PointOfEvaluation.Name,
                    OfficeName = ii.Office.Code == "00009000" ? "ภญ." : "ตส.",
                    OfficeGroup = "ส่วนกลาง",
                    ii.ScoreValue,
                    ii.ScoreApprove,
                    ii.PointOfEvaluation.Year.Year
                }) ;
                scoreDrafts = scoreDrafts.Concat(scoreCenter);
            }

            return Json(await DataSourceLoader.LoadAsync(scoreDrafts, loadOptions));
        }

        public async Task<object> CentralTarget(DataSourceLoadOptions loadOptions, int yearPoint = 0, int lastMonth = 0, string officeCode = null)
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


            int[] monthRound;
            if (lastMonth == 0)
            {
                if (DateTime.Now.Month >= 4 || DateTime.Now.Month <= 9)
                {
                    lastMonth = 3;
                }
                else
                {
                    lastMonth = 9;
                }
            }

            if (lastMonth == 3)
            {
                monthRound = new int[] { 10, 11, 12, 1, 2, 3 };
            }
            else
            {
                monthRound = new int[] { 4, 5, 6, 7, 8, 9 };
            }

            DateTime finishedDate = new DateTime(yearForQuery.Year, lastMonth, DateTime.DaysInMonth(yearForQuery.Year, lastMonth));

            if (officeCode == null)
            {
                var user = await _userManager.GetUserAsync(User);
                officeCode = user.OfficeId;
            }


            var report2 = _context.PointOfEvaluations.Include(ofc => ofc.OwnerOffice).Include(iss => iss.IssueForEvaluations).Include(sd => sd.ScoreDrafts).Include(rd => rd.Rounds).Include(dv => dv.DataForEvaluations)
                .Where(pv =>
                pv.Year == yearForQuery &&
                pv.OwnerOffice.Code == officeCode &&
                pv.SubPoint == 0
                ).OrderBy(ob => ob.Point);

            DateTime? completedDate;
            List<DateTime?> startDateList = new List<DateTime?>();
            List<DateTime?> endDateList = new List<DateTime?>();
            foreach (var data in report2)
            {
                completedDate = null;
                startDateList = new List<DateTime?>();
                endDateList = new List<DateTime?>();
                foreach (var issue in data.IssueForEvaluations)
                {
                    if(issue.Month == lastMonth)
                    {
                        data.Issue = issue.Issue;
                    }
                }

                foreach(var ssd in data.ScoreDrafts)
                {
                    if(ssd.LastMonth == lastMonth)
                    {
                        data.ScoreTarget = ssd.ScoreValue;
                        break;
                    }
                }

                if(data.Unit == UnitOfPoint.ร้อยละ || data.Unit == UnitOfPoint.ระดับ_ร้อยละ || (data.Unit == UnitOfPoint.ระดับ && lastMonth == 9))
                {
                    if(data.ScoreTarget == 5)
                    {
                        data.Target = 100;
                    } else
                    {
                        data.Target = 0;
                    }
                } else // ระดับ
                {
                    foreach(var dv in data.DataForEvaluations)
                    {
                        if(dv.Month == lastMonth)
                        {
                            completedDate = dv.CompletedDate;
                            break;
                        }
                    }

                    foreach(var rd in data.Rounds)
                    {
                        if(rd.RoundNumber == 1)
                        {
                            startDateList.Add(rd.Rate1MonthStart);
                            startDateList.Add(rd.Rate2MonthStart);
                            startDateList.Add(rd.Rate3MonthStart);
                            startDateList.Add(rd.Rate4MonthStart);
                            startDateList.Add(rd.Rate5MonthStart);

                            endDateList.Add(rd.Rate1MonthStop);
                            endDateList.Add(rd.Rate2MonthStop);
                            endDateList.Add(rd.Rate3MonthStop);
                            endDateList.Add(rd.Rate4MonthStop);
                            endDateList.Add(rd.Rate5MonthStop);
                        
                            if (rd.Rate1MonthStart <= finishedDate && rd.Rate1MonthStop >= finishedDate)
                            {
                                data.Target = 1;
                            }
                            else if (rd.Rate2MonthStart <= finishedDate && rd.Rate2MonthStop >= finishedDate)
                            {
                                data.Target = 2;
                            }
                            else if (rd.Rate3MonthStart <= finishedDate && rd.Rate3MonthStop >= finishedDate)
                            {
                                data.Target = 3;
                            }
                            else if (rd.Rate4MonthStart <= finishedDate && rd.Rate4MonthStop >= finishedDate)
                            {
                                data.Target = 4;
                            }
                            else if (rd.Rate5MonthStart <= finishedDate && rd.Rate5MonthStop >= finishedDate)
                            {
                                data.Target = 5;
                            } else
                            {
                                data.Target = 0;
                            }
                        }
                    }

                    if(data.ScoreTarget > data.Target)
                    {
                        data.Target = 100;
                    } else if(data.ScoreTarget == data.Target)
                    {
                        if(data.Target == 0)
                        {
                            data.Target = 0;
                        } else
                        {
                            if(startDateList[(int)data.Target - 1] <= completedDate && endDateList[(int)data.Target - 1] >= completedDate)
                            {
                                data.Target = 100;
                            } else
                            {
                                data.Target = 0;
                            }
                        }
                    } 
                    else if(data.Target == 0)
                    {
                        for(var ii = 1; ii < endDateList.Count; ii++)
                        {
                            if(completedDate <= endDateList[ii] && completedDate >= endDateList[ii - 1])
                            {
                                data.Target = ii - 1;
                                break;
                            }
                        }

                        if(data.Target < data.ScoreTarget)
                        {
                            data.Target = 100;
                        } else if(data.Target == data.ScoreTarget)
                        {
                            if (startDateList[(int)data.Target] <= completedDate && endDateList[(int)data.Target] >= completedDate)
                            {
                                data.Target = 100;
                            }
                            else
                            {
                                data.Target = 0;
                            }
                        } else
                        {
                            data.Target = 0;
                        }
                    }
                    else
                    {
                        data.Target = 0;
                    }
                }

            }

            var point = report2.Select(slt => new
            {
                officeName = slt.OwnerOffice.Name,
                officeCode = slt.OwnerOffice.Code,
                unit = slt.Unit,
                hasSub = slt.HasSub,
                point = slt.Point,
                expectPlan = ((ExpectPlanRD)slt.ExpectPlan).ToString(),
                ddrive = ((DdriveRD)slt.Ddrive).ToString(),
                detailPlan = slt.DetailPlan,
                target = slt.ScoreTarget,
                isTarget = slt.Target >= 100 ? "✔" : "",
                isNotTarget = slt.Target >= 100 ? "" : "✔",
                issue = slt.Issue
            });


            return Json(await DataSourceLoader.LoadAsync(point, loadOptions));
        }
    }
}