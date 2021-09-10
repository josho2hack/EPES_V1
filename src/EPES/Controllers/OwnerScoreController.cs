﻿using System;
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
    public class OwnerScoreController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OwnerScoreController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions, string selectoffice, int yearPoint)
        {
            var user = await _userManager.GetUserAsync(User);
            string office;
            if (selectoffice == null)
            {
                office = user.OfficeId;
            }
            else
            {
                office = selectoffice;
            }
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

            var scoreDrafts = _context.ScoreDrafts.Include(p=>p.PointOfEvaluation).Where(sd => sd.Office.Code == office && sd.PointOfEvaluation.SubPoint == 0 && sd.PointOfEvaluation.Year == yearForQuery)
                                                    .Select(i => new
                                                    {
                                                        i.Id,
                                                        i.PointOfEvaluation.Plan,
                                                        i.PointOfEvaluation.Point,
                                                        i.PointOfEvaluation.SubPoint,
                                                        i.PointOfEvaluation.Name,
                                                        i.PointOfEvaluation.Weight,
                                                        i.ScoreValue,
                                                        i.ScoreApprove,
                                                        i.LastMonth,
                                                        i.PointOfEvaluation.Year.Year,
                                                        cal = (i.PointOfEvaluation.Weight * i.ScoreValue /100 ),
                                                        calApprove = (i.PointOfEvaluation.Weight * i.ScoreApprove / 100)
                                                    });

            return Json(await DataSourceLoader.LoadAsync(scoreDrafts, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> GetSub(DataSourceLoadOptions loadOptions, string selectoffice, int yearPoint)
        {
            var user = await _userManager.GetUserAsync(User);
            string office;
            if (selectoffice == null)
            {
                office = user.OfficeId;
            }
            else
            {
                office = selectoffice;
            }
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

            var scoreDrafts = _context.ScoreDrafts.Include(p => p.PointOfEvaluation).Include(d => d.PointOfEvaluation.DataForEvaluations).Where(sd => sd.Office.Code == office && sd.PointOfEvaluation.SubPoint == 0 && sd.PointOfEvaluation.Year == yearForQuery);                                                  

            foreach (var item in scoreDrafts)
            {
                if (!item.PointOfEvaluation.WeightAll)
                {
                    foreach (var data in item.PointOfEvaluation.DataForEvaluations)
                    {
                        if (data.Month == item.LastMonth)
                        {
                            item.weightOfMonth = data.Weight;
                        }
                    }
                }
            }

            var score = scoreDrafts.Select(i => new
            {
                i.Id,
                i.PointOfEvaluation.Plan,
                i.PointOfEvaluation.Point,
                i.PointOfEvaluation.SubPoint,
                i.PointOfEvaluation.Name,
                i.PointOfEvaluation.Weight,
                i.ScoreValue,
                i.ScoreApprove,
                i.LastMonth,
                i.PointOfEvaluation.Year.Year,
                cal = (i.weightOfMonth * i.ScoreValue / 100),
                calApprove = (i.weightOfMonth * i.ScoreApprove / 100),
                i.weightOfMonth
            });



            return Json(await DataSourceLoader.LoadAsync(scoreDrafts, loadOptions));
        }
    }
}