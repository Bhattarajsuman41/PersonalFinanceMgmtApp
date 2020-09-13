using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalFinanceMgmtApp.ViewModel
{
    public class NewExpenseEntry : ExpenseTypeVM
    {
        public int expenseId { get; set; }
        public DateTime entryDate { get; set; }
        public double ExpenseAmount{ get; set; }

        public IEnumerable<SelectListItem> ExpenseTypes { get; set; }
        
    }
}