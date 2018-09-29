using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Data.Common;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI.HtmlControls;

namespace Linkpager
{
    public partial class EditPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
            // First Load Events
            if (!IsPostBack)
            {
                // Make sure current user is Author
                CheckAuthor();
                BindLinkHead();
                BindLinkList();
            }

            // If not logged in redirect to public
            if (User.Identity.IsAuthenticated == false)
            {
                Server.Transfer("~/Default.aspx");
            }

            // Set Preview Hyperlink
            this.hyp_Preview.NavigateUrl = "Viewpage.aspx?ListId=" + grabListId();

            // Run Javascript for Popped Window
            HtmlGenericControl body = (HtmlGenericControl)
            Page.Master.FindControl("PrivateBody");
            body.Attributes.Add("onload", "EndHandler();");
        }


//// BEGIN GENERAL ////

        // Return ListId from QueryString
        public string grabListId()
        {
            string qString = Request.QueryString["ListId"];
            return qString;
        }

        // Fetch the HTML Title from remote page
        public void GetTitle()
        {
            // Check Prefix
            System.Globalization.CompareInfo cmpUrl = System.Globalization.CultureInfo.InvariantCulture.CompareInfo;
            if (cmpUrl.IsPrefix(this.tb_LinkUrl.Text, "http") == false)
            {
                this.tb_LinkUrl.Text = "http://" + this.tb_LinkUrl.Text;
            }
        }


        // Fetch Screenshot        
        public void GetImage()
        {

            PS.OnlineImageOptimizer.ImageOptimizer op = new PS.OnlineImageOptimizer.ImageOptimizer();
            op.ImgQuality = 80;
            op.MaxHeight = 150;
            op.MaxWidth = 150;

            Bitmap bmp = FetchImage.GetWebSiteThumbnail(tb_LinkUrl.Text, 1024, 1024, 150, 150);

            // Save Image as JPG
            bmp.Save(Server.MapPath("~/Images/Users/" + FetchUser.UserName() + "/temp.jpg"), ImageFormat.Jpeg);
            // Optimize Image
            op.Optimize(Server.MapPath("~/Images/Users/" + FetchUser.UserName() + "/temp.jpg"));
            // Set Image Control
            img_add_newlink.ImageUrl = "~/Images/Users/" + FetchUser.UserName() + "/temp.jpg";

        }

        // Make Sure Viewer is Author
        public void CheckAuthor()
        {
            SqlConnection sqlConnAuth = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmdauth = new SqlCommand("SELECT tbl_ListsMaster.UserId FROM tbl_ListsMaster WHERE tbl_ListsMaster.ListId ='" + grabListId() + "'", sqlConnAuth);
            cmdauth.Connection.Open();

            SqlDataReader CheckAuthor;
            CheckAuthor = cmdauth.ExecuteReader();

            while (CheckAuthor.Read())
            {
                this.lb_AuthorId.Text = CheckAuthor["UserId"].ToString();
            }

            cmdauth.Connection.Close();
            cmdauth.Connection.Dispose();

            if (User.Identity.IsAuthenticated == true)
            {
                // Get UserID
                this.lb_UserId.Text = FetchUser.UserID();
            }

            else
            {
                this.lb_UserId.Text = "Guest";
            }


            if (this.lb_AuthorId.Text == this.lb_UserId.Text)
            {
                // User is Author
            }

            else
            {
                // User is NOT Author
                Server.Transfer("/Default.aspx");
            }
        }


        private void BindFolders(DropDownList ddl_folderMove)
        {
            string FolderSelectSQL = "SELECT UPPER(FolderName) as FolderName, FolderId FROM tbl_FoldersMaster WHERE UserId ='" + FetchUser.UserID() + "' ORDER BY FolderName ASC";
            // Use SQL Statement to Select Records from DB    

            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmd = new SqlCommand(FolderSelectSQL, sqlConn);
            cmd.Connection.Open();
            SqlDataReader RepValues;
            RepValues = cmd.ExecuteReader();

            ddl_folderMove.DataSource = RepValues;
            ddl_folderMove.DataTextField = "FolderName";
            ddl_folderMove.DataValueField = "FolderId";
            ddl_folderMove.DataBind();
            cmd.Connection.Close();
            ddl_folderMove.Items.Insert(0, "<- Select Destination Folder ->");
            cmd.Connection.Dispose();

        }

        // Resize Uploaded Image
        private Bitmap ResizeImage(Stream streamImage, int maxWidth, int maxHeight)
        {
            Bitmap originalImage = new Bitmap(streamImage);
            int newWidth = originalImage.Width;
            int newHeight = originalImage.Height;
            double aspectRatio = (double)originalImage.Width / (double)originalImage.Height;

            if (aspectRatio <= 1 && originalImage.Width > maxWidth)
            {
                newWidth = maxWidth;
                newHeight = (int)Math.Round(newWidth / aspectRatio);
            }
            else if (aspectRatio > 1 && originalImage.Height > maxHeight)
            {
                newHeight = maxHeight;
                newWidth = (int)Math.Round(newHeight * aspectRatio);
            }

            return new Bitmap(originalImage, newWidth, newHeight);
        }



