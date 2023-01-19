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
                //month = DateTime.Now.Month - 1;
                month = DateTime.Now.AddMonths(-1).Month;
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

            /*
            -- year < 2023
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
            */

            /* year >= 2023 */
            var avgFlagshipCascade = await _context.ScoreDrafts.Include(ofc => ofc.Office).Where(
                sd => sd.PointOfEvaluation.Year == yearForQuery &&
                sd.LastMonth == month &&
                sd.Office.Code != "00000000" &&
                sd.Office.Code.Substring(0, 2) == "00" &&
                sd.PointOfEvaluation.SubPoint == 0 &&
                (sd.PointOfEvaluation.Plan == TypeOfPlan.Flagship || sd.PointOfEvaluation.Plan == TypeOfPlan.Cascade)
                ).Select(slt => new { slt.ScoreValue }).DefaultIfEmpty().AverageAsync(s => s.ScoreValue);

            var avgJointKpi = await _context.ScoreDrafts.Include(ofc => ofc.Office).Where(sd =>
                sd.PointOfEvaluation.Year == yearForQuery &&
                sd.LastMonth == month &&
                sd.Office.Code != "00000000" &&
                sd.Office.Code.Substring(0, 2) == "00" &&
                sd.PointOfEvaluation.SubPoint == 0 &&
                sd.PointOfEvaluation.Plan == TypeOfPlan.Joint_KPI
                ).Select(slt => new { slt.ScoreValue }).DefaultIfEmpty().AverageAsync(s => s.ScoreValue);

            return new[] { new { office = "ภาพรวม", score = avgAll }, new { office = "แผนงานระดับนโยบายสำคัญ (Flagship) และแผนงานกำกับแผนงานระดับนโยบายที่ถ่ายทอดไปยังหน่วยปฏิบัติ (Cascade) น้ำหนัก 90 %", score = avgFlagshipCascade }, new { office = "ตัวชี้วัดร่วม (Joint KPI) น้ำหนัก 10 %", score = avgJointKpi } };

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
                //month = DateTime.Now.Month - 1;
                month = DateTime.Now.AddMonths(-1).Month;
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
            /*
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
            */

            var avgFlagship = await _context.ScoreDrafts.Include(ofc => ofc.Office).Where(
                sd => sd.PointOfEvaluation.Year == yearForQuery &&
                sd.LastMonth == month &&
                sd.Office.Code != "00000000" &&
                sd.Office.Code.Substring(0, 2) != "00" &&
                sd.Office.Code.Substring(5, 3) == "000" &&
                sd.PointOfEvaluation.SubPoint == 0 &&
                (sd.PointOfEvaluation.Plan == TypeOfPlan.Flagship)
                ).Select(slt => new { slt.ScoreValue }).DefaultIfEmpty().AverageAsync(s => s.ScoreValue);

            var avgCascade = await _context.ScoreDrafts.Include(ofc => ofc.Office).Where(
                sd => sd.PointOfEvaluation.Year == yearForQuery &&
                sd.LastMonth == month &&
                sd.Office.Code != "00000000" &&
                sd.Office.Code.Substring(0, 2) != "00" &&
                sd.Office.Code.Substring(5, 3) == "000" &&
                sd.PointOfEvaluation.SubPoint == 0 &&
                (sd.PointOfEvaluation.Plan == TypeOfPlan.Cascade)
                ).Select(slt => new { slt.ScoreValue }).DefaultIfEmpty().AverageAsync(s => s.ScoreValue);

            var avgJointKpi = await _context.ScoreDrafts.Include(ofc => ofc.Office).Where(sd =>
                sd.PointOfEvaluation.Year == yearForQuery &&
                sd.LastMonth == month &&
                sd.Office.Code != "00000000" &&
                sd.Office.Code.Substring(0, 2) != "00" &&
                sd.Office.Code.Substring(5, 3) == "000" &&
                sd.PointOfEvaluation.SubPoint == 0 &&
                sd.PointOfEvaluation.Plan == TypeOfPlan.Joint_KPI
                ).Select(slt => new { slt.ScoreValue }).DefaultIfEmpty().AverageAsync(s => s.ScoreValue);

            return new[] { new { office = "ภาพรวม", score = avgAll }, new { office = "แผนงานระดับนโยบายสำคัญ (Flagship) น้ำหนัก 10 %", score = avgFlagship }, new { office = "แผนงานระดับนโยบายที่ถ่ายทอดไปยังหน่วยปฏิบัติ (Cascade) น้ำหนัก 80 %", score = avgCascade }, new { office = "ตัวชี้วัดร่วม (Joint KPI) น้ำหนัก 10%", score = avgJointKpi } };

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
                yearForQuery = new DateTime(yearPoint, 1, 1);
            }

            int month;
            if (lastMonth == 0)
            {
                if(DateTime.Now.Month == 1)
                {
                    month = 12;
                } else
                {
                    month = DateTime.Now.Month - 1;
                }
            }
            else
            {
                month = lastMonth;
            }

            if(yearForQuery.Year >= 2023)
            {
                var scorePak = _context.ScoreDrafts
                    .Include(so => so.Office)
                    .Include(sp => sp.PointOfEvaluation)
                    .Where(
                        sd => sd.PointOfEvaluation.Plan == TypeOfPlan.Cascade &&
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

                var scorePY = _context.ScoreDrafts.Include(so => so.Office).Include(sp => sp.PointOfEvaluation)
                    .Where(sd =>
                    sd.PointOfEvaluation.Plan == TypeOfPlan.Cascade &&
                    sd.PointOfEvaluation.SubPoint == 0 &&
                    (sd.Office.Code == "00009000" || sd.Office.Code == "00003000") &&
                    sd.LastMonth == month &&
                    sd.PointOfEvaluation.Year == yearForQuery
                    );

                var scoreCenter = scorePY.Select(ii => new
                {
                    Point = ii.Office.Code == "00009000" ? ii.PointOfEvaluation.Point == 4 ? 5 : ii.PointOfEvaluation.Point == 5 ? 6 : ii.PointOfEvaluation.Point == 8 ? 10 : ii.PointOfEvaluation.Point == 9 ? 11 : ii.PointOfEvaluation.Point == 12 ? 15 : ii.PointOfEvaluation.Point == 10 ? 16 : ii.PointOfEvaluation.Point == 11 ? 17 : ii.PointOfEvaluation.Point == 7 ? 20 : ii.PointOfEvaluation.Point == 6 ? 7 : ii.PointOfEvaluation.Point : ii.PointOfEvaluation.Point == 4 ? 5 : ii.PointOfEvaluation.Point == 5 ? 21 : ii.PointOfEvaluation.Point == 6 ? 22 : ii.PointOfEvaluation.Point == 7 ? 15 : ii.PointOfEvaluation.Point,
                    PointName = ii.Office.Code == "00009000" ? ii.PointOfEvaluation.Point == 3 ? "ร้อยละของผลการจัดเก็บภาษีเทียบกับเป้าหมายที่กำหนด" : ii.PointOfEvaluation.Point == 4 ? "ร้อยละของการแนะนำ ตรวจสอบ และการตรวจคืนภาษีอากรผู้ประกอบการ" : ii.PointOfEvaluation.Point == 5 ? "แผนเพิ่มประสิทธิภาพการบริหารจัดเก็บภาษีอากรจากการแนะนำและตรวจสอบภาษีอากร" : ii.PointOfEvaluation.Point == 6 ? "ระดับความสำเร็จของร้อยละเฉลี่ยถ่วงน้ำหนักในการสอบยันใบกำกับภาษี" : ii.PointOfEvaluation.Point == 8 ? "ร้อยละของผลการเร่งรัดหนี้ได้เม็ดเงินเปรียบเทียบกับประมาณการ" : ii.PointOfEvaluation.Point == 9 ? "ร้อยละของผลการจำหน่ายหนี้ภาษีอากรเปรียบเทียบกับประมาณการ" : ii.PointOfEvaluation.Point == 10 ? "ร้อยละของจำนวนแบบฯ ที่ยื่นทางอิเล็กทรอนิกส์เทียบกับจำนวนแบบฯ ทั้งหมด" : ii.PointOfEvaluation.Point == 11 ? "ร้อยละของจำนวนแบบฯ ภ.ง.ด.1 ภ.ง.ด.3 ภ.พ.30 ภ.ง.ด.50 และ ภ.ง.ด.53  ที่ยื่นทางอิเล็กทรอนิกส์ เทียบกับจำนวนแบบฯ ภ.ง.ด.1 ภ.ง.ด.3 ภ.พ.30 ภ.ง.ด.50 และ ภ.ง.ด.53 ทั้งหมด" : ii.PointOfEvaluation.Point == 12 ? "ร้อยละของงานค้างหนังสือร้องเรียนแหล่งภาษีที่ลดลง เปรียบเทียบกับงานค้างดำเนินการ ณ วันที่ 30 กันยายน 2565 หรือการดำเนินการงานค้างแล้วเสร็จเพิ่มขึ้น ร้อยละ 5 ของร้อยละงานค้างที่ดำเนินการได้ ณ วันที่ 30 กันยายน 2565" : ii.PointOfEvaluation.Name : ii.PointOfEvaluation.Point == 4 ? "ร้อยละของการแนะนำ ตรวจสอบ และการตรวจคืนภาษีอากรผู้ประกอบการ" : ii.PointOfEvaluation.Point == 7 ? "ร้อยละของงานค้างหนังสือร้องเรียนแหล่งภาษีที่ลดลง เปรียบเทียบกับงานค้างดำเนินการ ณ วันที่ 30 กันยายน 2565 หรือการดำเนินการงานค้างแล้วเสร็จเพิ่มขึ้น ร้อยละ 5 ของร้อยละงานค้างที่ดำเนินการได้ ณ วันที่ 30 กันยายน 2565" : ii.PointOfEvaluation.Name,
                    OfficeName = ii.Office.Code == "00009000" ? "ภญ." : "ตส.",
                    OfficeGroup = "ส่วนกลาง",
                    ii.ScoreValue,
                    ii.ScoreApprove,
                    ii.PointOfEvaluation.Year.Year
                }) ;
                scoreDrafts = scoreDrafts.Concat(scoreCenter);

                return Json(await DataSourceLoader.LoadAsync(scoreDrafts, loadOptions));
            } else
            {
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

                var scorePY = _context.ScoreDrafts.Include(so => so.Office).Include(sp => sp.PointOfEvaluation)
                    .Where(sd =>
                    sd.PointOfEvaluation.Plan == TypeOfPlan.B &&
                    sd.PointOfEvaluation.SubPoint == 0 &&
                    (sd.Office.Code == "00009000" || sd.Office.Code == "00003000") &&
                    sd.LastMonth == month &&
                    sd.PointOfEvaluation.Year == yearForQuery
                    );

                if(yearForQuery.Year == 2021)
                {
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
                    var scoreCenter = scorePY.Select(ii => new
                    {
                        Point = ii.Office.Code == "00009000" ? ii.PointOfEvaluation.Point == 2 ? 1 : ii.PointOfEvaluation.Point == 3 ? 2 : ii.PointOfEvaluation.Point == 4 ? 3 : ii.PointOfEvaluation.Point == 5 ? 4 : ii.PointOfEvaluation.Point == 6 ? 17 : ii.PointOfEvaluation.Point == 9 ? 13 : ii.PointOfEvaluation.Point == 10 ? 14 : ii.PointOfEvaluation.Point == 11 ? 12 : ii.PointOfEvaluation.Point : ii.PointOfEvaluation.Point == 4 ? 2 : ii.PointOfEvaluation.Point == 5 ? 18 : ii.PointOfEvaluation.Point == 6 ? 19 : ii.PointOfEvaluation.Point == 7 ? 12 : ii.PointOfEvaluation.Point ,
                        PointName = ii.Office.Code == "00009000" ? ii.PointOfEvaluation.Point == 2 ? "ร้อยละของผลการจัดเก็บภาษีเทียบกับประมาณการ" : ii.PointOfEvaluation.Point == 3 ? "ร้อยละของการแนะนำ ตรวจสอบ และการตรวจคืนภาษีอากรผู้ประกอบการ" : ii.PointOfEvaluation.Point == 4 ? "ร้อยละของผู้ประกอบการที่ดำเนินการแนะนำและตรวจสอบภาษีอากรแล้วเสร็จ และมีภาษีชำระ" : ii.PointOfEvaluation.Point == 5 ? "ระดับความสำเร็จของร้อยละเฉลี่ยถ่วงน้ำหนักในการสอบยันใบกำกับภาษี" : ii.PointOfEvaluation.Point == 7 ? "ร้อยละของผลการเร่งรัดหนี้ได้เม็ดเงินเปรียบเทียบกับประมาณการ" : ii.PointOfEvaluation.Point == 8 ? "ร้อยละของผลการจำหน่ายหนี้ภาษีอากรเปรียบเทียบกับประมาณการ" : ii.PointOfEvaluation.Point == 9 ? "ร้อยละของจำนวนแบบฯ ที่ยื่นผ่านอินเทอร์เน็ตเทียบกับจำนวนแบบฯ ทั้งหมด " : ii.PointOfEvaluation.Point == 10 ? "ร้อยละของจำนวนแบบฯ ภ.ง.ด.1 ภ.พ.30 ภ.ง.ด.50 และ ภ.ง.ด.53  ที่ยื่นผ่านอินเทอร์เน็ต เทียบกับจำนวนแบบฯ ภ.ง.ด.1, ภ.พ.30, ภ.ง.ด.50 และ ภ.ง.ด.53 ทั้งหมด" : ii.PointOfEvaluation.Name : ii.PointOfEvaluation.Point == 4 ? "ร้อยละของการแนะนำ ตรวจสอบ และการตรวจคืนภาษีอากรผู้ประกอบการ" : ii.PointOfEvaluation.Name,
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
        }

        public async Task<object> CentralTarget(DataSourceLoadOptions loadOptions, int yearPoint, int lastMonth, string officeCode)
        {
            DateTime yearForQuery = new DateTime(yearPoint, 1, 1);
            DateTime finishedDate = new DateTime(yearForQuery.Year, lastMonth, DateTime.DaysInMonth(yearForQuery.Year, lastMonth));

            var report2 = _context.PointOfEvaluations.Include(ofc => ofc.OwnerOffice).Include(iss => iss.IssueForEvaluations).Include(sd => sd.ScoreDrafts).Include(rd => rd.Rounds).Include(dv => dv.DataForEvaluations).Include(tt => tt.Theme).Include(ee => ee.End).Include(ww => ww.Way)
                .Where(pv =>
                pv.Year == yearForQuery &&
                pv.OwnerOffice.Code == officeCode &&
                pv.SubPoint == 0
                ).OrderBy(ob => ob.Point);

            DateTime? completedDate;
            decimal? result;
            foreach (var data in report2)
            {
                completedDate = null;
                result = null;
                foreach (var issue in data.IssueForEvaluations)
                {
                    if(issue.Month == lastMonth)
                    {
                        data.Issue = issue.Issue;
                        break;
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
                } else // ระดับ in march
                {

                    foreach(var dv in data.DataForEvaluations)
                    {
                        if(dv.Month == lastMonth)
                        {
                            completedDate = dv.CompletedDate;
                            result = dv.Result;
                            break;
                        }
                    }

                    foreach(var rd in data.Rounds)
                    {
                        if(rd.RoundNumber == 1)
                        {
                            if(result == 1 && completedDate <= rd.Rate1MonthStop)
                            {
                                data.Target = 100;
                            }
                            else if(result == 2 && completedDate <= rd.Rate2MonthStop)
                            {
                                data.Target = 100;
                            }
                            else if(result == 3 && completedDate <= rd.Rate3MonthStop)
                            {
                                data.Target = 100;
                            }
                            else if(result == 4 && completedDate <= rd.Rate4MonthStop)
                            {
                                data.Target = 100;
                            }
                            else if(result == 5 && completedDate <= rd.Rate5MonthStop)
                            {
                                data.Target = 100;
                            }
                            else
                            {
                                data.Target = 0;
                            }
                        }
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
                expectPlan = slt.ExpectPlan == null ? "-" : ((ExpectPlanRD)slt.ExpectPlan).ToString(),
                ddrive = slt.Ddrive == null ? "-" : ((DdriveRD)slt.Ddrive).ToString(),
                detailPlan = slt.DetailPlan,
                target = slt.ScoreTarget,
                isTarget = slt.Target >= 100 ? "✔" : "",
                isNotTarget = slt.Target >= 100 ? "" : "✔",
                issue = slt.Issue,
                theme = slt.ThemeId == null ? "-" : slt.Theme.ThemeName,
                end = slt.EndId == null ? "-" : slt.End.EndName,
                way = slt.WayId == null ? "-" : slt.Way.WayName
            });


            return Json(await DataSourceLoader.LoadAsync(point, loadOptions));
        }
    }
}