using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace Linkpager
{
    public partial class Linkpages : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            // If not logged in redirect to public
            if (User.Identity.IsAuthenticated == false)
            {
                Server.Transfer("~/Default.aspx");
            }

            // First Load Events
            if (!IsPostBack)
            {
                // Set Initial FolderId Session
                if (Session["FolderId"] == null)
                {
                    Session["FolderId"] = "0";
                }

                BindFolderList();
            }
        }


//// BEGIN LIST REPEATER EVENTS ////

        protected void List_Repeater_OnItemDataBound(Object sender, RepeaterItemEventArgs e)
        {
            // Check Both Repeater RowTypes
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                // Make LinkButtons do AsyncPostback
                LinkButton lbn_delete_list = e.Item.FindControl("lbn_delete_list") as LinkButton;
                ScriptManager sm = ScriptManager.GetCurrent(this.Page);
                sm.RegisterAsyncPostBackControl(lbn_delete_list);
            }

            // Trim Descriptions to 55 Characters and add "..."
            System.Data.Common.DbDataRecord rec = (System.Data.Common.DbDataRecord)e.Item.DataItem;
            // Make sure that you have the data
            if (rec != null)
            {
                Label lbldesc = (Label)e.Item.FindControl("lblListDesc");
                string sListDescription = rec["ListDescription"].ToString();
                int stringLength = sListDescription.Length;

                if (stringLength > 55)
                {
                    lbldesc.Text = sListDescription.Substring(0, Math.Min(55, sListDescription.ToString().Length)) + "...";
                }

                else
                {
                    // List Description is Under 55 Char
                    lbldesc.Text = sListDescription;
                }
            }

        }

        protected void List_Repeater_OnItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteList")
            {

                // Delete List Association 
                SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                SqlCommand cmd = new SqlCommand("usp_list_delete", sqlConn);
                string sListId = e.CommandArgument.ToString();
                int ListId = Int32.Parse(sListId);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ListId", SqlDbType.Int).Value = ListId;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                cmd.Connection.Dispose();

                // Call Update Panel to avoid duplicates
                udp_ListRepeater.Update();
            }
        }

//// END LIST REPEATER EVENTS ////


