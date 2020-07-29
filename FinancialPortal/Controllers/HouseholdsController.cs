using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinancialPortal.Helpers;
using FinancialPortal.Models;
using Microsoft.AspNet.Identity;
using FinancialPortal.Extensions;
using System.Threading.Tasks;

namespace FinancialPortal.Controllers
{
    [Authorize]
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RolesHelper rolesHelper = new RolesHelper();

        public ActionResult Dashboard()
        {
            var houseId = db.Users.Find(User.Identity.GetUserId()).HouseholdId;
            //var house = db.Households.Find(h => h.BankAccounts).Include;
            return View();
        }

        // GET: Households
        public ActionResult Index()
        {

            return View();
        }

        // GET: Households/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // GET: Households/Create
        public ActionResult Create()
        {
            return View(new Household());
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,HouseName,Greeting,Created")] Household household)
        {
            if (ModelState.IsValid)
            {
                household.Created = DateTime.Now;
                db.Households.Add(household);
                db.SaveChanges();

                var user = db.Users.Find(User.Identity.GetUserId());
                user.HouseholdId = household.Id;
                rolesHelper.AddUserToRole(user.Id, "Head");
                db.SaveChanges();

                await HttpContextBaseExtension.RefreshAuthentication(HttpContext, user);

                return RedirectToAction("Index", "Home");

            }
            return RedirectToAction("Dashboard");
        }

        // GET: Households/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Greeting,Created")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(household);
        }

        // GET: Households/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveHHMember(string userId)
        {
            var member = db.Users.Find(userId);
            if (member != null)
            {
                member.HouseholdId = null;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TransferPower(string memberId)
        {
            var userId = User.Identity.GetUserId();

            //Remove our role as Head of Household  Head
            //Add the role of Member to former Head
            rolesHelper.RemoveUserFromRole(userId, "Head");
            rolesHelper.AddUserToRole(userId, "Member");

            //Drop role from Member
            //Add role of Head to former Member
            rolesHelper.RemoveUserFromRole(memberId, "Member");
            rolesHelper.AddUserToRole(memberId, "Head");

           await HttpContext.RefreshAuthentication(db.Users.Find(userId));

            //Reauthorize current user
            return RedirectToAction("Index","Home");
        }
    }
    
}
