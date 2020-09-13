using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalFinanceMgmtApp.ViewModel
{
    public class totalExpenseForMonth
    {
        public string MonthName { get; set; }
        public double totalBlc { get; set; }
        public double totalShping { get; set; }
        public double totalBills { get; set; }
        public double TotalFd { get; set; }
        public double TotalOthers { get; set; }
    }
        
}