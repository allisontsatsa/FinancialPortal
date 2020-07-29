using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinancialPortal.Extensions;
using FinancialPortal.Models;
using FinancialPortal.ViewModels;
using Microsoft.AspNet.Identity;

namespace FinancialPortal.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
        public ActionResult Index()
        {
            var hhId = User.Identity.GetHouseholdId();
            var categories = db.Categories.Where(a => a.HouseholdId == hhId).ToList();
            var debit = categories.Where(d => d.DebitCredit == false).ToList();

            CategoryViewModel model = new CategoryViewModel();
            model.Categories = categories;
            model.DebitTransId = db.TransactionTypes.FirstOrDefault(t => t.Type == "Withdrawal").Id;
            model.CreditTransId = db.TransactionTypes.FirstOrDefault(t => t.Type == "Deposit").Id;

            return View(model);
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult CreateDebitCategory()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDebitCategory(string name, string description, decimal target)
        {
            var hhId = User.Identity.GetHouseholdId();
            var category = new Category
            {
                Name = name,
                Description = description,
                TargetAmount = target,
                HouseholdId = hhId,
                DebitCredit = false

            };
            if (ModelState.IsValid)
            {

                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index", "Categories");
            }

            //ViewBag.Categories = new SelectList(db.Categories, "Id", "Name", category.Name);
            return View(category);
        }

        public ActionResult CreateCreditCategory()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCreditCategory([Bind(Include = "Name,Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                var hhId = User.Identity.GetHouseholdId();

                category.HouseholdId = hhId;
                category.DebitCredit = true;
                category.TargetAmount = 0;
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index", "Categories");
            }

            ViewBag.Categories = new SelectList(db.Categories, "Id", "Name", category.Name);
            return View(category);
        }


        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)

        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseHoldId = new SelectList(db.Households, "Id", "HouseName", category.HouseholdId);
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HouseHoldId,Name,Description,TargetAmount")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseHoldId = new SelectList(db.Households, "Id", "HouseName", category.HouseholdId);
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