//// END GENERAL /////


//// BEGIN COMMAND REPEATER EVENTS /////

        protected void Commands_Repeater_OnItemCommand(object source, RepeaterCommandEventArgs e)
        {

            if (e.CommandName == "DeleteLink")
            {
                // Delete Folder Association 
                string sLinkId = e.CommandArgument.ToString();
                int LinkId = Int32.Parse(sLinkId);
                SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                SqlCommand cmd = new SqlCommand("usp_link_delete", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@LinkId", SqlDbType.Int).Value = LinkId;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                cmd.Connection.Dispose();
            }
            else if (e.CommandName == "EditLink")
            {
               
                if (Link_Datalist.EditItemIndex == e.Item.ItemIndex)
                {
                    Link_Datalist.EditItemIndex = -1;
                }

                else
                {
                    Link_Datalist.EditItemIndex = e.Item.ItemIndex;
                }
                               
            }

                // Call Update Panel to avoid duplicates
                BindLinkList();       
        }

//// END COMMAND REPEATER EVENTS /////


//// BEGIN LINK BUTTONS ////

        protected void lbn_delete_linkpage_Click(object sender, EventArgs e)
        {
            // Delete List Association 
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmd = new SqlCommand("usp_list_delete", sqlConn);
            string sListId = Request.QueryString["ListId"].ToString();
            int ListId = Int32.Parse(sListId);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ListId", SqlDbType.Int).Value = ListId;
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            cmd.Connection.Dispose();

            BindLinkList();

        }

        protected void lbn_edit_linkpage_Click(object sender, EventArgs e)
        {
            if (Head_Datalist.EditItemIndex == 0)
            {
            Head_Datalist.EditItemIndex = -1;
            }

            else
            {
                Head_Datalist.EditItemIndex = 0;
            }
            BindLinkHead();
        }

        protected void lbn_sortLink_ZA_Click(object sender, CommandEventArgs e)
        {
            Session["LinkSort"] = e.CommandArgument.ToString();
            BindLinkList();
        }

        protected void lbn_sortLink_AZ_Click(object sender, CommandEventArgs e)
        {
            Session["LinkSort"] = e.CommandArgument.ToString();
            BindLinkList();
        }

        protected void lbn_sortLink_New_Click(object sender, CommandEventArgs e)
        {
            Session["LinkSort"] = e.CommandArgument.ToString();
            BindLinkList();
        }

        protected void lbn_sortLink_Old_Click(object sender, CommandEventArgs e)
        {
            Session["LinkSort"] = e.CommandArgument.ToString();
            BindLinkList();
        }

        protected void lbn_sortLink_Private_Click(object sender, CommandEventArgs e)
        {
            Session["LinkSort"] = e.CommandArgument.ToString();
            BindLinkList();
        }

//// END LINK BUTTONS ////

//// BEGIN BUTTONS /////

        protected void btn_addlink_Click(object sender, EventArgs e)
        {

            if (Page.IsValid)
            {

                // Insert Favorite to DB
                SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                SqlCommand cmd = new SqlCommand("INSERT INTO tbl_LinksMaster (UserId, LinkUrl, LinkName, LinkDescription, LinkPrivate)VALUES(@UserId, @LinkURL, @LinkTitle, @LinkDescription, @LinkPrivate);SELECT @@IDENTITY", sqlConn);

                cmd.Parameters.Add("@UserId", SqlDbType.VarChar, 255).Value = FetchUser.UserID();
                cmd.Parameters.Add("@LinkURL", SqlDbType.VarChar, 255).Value = this.tb_LinkUrl.Text;
                cmd.Parameters.Add("@LinkTitle", SqlDbType.VarChar, 255).Value = this.tb_LinkTitle.Text;
                cmd.Parameters.Add("@LinkDescription", SqlDbType.VarChar).Value = this.tb_Description.Text;
                cmd.Parameters.Add("@LinkPrivate", SqlDbType.Bit, 1).Value = this.cb_isprivate.Checked;

                cmd.Connection.Open();

                string LinkIdReturn = cmd.ExecuteScalar().ToString();

                cmd.Connection.Close();
                cmd.Connection.Dispose();

                // Add Link to List
                SqlConnection sqlConn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                SqlCommand add = new SqlCommand("INSERT INTO tbl_ListsMembers (LinkId, ListId)VALUES('" + LinkIdReturn + "','" + grabListId() + "')", sqlConn2);
                add.Connection.Open();
                add.ExecuteNonQuery();
                add.Connection.Close();
                add.Connection.Dispose();

                //Redirect to self to avoid duplicates
                BindLinkList();

                this.tb_LinkUrl.Text = "";
                this.tb_LinkTitle.Text = "";
                this.tb_Description.Text = "";

            }
        }


        protected void btn_fetchDetails_Click(object sender, EventArgs e)
        {
            GetImage();
            GetTitle();

            if (Page.IsValid)
            {
                // Fetched the title... 
                string sFullHtml = FetchTitle.FetchHTML(this.tb_LinkUrl.Text);
                string sTitle = FetchTitle.FetchTitleFromHTML(sFullHtml);
                this.tb_LinkTitle.Text = sTitle;
            }
            
        }

        protected void btn_uplinkimage_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {

                PS.OnlineImageOptimizer.ImageOptimizer op = new PS.OnlineImageOptimizer.ImageOptimizer();
                op.ImgQuality = 80;
                op.MaxHeight = 150;
                op.MaxWidth = 200;
             
                Bitmap bmp = ResizeImage(upl_linkimage.PostedFile.InputStream, 200, 150);
                bmp.Save(Server.MapPath("~/Images/Users/" + FetchUser.UserName() + "/temp.jpg"), ImageFormat.Jpeg);
                op.Optimize(Server.MapPath("~/Images/Users/" + FetchUser.UserName() + "/temp.jpg"));
                img_add_newlink.ImageUrl = "~/Images/Users/" + FetchUser.UserName() + "/temp.jpg";
            }
        }


        protected void btn_uplistimage_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {

                FileUpload upl_listimage = (FileUpload)Head_Datalist.Items[Head_Datalist.EditItemIndex].FindControl("upl_listimage");
                System.Web.UI.WebControls.Image ListImage_Placeholder = (System.Web.UI.WebControls.Image)Head_Datalist.Items[Head_Datalist.EditItemIndex].FindControl("ListImage_Placeholder");

                if (upl_listimage.HasFile)
                {

                    PS.OnlineImageOptimizer.ImageOptimizer op = new PS.OnlineImageOptimizer.ImageOptimizer();
                    op.ImgQuality = 80;
                    op.MaxHeight = 200;
                    op.MaxWidth = 200;

                    Bitmap bmp = ResizeImage(upl_listimage.PostedFile.InputStream, 200, 200);
                    bmp.Save(Server.MapPath("~/Images/Users/" + FetchUser.UserName() + "/temp.jpg"), ImageFormat.Jpeg);
                    op.Optimize(Server.MapPath("~/Images/Users/" + FetchUser.UserName() + "/temp.jpg"));
                    ListImage_Placeholder.ImageUrl = "~/Images/Users/" + FetchUser.UserName() + "/temp.jpg";
                }
                else
                {
                   
                }

            }
        }

