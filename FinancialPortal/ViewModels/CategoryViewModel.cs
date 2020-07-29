using FinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.ViewModels
{
    public class CategoryViewModel
    {
        public List<Category> Categories { get; set; }
        public int DebitTransId { get; set; }
        public int CreditTransId { get; set; }
    }
}