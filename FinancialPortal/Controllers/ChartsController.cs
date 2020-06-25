using FinancialPortal.Extensions;
using FinancialPortal.Models;
using FinancialPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinancialPortal.Controllers
{
    public class ChartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Charts
        public ActionResult Index()
        {
            ViewBag.BankAccounts = new SelectList(db.BankAccounts, "Id", "Name");
            ViewBag.TransactionTypes = new SelectList(db.TransactionTypes, "Id", "Name");

            return View();
        }
        public JsonResult GetSpendingChartData()
        {
            //Setting up (Instantiating) data structures
            var barChartVM = new BarChartVM();
            var actualChartDataSet = new BarChartDataSet();
            var targetChartDataSet = new BarChartDataSet();


            var hhId = User.Identity.GetHouseholdId();
            //Select first seven of the latest
            var categories = db.Categories.Where(c => c.HouseholdId == hhId && c.DebitCredit == false).ToList();  //False 'DebitCredit' indicates a Debit

            foreach (var cat in categories)
            {

                var tTypeId = db.TransactionTypes.FirstOrDefault(t => t.Type == "Withdrawal").Id;
                var totalActual = cat.CategoryItems.SelectMany(c => c.Transactions).Where(t => t.TransactionTypeId == tTypeId).Select(t => t.Amount).Sum();
                var totalTarget = cat.TargetAmount;

                //Adding actual/target amounts to datasets
                actualChartDataSet.data.Add(totalActual);
                targetChartDataSet.data.Add(totalTarget);

                //Adding chart labels to ViewModel
                barChartVM.labels.Add(cat.Name);
            }


            //Adding 'Actual Spending' data to the open dataset
            actualChartDataSet.label = "Actual Spending";
            actualChartDataSet.backgroundColor = "rgba(60,141,188,0.9)";
            actualChartDataSet.borderColor = "rgba(60,141,188,0.8)";
            actualChartDataSet.pointRadius = false;
            actualChartDataSet.pointColor = "#208cff";
            actualChartDataSet.pointStrokeColor = "#208cff";
            actualChartDataSet.pointHighlightFill = "#208cff";
            actualChartDataSet.pointHighlightStroke = "#208cff";

            //Adding 'Target Spending' data to the closed dataset
            targetChartDataSet.label = "Target Spending";
            targetChartDataSet.backgroundColor = "rgba(210, 214, 222, 1)";
            targetChartDataSet.borderColor = "rgba(210, 214, 222, 1)";
            targetChartDataSet.pointRadius = false;
            targetChartDataSet.pointColor = "rgba(210, 214, 222, 1)";
            targetChartDataSet.pointStrokeColor = "#c1c7d1";
            targetChartDataSet.pointHighlightFill = "#fff";
            targetChartDataSet.pointHighlightStroke = "rgba(220,220,220,1)";

            //Adding datasets to ViewModel
            barChartVM.datasets.Add(actualChartDataSet);
            barChartVM.datasets.Add(targetChartDataSet);

            //Sending all data via ViewModel to the View
            return Json(barChartVM, JsonRequestBehavior.AllowGet);

        }

        // GET: GetCurrentBalancChart
        public JsonResult AccountBalanceChartData()
        {
            var hhId = User.Identity.GetHouseholdId();
            var colorList = new List<string>();

            colorList.Add("#4f98c3");
            colorList.Add("#d2d6de");
            colorList.Add("#232323");
            colorList.Add("#FFFF00");
            colorList.Add("#000000");

            var rand = new Random();
            var chartVM = new ChartVM();
            var chartDataSet = new ChartDataSet();
            var bankaccounts = db.BankAccounts.Where(b => b.HouseholdId == hhId).ToList();
            var dataKey = 0;
            //chartVM.Datasets.Insert(dataKey, "");
            foreach (var accountbalance in bankaccounts)
            {
                //barChartVM.Datasets.Add(new KeyValuePair<int, string>(count, colorList[dataKey]));

                chartDataSet.data.Add(accountbalance.CurrentBalance);
                chartDataSet.backgroundColor.Add(colorList[dataKey]);
                chartVM.labels.Add(accountbalance.BankAccountType.Type);
                dataKey++;
            }
            chartVM.datasets.Add(chartDataSet);

            return Json(chartVM, JsonRequestBehavior.AllowGet);
        }

    }
}
