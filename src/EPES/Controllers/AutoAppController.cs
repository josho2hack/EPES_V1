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
    public class AutoAppController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AutoAppController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            //var office = await _context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000").ToListAsync();
            DateTime yearForRequest;
            if (DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
            {
                yearForRequest = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);
            }
            else
            {
                yearForRequest = new DateTime(DateTime.Now.Year, 1, 1);
            }

            string url = "";
            for (int i = 1; i <= 12; i++)
            {
                for (int pak = 0; pak <= 12; pak++)
                {
                    url = "http://10.20.37.11:7072/serviceTier/webapi/All/officeId/" + pak.ToString("D2") + "000000" + "/year/" + (yearForRequest.Year + 543).ToString("D4") + "/month/" + i.ToString("D2") + "/";
                    //url = "http://10.20.37.11:7072/serviceTier/webapi/All/officeId/" + pak.ToString("D2") + "000000" + "/year/2563" + "/month/" + i.ToString("D2") + "/";
                    GetTCL(url, yearForRequest, i);
                }
            }
            return LocalRedirect("/");
        }

        private void GetTCL(string url, DateTime yearForRequest, int i)
        {
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null)
            {
                return;
            }
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
                        if (t.officeCode == "00000722")
                        {
                            var dataForEvaluation = _context.DataForEvaluations
                                                    //.Include(d => d.PointOfEvaluation)
                                                    .Where(d => d.Office.Code == "00009000" && d.PointOfEvaluation.AutoApp == AutoApps.ข้อมูลการจัดเก็บภาษีอากร && d.PointOfEvaluation.Year == yearForRequest && d.Month == i)
                                                    .FirstOrDefault();

                            if (dataForEvaluation != null)
                            {
                                //dataForEvaluation.Expect = t.CMCYforcast/1000000;
                                dataForEvaluation.Result = t.CMcurrentYear/1000000;
                                _context.SaveChanges();
                            }
                            //else
                            //{
                            //    dataForEvaluation = new DataForEvaluation();
                            //    dataForEvaluation.Expect = t.CMCYforcast;
                            //    dataForEvaluation.Result = t.CMcurrentYear;
                            //    dataForEvaluation.OfficeId = _context.Offices.Where(o => o.Code == "00009000").FirstOrDefault().Id;
                            //    dataForEvaluation.PointOfEvaluationId = _context.PointOfEvaluations.Where(poe => poe.OwnerOfficeId == dataForEvaluation.OfficeId && poe.Year == yearForRequest && poe.AutoApp == AutoApps.ข้อมูลการจัดเก็บภาษีอากร).FirstOrDefault().Id;
                            //    dataForEvaluation.Month = i;
                            //    _context.DataForEvaluations.Add(dataForEvaluation);
                            //    _context.SaveChanges();
                            //}
                        } // ภญ.
                        else
                        {
                            var dataForEvaluation = _context.DataForEvaluations
                                                    //.Include(d => d.PointOfEvaluation)
                                                    .Where(d => d.Office.Code == t.officeCode && d.PointOfEvaluation.AutoApp == AutoApps.ข้อมูลการจัดเก็บภาษีอากร && d.PointOfEvaluation.Year == yearForRequest && d.Month == i)
                                                    .FirstOrDefault();

                            if (dataForEvaluation != null)
                            {
                                //dataForEvaluation.Expect = t.CMCYforcast/1000000;
                                dataForEvaluation.Result = t.CMcurrentYear/1000000;
                                _context.SaveChanges();
                            }
                            //else
                            //{
                            //    dataForEvaluation = new DataForEvaluation();
                            //    dataForEvaluation.Expect = t.CMCYforcast;
                            //    dataForEvaluation.Result = t.CMcurrentYear;
                            //    dataForEvaluation.OfficeId = _context.Offices.Where(o => o.Code == t.officeCode).FirstOrDefault().Id;
                            //    dataForEvaluation.PointOfEvaluationId = _context.PointOfEvaluations.Where(poe => poe.OwnerOfficeId == dataForEvaluation.OfficeId && poe.Year == yearForRequest && poe.Name.Contains("ผลการจัดเก็บภาษี") && poe.Unit == 0).FirstOrDefault().Id;
                            //    dataForEvaluation.Month = i;
                            //    _context.DataForEvaluations.Add(dataForEvaluation);
                            //    _context.SaveChanges();
                            //}
                        } // สภ.
                    }
                }
            }
        } //End getTCL
    }
}