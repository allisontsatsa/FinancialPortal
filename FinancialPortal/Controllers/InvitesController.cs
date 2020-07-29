using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using FinancialPortal.Extensions;
using FinancialPortal.Models;
using Microsoft.AspNet.Identity;

namespace FinancialPortal.Controllers
{
    public class InvitesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Invites
        public ActionResult Index()
        {
            var invites = db.Invites.Include(i => i.Household);
            return View(invites.ToList());
        }

        // GET: Invites/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invite invite = db.Invites.Find(id);
            if (invite == null)
            {
                return HttpNotFound();
            }
            return View(invite);
        }

        // GET: Invites/Create
        public ActionResult Create()
        {
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name");
            return View(new Invite());
        }

        // POST: Invites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Invite invite)
        {
            var householdId = User.Identity.GetHouseholdId();

            if (ModelState.IsValid)
            {
                invite.Created = DateTime.Now;
                invite.HouseholdId = householdId.Value;
                invite.Code = Guid.NewGuid();
                invite.Valid = true;
                db.Invites.Add(invite);
                db.SaveChanges();

                var callbackUrl = Url.Action("AcceptInvite", "Account", new { email = invite.RecipientEmail, code = invite.Code }, protocol: Request.Url.Scheme);

                try
                {
                    var emailAddress = WebConfigurationManager.AppSettings["EmailFrom"];
                    var emailFrom = $"Financial Portal<{emailAddress}>";

                    var email = new MailMessage(emailFrom, invite.RecipientEmail)
                    {
                        Subject = $"Invite from {User.Identity.GetFullName()}",
                        Body = $"{invite.Body}<hr>Please accept this invitation by clicking <a href='" + callbackUrl + "'>here</a>",
                        IsBodyHtml = true
                    };

                    var emailSvc = new EmailService();
                    await emailSvc.SendAsync(email);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
                return RedirectToAction("Index", "Home");
            }

            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", invite.HouseholdId);
            return View();
        }

        // GET: Invites/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invite invite = db.Invites.Find(id);
            if (invite == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", invite.HouseholdId);
            return View(invite);
        }

        // POST: Invites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HouseholdId")] Invite invite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invite).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", invite.HouseholdId);
            return View(invite);
        }

        // GET: Invites/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invite invite = db.Invites.Find(id);
            if (invite == null)
            {
                return HttpNotFound();
            }
            return View(invite);
        }

        // POST: Invites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invite invite = db.Invites.Find(id);
            db.Invites.Remove(invite);
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
