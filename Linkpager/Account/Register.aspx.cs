using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Linkpager.Account
{
    public partial class Register : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {


            // First Load Events
            if (!IsPostBack)
            {

                // If logged in redirect to console
                if (User.Identity.IsAuthenticated == true)
                {
                    Response.Redirect("~/Linkpages.aspx");
                }

            }


            RegisterUser.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];
        }

        protected void RegisterUser_CreatedUser(object sender, EventArgs e)
        {

            // Create User Folder for Images
            string UserFolderPath = Server.MapPath("~/Images/Users/" + RegisterUser.UserName);
            System.IO.Directory.CreateDirectory(UserFolderPath);


            // Create Persistant Cookie
            FormsAuthentication.SetAuthCookie(RegisterUser.UserName, false /* createPersistentCookie */);
            
            // Grab Destination Page
            string continueUrl = RegisterUser.ContinueDestinationPageUrl;
            
            // If No Desgination Page, Forward to Root
            if (String.IsNullOrEmpty(continueUrl))
            {
                continueUrl = "~/Linkpages.aspx";
            }
            Response.Redirect(continueUrl);
        }

    }
}
