using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PersonalFinanceMgmtApp.DataLayer;
using PersonalFinanceMgmtApp.ViewModel;
using PagedList;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.Owin;

namespace PersonalFinanceMgmtApp.BusinessLogicLayer
{
    public class operations
    {
        PersonalFinanceDBEntities db = new PersonalFinanceDBEntities();
        public operations()
        {
            PersonalFinanceDBEntities _db = db;
        }

        #region dropdown
        public IEnumerable<SelectListItem> getAllExpense()
        {
            var items = db.ExpenseTypes.ToList().Select(m => new SelectListItem
            {
                Value = m.ExpenseTypeId.ToString(),
                Text = m.ExpenseTypeName
            });
            return items;
        }
        public IEnumerable<SelectListItem> getChartDropdownValue()
        {
            var items = db.Charts.ToList().Select(m => new SelectListItem
            {
                Value = m.ChartsId.ToString(),
                Text = m.ChartsName
            });
            return items;
        }
        public IEnumerable<SelectListItem> getMonthDropdownValue()
        {
            var items = db.Months.ToList().Select(m => new SelectListItem
            {
                Value = m.MonthId.ToString(),
                Text = m.MonthName
            });
            return items;
        }
        #endregion
        public totalExpenseForMonth getTotalExpenseForMonth(List<ExpenseRecord> er)
        {
            totalExpenseForMonth expense = new totalExpenseForMonth();
            var monthId = er.ElementAt(0).entryDate.Month;
            expense.MonthName = db.Months.Where(x => x.MonthId == monthId).Select(x => x.MonthName).First().ToString();
            foreach (var item in er.OrderByDescending(x=>x.expenseId))
            {
                var eId = item.expenseId;
                var eType = item.expenseType;
               

                if (eId == 2 && eType == "Shopping")
                {
                    expense.totalShping += Convert.ToInt32(item.amount);
                }
                else if (eId == 3 && eType == "Food")
                {
                    expense.TotalFd += Convert.ToInt32(item.amount);
                }

                else if (eId == 4 && eType == "Bills")
                {
                    expense.totalBills += Convert.ToInt32(item.amount);
                }

                else if(eId== 5)
                {
                    expense.TotalOthers += Convert.ToInt32(item.amount);
                }
                else if (eId == 1 && eType == "Balance")
                {
                    expense.totalBlc += Convert.ToInt32(item.amount)-(expense.totalBills+ expense.totalShping+expense.TotalFd+expense.TotalOthers);
                }
            }
            return expense;
        }

        public totalExpenseForMonth getAllSavedExpenseForMonthForPieCharts(int? monthId, string userId)
        {
            totalExpenseForMonth record = new totalExpenseForMonth();

            if (monthId != null)
            {



                var list1 = db.NewExpenseEntryRecords.Where(x => x.MonthId == monthId && x.UserId == userId).ToList();




                foreach (var item in list1)
                {

                    if (item.ExpenseTypeId == 1)
                    {
                        record.totalBlc += Convert.ToInt32(item.Amount);
                    }

                    else if (item.ExpenseTypeId == 2)
                    {
                        record.totalShping += Convert.ToInt32(item.Amount);
                    }
                    else if (item.ExpenseTypeId == 3)
                    {
                        record.TotalFd += Convert.ToInt32(item.Amount);
                    }

                    else if (item.ExpenseTypeId == 4)
                    {
                        record.totalBills += Convert.ToInt32(item.Amount);
                    }

                    else
                    {
                        record.TotalOthers += Convert.ToInt32(item.Amount);
                    }
                }

            }
            else
            {
                var result = db.NewExpenseEntryRecords.Where(x=>x.UserId== userId).ToList().OrderByDescending(x => x.MonthId).Select(x => x.MonthId).First();

                var list1 = db.NewExpenseEntryRecords.Where(x => x.MonthId == result && x.UserId == userId).ToList();




                foreach (var item in list1)
                {

                    if (item.ExpenseTypeId == 1)
                    {
                        record.totalBlc += Convert.ToInt32(item.Amount);
                    }

                    else if (item.ExpenseTypeId == 2)
                    {
                        record.totalShping += Convert.ToInt32(item.Amount);
                    }
                    else if (item.ExpenseTypeId == 3)
                    {
                        record.TotalFd += Convert.ToInt32(item.Amount);
                    }

                    else if (item.ExpenseTypeId == 4)
                    {
                        record.totalBills += Convert.ToInt32(item.Amount);
                    }

                    else
                    {
                        record.TotalOthers += Convert.ToInt32(item.Amount);
                    }
                }


            }

            return record ;
        }


