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

            var scoreDrafts = _context.ScoreDrafts.Include(s => s.Office)
                                                  .Where(sd => sd.PointOfEvaluation.Year == yearForQuery &&
                                                    sd.LastMonth == month &&
                                                    sd.Office.Code != "00000000" &&
                                                    !sd.PointOfEvaluation.HasSub)
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

            var scoreDrafts = _context.ScoreDrafts.Include(s => s.Office)
                                                  .Where(sd => sd.PointOfEvaluation.Year == yearForQuery &&
                                                    sd.LastMonth == month &&
                                                    sd.Office.Code != "00000000" &&
                                                    sd.Office.Code.Substring(0, 3) == "000" &&
                                                    !sd.PointOfEvaluation.HasSub)
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
                                                    !sd.PointOfEvaluation.HasSub)
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
                                                    !sd.PointOfEvaluation.HasSub)
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
                                                    !sd.PointOfEvaluation.HasSub)
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
                                                    !sd.PointOfEvaluation.HasSub)
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
                                                        !sd.PointOfEvaluation.HasSub)
                                             .Select(i => new
                                             {
                                                 i.ScoreValue
                                             });
            var avgAll = await avg.DefaultIfEmpty().AverageAsync(s => s.ScoreValue);

            var avgAB = await _context.ScoreDrafts.Include(s => s.Office).Where(sd => sd.PointOfEvaluation.Year == yearForQuery &&
                                                        sd.LastMonth == month &&
                                                        sd.Office.Code != "00000000" &&
                                                        sd.Office.Code.Substring(0, 3) == "000" &&
                                                        !sd.PointOfEvaluation.HasSub &&
                                                        (sd.PointOfEvaluation.Plan == TypeOfPlan.A || sd.PointOfEvaluation.Plan == TypeOfPlan.B))
                                             .Select(i => new
                                             {
                                                 i.ScoreValue
                                             }).DefaultIfEmpty().AverageAsync(s => s.ScoreValue);

            var avgC = await _context.ScoreDrafts.Include(s => s.Office).Where(sd => sd.PointOfEvaluation.Year == yearForQuery &&
                                                        sd.LastMonth == month &&
                                                        sd.Office.Code != "00000000" &&
                                                        sd.Office.Code.Substring(0, 3) == "000" &&
                                                        !sd.PointOfEvaluation.HasSub &&
                                                        sd.PointOfEvaluation.Plan == TypeOfPlan.C)
                                             .Select(i => new
                                             {
                                                 i.ScoreValue
                                             }).DefaultIfEmpty().AverageAsync(s => s.ScoreValue);

            var avgD = await _context.ScoreDrafts.Include(s => s.Office).Where(sd => sd.PointOfEvaluation.Year == yearForQuery &&
                                                        sd.LastMonth == month &&
                                                        sd.Office.Code != "00000000" &&
                                                        sd.Office.Code.Substring(0, 3) == "000" &&
                                                        !sd.PointOfEvaluation.HasSub &&
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
                                                        !sd.PointOfEvaluation.HasSub)
                                             .Select(i => new
                                             {
                                                 i.ScoreValue
                                             }).DefaultIfEmpty().AverageAsync(s => s.ScoreValue);

            var avgB = await _context.ScoreDrafts.Where(sd => sd.PointOfEvaluation.Year == yearForQuery &&
                                                        sd.LastMonth == month &&
                                                        sd.Office.Code != "00000000" &&
                                                        sd.Office.Code.Substring(0, 3) != "000" &&
                                                        sd.Office.Code.Substring(5, 3) == "000" &&
                                                        !sd.PointOfEvaluation.HasSub &&
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
                                                        !sd.PointOfEvaluation.HasSub &&
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
                                                        !sd.PointOfEvaluation.HasSub &&
                                                        sd.PointOfEvaluation.Plan == TypeOfPlan.D)
                                             .Select(i => new
                                             {
                                                 i.ScoreValue
                                             }).DefaultIfEmpty().AverageAsync(s => s.ScoreValue);

            return new[] { new { office = "ภาพรวม", score = avgAll }, new { office = "แผนงานระดับนโยบาย (Top-Down : B) น้ำหนัก 70 %", score = avgB }, new { office = "แผนงานที่หน่วยงานกำหนดเอง (Bottom-Up : C) น้ำหนัก 15 %", score = avgC }, new { office = "แผนงานร่วม (Joint KPI : D) น้ำหนัก 15%", score = avgD } };

            //return Json(await DataSourceLoader.LoadAsync(hqScore.AsQueryable(), loadOptions));
        }
    }
}