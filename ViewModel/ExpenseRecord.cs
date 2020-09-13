using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalFinanceMgmtApp.ViewModel
{
    public class ExpenseRecord 
    {
        public int expenseId { get; set; }
        
        public string month { get; set; }

        public string expenseType { get; set; }

        public double amount { get; set; }

        public DateTime entryDate { get; set; }
    }
}