        public IPagedList<ExpenseRecord> getAllSavedExpense(int? expenseId,int? monthId, int? page, string userId)
        {
           if(monthId != null && expenseId == null)
            {
                var result = db.NewExpenseEntryRecords.Where(x=>x.MonthId == monthId && x.UserId == userId).ToList().OrderByDescending(x => x.MonthId);
                List<ExpenseRecord> record = new List<ExpenseRecord>();

                foreach (var items in result)
                {
                    var expenseType = db.ExpenseTypes.Where(x => x.ExpenseTypeId == items.ExpenseTypeId).Select(x => x.ExpenseTypeName).FirstOrDefault().ToString();
                    var month = items.EntryDate.Value.Month;
                    var monthName = db.Months.Where(x => x.MonthId == month).Select(x => x.MonthName).FirstOrDefault();
                    record.Add(new ExpenseRecord()
                    {
                        entryDate = Convert.ToDateTime(items.EntryDate),
                        amount = Convert.ToDouble(items.Amount),
                        expenseId = Convert.ToInt32(items.ExpenseTypeId),
                        expenseType = expenseType,
                        month = Convert.ToString(monthName)
                    });

                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return record.ToPagedList(pageNumber, pageSize);
            }
            
            else if(expenseId != null && monthId == null)
            {
                var result = db.NewExpenseEntryRecords.Where(x=>x.ExpenseTypeId == expenseId && x.UserId == userId).ToList().OrderBy(x => x.ExpenseTypeId);
                List<ExpenseRecord> record = new List<ExpenseRecord>();

                foreach (var items in result)
                {
                    var expenseType = db.ExpenseTypes.Where(x => x.ExpenseTypeId == items.ExpenseTypeId).Select(x => x.ExpenseTypeName).FirstOrDefault().ToString();
                    var month = items.EntryDate.Value.Month;
                    var monthName = db.Months.Where(x => x.MonthId == month).Select(x => x.MonthName).FirstOrDefault();
                    record.Add(new ExpenseRecord()
                    {
                        entryDate = Convert.ToDateTime(items.EntryDate),
                        amount = Convert.ToDouble(items.Amount),
                        expenseId = Convert.ToInt32(items.ExpenseTypeId),
                        expenseType = expenseType,
                        month = Convert.ToString(monthName)
                    });

                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return record.ToPagedList(pageNumber, pageSize);
            }
            else if (monthId != null && expenseId != null)
            {
                var result = db.NewExpenseEntryRecords.Where(x => x.MonthId == monthId && x.ExpenseTypeId == expenseId && x.UserId == userId).ToList().OrderByDescending(x => x.MonthId);
                List<ExpenseRecord> record = new List<ExpenseRecord>();

                foreach (var items in result)
                {
                    var expenseType = db.ExpenseTypes.Where(x => x.ExpenseTypeId == items.ExpenseTypeId).Select(x => x.ExpenseTypeName).FirstOrDefault().ToString();
                    var month = items.EntryDate.Value.Month;
                    var monthName = db.Months.Where(x => x.MonthId == month).Select(x => x.MonthName).FirstOrDefault();
                    record.Add(new ExpenseRecord()
                    {
                        entryDate = Convert.ToDateTime(items.EntryDate),
                        amount = Convert.ToDouble(items.Amount),
                        expenseId = Convert.ToInt32(items.ExpenseTypeId),
                        expenseType = expenseType,
                        month = Convert.ToString(monthName)
                    });

                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return record.ToPagedList(pageNumber, pageSize);
            }
            else
            {
                var result = db.NewExpenseEntryRecords.Where(x => x.UserId == userId).ToList().OrderByDescending(x => x.MonthId);
                List<ExpenseRecord> record = new List<ExpenseRecord>();

                foreach (var items in result)
                {
                    var expenseType = db.ExpenseTypes.Where(x => x.ExpenseTypeId == items.ExpenseTypeId).Select(x => x.ExpenseTypeName).FirstOrDefault().ToString();
                    var month = items.EntryDate.Value.Month;
                    var monthName = db.Months.Where(x => x.MonthId == month).Select(x => x.MonthName).FirstOrDefault();
                    record.Add(new ExpenseRecord()
                    {
                        entryDate = Convert.ToDateTime(items.EntryDate),
                        amount = Convert.ToDouble(items.Amount),
                        expenseId = Convert.ToInt32(items.ExpenseTypeId),
                        expenseType = expenseType,
                        month = Convert.ToString(monthName)
                    });

                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return record.ToPagedList(pageNumber, pageSize);
            }

        }
        public IPagedList<ExpenseRecord> getAllSavedExpensebyMonth(int? monthId, int? page, string userId)
        {
            try
            {
                if (monthId != null)
                {
                    var result = db.NewExpenseEntryRecords.Where(x => x.MonthId == monthId && x.UserId == userId).ToList().OrderByDescending(x => x.MonthId).GroupBy(x => x.MonthId).FirstOrDefault();
                    List<ExpenseRecord> record = new List<ExpenseRecord>();


                    foreach (var items in result)
                    {
                        var expenseType = db.ExpenseTypes.Where(x => x.ExpenseTypeId == items.ExpenseTypeId).Select(x => x.ExpenseTypeName).FirstOrDefault().ToString();
                        var month = items.EntryDate.Value.Month;
                        record.Add(new ExpenseRecord()
                        {
                            entryDate = Convert.ToDateTime(items.EntryDate),
                            amount = Convert.ToDouble(items.Amount),
                            expenseId = Convert.ToInt32(items.ExpenseTypeId),
                            expenseType = expenseType,
                            month = Convert.ToString(month)
                        });

                    }
                    int pageSize = 5;
                    int pageNumber = (page ?? 1);
                    return record.ToPagedList(pageNumber, pageSize);
                }
                else
                {
                    var result = db.NewExpenseEntryRecords.Where(x => x.UserId == userId).ToList().OrderByDescending(x => x.MonthId).GroupBy(x => x.MonthId).FirstOrDefault();
                    List<ExpenseRecord> record = new List<ExpenseRecord>();


                    foreach (var items in result)
                    {
                        var expenseType = db.ExpenseTypes.Where(x => x.ExpenseTypeId == items.ExpenseTypeId).Select(x => x.ExpenseTypeName).FirstOrDefault().ToString();
                        var month = items.EntryDate.Value.Month;
                        record.Add(new ExpenseRecord()
                        {
                            entryDate = Convert.ToDateTime(items.EntryDate),
                            amount = Convert.ToDouble(items.Amount),
                            expenseId = Convert.ToInt32(items.ExpenseTypeId),
                            expenseType = expenseType,
                            month = Convert.ToString(month)
                        });

                    }
                    int pageSize = 5;
                    int pageNumber = (page ?? 1);
                    return record.ToPagedList(pageNumber, pageSize);
                }
            }
            catch(NullReferenceException e)
            {
                throw new NullReferenceException("Value Cannot be null", e);
            }
            

        }
        public List<ExpenseForMonthVM> getAllSavedExpenseForMonth(string userId)
        {
            var result = db.NewExpenseEntryRecords.Where(x=>x.UserId == userId).ToList().OrderByDescending(x => x.MonthId).GroupBy(x => x.ExpenseTypeId);

            List<ExpenseForMonthVM> record = new List<ExpenseForMonthVM>();



            foreach (var items in result)
            {


                var expenseType = db.ExpenseTypes.Where(x => x.ExpenseTypeId == items.Key).Select(x => x.ExpenseTypeName).FirstOrDefault().ToString();
                var totalAmount = 0;
                var dahh = items.GroupBy(x => x.MonthId);
                foreach (var item in dahh)
                {
                    var month = db.Months.Where(x => x.MonthId == item.Key).Select(x => x.MonthName).FirstOrDefault().ToString();
                    foreach (var o in item)
                    {

                        totalAmount += Convert.ToInt32(o.Amount);

                    }

                    record.Add(new ExpenseForMonthVM
                    {
                        expenseType = expenseType,
                        expenseForSingleExpenseTypeVMs = new List<expenseForSingleExpenseTypeVM>
                        {
                           new expenseForSingleExpenseTypeVM
                           {
                               monthId = Convert.ToInt32(item.Key),
                               month = month,
                               totalAmount = totalAmount
                           }
                        }
                    });
                    totalAmount = 0;
                }
            }
            //var ff = record.GroupBy(x => x.expenseType);

            return record;

        }

        public List<ExpenseRecord> getSavedExpensebyMonth(int? monthId, string userId)
        {
            if(monthId != null)
            {
                var result = db.NewExpenseEntryRecords.Where(x=>x.MonthId == monthId && x.UserId == userId).ToList().OrderByDescending(x => x.MonthId).GroupBy(x => x.MonthId).FirstOrDefault();
                List<ExpenseRecord> record = new List<ExpenseRecord>();

                foreach (var items in result)
                {
                    var expenseType = db.ExpenseTypes.Where(x => x.ExpenseTypeId == items.ExpenseTypeId).Select(x => x.ExpenseTypeName).FirstOrDefault().ToString();
                    var month = items.EntryDate.Value.Month;
                    record.Add(new ExpenseRecord()
                    {
                        entryDate = Convert.ToDateTime(items.EntryDate),
                        amount = Convert.ToDouble(items.Amount),
                        expenseId = Convert.ToInt32(items.ExpenseTypeId),
                        expenseType = expenseType,
                        month = Convert.ToString(month)
                    });

                }
                return record;

            }
            else
            {
                var result = db.NewExpenseEntryRecords.Where(x => x.UserId == userId).ToList().OrderByDescending(x => x.MonthId).GroupBy(x => x.MonthId).FirstOrDefault();
                List<ExpenseRecord> record = new List<ExpenseRecord>();

                foreach (var items in result)
                {
                    var expenseType = db.ExpenseTypes.Where(x => x.ExpenseTypeId == items.ExpenseTypeId).Select(x => x.ExpenseTypeName).FirstOrDefault().ToString();
                    var month = items.EntryDate.Value.Month;
                    record.Add(new ExpenseRecord()
                    {
                        entryDate = Convert.ToDateTime(items.EntryDate),
                        amount = Convert.ToDouble(items.Amount),
                        expenseId = Convert.ToInt32(items.ExpenseTypeId),
                        expenseType = expenseType,
                        month = Convert.ToString(month)
                    });

                }
                return record;

            }

        }


        public string saveNewExpense(NewExpenseEntry exp, string userId)
        {

            if (exp != null)
            {
                //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                //var userId = UserManagerExtensions.FindById(HttpContext.Current.User.Identity.GetUserId());
                DataLayer.NewExpenseEntryRecord et = new DataLayer.NewExpenseEntryRecord();

                et.EntryDate = exp.entryDate;
                et.ExpenseTypeId = exp.expenseId;
                et.Amount = Convert.ToDecimal(exp.ExpenseAmount);
                et.MonthId = exp.entryDate.Month;
                et.UserId = userId;
                db.NewExpenseEntryRecords.Add(et);
                db.SaveChanges();


                NewExpenseRecord ner = new NewExpenseRecord
                {
                    MonthId = exp.entryDate.Month,
                    ExpenseTypeId = exp.expenseId,
                    Amount = Convert.ToDecimal(exp.ExpenseAmount),
                    UserId = userId
            };
            db.NewExpenseRecords.Add(ner);
            db.SaveChanges();
            return "Saved Successfully!!!";
            }
            else
            {
                return "Error in Saving!!!";            }
        }



    }
}



