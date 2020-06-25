using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace FinancialPortal.Extensions
{
    public static class IdentityExtensions
    {
        public static int? GetHouseholdId(this IIdentity user)
        {
            var claimsIdentity = (ClaimsIdentity)user;
            var HouseholdClaim = claimsIdentity.Claims
                .FirstOrDefault(c => c.Type == "HouseholdId");
            if (HouseholdClaim != null)
            {
                var result = HouseholdClaim.Value != "" ? int.Parse(HouseholdClaim.Value) : 0;
                return result;
            }
            else
            {
                return null;
            }
        }
        public static string GetFullName(this IIdentity user)
        {
            var ClaimsUser = (ClaimsIdentity)user;
            var claim = ClaimsUser.Claims.FirstOrDefault(c => c.Type == "FullName");
            return claim != null ? claim.Value : null;
        }

        public static string GetUserAvatar(this IIdentity user)
        {
            var ClaimsUser = (ClaimsIdentity)user;
            var claim = ClaimsUser.Claims.FirstOrDefault(c => c.Type == "AvatarPath");
            return claim != null ? claim.Value : null;
        }
    }
}