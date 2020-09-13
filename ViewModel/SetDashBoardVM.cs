using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace PersonalFinanceMgmtApp.ViewModel
{
    public class SetDashBoardVM
    {
        public int monthId { get; set; }
        public int expenseId { get; set; }
        public IEnumerable<SelectListItem> Months { get; set; }
        public IEnumerable<SelectListItem> ExpenseTypes{ get; set; }
        public totalExpenseForMonth totalExpenseForMonth { get; set; }
        public IPagedList<ExpenseRecord> expenseRecords { get; set; }
    }
}