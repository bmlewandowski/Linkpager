using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Linkpager
{
    public class FetchUser
    {
        // Return ListId from QueryString
        public static string UserID()
        {
            // Get UserID from ASP.NET Membership
            MembershipUser currentUser = Membership.GetUser();
            string UserID = currentUser.ProviderUserKey.ToString();
            return UserID;
        }

        // Return ListId from QueryString
        public static string UserName()
        {
            // Get UserID from ASP.NET Membership
            MembershipUser currentUser = Membership.GetUser();
            string UserName = currentUser.UserName.ToString();
            return UserName;
        }

    }
}