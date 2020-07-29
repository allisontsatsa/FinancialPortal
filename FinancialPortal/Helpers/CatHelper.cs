using FinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Helpers
{
    public class CatHelper
    {
        public static string GetCatName(int id)
        {
            var db = new ApplicationDbContext();
            return db.Categories.Find(id).Name;
        }
    }
}