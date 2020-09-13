using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalFinanceMgmtApp.ViewModel
{
    public class ExpenseTypeVM
    {
        public int Id { get; set; }
        public  string expenseType { get; set; }

        public double Amount { get; set; }
        public string ExpenseColor { get; set; }
    }
}