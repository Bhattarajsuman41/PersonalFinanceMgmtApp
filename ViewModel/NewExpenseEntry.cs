using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalFinanceMgmtApp.ViewModel
{
    public class NewExpenseEntry : ExpenseTypeVM
    {
        [Required(ErrorMessage = "This Field is required")]
        public int expenseId { get; set; }
        [Required(ErrorMessage = "This Field is required")]
        public DateTime entryDate { get; set; }

        [Required(ErrorMessage = "This Field is required")]
        [Range(1, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public double ExpenseAmount { get; set; }

        public IEnumerable<SelectListItem> ExpenseTypes { get; set; }

    }
}