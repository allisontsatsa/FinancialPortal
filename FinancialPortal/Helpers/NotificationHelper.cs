using FinancialPortal.Controllers;
using FinancialPortal.Extensions;
using FinancialPortal.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;

namespace FinancialPortal.Helpers
{
    public class NotificationHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void ManageNotifications(BankAccount account, Transaction transaction)
        {
            ManageNotification(account, transaction);
        }
        private void ManageNotification(BankAccount account, Transaction transaction)
        {
            var overdraft = account.CurrentBalance < 0;
            var warning = account.CurrentBalance <= account.LowBalance;


            if (overdraft || warning)
            {
                var variableText = account.CurrentBalance < 0 ? "an overdraft" : "a low balance level";
                var message = $"Your transaction in the amount of {transaction.Amount} has resulted in {variableText}";
                GenerateNotification(message, account.HouseholdId);
            }

        }
        private void GenerateNotification(string message, int householdId)
        {
            var newNotification = new Notification
            {
                HouseholdId = householdId,
                Created = DateTime.Now,
                Subject = "Check Yo Balance!",
                Body = message,
                IsRead = false

            };
            db.Notifications.Add(newNotification);
            db.SaveChanges();
                
        }
        public static List<Notification> GetNotifications()
        {
            var hhId = HttpContext.Current.User.Identity.GetHouseholdId();

            if (hhId == null)
                return new List<Notification>();

            var db = new ApplicationDbContext();
            return db.Notifications.Where(t => t.HouseholdId == hhId && !t.IsRead).ToList();
        }


    }




}

