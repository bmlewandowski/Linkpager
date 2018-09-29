using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Common;

namespace Linkpager
{
    public partial class Viewpage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            PopulateLabels();
            PopulateLinkRepeater();

        }



//// BEGIN GENERAL /////

        // Return ListId from QueryString
        public string grabListId()
        {
            string qString = Request.QueryString["ListId"];
            return qString;
        }


        public void PopulateLabels()
        {

            // Select Lists from DB
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT tbl_ListsMaster.ListId, tbl_ListsMaster.ListName, tbl_ListsMaster.ListDescription, tbl_ListsMaster.ListDate, tbl_ListsMaster.UserId, aspnet_Users.UserId, aspnet_Users.UserName FROM tbl_ListsMaster, aspnet_Users WHERE aspnet_Users.UserId = tbl_ListsMaster.UserId AND  tbl_ListsMaster.ListId ='" + grabListId() + "'", sqlConn);
            cmd.Connection.Open();

            SqlDataReader GetListTitle;
            GetListTitle = cmd.ExecuteReader();
            while (GetListTitle.Read())
            {
                string AuthorName = GetListTitle["UserName"].ToString();
                string ListName = GetListTitle["ListName"].ToString();
                string ListDescription = GetListTitle["ListDescription"].ToString();
                this.lbl_ListTitle.Text = ListName;
                this.lbl_Author.Text = AuthorName;
                this.lbl_ListDescription.Text = ListDescription;
                Page.Header.Title = "Linkpager | " + ListName + " | " + AuthorName + " | " + ListDescription;
            }

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }


        public void PopulateLinkRepeater()
        {

            // Select Lists from DB
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT tbl_ListsMaster.ListId, tbl_ListsMaster.ListName, tbl_ListsMaster.ListDescription, tbl_ListsMembers.ListsMembersId, tbl_ListsMembers.ListId, tbl_ListsMembers.LinkId, tbl_ListsMembers.ListsMembersDate, tbl_LinksMaster.LinkId, tbl_LinksMaster.LinkUrl, tbl_LinksMaster.LinkName, tbl_LinksMaster.LinkDescription, tbl_LinksMaster.LinkDate FROM tbl_ListsMaster, tbl_ListsMembers, tbl_LinksMaster WHERE tbl_ListsMaster.ListId ='" + grabListId() + "' AND tbl_ListsMembers.ListId ='" + grabListId() + "' AND tbl_ListsMembers.LinkId = tbl_LinksMaster.LinkId AND tbl_LinksMaster.LinkPrivate = 'false'", sqlConn);
            cmd.Connection.Open();

            SqlDataReader RepValues;
            RepValues = cmd.ExecuteReader();

            this.Link_Repeater.DataSource = RepValues;
            this.Link_Repeater.DataBind();

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }


//// END GENERAL /////


//// BEGIN REPEATER EVENTS ////

        protected void Link_Repeater_OnItemDataBound(Object sender, RepeaterItemEventArgs e)
        {
            // Trim text to set number of characters and add "..."
            DbDataRecord rec = (DbDataRecord)e.Item.DataItem;
            // Make sure that you have the data
            if (rec != null)
            {
                Label lblname = (Label)e.Item.FindControl("lbl_LinkName");
                string sLinkName = rec["LinkName"].ToString();
                int stringLengthName = sLinkName.Length;

                Label lbldesc = (Label)e.Item.FindControl("lbl_LinkDescription");
                string sLinkDesc = rec["LinkDescription"].ToString();
                int stringLengthDesc = sLinkDesc.Length;

                // Check Name
                if (stringLengthName > 37)
                {
                    lblname.Text = sLinkName.Substring(0, Math.Min(37, sLinkName.ToString().Length)) + "...";
                }

                else
                {
                    // List Name is Under 37 Char
                    lblname.Text = sLinkName;
                }

                // Check Description
                if (stringLengthDesc > 230)
                {
                    lbldesc.Text = sLinkDesc.Substring(0, Math.Min(230, sLinkDesc.ToString().Length)) + "...";
                }

                else
                {
                    // List Description is Under 230 Char
                    lbldesc.Text = sLinkDesc;
                }

            }
        }

//// END REPEATER EVENTS ////


    }
}