using EPES.Data;
using EPES.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EPES.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly ApplicationDbContext _context;
        public decimal CYcurrentYear = 0m;
        public decimal CYforcast = 0m;

        public HomeController(SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, IStringLocalizer<HomeController> localizer)
        {
            _localizer = localizer;
            _context = context;
            _signInManager = signInManager;
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        public async Task<IActionResult> Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("IndexMember");
            }
            DateTime yearForRequest;
            if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
            {
                yearForRequest = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);
            }
            else
            {
                yearForRequest = new DateTime(DateTime.Now.Year, 1, 1);
            }

            string url = "";
            //var m = DateTime.Now.Month;
            url = "http://10.20.37.11:7072/serviceTier/webapi/All/officeId/" + "00000000" + "/year/" + (yearForRequest.Year + 543).ToString("D4") + "/month/" + DateTime.Now.AddMinutes(-1).Month.ToString("D2");

            var webRequest = WebRequest.Create(url) as HttpWebRequest;

            if (webRequest != null)
            {
                webRequest.ContentType = "application/json";
                webRequest.UserAgent = "Nothing";
                webRequest.Timeout = 5000;
                try
                {
                    using (var s = webRequest.GetResponse().GetResponseStream())
                    {
                        using (var sr = new StreamReader(s))
                        {
                            var taxCollectionsAsJson = sr.ReadToEnd();
                            var taxCollections = JsonConvert.DeserializeObject<Rootobject>(taxCollectionsAsJson);
                            foreach (var t in taxCollections.taxCollection)
                            {
                                if (t.officeCode == "00000000")
                                {
                                    continue;
                                }
                                CYcurrentYear += t.CYcurrentYear;
                                CYforcast += t.CYforcast;
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.Timeout)
                    {
                        CYcurrentYear = 9999999;
                        CYforcast = 9999999;
                    }

                }
            }

            ViewBag.CYcurrentYear = CYcurrentYear / 1000000;
            ViewBag.CYforcast = CYforcast / 1000000;

            var approvedOffice = await _context.Scores.Where(s => s.Office.Code != "00000000" && s.Office.Code.Substring(5, 3) == "000" && s.LastMonth == DateTime.Now.AddMonths(-1).Month).Select(o => new { o.OfficeId }).Distinct().CountAsync();
            var entryOffice = await _context.ScoreDrafts.Where(s => s.Office.Code != "00000000" && s.Office.Code.Substring(5, 3) == "000" && s.LastMonth == DateTime.Now.AddMonths(-1).Month).Select(o => new { o.OfficeId }).Distinct().CountAsync();

            ViewBag.Approved = approvedOffice;
            ViewBag.Entry = entryOffice;

            return View();
        }

        [Authorize]
        public async Task<IActionResult> IndexMember()
        {
            DateTime yearForRequest;
            if (DateTime.Now.Month == 10 || DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
            {
                yearForRequest = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);
            }
            else
            {
                yearForRequest = new DateTime(DateTime.Now.Year, 1, 1);
            }

            string url = "";
            //var m = DateTime.Now.Month;
            url = "http://10.20.37.11:7072/serviceTier/webapi/All/officeId/" + "00000000" + "/year/" + (yearForRequest.Year + 543).ToString("D4") + "/month/" + DateTime.Now.AddMinutes(-1).Month.ToString("D2");

            var webRequest = WebRequest.Create(url) as HttpWebRequest;

            if (webRequest != null)
            {
                webRequest.ContentType = "application/json";
                webRequest.UserAgent = "Nothing";
                webRequest.Timeout = 5000;
                try
                {
                    using (var s = webRequest.GetResponse().GetResponseStream())
                    {
                        using (var sr = new StreamReader(s))
                        {
                            var taxCollectionsAsJson = sr.ReadToEnd();
                            var taxCollections = JsonConvert.DeserializeObject<Rootobject>(taxCollectionsAsJson);
                            foreach (var t in taxCollections.taxCollection)
                            {
                                if (t.officeCode == "00000000")
                                {
                                    continue;
                                }
                                CYcurrentYear += t.CYcurrentYear;
                                CYforcast += t.CYforcast;
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.Timeout)
                    {
                        CYcurrentYear = 9999999;
                        CYforcast = 9999999;
                    }

                }
            }

            ViewBag.CYcurrentYear = CYcurrentYear / 1000000;
            ViewBag.CYforcast = CYforcast / 1000000;

            var approvedOffice = await _context.Scores.Where(s => s.Office.Code != "00000000" && s.Office.Code.Substring(5, 3) == "000" && s.LastMonth == DateTime.Now.AddMonths(-1).Month).Select(o => new { o.OfficeId }).Distinct().CountAsync();
            var entryOffice = await _context.ScoreDrafts.Where(s => s.Office.Code != "00000000" && s.Office.Code.Substring(5, 3) == "000" && s.LastMonth == DateTime.Now.AddMonths(-1).Month).Select(o => new { o.OfficeId }).Distinct().CountAsync();

            ViewBag.Approved = approvedOffice;
            ViewBag.Entry = entryOffice;

            var expectHQ = await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 2 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code == "00009000").Select(o => new { o.Expect }).SumAsync(s => s.Expect);

            var expectNHQ = await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 1 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000").Select(o => new { o.Expect }).SumAsync(s => s.Expect);

            decimal expectHQLastMonth = 0;
            if (DateTime.Now.AddMonths(-1).Month >= 10)
            {
                var m = DateTime.Now.AddMonths(-1).Month;
                while (m >= 10)
                {
                    expectHQLastMonth += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 2 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code == "00009000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m -= 1;
                }
            }
            else
            {
                var m = 12;
                while (m >= 10)
                {
                    expectHQLastMonth += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 2 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code == "00009000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m -= 1;
                }
                m = 1;
                while (m <= DateTime.Now.AddMonths(-1).Month)
                {
                    expectHQLastMonth += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 2 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code == "00009000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m += 1;
                }
            }

            decimal expectNHQLastMonth = 0;
            if (DateTime.Now.AddMonths(-1).Month >= 10)
            {
                var m = DateTime.Now.AddMonths(-1).Month;
                while (m >= 10)
                {
                    expectNHQLastMonth += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 1 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m -= 1;
                }
            }
            else
            {
                var m = 12;
                while (m >= 10)
                {
                    expectNHQLastMonth += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 1 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m -= 1;
                }
                m = 1;
                while (m <= DateTime.Now.AddMonths(-1).Month)
                {
                    expectNHQLastMonth += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 1 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m += 1;
                }
            }

            //Result --------------------------------------
            decimal resultHQLastMonth = 0;
            if (DateTime.Now.AddMonths(-1).Month >= 10)
            {
                var m = DateTime.Now.AddMonths(-1).Month;
                while (m >= 10)
                {
                    resultHQLastMonth += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 2 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code == "00009000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m -= 1;
                }
            }
            else
            {
                var m = 12;
                while (m >= 10)
                {
                    resultHQLastMonth += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 2 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code == "00009000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m -= 1;
                }
                m = 1;
                while (m <= DateTime.Now.AddMonths(-1).Month)
                {
                    resultHQLastMonth += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 2 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code == "00009000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m += 1;
                }
            }

            decimal resultNHQLastMonth = 0;
            if (DateTime.Now.AddMonths(-1).Month >= 10)
            {
                var m = DateTime.Now.AddMonths(-1).Month;
                while (m >= 10)
                {
                    resultNHQLastMonth += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 1 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m -= 1;
                }
            }
            else
            {
                var m = 12;
                while (m >= 10)
                {
                    resultNHQLastMonth += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 1 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m -= 1;
                }
                m = 1;
                while (m <= DateTime.Now.AddMonths(-1).Month)
                {
                    resultNHQLastMonth += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 1 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m += 1;
                }
            }

            List<decimal> expect = new List<decimal>() { expectHQ, expectNHQ };
            List<decimal> expectLastMonth = new List<decimal>() { expectHQLastMonth, expectNHQLastMonth };
            List<decimal> result = new List<decimal>() { resultHQLastMonth, resultNHQLastMonth };
            ViewBag.Expect = expect;
            ViewBag.ExpectLastMonth = expectLastMonth;
            ViewBag.ResultLastMonth = result;

            //---------------หมวด 2 ----------------------
            var expectHQM2 = await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 10 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) == "000000").Select(o => new { o.Expect }).SumAsync(s => s.Expect);

            var expectNHQM2 = await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 10 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000").Select(o => new { o.Expect }).SumAsync(s => s.Expect);

            decimal expectHQLastMonthM2 = 0;
            if (DateTime.Now.AddMonths(-1).Month >= 10)
            {
                var m = DateTime.Now.AddMonths(-1).Month;
                while (m >= 10)
                {
                    expectHQLastMonthM2 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 10 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) == "000000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m -= 1;
                }
            }
            else
            {
                var m = 12;
                while (m >= 10)
                {
                    expectHQLastMonthM2 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 10 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) == "000000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m -= 1;
                }
                m = 1;
                while (m <= DateTime.Now.AddMonths(-1).Month)
                {
                    expectHQLastMonthM2 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 10 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) == "000000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m += 1;
                }
            }

            decimal expectNHQLastMonthM2 = 0;
            if (DateTime.Now.AddMonths(-1).Month >= 10)
            {
                var m = DateTime.Now.AddMonths(-1).Month;
                while (m >= 10)
                {
                    expectNHQLastMonthM2 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 10 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m -= 1;
                }
            }
            else
            {
                var m = 12;
                while (m >= 10)
                {
                    expectNHQLastMonthM2 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 10 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m -= 1;
                }
                m = 1;
                while (m <= DateTime.Now.AddMonths(-1).Month)
                {
                    expectNHQLastMonthM2 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 10 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m += 1;
                }
            }

            //Result --------------------------------------
            decimal resultHQLastMonthM2 = 0;
            if (DateTime.Now.AddMonths(-1).Month >= 10)
            {
                var m = DateTime.Now.AddMonths(-1).Month;
                while (m >= 10)
                {
                    resultHQLastMonthM2 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 10 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) == "000000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m -= 1;
                }
            }
            else
            {
                var m = 12;
                while (m >= 10)
                {
                    resultHQLastMonthM2 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 10 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) == "000000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m -= 1;
                }
                m = 1;
                while (m <= DateTime.Now.AddMonths(-1).Month)
                {
                    resultHQLastMonthM2 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 10 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) == "000000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m += 1;
                }
            }

            decimal resultNHQLastMonthM2 = 0;
            if (DateTime.Now.AddMonths(-1).Month >= 10)
            {
                var m = DateTime.Now.AddMonths(-1).Month;
                while (m >= 10)
                {
                    resultNHQLastMonthM2 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 10 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m -= 1;
                }
            }
            else
            {
                var m = 12;
                while (m >= 10)
                {
                    resultNHQLastMonthM2 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 10 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m -= 1;
                }
                m = 1;
                while (m <= DateTime.Now.AddMonths(-1).Month)
                {
                    resultNHQLastMonthM2 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 10 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m += 1;
                }
            }

            List<decimal> expectM2 = new List<decimal>() { expectHQM2, expectNHQM2 };
            List<decimal> expectLastMonthM2 = new List<decimal>() { expectHQLastMonthM2, expectNHQLastMonthM2 };
            List<decimal> resultM2 = new List<decimal>() { resultHQLastMonthM2, resultNHQLastMonthM2 };
            ViewBag.ExpectM2 = expectM2;
            ViewBag.ExpectLastMonthM2 = expectLastMonthM2;
            ViewBag.ResultLastMonthM2 = resultM2;

            //---------------หมวด 3 ----------------------
            var expectHQM3 = await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 8 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code == "00009000").Select(o => new { o.Expect }).SumAsync(s => s.Expect);

            var expectNHQM3 = await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 6 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(5, 3) == "000").Select(o => new { o.Expect }).SumAsync(s => s.Expect);

            decimal expectHQLastMonthM3 = 0;
            if (DateTime.Now.AddMonths(-1).Month >= 10)
            {
                var m = DateTime.Now.AddMonths(-1).Month;
                while (m >= 10)
                {
                    expectHQLastMonthM3 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 8 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code == "00009000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m -= 1;
                }
            }
            else
            {
                var m = 12;
                while (m >= 10)
                {
                    expectHQLastMonthM3 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 8 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code == "00009000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m -= 1;
                }
                m = 1;
                while (m <= DateTime.Now.AddMonths(-1).Month)
                {
                    expectHQLastMonthM3 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 8 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code == "00009000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m += 1;
                }
            }

            decimal expectNHQLastMonthM3 = 0;
            if (DateTime.Now.AddMonths(-1).Month >= 10)
            {
                var m = DateTime.Now.AddMonths(-1).Month;
                while (m >= 10)
                {
                    expectNHQLastMonthM3 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 6 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m -= 1;
                }
            }
            else
            {
                var m = 12;
                while (m >= 10)
                {
                    expectNHQLastMonthM3 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 6 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m -= 1;
                }
                m = 1;
                while (m <= DateTime.Now.AddMonths(-1).Month)
                {
                    expectNHQLastMonthM3 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 6 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m += 1;
                }
            }

            //Result --------------------------------------
            decimal resultHQLastMonthM3 = 0;
            if (DateTime.Now.AddMonths(-1).Month >= 10)
            {
                var m = DateTime.Now.AddMonths(-1).Month;
                while (m >= 10)
                {
                    resultHQLastMonthM3 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 8 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code == "00009000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m -= 1;
                }
            }
            else
            {
                var m = 12;
                while (m >= 10)
                {
                    resultHQLastMonthM3 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 8 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code == "00009000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m -= 1;
                }
                m = 1;
                while (m <= DateTime.Now.AddMonths(-1).Month)
                {
                    resultHQLastMonthM3 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 8 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code == "00009000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m += 1;
                }
            }

            decimal resultNHQLastMonthM3 = 0;
            if (DateTime.Now.AddMonths(-1).Month >= 10)
            {
                var m = DateTime.Now.AddMonths(-1).Month;
                while (m >= 10)
                {
                    resultNHQLastMonthM3 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 6 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m -= 1;
                }
            }
            else
            {
                var m = 12;
                while (m >= 10)
                {
                    resultNHQLastMonthM3 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 6 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m -= 1;
                }
                m = 1;
                while (m <= DateTime.Now.AddMonths(-1).Month)
                {
                    resultNHQLastMonthM3 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 6 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m += 1;
                }
            }

            List<decimal> expectM3 = new List<decimal>() { expectHQM3, expectNHQM3 };
            List<decimal> expectLastMonthM3 = new List<decimal>() { expectHQLastMonthM3, expectNHQLastMonthM3 };
            List<decimal> resultM3 = new List<decimal>() { resultHQLastMonthM3, resultNHQLastMonthM3 };
            ViewBag.ExpectM3 = expectM3;
            ViewBag.ExpectLastMonthM3 = expectLastMonthM3;
            ViewBag.ResultLastMonthM3 = resultM3;

            //---------------หมวด 4 ----------------------
            var expectHQM4 = await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 9 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) == "000000").Select(o => new { o.Expect }).SumAsync(s => s.Expect);

            var expectNHQM4 = await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 9 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000").Select(o => new { o.Expect }).SumAsync(s => s.Expect);

            decimal expectHQLastMonthM4 = 0;
            if (DateTime.Now.AddMonths(-1).Month >= 10)
            {
                var m = DateTime.Now.AddMonths(-1).Month;
                while (m >= 10)
                {
                    expectHQLastMonthM4 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 9 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) == "000000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m -= 1;
                }
            }
            else
            {
                var m = 12;
                while (m >= 10)
                {
                    expectHQLastMonthM4 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 9 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) == "000000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m -= 1;
                }
                m = 1;
                while (m <= DateTime.Now.AddMonths(-1).Month)
                {
                    expectHQLastMonthM4 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 9 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) == "000000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m += 1;
                }
            }

            decimal expectNHQLastMonthM4 = 0;
            if (DateTime.Now.AddMonths(-1).Month >= 10)
            {
                var m = DateTime.Now.AddMonths(-1).Month;
                while (m >= 10)
                {
                    expectNHQLastMonthM4 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 9 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m -= 1;
                }
            }
            else
            {
                var m = 12;
                while (m >= 10)
                {
                    expectNHQLastMonthM4 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 9 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m -= 1;
                }
                m = 1;
                while (m <= DateTime.Now.AddMonths(-1).Month)
                {
                    expectNHQLastMonthM4 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 9 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m += 1;
                }
            }

            //Result --------------------------------------
            decimal resultHQLastMonthM4 = 0;
            if (DateTime.Now.AddMonths(-1).Month >= 10)
            {
                var m = DateTime.Now.AddMonths(-1).Month;
                while (m >= 10)
                {
                    resultHQLastMonthM4 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 9 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) == "000000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m -= 1;
                }
            }
            else
            {
                var m = 12;
                while (m >= 10)
                {
                    resultHQLastMonthM4 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 9 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) == "000000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m -= 1;
                }
                m = 1;
                while (m <= DateTime.Now.AddMonths(-1).Month)
                {
                    resultHQLastMonthM4 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 9 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) == "000000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m += 1;
                }
            }

            decimal resultNHQLastMonthM4 = 0;
            if (DateTime.Now.AddMonths(-1).Month >= 10)
            {
                var m = DateTime.Now.AddMonths(-1).Month;
                while (m >= 10)
                {
                    resultNHQLastMonthM4 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 9 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m -= 1;
                }
            }
            else
            {
                var m = 12;
                while (m >= 10)
                {
                    resultNHQLastMonthM4 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 9 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m -= 1;
                }
                m = 1;
                while (m <= DateTime.Now.AddMonths(-1).Month)
                {
                    resultNHQLastMonthM4 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 9 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m += 1;
                }
            }

            List<decimal> expectM4 = new List<decimal>() { expectHQM4, expectNHQM4 };
            List<decimal> expectLastMonthM4 = new List<decimal>() { expectHQLastMonthM4, expectNHQLastMonthM4 };
            List<decimal> resultM4 = new List<decimal>() { resultHQLastMonthM4, resultNHQLastMonthM4 };
            ViewBag.ExpectM4 = expectM4;
            ViewBag.ExpectLastMonthM4 = expectLastMonthM4;
            ViewBag.ResultLastMonthM4 = resultM4;

            //---------------หมวด 5----------------------
            var expectHQM5 = await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 11 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code == "00009000").Select(o => new { o.Expect }).SumAsync(s => s.Expect);

            var expectNHQM5 = await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 13 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(5, 3) == "000").Select(o => new { o.Expect }).SumAsync(s => s.Expect);

            decimal expectHQLastMonthM5 = 0;
            if (DateTime.Now.AddMonths(-1).Month >= 10)
            {
                var m = DateTime.Now.AddMonths(-1).Month;
                while (m >= 10)
                {
                    expectHQLastMonthM5 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 11 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code == "00009000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m -= 1;
                }
            }
            else
            {
                var m = 12;
                while (m >= 10)
                {
                    expectHQLastMonthM5 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 11 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code == "00009000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m -= 1;
                }
                m = 1;
                while (m <= DateTime.Now.AddMonths(-1).Month)
                {
                    expectHQLastMonthM5 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 11 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code == "00009000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m += 1;
                }
            }

            decimal expectNHQLastMonthM5 = 0;
            if (DateTime.Now.AddMonths(-1).Month >= 10)
            {
                var m = DateTime.Now.AddMonths(-1).Month;
                while (m >= 10)
                {
                    expectNHQLastMonthM5 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 13 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m -= 1;
                }
            }
            else
            {
                var m = 12;
                while (m >= 10)
                {
                    expectNHQLastMonthM5 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 13 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m -= 1;
                }
                m = 1;
                while (m <= DateTime.Now.AddMonths(-1).Month)
                {
                    expectNHQLastMonthM5 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 13 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m += 1;
                }
            }

            //Result --------------------------------------
            decimal resultHQLastMonthM5 = 0;
            if (DateTime.Now.AddMonths(-1).Month >= 10)
            {
                var m = DateTime.Now.AddMonths(-1).Month;
                while (m >= 10)
                {
                    resultHQLastMonthM5 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 11 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code == "00009000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m -= 1;
                }
            }
            else
            {
                var m = 12;
                while (m >= 10)
                {
                    resultHQLastMonthM5 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 11 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code == "00009000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m -= 1;
                }
                m = 1;
                while (m <= DateTime.Now.AddMonths(-1).Month)
                {
                    resultHQLastMonthM5 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 11 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code == "00009000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m += 1;
                }
            }

            decimal resultNHQLastMonthM5 = 0;
            if (DateTime.Now.AddMonths(-1).Month >= 10)
            {
                var m = DateTime.Now.AddMonths(-1).Month;
                while (m >= 10)
                {
                    resultNHQLastMonthM5 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 13 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m -= 1;
                }
            }
            else
            {
                var m = 12;
                while (m >= 10)
                {
                    resultNHQLastMonthM5 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 13 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m -= 1;
                }
                m = 1;
                while (m <= DateTime.Now.AddMonths(-1).Month)
                {
                    resultNHQLastMonthM5 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 13 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m += 1;
                }
            }

            List<decimal> expectM5 = new List<decimal>() { expectHQM5, expectNHQM5 };
            List<decimal> expectLastMonthM5 = new List<decimal>() { expectHQLastMonthM5, expectNHQLastMonthM5 };
            List<decimal> resultM5 = new List<decimal>() { resultHQLastMonthM5, resultNHQLastMonthM5 };
            ViewBag.ExpectM5 = expectM5;
            ViewBag.ExpectLastMonthM5 = expectLastMonthM5;
            ViewBag.ResultLastMonthM5 = resultM5;

            //---------------หมวด 6----------------------
            var expectHQM6 = await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 15 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) == "000000").Select(o => new { o.Expect }).SumAsync(s => s.Expect);

            var expectNHQM6 = await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 15 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000").Select(o => new { o.Expect }).SumAsync(s => s.Expect);

            decimal expectHQLastMonthM6 = 0;
            if (DateTime.Now.AddMonths(-1).Month >= 10)
            {
                var m = DateTime.Now.AddMonths(-1).Month;
                while (m >= 10)
                {
                    expectHQLastMonthM6 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 15 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) == "000000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m -= 1;
                }
            }
            else
            {
                var m = 12;
                while (m >= 10)
                {
                    expectHQLastMonthM6 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 15 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) == "000000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m -= 1;
                }
                m = 1;
                while (m <= DateTime.Now.AddMonths(-1).Month)
                {
                    expectHQLastMonthM6 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 15 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) == "000000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m += 1;
                }
            }

            decimal expectNHQLastMonthM6 = 0;
            if (DateTime.Now.AddMonths(-1).Month >= 10)
            {
                var m = DateTime.Now.AddMonths(-1).Month;
                while (m >= 10)
                {
                    expectNHQLastMonthM6 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 15 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m -= 1;
                }
            }
            else
            {
                var m = 12;
                while (m >= 10)
                {
                    expectNHQLastMonthM6 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 15 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m -= 1;
                }
                m = 1;
                while (m <= DateTime.Now.AddMonths(-1).Month)
                {
                    expectNHQLastMonthM6 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 15 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Expect }).SumAsync(s => s.Expect);
                    m += 1;
                }
            }

            //Result --------------------------------------
            decimal resultHQLastMonthM6 = 0;
            if (DateTime.Now.AddMonths(-1).Month >= 10)
            {
                var m = DateTime.Now.AddMonths(-1).Month;
                while (m >= 10)
                {
                    resultHQLastMonthM6 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 15 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) == "000000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m -= 1;
                }
            }
            else
            {
                var m = 12;
                while (m >= 10)
                {
                    resultHQLastMonthM6 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 15 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) == "000000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m -= 1;
                }
                m = 1;
                while (m <= DateTime.Now.AddMonths(-1).Month)
                {
                    resultHQLastMonthM6 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 15 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) == "000000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m += 1;
                }
            }

            decimal resultNHQLastMonthM6 = 0;
            if (DateTime.Now.AddMonths(-1).Month >= 10)
            {
                var m = DateTime.Now.AddMonths(-1).Month;
                while (m >= 10)
                {
                    resultNHQLastMonthM6 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 15 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m -= 1;
                }
            }
            else
            {
                var m = 12;
                while (m >= 10)
                {
                    resultNHQLastMonthM6 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 15 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m -= 1;
                }
                m = 1;
                while (m <= DateTime.Now.AddMonths(-1).Month)
                {
                    resultNHQLastMonthM6 += await _context.DataForEvaluations.Where(s => s.PointOfEvaluation.Point == 15 && s.PointOfEvaluation.Year == yearForRequest && s.Office.Code.Substring(0, 3) != "000" && s.Office.Code.Substring(2, 6) != "000000" && s.Office.Code.Substring(5, 3) == "000" && s.Month == m).Select(o => new { o.Result }).SumAsync(s => s.Result);
                    m += 1;
                }
            }

            List<decimal> expectM6 = new List<decimal>() { expectHQM6, expectNHQM6 };
            List<decimal> expectLastMonthM6 = new List<decimal>() { expectHQLastMonthM6, expectNHQLastMonthM6 };
            List<decimal> resultM6 = new List<decimal>() { resultHQLastMonthM6, resultNHQLastMonthM6 };
            ViewBag.ExpectM6 = expectM6;
            ViewBag.ExpectLastMonthM6 = expectLastMonthM6;
            ViewBag.ResultLastMonthM6 = resultM6;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