//// END BUTTONS /////


//// BEGIN CHECKBOXES //// 

        protected void cb_isprivate_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void cb_editprivate_CheckedChanged(object sender, EventArgs e)
        {

        }

//// END CHECKBOXES //// 


//// BEGIN LINK DATALIST EVENTS ////

        protected void Link_Datalist_Delete(object source, DataListCommandEventArgs e)
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

            BindLinkList();

        }

        protected void Link_Datalist_Cancel(object source, DataListCommandEventArgs e)
        {
            Link_Datalist.EditItemIndex = -1;
            BindLinkList();
        }


        protected void Link_Datalist_Edit(object source, DataListCommandEventArgs e)
        {
            Link_Datalist.EditItemIndex = e.Item.ItemIndex;
            BindLinkList();
        }


        protected void Link_Datalist_Update(object source, DataListCommandEventArgs e)
        {
            TextBox tb_EditLinkTitle = (TextBox)e.Item.FindControl("tb_EditLinkTitle");
            TextBox tb_EditLinkDescription = (TextBox)e.Item.FindControl("tb_EditLinkDescription");
            CheckBox cb_editprivate = (CheckBox)e.Item.FindControl("cb_editprivate");
            FileUpload upl_linkimage = (FileUpload)e.Item.FindControl("upl_linkimage");

            string sLinkName = tb_EditLinkTitle.Text;
            string sLinkDescription = tb_EditLinkDescription.Text;
            string sLinkId = e.CommandArgument.ToString();

            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmd = new SqlCommand("UPDATE tbl_LinksMaster SET [LinkName] = @LinkName, [LinkDescription] = @LinkDescription, [LinkPrivate] = @LinkPrivate WHERE [LinkId] = @LinkId", sqlConn);
            cmd.Parameters.Add("@LinkName", SqlDbType.VarChar).Value = sLinkName;
            cmd.Parameters.Add("@LinkDescription", SqlDbType.VarChar).Value = sLinkDescription;
            cmd.Parameters.Add("@LinkId", SqlDbType.VarChar).Value = sLinkId;
            cmd.Parameters.Add("@LinkPrivate", SqlDbType.Bit, 1).Value = cb_editprivate.Checked;

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            cmd.Connection.Dispose();


            if (upl_linkimage.HasFile)
            {

                PS.OnlineImageOptimizer.ImageOptimizer op = new PS.OnlineImageOptimizer.ImageOptimizer();
                op.ImgQuality = 80;
                op.MaxHeight = 200;
                op.MaxWidth = 200;

                Bitmap bmp = ResizeImage(upl_linkimage.PostedFile.InputStream, 200, 200);
                bmp.Save(Server.MapPath("~/Images/Users/" + FetchUser.UserName() + "/temp.jpg"), ImageFormat.Jpeg);
                op.Optimize(Server.MapPath("~/Images/Users/" + FetchUser.UserName() + "/temp.jpg"));
                //img_link_edit.ImageUrl = "~/Images/Users/" + FetchUser.UserName() + "/temp.jpg";
            }
            else
            {

            }

            Link_Datalist.EditItemIndex = -1;

            BindLinkList();

        }

        protected void Link_Datalist_ItemDataBound(Object sender, DataListItemEventArgs e)
        {
            // Make sure you are not in edit mode
            if (e.Item.ItemType == ListItemType.EditItem)
            {
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(e.Item.FindControl("lbn_SubmitEditLink"));
            }

            else
            {
                // Trim text to set number of characters and add "..."
                DbDataRecord rec = (DbDataRecord)e.Item.DataItem;
                // Make sure that you have the data
                if (rec != null)
                {
                    // Mark private
                    if (rec["LinkPrivate"].ToString() == "True")
                    {
                        System.Web.UI.WebControls.Image img_isPrivate = (System.Web.UI.WebControls.Image)e.Item.FindControl("img_isPrivate");
                        img_isPrivate.Visible = true;
                    }

                    // Trim Name Labels
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
                    if (stringLengthDesc > 220)
                    {
                        lbldesc.Text = sLinkDesc.Substring(0, Math.Min(220, sLinkDesc.ToString().Length)) + "...";
                    }

                    else
                    {
                        // List Description is Under 220 Char
                        lbldesc.Text = sLinkDesc;
                    }
                }

            }
        }

