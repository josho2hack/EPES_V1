﻿using EPES.Data;
using EPES.Models;
using EPES.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPES.Controllers
{
    public class ResultsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ResultsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Results
        public async Task<IActionResult> Index(string selectoffice, int yearPoint = 0)
        {
            var user = await _userManager.GetUserAsync(User);
            var viewModel = new DataForEvaluationViewModel();

            DateTime yearForQuery = new DateTime(DateTime.Now.AddYears(yearPoint).Year, 1, 1);

            if (User.IsInRole("Admin"))
            {
                if (String.IsNullOrEmpty(selectoffice))
                {
                    viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", user.OfficeId);
                }
                else
                {
                    if (selectoffice.Substring(0, 3) == "000")
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    }
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);
                }
            }
            else
            {
                if (String.IsNullOrEmpty(selectoffice))
                {
                    if (user.OfficeId.Substring(0, 3) == "000")
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();

                        ViewBag.OfficeCode = new SelectList(await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => new { Code = b.OwnerOffice.Code, Name = b.OwnerOffice.Name }).Distinct().ToListAsync(), "Code", "Name", user.OfficeId);
                    }
                    else
                    {
                        ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Code", "Name", user.OfficeId);
                    }

                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                }
                else
                {
                    if (user.OfficeId.Substring(0, 3) == "000")
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();

                        ViewBag.OfficeCode = new SelectList(await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => new { Code = b.OwnerOffice.Code, Name = b.OwnerOffice.Name }).Distinct().ToListAsync(), "Code", "Name", selectoffice);
                    }
                    else
                    {
                        ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);
                    }

                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                }
            }

            ViewBag.selectoffice = selectoffice;
            viewModel.yearPoint = yearPoint;
            return View(viewModel);
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexPost(string selectoffice, int yearPoint)
        {
            var user = await _userManager.GetUserAsync(User);

            var viewModel = new DataForEvaluationViewModel();

            DateTime yearForQuery;
            if (yearPoint == 0)
            {
                yearForQuery = new DateTime(DateTime.Now.Year, 1, 1);
            }
            else
            {
                yearForQuery = new DateTime(DateTime.Now.AddYears(yearPoint).Year, 1, 1);
            }

            if (User.IsInRole("Admin"))
            {
                if (String.IsNullOrEmpty(selectoffice))
                {
                    viewModel.pointA = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.A) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.B) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.C) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", user.OfficeId);
                }
                else
                {
                    if (selectoffice.Substring(0, 3) == "000")
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    }
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);
                }
            }
            else
            {
                if (String.IsNullOrEmpty(selectoffice))
                {
                    if (user.OfficeId.Substring(0, 3) == "000")
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();

                        ViewBag.OfficeCode = new SelectList(await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => new { Code = b.OwnerOffice.Code, Name = b.OwnerOffice.Name }).Distinct().ToListAsync(), "Code", "Name", user.OfficeId);
                    }
                    else
                    {
                        ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Code", "Name", user.OfficeId);
                    }

                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                }
                else
                {
                    if (user.OfficeId.Substring(0, 3) == "000")
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();

                        ViewBag.OfficeCode = new SelectList(await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => new { Code = b.OwnerOffice.Code, Name = b.OwnerOffice.Name }).Distinct().ToListAsync(), "Code", "Name", selectoffice);
                    }
                    else
                    {
                        ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);
                    }

                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                }
            }

            ViewBag.selectoffice = selectoffice;
            viewModel.yearPoint = yearPoint;
            return View(viewModel);
        }

        // GET: Results/Details/5
        public async Task<IActionResult> Details(string selectoffice, int poeid, int yearPoint)
        {
            DataForEvaluationViewModel viewModel = new DataForEvaluationViewModel();
            viewModel.Point = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Where(p => p.Id == poeid).FirstOrDefaultAsync();
            if (viewModel.Point == null)
            {
                return NotFound();
            }

            viewModel.result1 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 1).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result2 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 2).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result3 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 3).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result4 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 4).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result5 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 5).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result6 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 6).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result7 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 7).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result8 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 8).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result9 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 9).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result10 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 10).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result11 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid  && d.Month == 11).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result12 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid  && d.Month == 12).Select(d => d.Result).FirstOrDefaultAsync();

            ViewBag.selectoffice = selectoffice;
            viewModel.yearPoint = yearPoint;
            return View(viewModel);
        }

        // GET: DataForEvaluations/Edit/5
        public async Task<IActionResult> Edit(string selectoffice, int poeid, int yearPoint)
        {
            DataForEvaluationViewModel viewModel = new DataForEvaluationViewModel();
            viewModel.Point = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Where(p => p.Id == poeid).FirstOrDefaultAsync();

            if (viewModel.Point == null)
            {
                return NotFound();
            }

            viewModel.result1 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 1).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result2 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 2).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result3 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 3).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result4 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 4).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result5 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 5).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result6 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 6).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result7 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 7).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result8 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 8).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result9 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 9).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result10 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 10).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result11 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 11).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result12 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.Month == 12).Select(d => d.Result).FirstOrDefaultAsync();

            List<Object> list = new List<object>();
            list.Add(new { Value = viewModel.Point.Rate1, Detail = "1." + viewModel.Point.DetailRate1 });
            list.Add(new { Value = viewModel.Point.Rate2, Detail = "2." + viewModel.Point.DetailRate2 });
            list.Add(new { Value = viewModel.Point.Rate3, Detail = "3." + viewModel.Point.DetailRate3 });
            list.Add(new { Value = viewModel.Point.Rate4, Detail = "4." + viewModel.Point.DetailRate4 });
            list.Add(new { Value = viewModel.Point.Rate5, Detail = "5." + viewModel.Point.DetailRate5 });
            ViewBag.SelectLevel = new SelectList(list, "Value", "Detail");

            List<Object> list2 = new List<object>();
            list2.Add(new { Value = viewModel.Point.Rate1, Detail = "1." + viewModel.Point.Detail2Rate1 });
            list2.Add(new { Value = viewModel.Point.Rate2, Detail = "2." + viewModel.Point.Detail2Rate2 });
            list2.Add(new { Value = viewModel.Point.Rate3, Detail = "3." + viewModel.Point.Detail2Rate3 });
            list2.Add(new { Value = viewModel.Point.Rate4, Detail = "4." + viewModel.Point.Detail2Rate4 });
            list2.Add(new { Value = viewModel.Point.Rate5, Detail = "5." + viewModel.Point.Detail2Rate5 });
            ViewBag.SelectLevel2 = new SelectList(list2, "Value", "Detail");

            ViewBag.selectoffice = selectoffice;
            viewModel.poeid = poeid;
            viewModel.yearPoint = yearPoint;
            return View(viewModel);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost([Bind("yearPoint,poeid,result1,result2,result3,result4,result5,result6,result7,result8,result9,result10,result11,result12")] DataForEvaluationViewModel viewModel, string selectoffice)
        {
            viewModel.Point = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Where(p => p.Id == viewModel.poeid).FirstOrDefaultAsync();

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                await SaveResult(viewModel.Point.Id, viewModel.Point.OwnerOffice.Id, 10, viewModel.result10, user.Id);
                await SaveResult(viewModel.Point.Id, viewModel.Point.OwnerOffice.Id, 11, viewModel.result11, user.Id);
                await SaveResult(viewModel.Point.Id, viewModel.Point.OwnerOffice.Id, 12, viewModel.result12, user.Id);
                await SaveResult(viewModel.Point.Id, viewModel.Point.OwnerOffice.Id, 1, viewModel.result1, user.Id);
                await SaveResult(viewModel.Point.Id, viewModel.Point.OwnerOffice.Id, 2, viewModel.result2, user.Id);
                await SaveResult(viewModel.Point.Id, viewModel.Point.OwnerOffice.Id, 3, viewModel.result3, user.Id);
                await SaveResult(viewModel.Point.Id, viewModel.Point.OwnerOffice.Id, 4, viewModel.result4, user.Id);
                await SaveResult(viewModel.Point.Id, viewModel.Point.OwnerOffice.Id, 5, viewModel.result5, user.Id);
                await SaveResult(viewModel.Point.Id, viewModel.Point.OwnerOffice.Id, 6, viewModel.result6, user.Id);
                await SaveResult(viewModel.Point.Id, viewModel.Point.OwnerOffice.Id, 7, viewModel.result7, user.Id);
                await SaveResult(viewModel.Point.Id, viewModel.Point.OwnerOffice.Id, 8, viewModel.result8, user.Id);
                await SaveResult(viewModel.Point.Id, viewModel.Point.OwnerOffice.Id, 9, viewModel.result9, user.Id);

                return RedirectToAction(nameof(Index), new { yearPoint = viewModel.yearPoint, selectoffice = selectoffice });
            }

            viewModel.result1 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == viewModel.poeid && d.Month == 1).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result2 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == viewModel.poeid && d.Month == 2).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result3 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == viewModel.poeid && d.Month == 3).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result4 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == viewModel.poeid && d.Month == 4).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result5 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == viewModel.poeid && d.Month == 5).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result6 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == viewModel.poeid && d.Month == 6).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result7 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == viewModel.poeid && d.Month == 7).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result8 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == viewModel.poeid && d.Month == 8).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result9 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == viewModel.poeid && d.Month == 9).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result10 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == viewModel.poeid && d.Month == 10).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result11 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == viewModel.poeid && d.Month == 11).Select(d => d.Result).FirstOrDefaultAsync();
            viewModel.result12 = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == viewModel.poeid && d.Month == 12).Select(d => d.Result).FirstOrDefaultAsync();

            List<Object> list = new List<object>();
            list.Add(new { Value = viewModel.Point.Rate1, Detail = "1." + viewModel.Point.DetailRate1 });
            list.Add(new { Value = viewModel.Point.Rate2, Detail = "2." + viewModel.Point.DetailRate2 });
            list.Add(new { Value = viewModel.Point.Rate3, Detail = "3." + viewModel.Point.DetailRate3 });
            list.Add(new { Value = viewModel.Point.Rate4, Detail = "4." + viewModel.Point.DetailRate4 });
            list.Add(new { Value = viewModel.Point.Rate5, Detail = "5." + viewModel.Point.DetailRate5 });
            ViewBag.SelectLevel = new SelectList(list, "Value", "Detail");

            List<Object> list2 = new List<object>();
            list2.Add(new { Value = viewModel.Point.Rate1, Detail = "1." + viewModel.Point.Detail2Rate1 });
            list2.Add(new { Value = viewModel.Point.Rate2, Detail = "2." + viewModel.Point.Detail2Rate2 });
            list2.Add(new { Value = viewModel.Point.Rate3, Detail = "3." + viewModel.Point.Detail2Rate3 });
            list2.Add(new { Value = viewModel.Point.Rate4, Detail = "4." + viewModel.Point.Detail2Rate4 });
            list2.Add(new { Value = viewModel.Point.Rate5, Detail = "5." + viewModel.Point.Detail2Rate5 });
            ViewBag.SelectLevel2 = new SelectList(list2, "Value", "Detail");
            ViewBag.selectoffice = selectoffice;
            return View(viewModel);
        }

        public async Task SaveResult(int poeid, int ownerofficeid, int month, decimal? result, string userid)
        {
            if (result != null)
            {
                DataForEvaluation dataForEvaluation;
                dataForEvaluation = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == month).FirstOrDefaultAsync();
                if (dataForEvaluation != null)
                {
                    dataForEvaluation.UpdateUserId = userid;
                    dataForEvaluation.Result = result;
                    try
                    {
                        _context.Update(dataForEvaluation);
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
                else
                {
                    dataForEvaluation = new DataForEvaluation();
                    dataForEvaluation.UpdateUserId = userid;
                    dataForEvaluation.PointOfEvaluationId = poeid;
                    dataForEvaluation.OfficeId = ownerofficeid;
                    dataForEvaluation.Month = month;
                    dataForEvaluation.Result = result;
                    try
                    {
                        _context.Add(dataForEvaluation);
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
            }// Result 1
        }

        private bool DataForEvaluationExists(int id)
        {
            return _context.DataForEvaluations.Any(e => e.Id == id);
        }

        public async Task<IActionResult> IndexMonth(string selectoffice, int yearPoint = 0)
        {
            var user = await _userManager.GetUserAsync(User);
            var viewModel = new DataForEvaluationViewModel();

            DateTime yearForQuery = new DateTime(DateTime.Now.AddYears(yearPoint).Year, 1, 1);

            if (User.IsInRole("Admin"))
            {
                if (String.IsNullOrEmpty(selectoffice))
                {
                    viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).ToListAsync();
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", user.OfficeId);
                }
                else
                {
                    if (selectoffice.Substring(0, 3) == "000")
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    }
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);
                }
            }
            else
            {
                if (String.IsNullOrEmpty(selectoffice))
                {
                    if (user.OfficeId.Substring(0, 3) == "000")
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();

                        ViewBag.OfficeCode = new SelectList(await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => new { Code = b.OwnerOffice.Code, Name = b.OwnerOffice.Name }).Distinct().ToListAsync(), "Code", "Name", user.OfficeId);
                    }
                    else
                    {
                        ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Code", "Name", user.OfficeId);
                    }

                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                }
                else
                {
                    if (user.OfficeId.Substring(0, 3) == "000")
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();

                        ViewBag.OfficeCode = new SelectList(await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => new { Code = b.OwnerOffice.Code, Name = b.OwnerOffice.Name }).Distinct().ToListAsync(), "Code", "Name", selectoffice);
                    }
                    else
                    {
                        ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);
                    }

                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                }
            }

            List<Object> list = new List<object>();
            for (int i = 1; i <= 12; i++)
            {
                list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") });
            }

            ViewBag.Month = new SelectList(list, "Value", "Month");
            ViewBag.selectoffice = selectoffice;
            viewModel.yearPoint = yearPoint;
            return View(viewModel);
        }

        [HttpPost, ActionName("IndexMonth")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexMonthPost(string selectoffice, int yearPoint)
        {
            var user = await _userManager.GetUserAsync(User);
            var viewModel = new DataForEvaluationViewModel();

            DateTime yearForQuery = new DateTime(DateTime.Now.AddYears(yearPoint).Year, 1, 1);

            if (User.IsInRole("Admin"))
            {
                if (String.IsNullOrEmpty(selectoffice))
                {
                    viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).ToListAsync();
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", user.OfficeId);
                }
                else
                {
                    if (selectoffice.Substring(0, 3) == "000")
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    }
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);
                }
            }
            else
            {
                if (String.IsNullOrEmpty(selectoffice))
                {
                    if (user.OfficeId.Substring(0, 3) == "000")
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();

                        ViewBag.OfficeCode = new SelectList(await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => new { Code = b.OwnerOffice.Code, Name = b.OwnerOffice.Name }).Distinct().ToListAsync(), "Code", "Name", user.OfficeId);
                    }
                    else
                    {
                        ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Code", "Name", user.OfficeId);
                    }

                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == user.OfficeId && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                }
                else
                {
                    if (user.OfficeId.Substring(0, 3) == "000")
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();

                        ViewBag.OfficeCode = new SelectList(await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && (p.OwnerOffice.Code == user.OfficeId || p.AuditOffice.Code == user.OfficeId) && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => new { Code = b.OwnerOffice.Code, Name = b.OwnerOffice.Name }).Distinct().ToListAsync(), "Code", "Name", selectoffice);
                    }
                    else
                    {
                        ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);
                    }

                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                }
            }

            List<Object> list = new List<object>();
            for (int i = 1; i <= 12; i++)
            {
                list.Add(new { Value = i, Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM") });
            }

            ViewBag.Month = new SelectList(list, "Value", "Month");
            ViewBag.selectoffice = selectoffice;
            viewModel.yearPoint = yearPoint;
            return View(viewModel);
        }
    }
}