//// BEGIN LIST UPDATE PANEL EVENTS ////


        protected void udp_ListRepeater_PreRender(object sender, EventArgs e)
        {

            // Set Initial ListSort Session
            if (Session["ListSort"] == null)
            {
                Session["ListSort"] = "0";
            }

            // Get Session Variable
            string sFolderId = Session["FolderId"].ToString();
            string sListSort = Session["ListSort"].ToString();

            // Instantiate SQL String
            string ListSelectSQL;

            // Select SQL Based on Session Variable
            if (sFolderId == "0" || sFolderId == null || sFolderId.Length == 0)
            {


                // Select SQL Based on Session Variable               
                if (sListSort == "NEW")
                {
                    ListSelectSQL = "SELECT tbl_ListsMaster.ListId, tbl_ListsMaster.ListName, tbl_ListsMaster.ListDescription, tbl_ListsMaster.ListDate FROM tbl_ListsMaster WHERE tbl_ListsMaster.UserId ='" + FetchUser.UserID() + "' ORDER BY tbl_ListsMaster.ListDate DESC";
                }

                else if (sListSort == "OLD")
                {
                    ListSelectSQL = "SELECT tbl_ListsMaster.ListId, tbl_ListsMaster.ListName, tbl_ListsMaster.ListDescription, tbl_ListsMaster.ListDate FROM tbl_ListsMaster WHERE tbl_ListsMaster.UserId ='" + FetchUser.UserID() + "' ORDER BY tbl_ListsMaster.ListDate ASC";
                }

                else if (sListSort == "DESC")
                {
                    ListSelectSQL = "SELECT tbl_ListsMaster.ListId, tbl_ListsMaster.ListName, tbl_ListsMaster.ListDescription FROM tbl_ListsMaster WHERE tbl_ListsMaster.UserId ='" + FetchUser.UserID() + "' ORDER BY tbl_ListsMaster.ListName DESC";
                }

                else //ASC or Default
                {
                    ListSelectSQL = "SELECT tbl_ListsMaster.ListId, tbl_ListsMaster.ListName, tbl_ListsMaster.ListDescription FROM tbl_ListsMaster WHERE tbl_ListsMaster.UserId ='" + FetchUser.UserID() + "' ORDER BY tbl_ListsMaster.ListName ASC";
                }

                
            }

            else
            {

                // Select SQL Based on Session Variable               
                if (sListSort == "NEW")
                {
                    ListSelectSQL = "SELECT tbl_ListsMaster.ListId, tbl_ListsMaster.ListName, tbl_FoldersMembers.FolderId, tbl_ListsMaster.ListDescription, tbl_ListsMaster.ListDate FROM tbl_ListsMaster, tbl_FoldersMembers WHERE tbl_FoldersMembers.FolderId ='" + sFolderId + "' AND tbl_FoldersMembers.ItemId = tbl_ListsMaster.ListId ORDER BY tbl_ListsMaster.ListDate DESC";
                }

                else if (sListSort == "OLD")
                {
                    ListSelectSQL = "SELECT tbl_ListsMaster.ListId, tbl_ListsMaster.ListName, tbl_FoldersMembers.FolderId, tbl_ListsMaster.ListDescription, tbl_ListsMaster.ListDate FROM tbl_ListsMaster, tbl_FoldersMembers WHERE tbl_FoldersMembers.FolderId ='" + sFolderId + "' AND tbl_FoldersMembers.ItemId = tbl_ListsMaster.ListId ORDER BY tbl_ListsMaster.ListDate ASC";
                }

                else if (sListSort == "DESC")
                {
                    ListSelectSQL = "SELECT tbl_ListsMaster.ListId, tbl_ListsMaster.ListName, tbl_FoldersMembers.FolderId, tbl_ListsMaster.ListDescription FROM tbl_ListsMaster, tbl_FoldersMembers WHERE tbl_FoldersMembers.FolderId ='" + sFolderId + "' AND tbl_FoldersMembers.ItemId = tbl_ListsMaster.ListId ORDER BY tbl_ListsMaster.ListName DESC";
                }

                else //ASC or Default
                {
                    ListSelectSQL = "SELECT tbl_ListsMaster.ListId, tbl_ListsMaster.ListName, tbl_FoldersMembers.FolderId, tbl_ListsMaster.ListDescription FROM tbl_ListsMaster, tbl_FoldersMembers WHERE tbl_FoldersMembers.FolderId ='" + sFolderId + "' AND tbl_FoldersMembers.ItemId = tbl_ListsMaster.ListId ORDER BY tbl_ListsMaster.ListName ASC";
                }
              
            }

            // Use SQL Statement to Select Records from DB  
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmd = new SqlCommand(ListSelectSQL, sqlConn);
            cmd.Connection.Open();
            SqlDataReader RepValues;
            RepValues = cmd.ExecuteReader();
            this.List_Repeater.DataSource = RepValues;
            this.List_Repeater.DataBind();
            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }


        protected void udp_ListRepeater_Unload(object sender, EventArgs e)
        {

        }

//// END LIST UPDATE PANEL EVENTS ////


