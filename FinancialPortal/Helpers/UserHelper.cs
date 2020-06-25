using FinancialPortal.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Extensions
{
    public class UserHelper
    {
        private ApplicationDbContext Db = new ApplicationDbContext();
        private string UserId { get; set; }

        public UserHelper()
        {
            UserId = HttpContext.Current.User.Identity.GetUserId();
        }

        public string GetUserAvatar()
        {
            if(UserId != null) 
            { 
            return Db.Users.Find(UserId).AvatarPath;
            }
            return string.Empty;
        }
    }

}
