using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PersonalFinanceMgmtApp.BusinessLogicLayer;
using PersonalFinanceMgmtApp.ViewModel;
using PagedList;
using Microsoft.AspNet.Identity;
using PersonalFinanceMgmtApp.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace PersonalFinanceMgmtApp.Controllers
{
    public class DashboardController : Controller
    {
        protected Models.ApplicationDbContext ApplicationDbContext { get; set; }
        protected UserManager<ApplicationUser> UserManager { get; set; }
        operations _bll = new operations();
        public DashboardController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
            operations bll = _bll;
        }
        // GET: Dashboard
        [HttpGet]
        public ActionResult Index(int? monthId, int? page)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var userId = user.Id.ToString();
            List<ExpenseRecord> er = new List<ExpenseRecord>();
            SetDashBoardVM dash = new SetDashBoardVM();
            //dash.expenseRecords = _bll.getAllSavedExpense();
            dash.Months = _bll.getMonthDropdownValue();
            dash.expenseRecords = _bll.getAllSavedExpensebyMonth(monthId,page, userId);
            er = _bll.getSavedExpensebyMonth(monthId, userId);
           dash.totalExpenseForMonth =  _bll.getTotalExpenseForMonth(er);
            return View(dash);
        }
        

        public ActionResult NewExpenseEntry()
        {
            
            //var userid = UserManagerExtensions.FindById(HttpContext.User.Identity.GetUserId());
            NewExpenseEntry nee = new NewExpenseEntry();
           nee.ExpenseTypes= _bll.getAllExpense();
            return View(nee);
        } 
        [HttpPost]
        public ActionResult SaveNewExpense(NewExpenseEntry exp)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var userId = user.Id.ToString();
            TempData["SuccessMessage"]  =  _bll.saveNewExpense(exp, userId);
            return RedirectToAction("NewExpenseEntry");
        }
        public ActionResult NewCharts()
        {
            ChartsVM charts = new ChartsVM();
            charts.expenseTypes = _bll.getAllExpense();
            charts.chartsValue = _bll.getChartDropdownValue();
            charts.Months = _bll.getMonthDropdownValue();
            return View(charts);
        }

        public JsonResult GetValueForCharts()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var userId = user.Id.ToString();
            ChartsVM charts = new ChartsVM();
            charts.expenseForMonthVMs = _bll.getAllSavedExpenseForMonth(userId);
            var result = JsonConvert.SerializeObject(charts.expenseForMonthVMs);
            //JObject json = JObject.Parse(result);
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetValueForMonthForPieChart(int? monthId)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var userId = user.Id.ToString();
            totalExpenseForMonth charts = new totalExpenseForMonth();
            charts = _bll.getAllSavedExpenseForMonthForPieCharts(monthId, userId);
            var result = JsonConvert.SerializeObject(charts);
            //JObject json = JObject.Parse(result);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ViewResult DisplayResult(int? expenseId, int? monthId, int? page)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var userId = user.Id.ToString();
            SetDashBoardVM dash = new SetDashBoardVM();
            dash.expenseRecords = _bll.getAllSavedExpense(expenseId,monthId, page, userId);
            dash.Months = _bll.getMonthDropdownValue();
            dash.ExpenseTypes = _bll.getAllExpense();



            return View(dash);
        }

        //public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        //{
        //    ViewBag.CurrentSort = sortOrder;
        //    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        //    ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

        //    if (searchString != null)
        //    {
        //        page = 1;
        //    }
        //    else
        //    {
        //        searchString = currentFilter;
        //    }

        //    ViewBag.CurrentFilter = searchString;

        //    var students = from s in db.Students
        //                   select s;
        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        students = students.Where(s => s.LastName.Contains(searchString)
        //                               || s.FirstMidName.Contains(searchString));
        //    }
        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            students = students.OrderByDescending(s => s.LastName);
        //            break;
        //        case "Date":
        //            students = students.OrderBy(s => s.EnrollmentDate);
        //            break;
        //        case "date_desc":
        //            students = students.OrderByDescending(s => s.EnrollmentDate);
        //            break;
        //        default:  // Name ascending 
        //            students = students.OrderBy(s => s.LastName);
        //            break;
        //    }

        //    int pageSize = 3;
        //    int pageNumber = (page ?? 1);
        //    return View(students.ToPagedList(pageNumber, pageSize));
        //}


    }
}