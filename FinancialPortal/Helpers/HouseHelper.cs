using FinancialPortal.Extensions;
using FinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Helpers
{
    public class HouseHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public List<Transaction> GetRecentTrans(int hhId)
        {
            var house = db.Households.Find(hhId);
            var recentTrans = new List<Transaction>();

            foreach (var member in house.Members)
            {
                foreach (var transaction in member.Transactions)
                {
                    recentTrans.Add(transaction);
                }
            }
            return recentTrans.OrderByDescending(t => t.Created).Take(5).ToList();
        }
    }
}