//// END FOLDER DATALIST EVENTS ////


//// BEGIN FOLDER DATALIST DATABIND ////

        protected void BindLinkList()
        {
            // Get Sort Order

            // Set Initial ListSort Session
            if (Session["LinkSort"] == null)
            {
                Session["LinkSort"] = "0";
            }

            string sLinkSort = Session["LinkSort"].ToString();

            // Instantiate SQL String
            string LinkSelectSQL;

            // Select SQL Based on Session Variable               
            if (sLinkSort == "NEW")
            {
                LinkSelectSQL = "SELECT tbl_ListsMaster.ListId, tbl_ListsMaster.ListName, tbl_ListsMaster.ListDescription, tbl_ListsMembers.ListsMembersId, tbl_ListsMembers.ListId, tbl_ListsMembers.LinkId, tbl_ListsMembers.ListsMembersDate, tbl_LinksMaster.LinkId, tbl_LinksMaster.LinkUrl, tbl_LinksMaster.LinkName, tbl_LinksMaster.LinkDescription, tbl_LinksMaster.LinkDate, tbl_LinksMaster.LinkPrivate FROM tbl_ListsMaster, tbl_ListsMembers, tbl_LinksMaster WHERE tbl_ListsMaster.ListId ='" + grabListId() + "' AND tbl_ListsMembers.ListId ='" + grabListId() + "' AND tbl_ListsMembers.LinkId = tbl_LinksMaster.LinkId ORDER BY tbl_LinksMaster.LinkDate DESC";
            }

            else if (sLinkSort == "OLD")
            {
                LinkSelectSQL = "SELECT tbl_ListsMaster.ListId, tbl_ListsMaster.ListName, tbl_ListsMaster.ListDescription, tbl_ListsMembers.ListsMembersId, tbl_ListsMembers.ListId, tbl_ListsMembers.LinkId, tbl_ListsMembers.ListsMembersDate, tbl_LinksMaster.LinkId, tbl_LinksMaster.LinkUrl, tbl_LinksMaster.LinkName, tbl_LinksMaster.LinkDescription, tbl_LinksMaster.LinkDate, tbl_LinksMaster.LinkPrivate FROM tbl_ListsMaster, tbl_ListsMembers, tbl_LinksMaster WHERE tbl_ListsMaster.ListId ='" + grabListId() + "' AND tbl_ListsMembers.ListId ='" + grabListId() + "' AND tbl_ListsMembers.LinkId = tbl_LinksMaster.LinkId ORDER BY tbl_LinksMaster.LinkDate ASC";
            }

            else if (sLinkSort == "PRIVATE")
            {
                LinkSelectSQL = "SELECT tbl_ListsMaster.ListId, tbl_ListsMaster.ListName, tbl_ListsMaster.ListDescription, tbl_ListsMembers.ListsMembersId, tbl_ListsMembers.ListId, tbl_ListsMembers.LinkId, tbl_ListsMembers.ListsMembersDate, tbl_LinksMaster.LinkId, tbl_LinksMaster.LinkUrl, tbl_LinksMaster.LinkName, tbl_LinksMaster.LinkDescription, tbl_LinksMaster.LinkDate, tbl_LinksMaster.LinkPrivate FROM tbl_ListsMaster, tbl_ListsMembers, tbl_LinksMaster WHERE tbl_ListsMaster.ListId ='" + grabListId() + "' AND tbl_ListsMembers.ListId ='" + grabListId() + "' AND tbl_ListsMembers.LinkId = tbl_LinksMaster.LinkId ORDER BY tbl_LinksMaster.LinkPrivate DESC";
            }

            else if (sLinkSort == "DESC")
            {
                LinkSelectSQL = "SELECT tbl_ListsMaster.ListId, tbl_ListsMaster.ListName, tbl_ListsMaster.ListDescription, tbl_ListsMembers.ListsMembersId, tbl_ListsMembers.ListId, tbl_ListsMembers.LinkId, tbl_ListsMembers.ListsMembersDate, tbl_LinksMaster.LinkId, tbl_LinksMaster.LinkUrl, tbl_LinksMaster.LinkName, tbl_LinksMaster.LinkDescription, tbl_LinksMaster.LinkDate, tbl_LinksMaster.LinkPrivate FROM tbl_ListsMaster, tbl_ListsMembers, tbl_LinksMaster WHERE tbl_ListsMaster.ListId ='" + grabListId() + "' AND tbl_ListsMembers.ListId ='" + grabListId() + "' AND tbl_ListsMembers.LinkId = tbl_LinksMaster.LinkId ORDER BY tbl_LinksMaster.LinkName DESC";
            }

            else //ASC or Default
            {
                LinkSelectSQL = "SELECT tbl_ListsMaster.ListId, tbl_ListsMaster.ListName, tbl_ListsMaster.ListDescription, tbl_ListsMembers.ListsMembersId, tbl_ListsMembers.ListId, tbl_ListsMembers.LinkId, tbl_ListsMembers.ListsMembersDate, tbl_LinksMaster.LinkId, tbl_LinksMaster.LinkUrl, tbl_LinksMaster.LinkName, tbl_LinksMaster.LinkDescription, tbl_LinksMaster.LinkDate, tbl_LinksMaster.LinkPrivate FROM tbl_ListsMaster, tbl_ListsMembers, tbl_LinksMaster WHERE tbl_ListsMaster.ListId ='" + grabListId() + "' AND tbl_ListsMembers.ListId ='" + grabListId() + "' AND tbl_ListsMembers.LinkId = tbl_LinksMaster.LinkId ORDER BY tbl_LinksMaster.LinkName ASC";
            }

            // Use SQL Statement to Select Records from DB    
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmd = new SqlCommand(LinkSelectSQL, sqlConn);

            cmd.Connection.Open();
            SqlDataReader RepValues;
            RepValues = cmd.ExecuteReader();
            this.Link_Datalist.DataSource = RepValues;
            this.Link_Datalist.DataBind();
            cmd.Connection.Close();
            cmd.Connection.Open();
            SqlDataReader CmdValues;
            CmdValues = cmd.ExecuteReader();
            this.Commands_Repeater.DataSource = CmdValues;
            this.Commands_Repeater.DataBind();
            cmd.Connection.Close();
            cmd.Connection.Dispose();

        }

