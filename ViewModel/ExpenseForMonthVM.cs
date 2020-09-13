using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalFinanceMgmtApp.ViewModel
{
    public class ExpenseForMonthVM
    {
        public string expenseType { get; set; }
       
        public totalExpenseForMonth totalExpenseForMonth { get; set; }
       public List<expenseForSingleExpenseTypeVM> expenseForSingleExpenseTypeVMs { get; set; }

    }
}