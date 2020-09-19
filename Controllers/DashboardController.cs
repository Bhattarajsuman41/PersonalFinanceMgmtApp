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
using Microsoft.Owin.Security;

namespace PersonalFinanceMgmtApp.Controllers
{
   [Authorize]
    public class DashboardController : Controller
    {
        protected Models.ApplicationDbContext ApplicationDbContext { get; set; }
        protected UserManager<ApplicationUser> UserManager { get; set; }
        operations _bll = new operations();
        List<ExpenseRecord> expenseRecord;

        public DashboardController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
            operations bll = _bll;
            this.expenseRecord = new List<ExpenseRecord>();
        }
        // GET: Dashboard
        [HttpGet]
        [HandleError]
        [HandleError(ExceptionType = typeof(NullReferenceException), View = "~/Views/Shared/Error.cshtml")]
        public ActionResult Index(int? monthId, int? page)
        {
            if (Session["user"] != null)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                if (user == null)
                {
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var userId = user.Id.ToString();
                    var password = user.PasswordHash;
                    Session["userId"] = userId;
                    Session["password"] = password;
                    List<ExpenseRecord> er = new List<ExpenseRecord>();
                    SetDashBoardVM dash = new SetDashBoardVM();
                    //dash.expenseRecords = _bll.getAllSavedExpense();
                    dash.Months = _bll.getMonthDropdownValue();
                    dash.expenseRecords = _bll.getAllSavedExpensebyMonth(monthId, page, userId);
                    er = _bll.getSavedExpensebyMonth(monthId, userId);
                    if (er.Count == 0)
                    {
                        dash.totalExpenseForMonth = new totalExpenseForMonth { MonthName = "January" };
                    }
                    else
                    {
                        dash.totalExpenseForMonth = _bll.getTotalExpenseForMonth(er);
                    }
                    return View(dash);
              

            }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        
        
        public ActionResult NewExpenseEntry()
        {

            if (Session["user"] != null)
            {
                NewExpenseEntry nee = new NewExpenseEntry();
                nee.ExpenseTypes = _bll.getAllExpense();
                return View(nee);
            }
            else
            {
               
                return RedirectToAction("Index", "Home");
            }
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
            if (Session["user"] != null)
            {
                ChartsVM charts = new ChartsVM();
                charts.expenseTypes = _bll.getAllExpense();
                charts.chartsValue = _bll.getChartDropdownValue();
                charts.Months = _bll.getMonthDropdownValue();
                return View(charts);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public JsonResult GetValueForCharts()
        {
            
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                var userId = user.Id.ToString();
                ChartsVM charts = new ChartsVM();
                charts.expenseForMonthVMs = _bll.getAllSavedExpenseForMonth(userId);
                var result = JsonConvert.SerializeObject(charts.expenseForMonthVMs);
                //JObject json = JObject.Parse(result);
                return Json(result, JsonRequestBehavior.AllowGet);

           
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
            dash.expenseRecords = _bll.getAllSavedExpense(expenseId, monthId, page, userId);
            dash.Months = _bll.getMonthDropdownValue();
            dash.ExpenseTypes = _bll.getAllExpense();
            return View(dash);

        }
        public JsonResult CheckExpensesMonth(int monthId)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var userId = user.Id.ToString();
            var result = _bll.CheckExpensesMonth(monthId, userId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

    }
}