//// END FOLDER DATALIST DATABIND ////


//// BEGIN HEAD DATALIST EVENTS ////


        protected void Head_Datalist_Edit(object source, DataListCommandEventArgs e)
        {
            Head_Datalist.EditItemIndex = e.Item.ItemIndex;
            BindLinkHead();
        }

        protected void Head_Datalist_Cancel(object source, DataListCommandEventArgs e)
        {
            Head_Datalist.EditItemIndex = -1;
            BindLinkHead(); ;
        }

        protected void Head_Datalist_Update(object source, DataListCommandEventArgs e)
        {
            DropDownList ddl_folderMove = (DropDownList)e.Item.FindControl("ddl_folderMove");
            TextBox tb_EditListTitle = (TextBox)e.Item.FindControl("tb_EditListTitle");
            TextBox tb_EditListDescription = (TextBox)e.Item.FindControl("tb_EditListDescription");
            FileUpload upl_listimage = (FileUpload)e.Item.FindControl("upl_listimage");
            System.Web.UI.WebControls.Image ListImage_Placeholder = (System.Web.UI.WebControls.Image)e.Item.FindControl("ListImage_Placeholder");
            //CheckBox cb_editprivate = (CheckBox)e.Item.FindControl("cb_editprivate");

            string sListName = tb_EditListTitle.Text;
            string sListDescription = tb_EditListDescription.Text;
            string sListId = grabListId();

            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

            // Check Dropdown for Selection

            if (ddl_folderMove.SelectedIndex != 0)
            {
                // Delete and Create New Folder Association
                SqlCommand mov = new SqlCommand("usp_list_move", sqlConn);
                string sFolderId = ddl_folderMove.SelectedValue.ToString();
                int FolderId = Int32.Parse(sFolderId);
                int ListId = Int32.Parse(sListId);

                mov.CommandType = CommandType.StoredProcedure;
                mov.Parameters.Add("@FolderId", SqlDbType.Int).Value = FolderId;
                mov.Parameters.Add("@ListId", SqlDbType.Int).Value = ListId;
                mov.Connection.Open();
                mov.ExecuteNonQuery();
                mov.Connection.Close();
                mov.Connection.Dispose();

                SqlConnection sqlConn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);              
                
                // Update List Name and Description
                SqlCommand cmd = new SqlCommand("UPDATE tbl_ListsMaster SET [ListName] = @ListName, [ListDescription] = @ListDescription WHERE [ListId] = @ListId", sqlConn2);
                cmd.Parameters.Add("@ListName", SqlDbType.VarChar).Value = sListName;
                cmd.Parameters.Add("@ListDescription", SqlDbType.VarChar).Value = sListDescription;
                cmd.Parameters.Add("@ListId", SqlDbType.VarChar).Value = sListId;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                cmd.Connection.Dispose();

            }

            else
            {
                SqlCommand cmd = new SqlCommand("UPDATE tbl_ListsMaster SET [ListName] = @ListName, [ListDescription] = @ListDescription WHERE [ListId] = @ListId", sqlConn);
                cmd.Parameters.Add("@ListName", SqlDbType.VarChar).Value = sListName;
                cmd.Parameters.Add("@ListDescription", SqlDbType.VarChar).Value = sListDescription;
                cmd.Parameters.Add("@ListId", SqlDbType.VarChar).Value = sListId;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                cmd.Connection.Dispose();
            }


            if (upl_listimage.HasFile)
            {

                PS.OnlineImageOptimizer.ImageOptimizer op = new PS.OnlineImageOptimizer.ImageOptimizer();
                op.ImgQuality = 80;
                op.MaxHeight = 200;
                op.MaxWidth = 200;

                Bitmap bmp = ResizeImage(upl_listimage.PostedFile.InputStream, 200, 200);
                bmp.Save(Server.MapPath("~/Images/Users/" + FetchUser.UserName() + "/temp.jpg"), ImageFormat.Jpeg);
                op.Optimize(Server.MapPath("~/Images/Users/" + FetchUser.UserName() + "/temp.jpg"));
                //img_list_edit.ImageUrl = "~/Images/Users/" + FetchUser.UserName() + "/temp.jpg";
            }
            else
            {

            }

            Head_Datalist.EditItemIndex = -1;
            BindLinkHead();

        }

        protected void Head_Datalist_ItemDataBound(Object sender, DataListItemEventArgs e)
        {

            // Make sure you are not in edit mode
            if (e.Item.ItemType == ListItemType.EditItem)
            {

                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(e.Item.FindControl("lbn_SubmitEditList"));
            }

            else
            {
                // Trim text to set number of characters and add "..."
                DbDataRecord rec = (DbDataRecord)e.Item.DataItem;
                // Make sure that you have the data
                if (rec != null)
                {
                    Label lblname = (Label)e.Item.FindControl("lbl_ListName");
                    string sListName = rec["ListName"].ToString();
                    int stringLengthName = sListName.Length;

                    Label lbldesc = (Label)e.Item.FindControl("lbl_ListDescription");
                    string sListDesc = rec["ListDescription"].ToString();
                    int stringLengthDesc = sListDesc.Length;

                    // Check Name
                    if (stringLengthName > 37)
                    {
                        lblname.Text = sListName.Substring(0, Math.Min(37, sListName.ToString().Length)) + "...";
                    }

                    else
                    {
                        // List Name is Under 37 Char
                        lblname.Text = sListName;
                    }

                    // Check Description
                    if (stringLengthDesc > 220)
                    {
                        lbldesc.Text = sListDesc.Substring(0, Math.Min(220, sListDesc.ToString().Length)) + "...";
                    }

                    else
                    {
                        // List Description is Under 220 Char
                        lbldesc.Text = sListDesc;
                    }
                }

            }
        }

        protected void Head_Datalist_PreRender(Object sender, EventArgs e)
        {
            if (Head_Datalist.EditItemIndex != -1)
                {
                    // Bind Dropdown List
                    DropDownList ddl_folderMove = (DropDownList)Head_Datalist.Items[Head_Datalist.EditItemIndex].FindControl("ddl_folderMove");
                    BindFolders(ddl_folderMove);
                }
        }

        //// END HEAD DATALIST EVENTS ////


        //// BEGIN HEAD DATALIST DATABIND ////

        protected void BindLinkHead()
        {

            // Instantiate SQL String
            string LinkSelectSQL;

            // Select SQL            
            LinkSelectSQL = "SELECT tbl_ListsMaster.ListId, tbl_ListsMaster.ListName, tbl_ListsMaster.ListDescription, tbl_ListsMaster.ListDate, tbl_ListsMaster.UserId, aspnet_Users.UserId, aspnet_Users.UserName FROM tbl_ListsMaster, aspnet_Users WHERE aspnet_Users.UserId = tbl_ListsMaster.UserId AND  tbl_ListsMaster.ListId ='" + grabListId() + "'";

            // Use SQL Statement to Select Records from DB    
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmd = new SqlCommand(LinkSelectSQL, sqlConn);

            cmd.Connection.Open();
            SqlDataReader RepValues;
            RepValues = cmd.ExecuteReader();
            this.Head_Datalist.DataSource = RepValues;
            this.Head_Datalist.DataBind();
            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }

//// END HEAD DATALIST DATABIND ////


    }
}