//// BEGIN LINK BUTTONS ////

        protected void lbn_selectList_Click(object sender, CommandEventArgs e)
        {
            Session["FolderId"] = e.CommandArgument.ToString();
            udp_ListRepeater.Update();
        }

        protected void lbn_selectHome_Click(object sender, CommandEventArgs e)
        {
            Session["FolderId"] = e.CommandArgument.ToString();
            udp_ListRepeater.Update();
        }

        protected void lbn_sortFolder_ZA_Click(object sender, CommandEventArgs e)
        {
            Session["FolderSort"] = e.CommandArgument.ToString();
            BindFolderList();
        }

        protected void lbn_sortFolder_AZ_Click(object sender, CommandEventArgs e)
        {
            Session["FolderSort"] = e.CommandArgument.ToString();
            BindFolderList();
        }

        protected void lbn_sortFolder_New_Click(object sender, CommandEventArgs e)
        {
            Session["FolderSort"] = e.CommandArgument.ToString();
            BindFolderList();
        }

        protected void lbn_sortFolder_Old_Click(object sender, CommandEventArgs e)
        {
            Session["FolderSort"] = e.CommandArgument.ToString();
            BindFolderList();
        }

        protected void lbn_sortList_ZA_Click(object sender, CommandEventArgs e)
        {
            Session["ListSort"] = e.CommandArgument.ToString();
            udp_ListRepeater.Update();
        }

        protected void lbn_sortList_AZ_Click(object sender, CommandEventArgs e)
        {
            Session["ListSort"] = e.CommandArgument.ToString();
            udp_ListRepeater.Update();
        }

        protected void lbn_sortList_New_Click(object sender, CommandEventArgs e)
        {
            Session["ListSort"] = e.CommandArgument.ToString();
            udp_ListRepeater.Update();
        }

        protected void lbn_sortList_Old_Click(object sender, CommandEventArgs e)
        {
            Session["ListSort"] = e.CommandArgument.ToString();
            udp_ListRepeater.Update();
        }

        protected void btn_addfolder_Click(object sender, EventArgs e)
        {

            if (Page.IsValid)
            {

                // Insert Folder to DB
                SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                SqlCommand cmd = new SqlCommand("INSERT INTO tbl_FoldersMaster (UserId, FolderName)VALUES(@UserId, @FolderName)", sqlConn);
                cmd.Parameters.Add("@UserId", SqlDbType.VarChar, 255).Value = FetchUser.UserID();
                cmd.Parameters.Add("@FolderName", SqlDbType.VarChar, 255).Value = this.tb_addfolder.Text;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                cmd.Connection.Dispose();

                // Clear Textbox
                this.tb_addfolder.Text = "";

                // Call Update Panel to avoid duplicates
                BindFolderList();
            }
        }


        protected void btn_addlist_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                // Insert List to DB and Return New ID
                SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                SqlCommand cmd = new SqlCommand("INSERT INTO tbl_ListsMaster (UserId, ListName, ListDescription)VALUES(@UserId, @ListName, @ListDescription);SELECT @@IDENTITY", sqlConn);
                cmd.Parameters.Add("@UserId", SqlDbType.VarChar, 255).Value = FetchUser.UserID();
                cmd.Parameters.Add("@ListName", SqlDbType.VarChar, 255).Value = this.tb_addlist.Text;
                cmd.Parameters.Add("@ListDescription", SqlDbType.VarChar, 255).Value = this.tb_list_descrip.Text;
                cmd.Connection.Open();
                Int32 ListId = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Connection.Close();
                cmd.Connection.Dispose();

                // Check for null folderid ie HOME folder
                string sFolderId = Session["FolderId"].ToString();
                if (sFolderId == "0" || sFolderId == null || sFolderId.Length == 0)
                {
                    // Clear Fields
                    this.tb_addlist.Text = "";
                    this.tb_list_descrip.Text = "";
                }

                // Has FolderId so Adding List to Folder
                SqlConnection sqlConn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                SqlCommand add = new SqlCommand("INSERT INTO tbl_FoldersMembers (FolderId, ItemId)VALUES(@FolderId, @ItemId)", sqlConn2);
                add.Parameters.Add("@FolderId", SqlDbType.VarChar, 255).Value = sFolderId;
                add.Parameters.Add("@ItemId", SqlDbType.VarChar, 255).Value = ListId;
                add.Connection.Open();
                add.ExecuteNonQuery();
                add.Connection.Close();
                add.Connection.Dispose();

                // Clear Fields
                this.tb_addlist.Text = "";
                this.tb_list_descrip.Text = "";

               
            }
        }


//// END LINK BUTTONS ////

//// BEGIN LABEL EVENTS ////

        protected void lbl_activefolder_PreRender(object sender, EventArgs e)
        {

            if (Session["FolderId"].ToString() == "0")
            {
                this.lbl_activefolder.Text = "HOME";
            }

            else
            {
                // Select Lists from DB
                SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                SqlCommand cmd = new SqlCommand("SELECT FolderName, FolderId FROM tbl_FoldersMaster WHERE FolderId ='" + Session["FolderId"].ToString() + "'", sqlConn);
                cmd.Connection.Open();

                SqlDataReader RepValues;
                RepValues = cmd.ExecuteReader();

                while (RepValues.Read())
                {
                    string FolderTitle = RepValues.GetString(0).ToUpper();
                    this.lbl_activefolder.Text = FolderTitle;
                }

                cmd.Connection.Close();
                cmd.Connection.Dispose();
            }
        }

