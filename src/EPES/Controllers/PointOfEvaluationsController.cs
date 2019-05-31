using EPES.Data;
using EPES.Models;
using EPES.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EPES.Controllers
{
    [Authorize]
    public class PointOfEvaluationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PointOfEvaluationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: PointOfEvaluations
        public async Task<IActionResult> Index(int yearPoint = 0, int selectoffice = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            var officeCode = user.OfficeId;
            var officeselect = await _context.Offices.FindAsync(selectoffice);
            //var office = await _context.Offices.SingleAsync(o => o.Code == officeCode);

            DateTime yearForQuery;
            if (yearPoint == 0)
            {
                yearForQuery = new DateTime(DateTime.Now.Year, 1, 1);
            }
            else
            {
                yearForQuery = new DateTime(DateTime.Now.AddYears(yearPoint).Year, 1, 1);
            }

            var viewModel = new PointOfEvaluationViewModel();
            if (officeCode == "00013000")
            {
                if (selectoffice == 1)
                {
                    viewModel.pointA = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.A) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.B) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.C) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                }
                else
                {
                    
                    if (officeselect.Code.Substring(0, 3) == "000")
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && (p.OwnerOfficeId == selectoffice) && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                        viewModel.pointB = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.B) && (p.OwnerOfficeId == selectoffice || p.AuditOfficeId == selectoffice) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    }
                    else
                    {
                        viewModel.pointB = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.B) && (p.OwnerOfficeId == null) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    }

                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.C) && (p.OwnerOfficeId == selectoffice || p.OwnerOfficeId == null) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                }
                ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", selectoffice);
                ViewBag.selectoffice = selectoffice;
            }
            else
            {

                if (officeCode.Substring(0, 3) == "000")
                {
                    viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == officeCode && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && (p.AuditOffice.Code == officeCode || p.OwnerOffice.Code == officeCode) && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == officeCode && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                }
                else
                {
                    // List Plan B All Office
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOfficeId == null && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();

                    if (officeCode.Substring(2, 6) == "000000")
                    {
                        viewModel.pointC = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Where(p => p.Plan == TypeOfPlan.C && (p.OwnerOffice == null || p.OwnerOffice.Code.StartsWith(officeCode.Substring(0, 2))) && p.Year == yearForQuery).ToListAsync();
                    }
                    else
                    {
                        viewModel.pointC = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Where(p => (p.Plan == TypeOfPlan.C && p.OwnerOffice == null && p.Year == yearForQuery) || (p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == officeCode && p.Year == yearForQuery)).ToListAsync();
                    }
                }
            }
            viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();

            viewModel.yearPoint = yearPoint;
            return View(viewModel);
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexPost(int yearPoint = 0, int selectoffice = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            var officeCode = user.OfficeId;
            var officeselect = await _context.Offices.FindAsync(selectoffice);
            //var office = await _context.Offices.SingleAsync(o => o.Code == officeCode);

            DateTime yearForQuery;
            if (yearPoint == 0)
            {
                yearForQuery = new DateTime(DateTime.Now.Year, 1, 1);
            }
            else
            {
                yearForQuery = new DateTime(DateTime.Now.AddYears(yearPoint).Year, 1, 1);
            }

            var viewModel = new PointOfEvaluationViewModel();
            if (officeCode == "00013000")
            {
                if (selectoffice == 1)
                {
                    viewModel.pointA = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.A) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.B) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.C) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                }
                else
                {
                    if (officeselect.Code.Substring(0, 3) == "000")
                    {
                        viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && (p.OwnerOfficeId == selectoffice) && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                        viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && (p.OwnerOfficeId == selectoffice || p.AuditOfficeId == selectoffice) && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    }
                    else
                    {
                        viewModel.pointB = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.B) && (p.OwnerOfficeId == null) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    }

                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => (p.Plan == TypeOfPlan.C) && (p.OwnerOfficeId == selectoffice || p.OwnerOfficeId == null) && (p.Year == yearForQuery)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                }
                ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", selectoffice);
                ViewBag.selectoffice = selectoffice;
            }
            else
            {

                if (officeCode.Substring(0, 3) == "000")
                {
                    viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == officeCode && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && (p.AuditOffice.Code == officeCode || p.OwnerOffice.Code == officeCode) && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                    viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == officeCode && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();
                }
                else
                {
                    // List Plan B All Office
                    viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOfficeId == null && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();

                    if (officeCode.Substring(2, 6) == "000000")
                    {
                        viewModel.pointC = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Where(p => p.Plan == TypeOfPlan.C && (p.OwnerOffice == null || p.OwnerOffice.Code.StartsWith(officeCode.Substring(0, 2))) && p.Year == yearForQuery).ToListAsync();
                    }
                    else
                    {
                        viewModel.pointC = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Where(p => (p.Plan == TypeOfPlan.C && p.OwnerOffice == null && p.Year == yearForQuery) || (p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == officeCode && p.Year == yearForQuery)).ToListAsync();
                    }
                }
            }

            viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).ToListAsync();

            viewModel.yearPoint = yearPoint;
            return View(viewModel);
        }

        // GET: PointOfEvaluations/Details/5
        public async Task<IActionResult> Details(int? id, int yearPoint = 0, int selectoffice = 1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pointOfEvaluation = await _context.PointOfEvaluations
                .Include(p => p.OwnerOffice)
                .Include(p => p.AuditOffice)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pointOfEvaluation == null)
            {
                return NotFound();
            }

            ViewBag.selectoffice = selectoffice;
            ViewBag.yearPoint = yearPoint;
            return View(pointOfEvaluation);
        }

        // GET: PointOfEvaluations/Create
        public async Task<IActionResult> Create(int plan, int yearPoint, int selectoffice = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            var office = await _context.Offices.Where(o => o.Code == user.OfficeId).FirstOrDefaultAsync();
            switch (plan)
            {
                case 0:
                    ViewBag.Plan = "A";
                    ViewBag.PlanValue = 0;
                    if (selectoffice == 1)
                    {
                        ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", office.Id);
                    }
                    else
                    {
                        ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", selectoffice);
                    }
                    ViewBag.AuditOfficeId = ViewBag.OfficeId;
                    break;
                case 1:
                    ViewBag.Plan = "B";
                    ViewBag.PlanValue = 1;
                    ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name");
                    if (selectoffice == 1)
                    {
                        ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", office.Id);
                    }
                    else
                    {
                        ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", selectoffice);
                    }
                    break;
                case 2:
                    ViewBag.Plan = "C";
                    ViewBag.PlanValue = 2;
                    if (selectoffice == 1)
                    {
                        ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", office.Id);
                        ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", office.Id);
                    }
                    else
                    {
                        ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", selectoffice);
                        ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", selectoffice);
                    }
                    break;
                case 3:
                    ViewBag.Plan = "D";
                    ViewBag.PlanValue = 3;
                    //ViewBag.OfficeId = "00000000";
                    if (selectoffice == 1)
                    {
                        ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", office.Id);
                    }
                    else
                    {
                        ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", selectoffice);
                    }
                    break;
            }

            ViewBag.selectoffice = selectoffice;
            ViewBag.yearPoint = yearPoint;
            return View();
        }

        // POST: PointOfEvaluations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DetailPlan,Point,Plan,Name,Unit,Weight,Rate1,DetailRate1,Rate2,DetailRate2,Rate3,DetailRate3,Rate4,DetailRate4,Rate5,DetailRate5,Detail2Rate1,Detail2Rate2,Detail2Rate3,Detail2Rate4,Detail2Rate5,OwnerOfficeId,AuditOfficeId")] PointOfEvaluation dataView, int yearPoint, decimal? expect1, decimal? expect2, decimal? expect3, decimal? expect4, decimal? expect5, decimal? expect6, decimal? expect7, decimal? expect8, decimal? expect9, decimal? expect10, decimal? expect11, decimal? expect12, int selectoffice = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            var office = await _context.Offices.Where(o => o.Code == user.OfficeId).FirstOrDefaultAsync();

            if (dataView.OwnerOfficeId == null)
            {
                if (dataView.Plan == TypeOfPlan.A)
                {
                    dataView.OwnerOffice = office;
                }
                else if (dataView.Plan == TypeOfPlan.B)
                {
                    dataView.OwnerOffice = null;
                }
                else if (dataView.Plan == TypeOfPlan.C)
                {
                    if (User.IsInRole("Admin"))
                    {
                        dataView.OwnerOffice = null;
                    }
                    else
                    {
                        dataView.OwnerOffice = office;
                    }

                }
                else
                {
                    dataView.OwnerOffice = await _context.Offices.Where(o => o.Code == "00000000").FirstOrDefaultAsync();
                }
            }

            if (dataView.AuditOfficeId == null)
            {
                dataView.AuditOffice = office;
            }

            dataView.Year = new DateTime(DateTime.Now.AddYears(yearPoint).Year, 1, 1);
            dataView.UpdateUserId = user.Id;
            dataView.SubPoint = 0;


            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(dataView);
                    await _context.SaveChangesAsync();
                    if (dataView.OwnerOffice != null)  //บันทึกเฉพาะมอบให้หน่วยงานเดียวเท่านั้น
                    {
                        await SaveExpect(dataView.Id, dataView.OwnerOffice.Id, 10, expect10, user.Id);
                        await SaveExpect(dataView.Id, dataView.OwnerOffice.Id, 11, expect11, user.Id);
                        await SaveExpect(dataView.Id, dataView.OwnerOffice.Id, 12, expect12, user.Id);
                        await SaveExpect(dataView.Id, dataView.OwnerOffice.Id, 1, expect1, user.Id);
                        await SaveExpect(dataView.Id, dataView.OwnerOffice.Id, 2, expect2, user.Id);
                        await SaveExpect(dataView.Id, dataView.OwnerOffice.Id, 3, expect3, user.Id);
                        await SaveExpect(dataView.Id, dataView.OwnerOffice.Id, 4, expect4, user.Id);
                        await SaveExpect(dataView.Id, dataView.OwnerOffice.Id, 5, expect5, user.Id);
                        await SaveExpect(dataView.Id, dataView.OwnerOffice.Id, 6, expect6, user.Id);
                        await SaveExpect(dataView.Id, dataView.OwnerOffice.Id, 7, expect7, user.Id);
                        await SaveExpect(dataView.Id, dataView.OwnerOffice.Id, 8, expect8, user.Id);
                        await SaveExpect(dataView.Id, dataView.OwnerOffice.Id, 9, expect9, user.Id);
                    }
                    return RedirectToAction(nameof(Index), new { yearPoint = yearPoint, selectoffice = selectoffice });
                }
            }
            catch (DbUpdateException)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "ไม่สามารถบันทึกข้อมูล. " +
                    "ลองพยายามบันทึกอีกครั้ง " +
                    "โปรดแจ้งผู้ดูแลระบบ");
            }

            switch (dataView.Plan)
            {
                case TypeOfPlan.A:
                    ViewBag.Plan = "A";
                    ViewBag.PlanValue = 0;
                    if (selectoffice == 1)
                    {
                        ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", office.Id);
                    }
                    else
                    {
                        ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", selectoffice);
                    }
                    ViewBag.AuditOfficeId = ViewBag.OfficeId;
                    break;
                case TypeOfPlan.B:
                    ViewBag.Plan = "B";
                    ViewBag.PlanValue = 1;
                    if (selectoffice == 1)
                    {
                        ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name");
                        ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", office.Id);
                    }
                    else
                    {
                        ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", selectoffice);
                        ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", selectoffice);
                    }
                    break;
                case TypeOfPlan.C:
                    ViewBag.Plan = "C";
                    ViewBag.PlanValue = 2;
                    if (selectoffice == 1)
                    {
                        ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name");
                        ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", office.Id);
                    }
                    else
                    {
                        ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name");
                        ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name", selectoffice);
                    }
                    break;
                case TypeOfPlan.D:
                    ViewBag.Plan = "D";
                    ViewBag.PlanValue = 3;
                    //ViewBag.OfficeId = "00000000";
                    if (selectoffice == 1)
                    {
                        ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", office.Id);
                    }
                    else
                    {
                        ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", selectoffice);
                    }
                    break;
            }

            ViewBag.selectoffice = selectoffice;

            ViewBag.yearPoint = yearPoint;
            return View(dataView);
        }

        // GET: PointOfEvaluations/Edit/5
        public async Task<IActionResult> Edit(int? id, int yearPoint = 0, int selectoffice = 1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pointOfEvaluation = await _context.PointOfEvaluations.Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Where(p => p.Id == id).FirstOrDefaultAsync();
            if (pointOfEvaluation == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var office = await _context.Offices.Where(o => o.Code == user.OfficeId).FirstOrDefaultAsync();
            switch (pointOfEvaluation.Plan)
            {
                case TypeOfPlan.A:
                    ViewBag.Plan = "A";
                    ViewBag.PlanValue = 0;
                    ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", office.Id);
                    ViewBag.AuditOfficeId = ViewBag.OfficeId;
                    break;
                case TypeOfPlan.B:
                    ViewBag.Plan = "B";
                    ViewBag.PlanValue = 1;
                    ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name");
                    ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name");
                    break;
                case TypeOfPlan.C:
                    ViewBag.Plan = "C";
                    ViewBag.PlanValue = 2;
                    ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name");
                    ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name");
                    break;
                case TypeOfPlan.D:
                    ViewBag.Plan = "D";
                    ViewBag.PlanValue = 3;
                    //ViewBag.OfficeId = "00000000";
                    ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name");
                    break;
            }

            ViewBag.selectoffice = selectoffice;

            ViewBag.yearPoint = yearPoint;
            return View(pointOfEvaluation);
        }

        // POST: PointOfEvaluations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id, int yearpoint = 0, int selectoffice = 1)//, [Bind("Id,Year,Point,SubPoint,Plan,Name,Unit,Weight,Rate1,Rate2,Rate3,Rate4,Rate5")] PointOfEvaluation pointOfEvaluation)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);

            var pointOfEvaluationToUpdate = await _context.PointOfEvaluations.FirstOrDefaultAsync(p => p.Id == id);
            pointOfEvaluationToUpdate.UpdateUserId = user.Id;

            if (await TryUpdateModelAsync<PointOfEvaluation>(
                pointOfEvaluationToUpdate, "",
                p => p.Year, p => p.DetailPlan, p => p.Point, p => p.Plan, p => p.Name, p => p.Unit, p => p.Weight,
                p => p.Rate1, p => p.DetailRate1, p => p.Rate2, p => p.DetailRate2, p => p.Rate3, p => p.DetailRate3, p => p.Rate4, p => p.DetailRate4, p => p.Rate5, p => p.DetailRate5, p => p.OwnerOfficeId, p => p.AuditOfficeId, p => p.UpdateUserId, p => p.Detail2Rate1, p => p.Detail2Rate2, p => p.Detail2Rate3, p => p.Detail2Rate4, p => p.Detail2Rate5))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { yearPoint = yearpoint });
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "ไม่สามารถบันทึกข้อมูล. " +
                        "ลองพยายามบันทึกอีกครั้ง " +
                        "โปรดแจ้งผู้ดูแลระบบ");
                }
                return RedirectToAction(nameof(Index), new { yearPoint = yearpoint, selectoffice = pointOfEvaluationToUpdate.AuditOfficeId });
            }

            var office = await _context.Offices.Where(o => o.Code == user.OfficeId).FirstOrDefaultAsync();
            switch (pointOfEvaluationToUpdate.Plan)
            {
                case TypeOfPlan.A:
                    ViewBag.Plan = "A";
                    ViewBag.PlanValue = 0;
                    ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name", office.Id);
                    ViewBag.AuditOfficeId = ViewBag.OfficeId;
                    break;
                case TypeOfPlan.B:
                    ViewBag.Plan = "B";
                    ViewBag.PlanValue = 1;
                    ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name");
                    ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name");
                    break;
                case TypeOfPlan.C:
                    ViewBag.Plan = "C";
                    ViewBag.PlanValue = 2;
                    ViewBag.OfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name");
                    ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Id", "Name");
                    break;
                case TypeOfPlan.D:
                    ViewBag.Plan = "D";
                    ViewBag.PlanValue = 3;
                    //ViewBag.OfficeId = "00000000";
                    ViewBag.AuditOfficeId = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(0, 3) == "000"), "Id", "Name");
                    break;
            }

            ViewBag.selectoffice = selectoffice;
            return View(pointOfEvaluationToUpdate);
        }

        // GET: PointOfEvaluations/Delete/5
        public async Task<IActionResult> Delete(int? id, int yearPoint, bool? saveChangesError = false, int selectoffice = 1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pointOfEvaluation = await _context.PointOfEvaluations
                .Include(p => p.OwnerOffice)
                .Include(p => p.AuditOffice)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pointOfEvaluation == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "ลบไม่ได้." +
                    "อาจมีการบันทึกเป้าหมายหรือผลการปฎิบัติแล้ว " +
                    "โปรดแจ้งผู้ดูแลระบบ";
            }

            ViewBag.selectoffice = selectoffice;
            ViewBag.yearPoint = yearPoint;
            return View(pointOfEvaluation);
        }

        // POST: PointOfEvaluations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int Id, int yearP, int selectoffice = 1)
        {
            var user = await _userManager.GetUserAsync(User);

            var pointOfEvaluation = await _context.PointOfEvaluations.FindAsync(Id);
            if (pointOfEvaluation == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.PointOfEvaluations.Remove(pointOfEvaluation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { yearPoint = yearP, selectoffice = selectoffice });
            }
            catch (DbUpdateException /*ex*/)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = Id, yearPoint = yearP, saveChangesError = true, selectoffice = selectoffice });
            }
        }

        private bool PointOfEvaluationExists(int id)
        {
            return _context.PointOfEvaluations.Any(e => e.Id == id);
        }

        //private void PopulateOfficesDropDownList(object selectedOffice = null)
        //{
        //    var officesQuery = from d in _context.Offices
        //                       where (d.Code.Substring(5, 3) == "000")
        //                       orderby d.Id
        //                       select d;
        //    ViewBag.OfficeId = new SelectList(officesQuery.AsNoTracking(), "Id", "Name", selectedOffice);
        //}

        //private void PopulateAuditOfficesDropDownList(object selectedOffice = null)
        //{
        //    var officesQuery = from d in _context.Offices
        //                       where (d.Code != "00000000" && d.Code.Substring(5, 3) == "000")
        //                       orderby d.Id
        //                       select d;
        //    ViewBag.AuditOfficeId = new SelectList(officesQuery.AsNoTracking(), "Id", "Name", selectedOffice);
        //}

        public async Task SaveExpect(int poeid, int ownerofficeid, int month, decimal? expect, string userid)
        {
            if (expect != null)
            {
                DataForEvaluation dataForEvaluation;
                dataForEvaluation = await _context.DataForEvaluations.Where(d => d.PointOfEvaluationId == poeid && d.OfficeId == ownerofficeid && d.Month == month).FirstOrDefaultAsync();
                if (dataForEvaluation != null)
                {
                    dataForEvaluation.UpdateUserId = userid;
                    dataForEvaluation.Expect = expect;
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
                    dataForEvaluation.Expect = expect;
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
            }// Expect 1
        }
    }
}
