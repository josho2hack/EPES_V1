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
using FileHelpers;

namespace EPES.Controllers
{
    public class AutoAppController : Controller
    {
        public DateTime year;
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

            var yearForCompletedDate = yearForRequest.Year;
            if(i >= 10)
            {
                yearForCompletedDate = yearForCompletedDate - 1;
            }

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
                                                    .Where(d => d.Office.Code == "00009000" && d.PointOfEvaluation.AutoApp == AutoApps.การจัดเก็บภาษีอากร && d.PointOfEvaluation.Year == yearForRequest && d.Month == i)
                                                    .FirstOrDefault();

                            if (dataForEvaluation != null)
                            {
                                dataForEvaluation.Expect = t.CMCYforcast/1000000;
                                dataForEvaluation.Result = t.CMcurrentYear / 1000000;
                                //dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                                dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                                dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                                _context.SaveChanges();
                            }
                        } // ภญ.
                        else
                        {
                            var dataForEvaluation = _context.DataForEvaluations
                                                    .Where(d => d.Office.Code == t.officeCode && d.PointOfEvaluation.AutoApp == AutoApps.การจัดเก็บภาษีอากร && d.PointOfEvaluation.Year == yearForRequest && d.Month == i)
                                                    .FirstOrDefault();

                            if (dataForEvaluation != null)
                            {
                                dataForEvaluation.Expect = t.CMCYforcast/1000000;
                                dataForEvaluation.Result = t.CMcurrentYear / 1000000;
                                //dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                                dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                                dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                                _context.SaveChanges();
                            }
                        } // สภ.
                    }
                }
            }
        } //End getTCL

        public IActionResult GetTax100()
        {
            string url = "";
            var m = DateTime.Now.Month;
            //var office = await _context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000").ToListAsync();
            DateTime yearForRequest;
            if (m == 11 || m == 12)
            {
                yearForRequest = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);
                if (m == 11)
                {
                        for (int pak = 0; pak <= 12; pak++)
                        {
                            url = "http://10.20.17.178:8080/serviceTier/webapi/All200/officeId/" + pak.ToString("D2") + "000000" + "/year/" + (yearForRequest.Year + 543).ToString("D4") + "/month/" + "10" + "/part/2";
                            Get100(url, yearForRequest, 10);
                        }
                }
                if (m == 12)
                {
                    for (int i = 10; i <= 11; i++)
                    {
                        for (int pak = 0; pak <= 12; pak++)
                        {
                            url = "http://10.20.17.178:8080/serviceTier/webapi/All200/officeId/" + pak.ToString("D2") + "000000" + "/year/" + (yearForRequest.Year + 543).ToString("D4") + "/month/" + i.ToString("D2") + "/part/2";
                            Get100(url, yearForRequest, i);
                        }
                    }
                }
            }
            else
            {
                yearForRequest = new DateTime(DateTime.Now.Year, 1, 1);
                if (m == 10)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        for (int pak = 0; pak <= 12; pak++)
                        {
                            url = "http://10.20.17.178:8080/serviceTier/webapi/All200/officeId/" + pak.ToString("D2") + "000000" + "/year/" + (yearForRequest.Year + 543).ToString("D4") + "/month/" + i.ToString("D2") + "/part/2";
                            Get100(url, yearForRequest, i);
                        }
                    }
                }
                else
                {
                    for (int i = 10; i <= 12; i++)
                    {
                        for (int pak = 0; pak <= 12; pak++)
                        {
                            url = "http://10.20.17.178:8080/serviceTier/webapi/All200/officeId/" + pak.ToString("D2") + "000000" + "/year/" + (yearForRequest.Year + 543).ToString("D4") + "/month/" + i.ToString("D2") + "/part/2";
                            Get100(url, yearForRequest, i);
                        }
                    }

                    for (int i = 1; i < m; i++)
                    {
                        for (int pak = 0; pak <= 12; pak++)
                        {
                            url = "http://10.20.17.178:8080/serviceTier/webapi/All200/officeId/" + pak.ToString("D2") + "000000" + "/year/" + (yearForRequest.Year + 543).ToString("D4") + "/month/" + i.ToString("D2") + "/part/2";
                            Get100(url, yearForRequest, i);
                        }
                    }
                }
            }

            return LocalRedirect("/");
        }

        private void Get100(string url, DateTime yearForRequest, int i)
        {
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null)
            {
                return;
            }
            webRequest.ContentType = "application/json";
            webRequest.UserAgent = "Nothing";


            var yearForCompletedDate = yearForRequest.Year;
            if (i >= 10)
            {
                yearForCompletedDate = yearForCompletedDate - 1;
            }

            using (var s = webRequest.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var taxCollectionsAsJson = sr.ReadToEnd();
                    var taxCollections = JsonConvert.DeserializeObject<Rootobject>(taxCollectionsAsJson);
                    foreach (var t in taxCollections.taxCollection)
                    {
                        var dataForEvaluation = _context.DataForEvaluations
                                                .Where(d => d.Office.Code == t.officeCode && d.PointOfEvaluation.AutoApp == AutoApps.ผู้ประกอบการรายใหญ่ในท้องที่ && d.PointOfEvaluation.Year == yearForRequest && d.Month == i)
                                                .FirstOrDefault();

                        if (dataForEvaluation != null)
                        {
                            dataForEvaluation.Expect = t.CMlastYear / 1000000;
                            dataForEvaluation.ResultLevelRate = t.CMcurrentYear / 1000000;
                            //dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                            dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                            if(i >= 10 || i <= 3)
                            {
                                dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                            }
                            _context.SaveChanges();
                        }
                    }
                }
            }
        } //End get100

        public IActionResult GetCanvass56_1() //สน. ตัวชี้วัด 64 12
        {
            string url = "http://10.2.1.167/data/epes/canvass56-1.csv";
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null)
            {
                return LocalRedirect("/");
            }

            DateTime yearForRequest;

            using (var s = webRequest.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    //var canvass56_1 = sr.ReadToEnd();
                    DataForEvaluation dataForEvaluation;
                    var engine = new FileHelperEngine<Canvass56>();
                    var records = engine.ReadStream(sr);

                    decimal[][] sumExpect = new decimal[12][];
                    decimal[][] sumResult = new decimal[12][];

                    for (int i = 0; i < 12; i++)
                    {
                        sumExpect[i] = new decimal[13];
                        sumResult[i] = new decimal[13];
                    }

                    //return Ok(records);
                    foreach (var record in records)
                    {
                        if (record.month == 10 || record.month == 11 || record.month == 12)
                        {
                            yearForRequest = new DateTime(record.year - 542, 1, 1);
                        }
                        else
                        {
                            yearForRequest = new DateTime(record.year - 543, 1, 1);
                        }


                        var yearForCompletedDate = yearForRequest.Year;
                        if (record.month >= 10)
                        {
                            yearForCompletedDate = yearForCompletedDate - 1;
                        }


                        dataForEvaluation = _context.DataForEvaluations
                                                .Where(d => d.Office.Code == record.officeID && d.PointOfEvaluation.AutoApp == AutoApps.ผู้เสียภาษีรายใหม่ && d.PointOfEvaluation.Year == yearForRequest && d.Month == record.month)
                                                .FirstOrDefault();

                        if (dataForEvaluation != null)
                        {
                            dataForEvaluation.Expect = record.expect;
                            dataForEvaluation.Result = record.result;
                            // dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                            dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                            dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                            _context.SaveChanges();
                        }

                        year = yearForRequest;

                        switch (record.officeID.Substring(0, 2))
                        {
                            case "01":
                                sumExpect[0][record.month] += record.expect;
                                sumResult[0][record.month] += record.result;
                                break;
                            case "02":
                                sumExpect[1][record.month] += record.expect;
                                sumResult[1][record.month] += record.result;
                                break;
                            case "03":
                                sumExpect[2][record.month] += record.expect;
                                sumResult[2][record.month] += record.result;
                                break;
                            case "04":
                                sumExpect[3][record.month] += record.expect;
                                sumResult[3][record.month] += record.result;
                                break;
                            case "05":
                                sumExpect[4][record.month] += record.expect;
                                sumResult[4][record.month] += record.result;
                                break;
                            case "06":
                                sumExpect[5][record.month] += record.expect;
                                sumResult[5][record.month] += record.result;
                                break;
                            case "07":
                                sumExpect[6][record.month] += record.expect;
                                sumResult[6][record.month] += record.result;
                                break;
                            case "08":
                                sumExpect[7][record.month] += record.expect;
                                sumResult[7][record.month] += record.result;
                                break;
                            case "09":
                                sumExpect[8][record.month] += record.expect;
                                sumResult[8][record.month] += record.result;
                                break;
                            case "10":
                                sumExpect[9][record.month] += record.expect;
                                sumResult[9][record.month] += record.result;
                                break;
                            case "11":
                                sumExpect[10][record.month] += record.expect;
                                sumResult[10][record.month] += record.result;
                                break;
                            case "12":
                                sumExpect[11][record.month] += record.expect;
                                sumResult[11][record.month] += record.result;
                                break;
                        }
                    }// end foreach stream

                    for (int i = 1; i <= 12; i++)
                    {
                        for (int j = 1; j < 13; j++)
                        {
                            if (sumResult[i - 1][j] != 0)
                            {
                                dataForEvaluation = _context.DataForEvaluations
                                                .Where(d => d.Office.Code == i.ToString("D2") + "000000" && d.PointOfEvaluation.AutoApp == AutoApps.ผู้เสียภาษีรายใหม่ && d.PointOfEvaluation.Year == year && d.Month == j)
                                                .FirstOrDefault();

                                if (dataForEvaluation != null)
                                {
                                    dataForEvaluation.Expect = sumExpect[i - 1][j];
                                    dataForEvaluation.Result = sumResult[i - 1][j];
                                    if(j >= 10)
                                    {
                                        dataForEvaluation.CompletedDate = new DateTime(year.Year - 1, dataForEvaluation.Month, DateTime.DaysInMonth(year.Year -1, dataForEvaluation.Month));
                                    } else
                                    {
                                        dataForEvaluation.CompletedDate = new DateTime(year.Year, dataForEvaluation.Month, DateTime.DaysInMonth(year.Year, dataForEvaluation.Month));
                                    }
                                    dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                                    _context.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
            return LocalRedirect("/");

        } //End สน. ตัวชี้วัด 64 12

        public IActionResult GetCanvass56_2() //สน. ตัวชี้วัด 64 13
        {
            string url = "http://10.2.1.167/data/epes/canvass56-2.csv";
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null)
            {
                return LocalRedirect("/");
            }

            DateTime yearForRequest;

            using (var s = webRequest.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    //var canvass56_1 = sr.ReadToEnd();
                    DataForEvaluation dataForEvaluation;
                    var engine = new FileHelperEngine<Canvass56>();
                    var records = engine.ReadStream(sr);
                    //return Ok(records);

                    decimal[][] sumExpect = new decimal[12][];
                    decimal[][] sumResult = new decimal[12][];

                    for (int i = 0; i < 12; i++)
                    {
                        sumExpect[i] = new decimal[13];
                        sumResult[i] = new decimal[13];
                    }

                    foreach (var record in records)
                    {
                        if (record.month == 10 || record.month == 11 || record.month == 12)
                        {
                            yearForRequest = new DateTime(record.year - 542, 1, 1);
                        }
                        else
                        {
                            yearForRequest = new DateTime(record.year - 543, 1, 1);
                        }

                        var yearForCompletedDate = yearForRequest.Year;
                        if (record.month >= 10)
                        {
                            yearForCompletedDate = yearForCompletedDate - 1;
                        }

                        dataForEvaluation = _context.DataForEvaluations
                                                .Where(d => d.Office.Code == record.officeID && d.PointOfEvaluation.AutoApp == AutoApps.ผู้เสียภาษีรายใหม่ที่ชำระภาษี && d.PointOfEvaluation.Year == yearForRequest && d.Month == record.month)
                                                .FirstOrDefault();

                        if (dataForEvaluation != null)
                        {
                            dataForEvaluation.Expect = record.expect;
                            //dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                            dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                            dataForEvaluation.Result = record.result;
                            dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                            _context.SaveChanges();
                        }

                        year = yearForRequest;

                        switch (record.officeID.Substring(0, 2))
                        {
                            case "01":
                                sumExpect[0][record.month] += record.expect;
                                sumResult[0][record.month] += record.result;
                                break;
                            case "02":
                                sumExpect[1][record.month] += record.expect;
                                sumResult[1][record.month] += record.result;
                                break;
                            case "03":
                                sumExpect[2][record.month] += record.expect;
                                sumResult[2][record.month] += record.result;
                                break;
                            case "04":
                                sumExpect[3][record.month] += record.expect;
                                sumResult[3][record.month] += record.result;
                                break;
                            case "05":
                                sumExpect[4][record.month] += record.expect;
                                sumResult[4][record.month] += record.result;
                                break;
                            case "06":
                                sumExpect[5][record.month] += record.expect;
                                sumResult[5][record.month] += record.result;
                                break;
                            case "07":
                                sumExpect[6][record.month] += record.expect;
                                sumResult[6][record.month] += record.result;
                                break;
                            case "08":
                                sumExpect[7][record.month] += record.expect;
                                sumResult[7][record.month] += record.result;
                                break;
                            case "09":
                                sumExpect[8][record.month] += record.expect;
                                sumResult[8][record.month] += record.result;
                                break;
                            case "10":
                                sumExpect[9][record.month] += record.expect;
                                sumResult[9][record.month] += record.result;
                                break;
                            case "11":
                                sumExpect[10][record.month] += record.expect;
                                sumResult[10][record.month] += record.result;
                                break;
                            case "12":
                                sumExpect[11][record.month] += record.expect;
                                sumResult[11][record.month] += record.result;
                                break;
                        }
                    }
                    for (int i = 1; i <= 12; i++)
                    {
                        for (int j = 1; j < 13; j++)
                        {
                            if (sumResult[i - 1][j] != 0)
                            {
                                dataForEvaluation = _context.DataForEvaluations
                                                .Where(d => d.Office.Code == i.ToString("D2") + "000000" && d.PointOfEvaluation.AutoApp == AutoApps.ผู้เสียภาษีรายใหม่ที่ชำระภาษี && d.PointOfEvaluation.Year == year && d.Month == j)
                                                .FirstOrDefault();

                                if (dataForEvaluation != null)
                                {
                                    dataForEvaluation.Expect = sumExpect[i - 1][j];
                                    dataForEvaluation.Result = sumResult[i - 1][j];
                                    if(j >= 10)
                                    {
                                        dataForEvaluation.CompletedDate = new DateTime(year.Year - 1, dataForEvaluation.Month, DateTime.DaysInMonth(year.Year - 1, dataForEvaluation.Month));
                                    } else
                                    {
                                        dataForEvaluation.CompletedDate = new DateTime(year.Year, dataForEvaluation.Month, DateTime.DaysInMonth(year.Year, dataForEvaluation.Month));
                                    }
                                    
                                    dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                                    _context.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
            return LocalRedirect("/");

        } //End สน. ตัวชี้วัด 64 13

        public IActionResult GetCanvass56_3() //สน. ตัวชี้วัด 64 14
        {
            string url = "http://10.2.1.167/data/epes/canvass56-3.csv";
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null)
            {
                return LocalRedirect("/");
            }

            DateTime yearForRequest;

            using (var s = webRequest.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    //var canvass56_1 = sr.ReadToEnd();
                    DataForEvaluation dataForEvaluation;
                    var engine = new FileHelperEngine<Canvass56>();
                    var records = engine.ReadStream(sr);

                    decimal[][] sumExpect = new decimal[12][];
                    decimal[][] sumResult = new decimal[12][];

                    for (int i = 0; i < 12; i++)
                    {
                        sumExpect[i] = new decimal[13];
                        sumResult[i] = new decimal[13];
                    }
                    //return Ok(records);
                    foreach (var record in records)
                    {
                        if (record.month == 10 || record.month == 11 || record.month == 12)
                        {
                            yearForRequest = new DateTime(record.year - 542, 1, 1);
                        }
                        else
                        {
                            yearForRequest = new DateTime(record.year - 543, 1, 1);
                        }

                        var yearForCompletedDate = yearForRequest.Year;
                        if (record.month >= 10)
                        {
                            yearForCompletedDate = yearForCompletedDate - 1;
                        }

                        dataForEvaluation = _context.DataForEvaluations
                                                .Where(d => d.Office.Code == record.officeID && d.PointOfEvaluation.AutoApp == AutoApps.การนำข้อมูลสำรวจไปใช้งาน && d.PointOfEvaluation.Year == yearForRequest && d.Month == record.month)
                                                .FirstOrDefault();

                        if (dataForEvaluation != null)
                        {
                            dataForEvaluation.Expect = record.expect;
                            dataForEvaluation.Result = record.result;
                            //dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                            dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                            dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                            _context.SaveChanges();
                        }

                        year = yearForRequest;

                        switch (record.officeID.Substring(0, 2))
                        {
                            case "01":
                                sumExpect[0][record.month] += record.expect;
                                sumResult[0][record.month] += record.result;
                                break;
                            case "02":
                                sumExpect[1][record.month] += record.expect;
                                sumResult[1][record.month] += record.result;
                                break;
                            case "03":
                                sumExpect[2][record.month] += record.expect;
                                sumResult[2][record.month] += record.result;
                                break;
                            case "04":
                                sumExpect[3][record.month] += record.expect;
                                sumResult[3][record.month] += record.result;
                                break;
                            case "05":
                                sumExpect[4][record.month] += record.expect;
                                sumResult[4][record.month] += record.result;
                                break;
                            case "06":
                                sumExpect[5][record.month] += record.expect;
                                sumResult[5][record.month] += record.result;
                                break;
                            case "07":
                                sumExpect[6][record.month] += record.expect;
                                sumResult[6][record.month] += record.result;
                                break;
                            case "08":
                                sumExpect[7][record.month] += record.expect;
                                sumResult[7][record.month] += record.result;
                                break;
                            case "09":
                                sumExpect[8][record.month] += record.expect;
                                sumResult[8][record.month] += record.result;
                                break;
                            case "10":
                                sumExpect[9][record.month] += record.expect;
                                sumResult[9][record.month] += record.result;
                                break;
                            case "11":
                                sumExpect[10][record.month] += record.expect;
                                sumResult[10][record.month] += record.result;
                                break;
                            case "12":
                                sumExpect[11][record.month] += record.expect;
                                sumResult[11][record.month] += record.result;
                                break;
                        }
                    }
                    for (int i = 1; i <= 12; i++)
                    {
                        for (int j = 1; j < 13; j++)
                        {
                            if (sumResult[i - 1][j] != 0)
                            {
                                dataForEvaluation = _context.DataForEvaluations
                                                .Where(d => d.Office.Code == i.ToString("D2") + "000000" && d.PointOfEvaluation.AutoApp == AutoApps.การนำข้อมูลสำรวจไปใช้งาน && d.PointOfEvaluation.Year == year && d.Month == j)
                                                .FirstOrDefault();

                                if (dataForEvaluation != null)
                                {
                                    dataForEvaluation.Expect = sumExpect[i - 1][j];
                                    dataForEvaluation.Result = sumResult[i - 1][j];
                                    if(j >= 10)
                                    {
                                        dataForEvaluation.CompletedDate = new DateTime(year.Year - 1, dataForEvaluation.Month, DateTime.DaysInMonth(year.Year - 1, dataForEvaluation.Month));
                                    } else
                                    {
                                        dataForEvaluation.CompletedDate = new DateTime(year.Year, dataForEvaluation.Month, DateTime.DaysInMonth(year.Year, dataForEvaluation.Month));
                                    }
                                    
                                    dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                                    _context.SaveChanges();
                                }
                            }
                        }
                    }//end pak
                    var offices = _context.Offices.Where(o => o.Code.Substring(0, 2) != "00" && o.Code.Substring(5, 3) == "000");
                    foreach (var item in offices)
                    {
                        if (DateTime.Now.Month < 10)
                        {
                            for (int j = 10; j <= 12; j++)
                            {
                                dataForEvaluation = _context.DataForEvaluations
                                                .Where(d => d.OfficeId == item.Id && d.PointOfEvaluation.AutoApp == AutoApps.การนำข้อมูลสำรวจไปใช้งาน && d.PointOfEvaluation.Year == year && d.Month == j && d.Approve != Approve.ผษ_อนุมัติ)
                                                .FirstOrDefault();

                                if (dataForEvaluation != null)
                                {
                                    dataForEvaluation.Expect = 0;
                                    dataForEvaluation.Result = 0;
                                    if (j >= 10)
                                    {
                                        dataForEvaluation.CompletedDate = new DateTime(year.Year - 1, dataForEvaluation.Month, DateTime.DaysInMonth(year.Year - 1, dataForEvaluation.Month));
                                    }
                                    else
                                    {
                                        dataForEvaluation.CompletedDate = new DateTime(year.Year, dataForEvaluation.Month, DateTime.DaysInMonth(year.Year, dataForEvaluation.Month));
                                    }
                                    dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                                    _context.SaveChanges();
                                }
                            }
                            for (int j = 1; j <= DateTime.Now.Month; j++)
                            {
                                dataForEvaluation = _context.DataForEvaluations
                                                .Where(d => d.OfficeId == item.Id && d.PointOfEvaluation.AutoApp == AutoApps.การนำข้อมูลสำรวจไปใช้งาน && d.PointOfEvaluation.Year == year && d.Month == j && d.Approve != Approve.ผษ_อนุมัติ)
                                                .FirstOrDefault();

                                if (dataForEvaluation != null)
                                {
                                    dataForEvaluation.Expect = 0;
                                    dataForEvaluation.Result = 0;
                                    if (j >= 10)
                                    {
                                        dataForEvaluation.CompletedDate = new DateTime(year.Year - 1, dataForEvaluation.Month, DateTime.DaysInMonth(year.Year - 1, dataForEvaluation.Month));
                                    }
                                    else
                                    {
                                        dataForEvaluation.CompletedDate = new DateTime(year.Year, dataForEvaluation.Month, DateTime.DaysInMonth(year.Year, dataForEvaluation.Month));
                                    }
                                    dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                                    _context.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            for (int j = 10; j <= DateTime.Now.Month; j++)
                            {
                                dataForEvaluation = _context.DataForEvaluations
                                                .Where(d => d.OfficeId == item.Id && d.PointOfEvaluation.AutoApp == AutoApps.การนำข้อมูลสำรวจไปใช้งาน && d.PointOfEvaluation.Year == year && d.Month == j && d.Approve != Approve.ผษ_อนุมัติ)
                                                .FirstOrDefault();

                                if (dataForEvaluation != null)
                                {
                                    dataForEvaluation.Expect = 0;
                                    dataForEvaluation.Result = 0;
                                    if (j >= 10)
                                    {
                                        dataForEvaluation.CompletedDate = new DateTime(year.Year - 1, dataForEvaluation.Month, DateTime.DaysInMonth(year.Year - 1, dataForEvaluation.Month));
                                    }
                                    else
                                    {
                                        dataForEvaluation.CompletedDate = new DateTime(year.Year, dataForEvaluation.Month, DateTime.DaysInMonth(year.Year, dataForEvaluation.Month));
                                    }
                                    dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                                    _context.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }

            //UpdateApprove();
            return LocalRedirect("/");

        } //End สน. ตัวชี้วัด 64 14

        public IActionResult GetP5() // [มจ.] ร้อยละของการบริหารการคืนภาษี ตัวชี้วัด 64/11, 65/9
        {
            string url = "http://localhost:8082/morjor/p5.csv";
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null)
            {
                return LocalRedirect("/");
            }

            DateTime yearForRequest;

            using (var s = webRequest.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    //var canvass56_1 = sr.ReadToEnd();
                    var engine = new FileHelperEngine<P11>();
                    var records = engine.ReadStream(sr);
                    //return Ok(records);
                    foreach (var record in records)
                    {
                        yearForRequest = new DateTime(record.year - 543, 1, 1);

                        var yearForCompletedDate = yearForRequest.Year;
                        if (record.month >= 10)
                        {
                            yearForCompletedDate = yearForCompletedDate - 1;
                        }

                        if (record.month == 1)
                        {
                            var dataForEvaluation = _context.DataForEvaluations
                                                .Where(d => d.Office.Code == record.officeID && d.PointOfEvaluation.AutoApp == AutoApps.ร้อยละของการบริหารการคืนภาษี && d.PointOfEvaluation.Year == yearForRequest && d.Month == record.month)
                                                .FirstOrDefault();
                            if (dataForEvaluation != null)
                            {
                                dataForEvaluation.Expect = record.expect;
                                dataForEvaluation.Result = record.result;
                                //dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                                dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                                dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                                _context.SaveChanges();
                            }
                            for (int i = 10; i <= 12; i++)
                            {
                                dataForEvaluation = _context.DataForEvaluations
                                                .Where(d => d.Office.Code == record.officeID && d.PointOfEvaluation.AutoApp == AutoApps.ร้อยละของการบริหารการคืนภาษี && d.PointOfEvaluation.Year == yearForRequest && d.Month == i)
                                                .FirstOrDefault();
                                if (dataForEvaluation != null)
                                {
                                    dataForEvaluation.Expect = 0;
                                    dataForEvaluation.Result = 0;
                                    //dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                                    dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                                    dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                                    _context.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            decimal sumExpect = 0;
                            decimal sumResult = 0;
                            for (int i = 1; i < record.month; i++)
                            {
                                var data = _context.DataForEvaluations
                                                .Where(d => d.Office.Code == record.officeID && d.PointOfEvaluation.AutoApp == AutoApps.ร้อยละของการบริหารการคืนภาษี && d.PointOfEvaluation.Year == yearForRequest && d.Month == i)
                                                .FirstOrDefault();
                                if (data != null)
                                {
                                    sumExpect += data.Expect;
                                    sumResult += data.Result;
                                }
                            }
                            var dataForEvaluation = _context.DataForEvaluations
                                                .Where(d => d.Office.Code == record.officeID && d.PointOfEvaluation.AutoApp == AutoApps.ร้อยละของการบริหารการคืนภาษี && d.PointOfEvaluation.Year == yearForRequest && d.Month == record.month)
                                                .FirstOrDefault();
                            if (dataForEvaluation != null)
                            {
                                dataForEvaluation.Expect = record.expect - sumExpect;
                                dataForEvaluation.Result = record.result - sumResult;
                                //dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                                dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                                dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                                _context.SaveChanges();
                            }
                        }
                    }
                }
            }

            //UpdateApprove();
            return LocalRedirect("/");

        } //End มจ. ตัวชี้วัด 64 11

        public IActionResult GetP7() // [บอ.] จำนวนแบบที่ยื่นผ่านอินเทอร์เน็ต_ยกเว้น_90_91_94 ตัวชี้วัด 64/15, 65/13
        {
            string url = "http://localhost:8082/boror/p7.csv";
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null)
            {
                return LocalRedirect("/");
            }

            DateTime yearForRequest;

            using (var s = webRequest.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var engine = new FileHelperEngine<P11>();
                    var records = engine.ReadStream(sr);

                    foreach (var record in records)
                    {
                        yearForRequest = new DateTime(record.year - 543, 1, 1);

                        var yearForCompletedDate = yearForRequest.Year;
                        if (record.month >= 10)
                        {
                            yearForCompletedDate = yearForCompletedDate - 1;
                        }

                        var dataForEvaluation = _context.DataForEvaluations
                                            .Where(d => d.Office.Code == record.officeID && d.PointOfEvaluation.AutoApp == AutoApps.จำนวนแบบที่ยื่นผ่านอินเทอร์เน็ต_ยกเว้น_90_91_94 && d.PointOfEvaluation.Year == yearForRequest && d.Month == record.month)
                                            .FirstOrDefault();
                        if (dataForEvaluation != null)
                        {
                            dataForEvaluation.Expect = record.expect;
                            dataForEvaluation.Result = record.result;
                            //dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                            dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                            dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                            _context.SaveChanges();
                        }
                    }
                }
            }

            //UpdateApprove();
            return LocalRedirect("/");

        } //End มจ. ตัวชี้วัด 64 15

        public IActionResult GetP8() // [บอ.] จำนวนแบบ_1_30_50_ที่ยื่นผ่านอินเทอร์เน็ต  ตัวชี้วัด 64/16, 65/14
        {
            string url = "http://localhost:8082/boror/p8.csv";
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null)
            {
                return LocalRedirect("/");
            }

            DateTime yearForRequest;

            using (var s = webRequest.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var engine = new FileHelperEngine<P11>();
                    var records = engine.ReadStream(sr);

                    foreach (var record in records)
                    {
                        yearForRequest = new DateTime(record.year - 543, 1, 1);

                        var yearForCompletedDate = yearForRequest.Year;
                        if (record.month >= 10)
                        {
                            yearForCompletedDate = yearForCompletedDate - 1;
                        }

                        var dataForEvaluation = _context.DataForEvaluations
                                            .Where(d => d.Office.Code == record.officeID && d.PointOfEvaluation.AutoApp == AutoApps.จำนวนแบบ_1_30_50_ที่ยื่นผ่านอินเทอร์เน็ต && d.PointOfEvaluation.Year == yearForRequest && d.Month == record.month)
                                            .FirstOrDefault();
                        if (dataForEvaluation != null)
                        {
                            dataForEvaluation.Expect = record.expect;
                            dataForEvaluation.Result = record.result;
                            //dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                            dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                            dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                            _context.SaveChanges();
                        }
                    }
                }
            }

            //UpdateApprove();
            return LocalRedirect("/");

        } //End มจ. ตัวชี้วัด 64 16

        public IActionResult GetP8_10() //มจ. ตัวชี้วัด 64 8-10
        {
            string url = "http://localhost:8082/morjor/p8.csv";
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null)
            {
                return LocalRedirect("/");
            }

            DateTime yearForRequest;

            using (var s = webRequest.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var engine = new FileHelperEngine<P11>();
                    var records = engine.ReadStream(sr);

                    foreach (var record in records)
                    {
                        yearForRequest = new DateTime(record.year - 543, 1, 1);
                        var yearForCompletedDate = yearForRequest.Year;
                        if (record.month >= 10)
                        {
                            yearForCompletedDate = yearForCompletedDate - 1;
                        }

                        var office = _context.Offices.Where(o => o.Code == record.officeID).FirstOrDefault();
                        var poe = _context.PointOfEvaluations.Where(p => p.AutoApp == AutoApps.เร่งรัดหนี้ && p.Year == yearForRequest && p.OwnerOfficeId == office.Id)
                                .FirstOrDefault();

                        var dataForEvaluation = _context.DataForEvaluations
                                            .Where(d => d.OfficeId == office.Id && d.PointOfEvaluationId == poe.Id && d.Month == record.month)
                                            .FirstOrDefault();

                        if (dataForEvaluation != null)
                        {
                            dataForEvaluation.Expect = record.expect;
                            dataForEvaluation.Result = record.result;
                            //dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                            dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                            dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                            _context.SaveChanges();
                        }
                    }
                }
            }

            url = "http://localhost:8082/morjor/p9.csv";
            webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null)
            {
                return LocalRedirect("/");
            }

            using (var s = webRequest.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var engine = new FileHelperEngine<P11>();
                    var records = engine.ReadStream(sr);

                    foreach (var record in records)
                    {
                        yearForRequest = new DateTime(record.year - 543, 1, 1);

                        var yearForCompletedDate = yearForRequest.Year;
                        if (record.month >= 10)
                        {
                            yearForCompletedDate = yearForCompletedDate - 1;
                        }

                        var dataForEvaluation = _context.DataForEvaluations
                                            .Where(d => d.Office.Code == record.officeID && d.PointOfEvaluation.AutoApp == AutoApps.จำหน่ายหนี้ && d.PointOfEvaluation.Year == yearForRequest && d.Month == record.month)
                                            .FirstOrDefault();
                        if (dataForEvaluation != null)
                        {
                            dataForEvaluation.Expect = record.expect;
                            dataForEvaluation.Result = record.result;
                            // dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                            dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                            dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                            _context.SaveChanges();
                        }
                    }
                }
            }

            url = "http://localhost:8082/morjor/p10.csv";
            webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null)
            {
                return LocalRedirect("/");
            }

            using (var s = webRequest.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var engine = new FileHelperEngine<P11>();
                    var records = engine.ReadStream(sr);

                    foreach (var record in records)
                    {
                        yearForRequest = new DateTime(record.year - 543, 1, 1);

                        var yearForCompletedDate = yearForRequest.Year;
                        if (record.month >= 10)
                        {
                            yearForCompletedDate = yearForCompletedDate - 1;
                        }

                        var dataForEvaluation = _context.DataForEvaluations
                                            .Where(d => d.Office.Code == record.officeID && d.PointOfEvaluation.AutoApp == AutoApps.ใบแจ้งภาษีอากรบนระบบ_DMS && d.PointOfEvaluation.Year == yearForRequest && d.Month == record.month)
                                            .FirstOrDefault();
                        if (dataForEvaluation != null)
                        {
                            dataForEvaluation.Expect = record.expect;
                            dataForEvaluation.Result = record.result;
                            //dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                            dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                            dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                            _context.SaveChanges();
                        }
                    }
                }
            }

            //UpdateApprove();
            return LocalRedirect("/");

        } //End มจ. ตัวชี้วัด 64 8-10

        public IActionResult GetP10() // 
        {
            string url = "http://localhost:8082/morjor/p10.csv";
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null)
            {
                return LocalRedirect("/");
            }

            DateTime yearForRequest;

            using (var s = webRequest.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var engine = new FileHelperEngine<P11>();
                    var records = engine.ReadStream(sr);

                    foreach (var record in records)
                    {
                        yearForRequest = new DateTime(record.year - 543, 1, 1);

                        var yearForCompletedDate = yearForRequest.Year;
                        if (record.month >= 10)
                        {
                            yearForCompletedDate = yearForCompletedDate - 1;
                        }

                        var dataForEvaluation = _context.DataForEvaluations
                                            .Where(d => d.Office.Code == record.officeID && d.PointOfEvaluation.AutoApp == AutoApps.จำหน่ายหนี้ && d.PointOfEvaluation.Year == yearForRequest && d.Month == record.month)
                                            .FirstOrDefault();
                        if (dataForEvaluation != null)
                        {
                            dataForEvaluation.Expect = record.expect;
                            dataForEvaluation.Result = record.result;
                            //dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                            dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                            dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                            _context.SaveChanges();
                        }
                    }
                }
            }
            //UpdateApprove();
            return LocalRedirect("/");
        }

        public IActionResult GetP9()
        {
            string url = "http://localhost:8082/morjor/p9.csv";
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null)
            {
                return LocalRedirect("/");
            }

            DateTime yearForRequest;

            using (var s = webRequest.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var engine = new FileHelperEngine<P11>();
                    var records = engine.ReadStream(sr);

                    foreach (var record in records)
                    {
                        yearForRequest = new DateTime(record.year - 543, 1, 1);

                        var yearForCompletedDate = yearForRequest.Year;
                        if (record.month >= 10)
                        {
                            yearForCompletedDate = yearForCompletedDate - 1;
                        }

                        var dataForEvaluation = _context.DataForEvaluations
                                            .Where(d => d.Office.Code == record.officeID && d.PointOfEvaluation.AutoApp == AutoApps.เร่งรัดหนี้ && d.PointOfEvaluation.Year == yearForRequest && d.Month == record.month)
                                            .FirstOrDefault();
                        if (dataForEvaluation != null)
                        {
                            dataForEvaluation.Expect = record.expect;
                            dataForEvaluation.Result = record.result;
                            //dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                            dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                            dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                            _context.SaveChanges();
                        }
                    }
                }
            }
            //UpdateApprove();
            return LocalRedirect("/");
        }

        public IActionResult GetP2_7() //มก. ตัวชี้วัด 64 2-7
        {
            string url = "http://localhost:8082/morgor/p2.csv";
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null)
            {
                return LocalRedirect("/");
            }

            DateTime yearForRequest;

            using (var s = webRequest.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var engine = new FileHelperEngine<P11>();
                    var records = engine.ReadStream(sr);

                    foreach (var record in records)
                    {
                        yearForRequest = new DateTime(record.year - 543, 1, 1);

                        var yearForCompletedDate = yearForRequest.Year;
                        if (record.month >= 10)
                        {
                            yearForCompletedDate = yearForCompletedDate - 1;
                        }

                        var dataForEvaluation = _context.DataForEvaluations
                                            .Where(d => d.Office.Code == record.officeID && d.PointOfEvaluation.AutoApp == AutoApps.ร้อยละของการตรวจคืนภาษี && d.PointOfEvaluation.Year == yearForRequest && d.Month == record.month)
                                            .FirstOrDefault();
                        if (dataForEvaluation != null)
                        {
                            dataForEvaluation.Expect = record.expect;
                            dataForEvaluation.Result = record.result;
                            //dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                            dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                            dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                            _context.SaveChanges();
                        }
                    }
                }
            }

            url = "http://localhost:8082/morgor/p3.csv";
            webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null)
            {
                return LocalRedirect("/");
            }

            using (var s = webRequest.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var engine = new FileHelperEngine<P11>();
                    var records = engine.ReadStream(sr);

                    foreach (var record in records)
                    {
                        yearForRequest = new DateTime(record.year - 543, 1, 1);

                        var yearForCompletedDate = yearForRequest.Year;
                        if (record.month >= 10)
                        {
                            yearForCompletedDate = yearForCompletedDate - 1;
                        }

                        var dataForEvaluation = _context.DataForEvaluations
                                            .Where(d => d.Office.Code == record.officeID && d.PointOfEvaluation.AutoApp == AutoApps.ร้อยละของการแนะนำและตรวจสอบภาษี && d.PointOfEvaluation.Year == yearForRequest && d.Month == record.month)
                                            .FirstOrDefault();
                        if (dataForEvaluation != null)
                        {
                            dataForEvaluation.Expect = record.expect;
                            dataForEvaluation.Result = record.result;
                            //dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                            dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                            dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                            _context.SaveChanges();
                        }
                    }
                }
            }

            url = "http://localhost:8082/morgor/p4.csv";
            webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null)
            {
                return LocalRedirect("/");
            }

            using (var s = webRequest.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var engine = new FileHelperEngine<P11>();
                    var records = engine.ReadStream(sr);

                    foreach (var record in records)
                    {
                        yearForRequest = new DateTime(record.year - 543, 1, 1);

                        var yearForCompletedDate = yearForRequest.Year;
                        if (record.month >= 10)
                        {
                            yearForCompletedDate = yearForCompletedDate - 1;
                        }

                        var dataForEvaluation = _context.DataForEvaluations
                                            .Where(d => d.Office.Code == record.officeID && d.PointOfEvaluation.AutoApp == AutoApps.ร้อยละของผู้ประกอบการที่ดำเนินการแนะนำ && d.PointOfEvaluation.Year == yearForRequest && d.Month == record.month)
                                            .FirstOrDefault();
                        if (dataForEvaluation != null)
                        {
                            dataForEvaluation.Expect = record.expect;
                            dataForEvaluation.Result = record.result;
                            //dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                            dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                            dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                            _context.SaveChanges();
                        }
                    }
                }
            }

            url = "http://localhost:8082/morgor/p5.1.csv";
            webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null)
            {
                return LocalRedirect("/");
            }

            using (var s = webRequest.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var engine = new FileHelperEngine<P11>();
                    var records = engine.ReadStream(sr);

                    foreach (var record in records)
                    {
                        yearForRequest = new DateTime(record.year - 543, 1, 1);

                        var yearForCompletedDate = yearForRequest.Year;
                        if (record.month >= 10)
                        {
                            yearForCompletedDate = yearForCompletedDate - 1;
                        }

                        var dataForEvaluation = _context.DataForEvaluations
                                            .Where(d => d.Office.Code == record.officeID && d.PointOfEvaluation.AutoApp == AutoApps.ร้อยละของการดำเนินการงานค้างสอบยันใบกำกับภาษี && d.PointOfEvaluation.Year == yearForRequest && d.Month == record.month)
                                            .FirstOrDefault();
                        if (dataForEvaluation != null)
                        {
                            dataForEvaluation.Expect = record.expect;
                            dataForEvaluation.Result = record.result;
                            //dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                            dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                            dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                            _context.SaveChanges();
                        }
                    }
                }
            }

            url = "http://localhost:8082/morgor/p5.2.csv";
            webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null)
            {
                return LocalRedirect("/");
            }

            using (var s = webRequest.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var engine = new FileHelperEngine<P11>();
                    var records = engine.ReadStream(sr);

                    foreach (var record in records)
                    {
                        yearForRequest = new DateTime(record.year - 543, 1, 1);

                        var yearForCompletedDate = yearForRequest.Year;
                        if (record.month >= 10)
                        {
                            yearForCompletedDate = yearForCompletedDate - 1;
                        }

                        var dataForEvaluation = _context.DataForEvaluations
                                            .Where(d => d.Office.Code == record.officeID && d.PointOfEvaluation.AutoApp == AutoApps.ร้อยละของการสอบยันใบกำกับภาษีที่ได้รับ && d.PointOfEvaluation.Year == yearForRequest && d.Month == record.month)
                                            .FirstOrDefault();
                        if (dataForEvaluation != null)
                        {
                            dataForEvaluation.Expect = record.expect;
                            dataForEvaluation.Result = record.result;
                            //dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                            dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                            dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                            _context.SaveChanges();
                        }
                    }
                }
            }

            url = "http://localhost:8082/morgor/p5.3.csv";
            webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null)
            {
                return LocalRedirect("/");
            }

            using (var s = webRequest.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var engine = new FileHelperEngine<P11>();
                    var records = engine.ReadStream(sr);

                    foreach (var record in records)
                    {
                        yearForRequest = new DateTime(record.year - 543, 1, 1);

                        var yearForCompletedDate = yearForRequest.Year;
                        if (record.month >= 10)
                        {
                            yearForCompletedDate = yearForCompletedDate - 1;
                        }

                        var dataForEvaluation = _context.DataForEvaluations
                                            .Where(d => d.Office.Code == record.officeID && d.PointOfEvaluation.AutoApp == AutoApps.ร้อยละของจำนวนรายที่มีผลการสอบยันใบกำกับภาษีพบประเด็นความผิด && d.PointOfEvaluation.Year == yearForRequest && d.Month == record.month)
                                            .FirstOrDefault();
                        if (dataForEvaluation != null)
                        {
                            dataForEvaluation.Expect = record.expect;
                            dataForEvaluation.Result = record.result;
                            //dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                            dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                            dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                            _context.SaveChanges();
                        }
                    }
                }
            }

            url = "http://localhost:8082/morgor/p6.csv";
            webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null)
            {
                return LocalRedirect("/");
            }

            using (var s = webRequest.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var engine = new FileHelperEngine<P11>();
                    var records = engine.ReadStream(sr);

                    foreach (var record in records)
                    {
                        yearForRequest = new DateTime(record.year - 543, 1, 1);

                        var yearForCompletedDate = yearForRequest.Year;
                        if (record.month >= 10)
                        {
                            yearForCompletedDate = yearForCompletedDate - 1;
                        }

                        var dataForEvaluation = _context.DataForEvaluations
                                            .Where(d => d.Office.Code == record.officeID && d.PointOfEvaluation.AutoApp == AutoApps.ร้อยละของการแนะนำและตรวจสอบภาษีอากรผู้เสียภาษีอากรรายกลางและรายย่อม && d.PointOfEvaluation.Year == yearForRequest && d.Month == record.month)
                                            .FirstOrDefault();
                        if (dataForEvaluation != null)
                        {
                            dataForEvaluation.Expect = record.expect;
                            dataForEvaluation.Result = record.result;
                            //dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                            dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                            dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                            _context.SaveChanges();
                        }
                    }
                }
            }

            url = "http://localhost:8082/morgor/p7.csv";
            webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null)
            {
                return LocalRedirect("/");
            }

            using (var s = webRequest.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var engine = new FileHelperEngine<P11>();
                    var records = engine.ReadStream(sr);

                    foreach (var record in records)
                    {
                        yearForRequest = new DateTime(record.year - 543, 1, 1);

                        var yearForCompletedDate = yearForRequest.Year;
                        if (record.month >= 10)
                        {
                            yearForCompletedDate = yearForCompletedDate - 1;
                        }

                        var dataForEvaluation = _context.DataForEvaluations
                                            .Where(d => d.Office.Code == record.officeID && d.PointOfEvaluation.AutoApp == AutoApps.ร้อยละของผู้เสียภาษีอากรที่ดำเนินการแนะนำและตรวจสอบภาษีอากรแล้วเสร็จ && d.PointOfEvaluation.Year == yearForRequest && d.Month == record.month)
                                            .FirstOrDefault();
                        if (dataForEvaluation != null)
                        {
                            dataForEvaluation.Expect = record.expect;
                            dataForEvaluation.Result = record.result;
                            //dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                            dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                            dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                            _context.SaveChanges();
                        }
                    }
                }
            }

            return LocalRedirect("/");

        } //End มก. ตัวชี้วัด 64 2-7

        public IActionResult GetMorgor()
        {
            AutoApps[] appArr = { AutoApps.ร้อยละของการแนะนำและตรวจสอบภาษี, AutoApps.ร้อยละของผู้ประกอบการที่ดำเนินการแนะนำ, AutoApps.ร้อยละของการดำเนินการงานค้างสอบยันใบกำกับภาษี, AutoApps.ร้อยละของการสอบยันใบกำกับภาษีที่ได้รับ, AutoApps.ร้อยละของจำนวนรายที่มีผลการสอบยันใบกำกับภาษีพบประเด็นความผิด, AutoApps.ร้อยละของการแนะนำและตรวจสอบภาษีอากรผู้เสียภาษีอากรรายกลางและรายย่อม, AutoApps.ร้อยละของผู้เสียภาษีอากรที่ดำเนินการแนะนำและตรวจสอบภาษีอากรแล้วเสร็จ };

            foreach(AutoApps app in appArr)
            {
                string url = "http://localhost:8082/morgor/p" + ((int)app) + ".csv";
                HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
                if (webRequest == null)
                {
                    return LocalRedirect("/");
                }

                DateTime yearForRequest;

                using (var s = webRequest.GetResponse().GetResponseStream())
                {
                    using (var sr = new StreamReader(s))
                    {
                        var engine = new FileHelperEngine<P11>();
                        var records = engine.ReadStream(sr);

                        foreach (var record in records)
                        {
                            yearForRequest = new DateTime(record.year - 543, 1, 1);

                            var yearForCompletedDate = yearForRequest.Year;
                            if (record.month >= 10)
                            {
                                yearForCompletedDate = yearForCompletedDate - 1;
                            }

                            var dataForEvaluation = _context.DataForEvaluations
                                                .Where(d => d.Office.Code == record.officeID && d.PointOfEvaluation.AutoApp == app && d.PointOfEvaluation.Year == yearForRequest && d.Month == record.month)
                                                .FirstOrDefault();
                            if (dataForEvaluation != null)
                            {
                                dataForEvaluation.Expect = record.expect;
                                dataForEvaluation.Result = record.result;
                                //dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                                dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                                dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                                _context.SaveChanges();
                            }
                        }
                    }
                }


            }


            return LocalRedirect("/");
        }

        public IActionResult GetP20()
        {

            string url = "http://localhost:8082/borror/p20.csv";
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null)
            {
                return LocalRedirect("/");
            }

            DateTime yearForRequest;

            using (var s = webRequest.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var engine = new FileHelperEngine<P11>();
                    var records = engine.ReadStream(sr);

                    foreach (var record in records)
                    {
                        yearForRequest = new DateTime(record.year - 543, 1, 1);

                        var yearForCompletedDate = yearForRequest.Year;
                        if (record.month >= 10)
                        {
                            yearForCompletedDate = yearForCompletedDate - 1;
                        }

                        var dataForEvaluation = _context.DataForEvaluations
                                            .Where(d => d.Office.Code == record.officeID && d.PointOfEvaluation.AutoApp == AutoApps.การเบิกจ่ายงบประมาณ && d.PointOfEvaluation.Year == yearForRequest && d.Month == record.month)
                                            .FirstOrDefault();
                        if (dataForEvaluation != null)
                        {
                            dataForEvaluation.Expect = record.expect;
                            dataForEvaluation.Result = record.result;
                            //dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                            dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                            dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                            _context.SaveChanges();
                        }
                    }
                }
            }
            return LocalRedirect("/");
        }

        public IActionResult GetP21()
        {
            //string url = "http://localhost:8082/sornor/p21.csv";
            string url = "http://10.2.1.167/data/epes/brq/brqepes.csv";
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null)
            {
                return LocalRedirect("/");
            }

            DateTime yearForRequest;
            int yearForCompletedDate;

            using (var s = webRequest.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var engine = new FileHelperEngine<P11>();
                    var records = engine.ReadStream(sr);

                    foreach (var record in records)
                    {
                        if (record.month >= 10)
                        {
                            yearForRequest = new DateTime(record.year - 542, 1, 1);
                            yearForCompletedDate = yearForRequest.Year - 1;
                        } else
                        {
                            yearForRequest = new DateTime(record.year - 543, 1, 1);
                            yearForCompletedDate = yearForRequest.Year;
                        }

                        var dataForEvaluation = _context.DataForEvaluations
                                            .Where(d => d.Office.Code == record.officeID && d.PointOfEvaluation.AutoApp == AutoApps.งานค้างหนังสือร้องเรียนแหล่งภาษี && d.PointOfEvaluation.Year == yearForRequest && d.Month == record.month)
                                            .FirstOrDefault();
                        if (dataForEvaluation != null)
                        {
                            dataForEvaluation.Expect = record.expect;
                            dataForEvaluation.Result = record.result;
                            //dataForEvaluation.CompletedDate = new DateTime(yearForRequest.Year, dataForEvaluation.Month, DateTime.DaysInMonth(yearForRequest.Year, dataForEvaluation.Month));
                            dataForEvaluation.CompletedDate = new DateTime(yearForCompletedDate, dataForEvaluation.Month, DateTime.DaysInMonth(yearForCompletedDate, dataForEvaluation.Month));
                            dataForEvaluation.Approve = Approve.ผษ_อนุมัติ;
                            _context.SaveChanges();
                        }

                    }
                }
            }

            return LocalRedirect("/");

        }

        public IActionResult GetExpectByProgramID()
        {
            string url = "http://localhost:8082/expect/expect.csv";
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null)
            {
                return LocalRedirect("/");
            }

            DateTime yearForRequest;

            using (var s = webRequest.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var engine = new FileHelperEngine<Expect>();
                    var records = engine.ReadStream(sr);

                    foreach (var record in records)
                    {
                        yearForRequest = new DateTime(record.year - 543, 1, 1);

                        string officeID;
                        if(record.officeID == "00000722")
                        {
                            officeID = "00009000";
                        } else
                        {
                            officeID = record.officeID;
                        }

                        var dataForEvaluation = _context.DataForEvaluations
                                            .Where(d => d.Office.Code == officeID && d.PointOfEvaluation.Point == record.point && d.PointOfEvaluation.Year == yearForRequest && d.Month == record.month)
                                            .FirstOrDefault();
                        if (dataForEvaluation != null)
                        {
                            dataForEvaluation.Expect = record.expect;
                            _context.SaveChanges();
                        }
                    }
                }
            }
            return LocalRedirect("/");
        }
    }
}