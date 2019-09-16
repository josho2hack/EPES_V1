﻿using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EPES.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using EPES.Data;
using System.IO;
using System.Net;

namespace EPES.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly ApplicationDbContext _context;
        public decimal CYcurrentYear = 0m;
        public decimal CYforcast = 0m;

        public HomeController(ApplicationDbContext context, IStringLocalizer<HomeController> localizer)
        {
            _localizer = localizer;
            _context = context;
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

        public IActionResult Index()
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
            var m = DateTime.Now.Month;
            url = "http://10.20.37.11:7072/serviceTier/webapi/All/officeId/" + "00000000" + "/year/" + (yearForRequest.Year + 543).ToString("D4") + "/month/" + DateTime.Now.Month.ToString("D2");

            var webRequest = WebRequest.Create(url) as HttpWebRequest;

            if (webRequest != null)
            {
                webRequest.ContentType = "application/json";
                webRequest.UserAgent = "Nothing";
                using (var s = webRequest.GetResponse().GetResponseStream())
                {
                    using (var sr = new StreamReader(s))
                    {
                        var taxCollectionsAsJson = sr.ReadToEnd();
                        var taxCollections = JsonConvert.DeserializeObject<Rootobject>(taxCollectionsAsJson);
                        foreach (var t in taxCollections.taxCollection)
                        {
                            CYcurrentYear += t.CYcurrentYear;
                            CYforcast += t.CYforcast;
                        }
                    }
                }
            }

            ViewBag.CYcurrentYear = CYcurrentYear / 1000000;
            ViewBag.CYforcast = CYforcast / 1000000;
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