//// END LABEL EVENTS ////

//// BEGIN FOLDER DATALIST EVENTS ////

        protected void Folder_Datalist_Delete(object source, DataListCommandEventArgs e)
        {
            // Delete Folder Association 
            string sFolderId = e.CommandArgument.ToString();
            int FolderId = Int32.Parse(sFolderId);
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmd = new SqlCommand("usp_folder_delete", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FolderId", SqlDbType.Int).Value = FolderId;
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            cmd.Connection.Dispose();

            BindFolderList();
            //Folder_Datalist.DataBind(); 
        }

        protected void Folder_Datalist_Cancel(object source, DataListCommandEventArgs e)
        {
            Folder_Datalist.EditItemIndex = -1;
            BindFolderList();
            //Folder_Datalist.DataBind();
        }


        protected void Folder_Datalist_Edit(object source, DataListCommandEventArgs e)
        {
            Folder_Datalist.EditItemIndex = e.Item.ItemIndex;
            BindFolderList();
        }


        protected void Folder_Datalist_Update(object source, DataListCommandEventArgs e)
        {

            TextBox tb_editfolder = (TextBox)e.Item.FindControl("tb_editfolder");

            // Update FolderName
            string sFolderName = tb_editfolder.Text;
            string sFolderId = e.CommandArgument.ToString();

            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmd = new SqlCommand("UPDATE tbl_FoldersMaster SET FolderName = @FolderName WHERE FolderId = '" + sFolderId + "'", sqlConn);

            cmd.Parameters.Add("@FolderName", SqlDbType.VarChar).Value = tb_editfolder.Text;

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            cmd.Connection.Dispose();

            Folder_Datalist.EditItemIndex = -1;

            BindFolderList();

        }

//// END FOLDER DATALIST EVENTS ////


//// BEGIN FOLDER DATALIST DATABIND ////

        protected void BindFolderList()
        {
            // Set Initial FolderSort Session
            if (Session["FolderSort"] == null)
            {
                Session["FolderSort"] = "0";
            }

            // Get Sort Order
            string sFolderSort = Session["FolderSort"].ToString();

            // Instantiate SQL String
            string FolderSelectSQL;

            // Select SQL Based on Session Variable               
            if (sFolderSort == "NEW")
            {
                FolderSelectSQL = "SELECT UPPER(FolderName) as FolderName, FolderId, FolderDate FROM tbl_FoldersMaster WHERE UserId ='" + FetchUser.UserID() + "' ORDER BY FolderDate DESC";
            }

            else if (sFolderSort == "OLD")
            {
                FolderSelectSQL = "SELECT UPPER(FolderName) as FolderName, FolderId, FolderDate FROM tbl_FoldersMaster WHERE UserId ='" + FetchUser.UserID() + "' ORDER BY FolderDate ASC";
            }

            else if (sFolderSort == "DESC")
            {
                FolderSelectSQL = "SELECT UPPER(FolderName) as FolderName, FolderId FROM tbl_FoldersMaster WHERE UserId ='" + FetchUser.UserID() + "' ORDER BY FolderName DESC";
            }

            else //ASC or Default
            {
                FolderSelectSQL = "SELECT UPPER(FolderName) as FolderName, FolderId FROM tbl_FoldersMaster WHERE UserId ='" + FetchUser.UserID() + "' ORDER BY FolderName ASC";
            }

            // Use SQL Statement to Select Records from DB    
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmd = new SqlCommand(FolderSelectSQL, sqlConn);
            cmd.Connection.Open();
            SqlDataReader RepValues;
            RepValues = cmd.ExecuteReader();
            this.Folder_Datalist.DataSource = RepValues;
            this.Folder_Datalist.DataBind();
            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }

//// END FOLDER DATALIST DATABIND ////
    }
}