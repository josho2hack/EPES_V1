﻿using EPES.Data;
using EPES.Models;
using EPES.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EPES.Controllers
{
    public class AuditController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;

        public AuditController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Audit
        public async Task<IActionResult> Index(string selectoffice, bool isUpdate = false, int month = 0, int yearPoint = 0)
        {
            var user = await _userManager.GetUserAsync(User);
            var viewModel = new ResultViewModel();
            int m;
            DateTime yearForQuery;
            List<Object> list = new List<object>();

            //บันทึกย้อนหลัง 1 เดือน
            if (DateTime.Now.Month == 1)
            {
                m = 12;
            }
            else
            {
                m = DateTime.Now.Month - 1;
            }

            //กรณีบันทึกเดือนตุลา ย้อนหลัง 1 เดือน จะเป็นปีงบประมาณที่ผ่านมา
            if (DateTime.Now.Month == 10)
            {
                yearPoint = -1;
            }

            viewModel.yearPoint = yearPoint;

            //ดึงข้อมูล เฉพาะในปีงบประมาณ ต.ค. (10) ปีก่อน - ก.ย. (09) ปีปัจจุบัน
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

                if (m < 10)
                {
                    for (int i = 10; i <= 12; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(-1).Year, i, 1).ToString("yyyy") });
                    }
                    for (int i = 1; i <= m; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                    }
                }
                else
                {
                    for (int i = 10; i <= m; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(-1).Year, i, 1).ToString("yyyy") });
                    }
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
                    //m = 9;
                }

                for (int i = 10; i <= 12; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(-1 + yearPoint).Year, i, 1).ToString("yyyy") });
                }
                for (int i = 1; i <= 9; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(yearPoint).Year, i, 1).ToString("yyyy") });
                }
            }

            if (User.IsInRole("Admin"))
            {
                if (String.IsNullOrEmpty(selectoffice))
                {
                    viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();

                    ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", user.OfficeId);
                }
                else
                {
                    if (selectoffice.Substring(0, 3) == "000")
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();
                    }
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();

                    ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);
                }
            }
            else // No Admin
            {
                if (String.IsNullOrEmpty(selectoffice))
                {
                    if (user.OfficeId.Substring(0, 3) == "000") // HQ Office
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();

                        var officeList = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => new { Code = b.OwnerOffice.Code, Name = b.OwnerOffice.Name }).Distinct().ToListAsync();
                        if (officeList.Count < 1)
                        {
                            officeList.Add(new { Code = user.OfficeId, Name = user.OfficeName });
                        }
                        ViewBag.OfficeCode = new SelectList(officeList, "Code", "Name", user.OfficeId);
                    }
                    else // Pak or ST
                    {
                        ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Code", "Name", user.OfficeId);
                    }

                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.Rounds).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();
                }
                else
                {
                    if (user.OfficeId.Substring(0, 3) == "000") // HQ Office
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();

                        var officeList = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => new { Code = b.OwnerOffice.Code, Name = b.OwnerOffice.Name }).Distinct().ToListAsync();
                        if (officeList.Count < 1)
                        {
                            officeList.Add(new { Code = user.OfficeId, Name = user.OfficeName });
                        }
                        ViewBag.OfficeCode = new SelectList(officeList, "Code", "Name", selectoffice);
                    }
                    else // Pak or ST
                    {
                        ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);
                    }

                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();
                }
            }

            if (month != 0)
            {
                m = month;
            }

            if (ViewBag.OfficeCode == null)
            {
                List<Object> oOffice = new List<Object>();
                oOffice.Add(new { Code = user.OfficeId, Name = user.OfficeName });
                ViewBag.OfficeCode = new SelectList(oOffice, "Code", "Name", user.OfficeId);
            }

            ViewBag.Month = new SelectList(list, "Value", "Month", m);
            ViewBag.selectoffice = selectoffice;
            viewModel.month = m;

            return View(viewModel);
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexPost(string selectoffice, ResultViewModel model, int yearPoint)
        {
            var user = await _userManager.GetUserAsync(User);
            var viewModel = new ResultViewModel();
            int m;
            DateTime yearForQuery;
            List<Object> list = new List<object>();

            //บันทึกย้อนหลัง 1 เดือน
            if (DateTime.Now.Month == 1)
            {
                m = 12;
            }
            else
            {
                m = DateTime.Now.Month - 1;
            }

            //กรณีบันทึกเดือนตุลา ย้อนหลัง 1 เดือน จะเป็นปีงบประมาณที่ผ่านมา
            if (DateTime.Now.Month == 10)
            {
                yearPoint = -1;
            }

            viewModel.yearPoint = yearPoint;

            //ดึงข้อมูล เฉพาะในปีงบประมาณ ต.ค. (10) ปีก่อน - ก.ย. (09) ปีปัจจุบัน
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

                if (m < 10)
                {
                    for (int i = 10; i <= 12; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(-1).Year, i, 1).ToString("yyyy") });
                    }
                    for (int i = 1; i <= m; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.Year, i, 1).ToString("yyyy") });
                    }
                }
                else
                {
                    for (int i = 10; i <= m; i++)
                    {
                        list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(-1).Year, i, 1).ToString("yyyy") });
                    }
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
                    //m = 9;
                }

                for (int i = 10; i <= 12; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(-1 + yearPoint).Year, i, 1).ToString("yyyy") });
                }
                for (int i = 1; i <= 9; i++)
                {
                    list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") + " " + new DateTime(DateTime.Now.AddYears(yearPoint).Year, i, 1).ToString("yyyy") });
                }
            }

            if (User.IsInRole("Admin"))
            {
                if (String.IsNullOrEmpty(selectoffice))
                {
                    viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();

                    ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", user.OfficeId);
                }
                else
                {
                    if (selectoffice.Substring(0, 3) == "000")
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();
                    }
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();

                    ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);
                }
            }
            else // No Admin
            {
                if (String.IsNullOrEmpty(selectoffice))
                {
                    if (user.OfficeId.Substring(0, 3) == "000") // HQ Office
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();

                        var officeList = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => new { Code = b.OwnerOffice.Code, Name = b.OwnerOffice.Name }).Distinct().ToListAsync();
                        if (officeList.Count < 1)
                        {
                            officeList.Add(new { Code = user.OfficeId, Name = user.OfficeName });
                        }
                        ViewBag.OfficeCode = new SelectList(officeList, "Code", "Name", user.OfficeId);
                    }
                    else // Pak or ST
                    {
                        ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Code", "Name", user.OfficeId);
                    }

                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();
                }
                else
                {
                    if (user.OfficeId.Substring(0, 3) == "000") // HQ Office
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();

                        var officeList = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => new { Code = b.OwnerOffice.Code, Name = b.OwnerOffice.Name }).Distinct().ToListAsync();
                        if (officeList.Count < 1)
                        {
                            officeList.Add(new { Code = user.OfficeId, Name = user.OfficeName });
                        }
                        ViewBag.OfficeCode = new SelectList(officeList, "Code", "Name", selectoffice);
                    }
                    else // Pak or ST
                    {
                        ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);
                    }

                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).ToListAsync();
                }
            }

            ViewBag.Month = new SelectList(list, "Value", "Month", model.month);
            ViewBag.selectoffice = selectoffice;
            viewModel.yearPoint = model.yearPoint;
            viewModel.month = model.month;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string selectoffice, List<UpdateDataViewModel> UpdateData, int yearPoint, int month, string Update)
        {
            //ViewBag.msg1 = "hello1";
            var user = await _userManager.GetUserAsync(User);
            if (Update == "บันทึก")
            {
                if (UpdateData != null)
                {
                    foreach (var item in UpdateData)
                    {
                        if (!item.hasSub)
                        {
                            if (item.Id != null)
                            {
                                var de = await _context.DataForEvaluations.Include(d => d.Office).FirstOrDefaultAsync(d => d.Id == item.Id);
                                if (de != null)
                                {
                                    if (item.Approve != null)
                                    {
                                        de.Approve = item.Approve;
                                    }
                                    
                                    de.CommentApproveLevel1 = item.Comment1;
                                    de.CommentApproveLevel2 = item.Comment2;
                                    de.CommentApproveLevel3 = item.Comment3;
                                    de.CommentApproveLevel4 = item.Comment4;
                                    de.UpdateUserId = user.Id;
                                    de.TimeUpdate = DateTime.Now;

                                    await _context.SaveChangesAsync();
                                }
                            }
                        }
                    }
                }
            }
            return RedirectToAction(nameof(Index), new { selectoffice = selectoffice, isUpdate = true, yearPoint = yearPoint, month = month });
        }

        [HttpPost]
        public async Task<IActionResult> FileSelection(IFormFile attachFile, int deid, string selectoffice, int yearPoint, int month)
        {
            // Learn to use the entire functionality of the dxFileUploader widget.
            // http://js.devexpress.com/Documentation/Guide/UI_Widgets/UI_Widgets_-_Deep_Dive/dxFileUploader/

            if (attachFile != null)
            {
                await SaveFile(attachFile, deid);
            }

            return RedirectToAction(nameof(Index), new { selectoffice = selectoffice, yearPoint = yearPoint, month = month });
        }
        public async Task<IActionResult> FileDelete(int deid, string selectoffice, int yearPoint, int month)
        {
            var de = await _context.DataForEvaluations.FirstAsync(d => d.Id == deid);
            if (de != null)
            {
                var user = await _userManager.GetUserAsync(User);
                de.UpdateUserId = user.Id;
                de.AttachFile = null;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { selectoffice = selectoffice, yearPoint = yearPoint, month = month });
        }
        public async Task SaveFile(IFormFile file, int deid)
        {

            var uniqueFile = Guid.NewGuid().ToString() + "_" + file.FileName;
            try
            {
                var path = Path.Combine(_hostingEnvironment.WebRootPath, "attach_files");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                using (var fileStream = System.IO.File.Create(Path.Combine(path, uniqueFile)))
                {
                    file.CopyTo(fileStream);
                }
            }
            catch
            {
                Response.StatusCode = 400;
            }

            var user = await _userManager.GetUserAsync(User);
            var de = await _context.DataForEvaluations.FirstAsync(d => d.Id == deid);
            if (de != null)
            {
                de.UpdateUserId = user.Id;
                de.AttachFile = uniqueFile;
                await _context.SaveChangesAsync();
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveAudit(UpdateDataViewModel UpdateData, string selectoffice, int yearPoint, int month)
        {
            var user = await _userManager.GetUserAsync(User);
            if (UpdateData.Id != null)
            {
                var de = await _context.DataForEvaluations.Include(d => d.Office).FirstOrDefaultAsync(d => d.Id == UpdateData.Id);
                if (de != null)
                {
                    if (UpdateData.Approve != null)
                    {
                        de.Approve = UpdateData.Approve;
                    }

                    de.CommentApproveLevel1 = UpdateData.Comment1;
                    de.CommentApproveLevel2 = UpdateData.Comment2;
                    de.CommentApproveLevel3 = UpdateData.Comment3;
                    de.CommentApproveLevel4 = UpdateData.Comment4;

                    de.Result = UpdateData.Result;
                    de.ResultLevelRate = UpdateData.ResultLevelRate;
                    de.Completed = UpdateData.Completed;
                    de.UpdateUserId = user.Id;
                    de.TimeUpdate = DateTime.Now;

                    try
                    {
                        _context.Update(de);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateException)
                    {
                        //Log the error (uncomment ex variable name and write a log.
                        ModelState.AddModelError("", "ไม่สามารถบันทึกข้อมูล. " +
                            "ลองพยายามบันทึกอีกครั้ง " +
                            "โปรดแจ้งผู้ดูแลระบบ");
                    }
                }
            }
            return RedirectToAction(nameof(Index), new { selectoffice = selectoffice, isUpdate = true, yearPoint = yearPoint, month = month });
        }
    }
}
