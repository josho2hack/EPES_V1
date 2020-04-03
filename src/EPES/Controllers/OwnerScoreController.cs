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
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var user = await _userManager.GetUserAsync(User);
            var scoreDrafts = _context.ScoreDrafts//.Include(p => p.OwnerOffice)
                                                  //.Include(p => p.ScoreDrafts)
                                                  //.Include(p => p.Scores)
                                                    //.Include(sd => sd.PointOfEvaluation)
                                                   //     .ThenInclude(p => p.Scores)
                                                    //.Include(p => p.Rounds)
                                                    .Where(sd => sd.Office.Code == user.OfficeId && !sd.PointOfEvaluation.HasSub)
                                                    .Select(i => new {
                                                    i.Id,
                                                    i.PointOfEvaluation.Plan,
                                                    i.PointOfEvaluation.Point,
                                                    i.PointOfEvaluation.SubPoint,
                                                    i.PointOfEvaluation.Name,
                                                    i.ScoreValue,
                                                    i.ScoreApprove,
                                                    i.LastMonth,
                                                    i.PointOfEvaluation.Year.Year
                                                    });

            return Json(await DataSourceLoader.LoadAsync(scoreDrafts, loadOptions));
        }
    }
}