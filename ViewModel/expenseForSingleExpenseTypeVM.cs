using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalFinanceMgmtApp.ViewModel
{
    public class expenseForSingleExpenseTypeVM
    {
        public int monthId { get; set; }
        public double totalAmount { get; set; }

        public string month { get; set; }
    }
}