using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalFinanceMgmtApp.ViewModel
{
    public class ChartsVM
    {
        public int chartId { get; set; }
        public int monthId { get; set; }
        public int expenseId { get; set; }
        public IEnumerable<SelectListItem> Months { get; set; }
        public IEnumerable<SelectListItem> chartsValue { get; set; }
        public IEnumerable<SelectListItem> expenseTypes { get; set; }

        public List<ExpenseForMonthVM> expenseForMonthVMs { get; set; }
    }
}
