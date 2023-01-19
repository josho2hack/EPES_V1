using DevExpress.Charts.Native;
using EPES.Data;
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
    [Authorize]
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
        public async Task<IActionResult> Index(string selectoffice, string message, bool isUpdate = false, int month = 0, int yearPoint = 0)
        {
            var user = await _userManager.GetUserAsync(User);
            var viewModel = new ResultViewModel();
            int m;
            DateTime yearForQuery;
            List<Object> list = new List<object>();

            //บันทึกย้อนหลัง 1 เดือน
            if (month == 0)
            {
                if (DateTime.Now.Month == 1)
                {
                    m = 12;
                }
                else
                {
                    m = DateTime.Now.Month - 1;
                }
            }
            else
            {
                m = month;
            }

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
            }
            else
            {
                yearForQuery = new DateTime(yearPoint, 1, 1);
            }
            viewModel.yearPoint = yearForQuery.Year;

            for (int i = 10; i <= 12; i++)
            {
                list.Add(new { Value = i, Month = new DateTime(yearForQuery.Year, i, 1).AddYears(-1).ToString("MMMM yyyy") });
            }
            for (int i = 1; i <= 9; i++)
            {
                list.Add(new { Value = i, Month = new DateTime(yearForQuery.Year, i, 1).ToString("MMMM yyyy") });
            }

            if (string.IsNullOrEmpty(selectoffice))
            {
                selectoffice = user.OfficeId;
            }

            DateTime year2023 = new DateTime(2023, 1, 1);
            if (yearForQuery >= year2023)
            {
                viewModel.pointFlagship = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.Flagship && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).Include(iss => iss.IssueForEvaluations).OrderBy(ob => ob.Point).ThenBy(ob => ob.SubPoint).ToListAsync();
                viewModel.pointCascade = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.Cascade && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).Include(iss => iss.IssueForEvaluations).OrderBy(ob => ob.Point).ThenBy(ob => ob.SubPoint).ToListAsync();
                viewModel.pointJointKPI = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.Joint_KPI && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).Include(iss => iss.IssueForEvaluations).OrderBy(ob => ob.Point).ThenBy(ob => ob.SubPoint).ToListAsync();
            }
            else
            {
                if (selectoffice.Substring(0, 3) == "000")
                {
                    viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).Include(iss => iss.IssueForEvaluations).ToListAsync();
                }
                viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).Include(iss => iss.IssueForEvaluations).ToListAsync();
                viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).Include(iss => iss.IssueForEvaluations).ToListAsync();
                viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).Include(iss => iss.IssueForEvaluations).ToListAsync();
            }

            if (User.IsInRole("Admin") || User.IsInRole("Special"))
            {
                ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);
            }
            else // No Admin
            {
                List<Office> officeList = new List<Office>();
                if (user.OfficeId.Substring(0, 3) == "000") // HQ Office
                {
                    officeList = await _context.PointOfEvaluations.Where(p => p.Year == yearForQuery && (p.OwnerOffice.Code == selectoffice || p.AuditOffice.Code == user.OfficeId)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => b.OwnerOffice).Distinct().ToListAsync();
                    if (officeList.Count < 1)
                    {
                        officeList.AddRange(_context.Offices.Where(ofc => ofc.Code == user.OfficeId));
                    }
                }
                else // Pak or ST
                {
                    officeList = await _context.Offices.Where(d => (d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000")).ToListAsync();
                }
                var userOffices = await _context.UserOffices.Include(of => of.Office).Where(ou => ou.UserName == user.UserName).Select(slt => slt.Office).ToListAsync();
                foreach (var uo in userOffices)
                {
                    if (!officeList.Contains(uo))
                    {
                        officeList.Add(uo);
                        if (uo.Code.Substring(0, 2) != "00" && uo.Code.Substring(2, 6) == "000000")
                        {
                            var subOffices = await _context.Offices.Where(so => so.Code.Substring(0, 2) == uo.Code.Substring(0, 2) && so.Code.Substring(2, 6) != "000000" && so.Code.Substring(5, 3) == "000").ToListAsync();
                            foreach (var so in subOffices)
                            {
                                if (!officeList.Contains(so))
                                {
                                    officeList.Add(so);
                                }
                            }
                        }
                    }
                }
                ViewBag.OfficeCode = new SelectList(officeList, "Code", "Name", selectoffice);
            }

            ViewBag.UserOffices = _context.UserOffices.Where(uo => uo.UserName == user.UserName).Select(slt => slt.Office.Code).ToList();
            ViewBag.Month = new SelectList(list, "Value", "Month", m);
            ViewBag.selectoffice = selectoffice;
            viewModel.month = m;

            var years = await _context.PointOfEvaluations.Select(slt => new { value = slt.Year.Year, year = slt.Year.ToString("yyyy") }).Distinct().OrderBy(ob => ob.year).ToListAsync();
            if (!years.Any(yy => yy.value == DateTime.Now.AddYears(1).Year))
            {
                years.Add(new { value = DateTime.Now.AddYears(1).Year, year = DateTime.Now.AddYears(1).ToString("yyyy") });
            }
            ViewBag.selectyear = new SelectList(years, "value", "year", yearForQuery.Year);

            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.message = message;
            }

            return View(viewModel);
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexPost(string selectoffice, string message, ResultViewModel model, int yearPoint)
        {
            var user = await _userManager.GetUserAsync(User);
            var viewModel = new ResultViewModel();
            int m;
            DateTime yearForQuery;
            List<Object> list = new List<object>();

            //บันทึกย้อนหลัง 1 เดือน
            if (model.month == 0)
            {
                if (DateTime.Now.Month == 1)
                {
                    m = 12;
                }
                else
                {
                    m = DateTime.Now.Month - 1;
                }
            }
            else
            {
                m = model.month;
            }

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
            }
            else
            {
                yearForQuery = new DateTime(yearPoint, 1, 1);
            }
            viewModel.yearPoint = yearForQuery.Year;

            for (int i = 10; i <= 12; i++)
            {
                list.Add(new { Value = i, Month = new DateTime(yearForQuery.Year, i, 1).AddYears(-1).ToString("MMMM yyyy") });
            }
            for (int i = 1; i <= 9; i++)
            {
                list.Add(new { Value = i, Month = new DateTime(yearForQuery.Year, i, 1).ToString("MMMM yyyy") });
            }

            if (string.IsNullOrEmpty(selectoffice))
            {
                selectoffice = user.OfficeId;
            }

            DateTime year2023 = new DateTime(2023, 1, 1);
            if (yearForQuery >= year2023)
            {
                viewModel.pointFlagship = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.Flagship && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).Include(iss => iss.IssueForEvaluations).OrderBy(ob => ob.Point).ThenBy(ob => ob.SubPoint).ToListAsync();
                viewModel.pointCascade = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.Cascade && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).Include(iss => iss.IssueForEvaluations).OrderBy(ob => ob.Point).ThenBy(ob => ob.SubPoint).ToListAsync();
                viewModel.pointJointKPI = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.Joint_KPI && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).Include(iss => iss.IssueForEvaluations).OrderBy(ob => ob.Point).ThenBy(ob => ob.SubPoint).ToListAsync();
            }
            else
            {
                if (selectoffice.Substring(0, 3) == "000")
                {
                    viewModel.pointA = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.A && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).Include(iss => iss.IssueForEvaluations).ToListAsync();
                }
                viewModel.pointB = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.B && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).Include(iss => iss.IssueForEvaluations).ToListAsync();
                viewModel.pointC = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.C && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).Include(iss => iss.IssueForEvaluations).ToListAsync();
                viewModel.pointD = await _context.PointOfEvaluations.Where(p => p.Plan == TypeOfPlan.D && p.OwnerOffice.Code == selectoffice && p.Year == yearForQuery).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Include(p => p.DataForEvaluations).Include(p => p.ScoreDrafts).Include(p => p.Rounds).Include(iss => iss.IssueForEvaluations).ToListAsync();
            }

            if (User.IsInRole("Admin") || User.IsInRole("Special"))
            {
                ViewBag.OfficeCode = new SelectList(_context.Offices.Where(d => d.Code != "00000000" && d.Code.Substring(5, 3) == "000"), "Code", "Name", selectoffice);
            }
            else // No Admin
            {
                List<Office> officeList = new List<Office>();
                if (user.OfficeId.Substring(0, 3) == "000") // HQ Office
                {
                    officeList = await _context.PointOfEvaluations.Where(p => p.Year == yearForQuery && (p.OwnerOffice.Code == selectoffice || p.AuditOffice.Code == user.OfficeId)).Include(p => p.OwnerOffice).Include(p => p.AuditOffice).Select(b => b.OwnerOffice).Distinct().ToListAsync();
                    if (officeList.Count < 1)
                    {
                        officeList.AddRange(_context.Offices.Where(ofc => ofc.Code == user.OfficeId));
                    }
                }
                else // Pak or ST
                {
                    officeList = await _context.Offices.Where(d => (d.Code != "00000000" && d.Code.StartsWith(user.OfficeId.Substring(0, 2)) && d.Code.Substring(5, 3) == "000")).ToListAsync();
                }
                var userOffices = await _context.UserOffices.Include(of => of.Office).Where(ou => ou.UserName == user.UserName).Select(slt => slt.Office).ToListAsync();
                foreach (var uo in userOffices)
                {
                    if (!officeList.Contains(uo))
                    {
                        officeList.Add(uo);
                        if (uo.Code.Substring(0, 2) != "00" && uo.Code.Substring(2, 6) == "000000")
                        {
                            var subOffices = await _context.Offices.Where(so => so.Code.Substring(0, 2) == uo.Code.Substring(0, 2) && so.Code.Substring(2, 6) != "000000" && so.Code.Substring(5, 3) == "000").ToListAsync();
                            foreach (var so in subOffices)
                            {
                                if (!officeList.Contains(so))
                                {
                                    officeList.Add(so);
                                }
                            }
                        }
                    }
                }
                ViewBag.OfficeCode = new SelectList(officeList, "Code", "Name", selectoffice);
            }

            ViewBag.UserOffices = _context.UserOffices.Where(uo => uo.UserName == user.UserName).Select(slt => slt.Office.Code).ToList();
            ViewBag.Month = new SelectList(list, "Value", "Month", m);
            ViewBag.selectoffice = selectoffice;
            viewModel.month = m;
            var years = await _context.PointOfEvaluations.Select(slt => new { value = slt.Year.Year, year = slt.Year.ToString("yyyy") }).Distinct().OrderBy(ob => ob.year).ToListAsync();
            if (!years.Any(yy => yy.value == DateTime.Now.AddYears(1).Year))
            {
                years.Add(new { value = DateTime.Now.AddYears(1).Year, year = DateTime.Now.AddYears(1).ToString("yyyy") });
            }
            ViewBag.selectyear = new SelectList(years, "value", "year", yearForQuery.Year);

            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.message = message;
            }


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
                try
                {
                    await SaveFile(attachFile, deid);
                } catch (Exception ex)
                {
                    return RedirectToAction(nameof(Index), new { selectoffice = selectoffice, yearPoint = yearPoint, month = month, message = "Error\nPlease contact admin\n" + ex.InnerException });
                }
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

            //var uniqueFile = Guid.NewGuid().ToString() + "_" + file.FileName;
            var uniqueFile = Guid.NewGuid().ToString() + ".pdf";
            try
            {
                var path = Path.Combine(_hostingEnvironment.WebRootPath, "attach_files");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                using (var fileStream = System.IO.File.Create(Path.Combine(path, uniqueFile)))
                {
                    file.CopyTo(fileStream);
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
            catch(Exception ex)
            {
                throw ex;
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

                    /*
                    de.CommentApproveLevel1 = UpdateData.Comment1;
                    de.CommentApproveLevel2 = UpdateData.Comment2;
                    de.CommentApproveLevel3 = UpdateData.Comment3;
                    de.CommentApproveLevel4 = UpdateData.Comment4;
                    */
                    if(de.Approve == Approve.หัวหน้าหน่วยงานอนุมัติ)
                    {
                        de.CommentApproveLevel1 = user.UserName;
                    }
                    if (de.Approve == Approve.สภ_ผู้กำกับตัวชี้วัดอนุมัติ)
                    {
                        de.CommentApproveLevel2 = user.UserName;
                    }
                    if(de.Approve == Approve.กองผู้กำกับตัวชี้วัดอนุมัติ)
                    {
                        de.CommentApproveLevel3 = user.UserName;
                    }
                    if(de.Approve == Approve.ผษ_อนุมัติ)
                    {
                        de.CommentApproveLevel4 = user.UserName;
                    }
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
                    catch (DbUpdateException ex)
                    {
                        //Log the error (uncomment ex variable name and write a log.
                        /*
                        ModelState.AddModelError("", "ไม่สามารถบันทึกข้อมูล. " +
                            "ลองพยายามบันทึกอีกครั้ง " +
                            "โปรดแจ้งผู้ดูแลระบบ");
                        */
                        return RedirectToAction(nameof(Index), new { selectoffice = selectoffice, yearPoint = yearPoint, month = month, message = "Error\nPlease contact admin\n" + ex.InnerException });
                    }
                }
            }

            if ((month == 3 || month == 9))
            {
                var officeId = await _context.Offices.Where(ofc => ofc.Code == selectoffice).FirstOrDefaultAsync();
                if(officeId != null && officeId.Code.Substring(0, 2) == "00")
                {
                    var issue = await _context.IssueForEvaluations.Where(iss => iss.OfficeId == officeId.Id && iss.PointOfEvaluationId == UpdateData.poeid && iss.Month == month).FirstOrDefaultAsync();
                    if(issue != null)
                    {
                        issue.Issue = UpdateData.Issue;
                        try
                        {
                            _context.Update(issue);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateException ex)
                        {
                            //Log the error (uncomment ex variable name and write a log.
                            /*
                            ModelState.AddModelError("", "ไม่สามารถบันทึกปัญหาและอุปสรรค/แนวทางแก้ไขได้. " +
                                "ลองพยายามบันทึกอีกครั้ง " +
                                "โปรดแจ้งผู้ดูแลระบบ");
                            */
                            return RedirectToAction(nameof(Index), new { selectoffice = selectoffice, yearPoint = yearPoint, month = month, message = "Error\nPlease contact admin\n" + ex.InnerException });
                        }
                    } else
                    {
                        IssueForEvaluations newIssue = new IssueForEvaluations();
                        newIssue.Issue = UpdateData.Issue;
                        newIssue.PointOfEvaluationId = UpdateData.poeid;
                        newIssue.OfficeId = officeId.Id;
                        newIssue.Month = month;

                        try
                        {
                            _context.Add(newIssue);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateException ex)
                        {
                            //Log the error (uncomment ex variable name and write a log.
                            /*
                            ModelState.AddModelError("", "ไม่สามารถสร้างปัญหาและอุปสรรค/แนวทางแก้ไขได้. " +
                                "ลองพยายามบันทึกอีกครั้ง " +
                                "โปรดแจ้งผู้ดูแลระบบ");
                            */
                            return RedirectToAction(nameof(Index), new { selectoffice = selectoffice, yearPoint = yearPoint, month = month, message = "Error\nPlease contact admin\n" + ex.InnerException });
                        }
                    }

                }
            }
            return RedirectToAction(nameof(Index), new { selectoffice = selectoffice, isUpdate = true, yearPoint = yearPoint, month = month });
        }
    }
}
