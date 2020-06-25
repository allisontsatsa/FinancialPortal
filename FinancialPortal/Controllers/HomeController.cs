using FinancialPortal.Extensions;
using FinancialPortal.Helpers;
using FinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinancialPortal.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public HouseHelper houseHelper = new HouseHelper();
        public ActionResult Index()
        {
            var hhId = User.Identity.GetHouseholdId();
            var model = db.Households.Find(hhId);

            if (model != null)
            {
                var categoryItems = db.Categories.Where(c => c.HouseholdId == hhId).SelectMany(c => c.CategoryItems);

                List<BankAccount> accounts = db.BankAccounts.Where(b => b.HouseholdId == hhId).ToList();

                ViewBag.Recent = houseHelper.GetRecentTrans(model.Id);

                ViewBag.BankAccountId = new SelectList(accounts, "Id", "Name");
                ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "Id", "Type");
                ViewBag.CategoryItemId = new SelectList(categoryItems, "Id", "Name");

            }
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}