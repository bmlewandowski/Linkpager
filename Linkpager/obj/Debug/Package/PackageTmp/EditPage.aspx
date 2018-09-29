<%@ Page Title="" Language="C#" MasterPageFile="~/Private.Master" AutoEventWireup="true" CodeBehind="EditPage.aspx.cs" Inherits="Linkpager.EditPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

<!--ADD PAGE SPECIFIC CSS-->
    <link href="Styles/editpage.css" rel="stylesheet" type="text/css" />

<!--LOAD JAVASCRIPT-->
    <script type="text/javascript" src="http://w.sharethis.com/button/buttons.js"></script><script type="text/javascript">                                                                                                    stLight.options({ publisher: '20fca839-abad-4612-86aa-9ccaeb1bb0e0' });</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">




<!--BEGIN UPDATE PANEL-->
    <asp:UpdatePanel ID="udp_LinkRepeater" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>

<!--BEGIN HEADER-->

	<div id="editpage_header">

		<div id="editpage_header_title">
			EDIT LINKPAGE
		</div>

		<div id="editpage_header_commands">
            
			<img src="Images/interface/addlink.png" class="editpage_headerbutton" id="editpage_addlink_button" onclick="toggle_generalpanel('editpage_addlink')" style="cursor:pointer;" alt="Add New Link" />			
            <asp:HyperLink ID="hyp_Preview" runat="server" Target="_blank" NavigateUrl="~/Viewpage.aspx" ImageUrl="~/Images/interface/preview.gif" style="cursor:pointer;" Title="Preview Public View"></asp:HyperLink>
			<img src="Images/interface/sort.gif" class="editpage_headerbutton" id="editpage_sort_button" onclick="toggle_generalpanel('editpage_sort')" style="cursor:pointer;" alt="Sort Links" />

		</div>

	</div>

<!--BEGIN HIDDEN PANELS-->

		<div class="editpage_hiddenpanel" id="editpage_sharelinkpage">SHARE</div>

		<div class="editpage_hiddenpanel" id="editpage_addlink">
        
            <asp:Image ID="img_add_newlink" runat="server" ImageUrl="~/Images/Interface/logo_link.png" ToolTip="Default Image" CssClass="button_right" />    
            <strong>Add Link</strong>
            <br /><br />
            URL: <asp:TextBox ID="tb_LinkUrl" runat="server" Width="350px" ValidationGroup="AddLink" />
            <asp:Button ID="btn_fetchDetails" runat="server" Text="Fetch Title" onclick="btn_fetchDetails_Click" BackColor="#F8D63D" BorderStyle="None" Height="25px" Width="100px" ValidationGroup="AddLink" ToolTip="Retrieve the current title from the selected website"  />
            <asp:RequiredFieldValidator ID="val_linkurl" runat="server" ErrorMessage="* Required" ControlToValidate="tb_LinkUrl" ValidationGroup="AddLink"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="val_linkurlreg" runat="server" ErrorMessage="enter valid URL" ControlToValidate="tb_LinkUrl" ValidationExpression="(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?" ValidationGroup="AddLink" Enabled="false"></asp:RegularExpressionValidator>
            <br />
            Title: <asp:TextBox ID="tb_LinkTitle" runat="server" Width="350px" ValidationGroup="AddLink" />
            <br /><br />
            Notes:
            <br />
            <asp:TextBox ID="tb_Description" runat="server" Height="75px" Width="400px" TextMode="MultiLine" />
  
  <br /><br />
  <!--BEGIN IMAGE SELECT-->
  Upload your own image: 
<asp:FileUpload ID="upl_linkimage" runat="server" />
<asp:RegularExpressionValidator ID="val_validimage" ControlToValidate="upl_linkimage" ValidationExpression="^.*\.((j|J)(p|P)(e|E)?(g|G)|(g|G)(i|I)(f|F)|(p|P)(n|N)(g|G))$" Text="Invalid image type" runat="server" ValidationGroup="UpLinkImage" />
<br />
<asp:Button ID="btn_uplinkimage" runat="server" Text="Upload Image" onclick="btn_uplinkimage_Click" BackColor="#F8D63D" BorderStyle="None" Height="25px" Width="100px" ToolTip="Upload Selected Image" ValidationGroup="UpLinkImage" />

<!--END IMAGE SELECT-->

            <br /><br /><br />
            <img src="Images/Interface/link_lock.png" alt="Make Private" style="vertical-align:middle;"/>
            <asp:CheckBox ID="cb_isprivate" runat="server" oncheckedchanged="cb_isprivate_CheckedChanged" />&nbsp;Make Private
           
            
            <br /><br /><br />
            <asp:Button ID="btn_addlink" runat="server" Text="Add Link" onclick="btn_addlink_Click" BackColor="#F8D63D" BorderStyle="None" Height="30px" Width="150px" ValidationGroup="AddLink" ToolTip="Add this link and details to the current Linkpage" />

            

          
        </div>

		<div class="editpage_hiddenpanel" id="editpage_sort">
        
            <strong>Sort Links</strong>
            <br /><br />
            <asp:LinkButton ID="lbn_sortLinkAZ" Runat="server" OnCommand="lbn_sortLink_AZ_Click" CommandArgument="ASC" ToolTip="Sort Links Alphabetically from A to Z">A to Z</asp:LinkButton><br />
		    <asp:LinkButton ID="lbn_sortLinkZA" Runat="server" OnCommand="lbn_sortLink_ZA_Click" CommandArgument="DESC" ToolTip="Sort Links Alphabetically from Z to A">Z to A</asp:LinkButton><br />
            <asp:LinkButton ID="lbn_sortLink_New" Runat="server" OnCommand="lbn_sortLink_New_Click" CommandArgument="NEW" ToolTip="Sort Links by Date from Newest to Oldest">Newest</asp:LinkButton><br />
            <asp:LinkButton ID="lbn_sortLink_Old" Runat="server" OnCommand="lbn_sortLink_Old_Click" CommandArgument="OLD" ToolTip="Sort Links by Date from Oldest to Newest">Oldest</asp:LinkButton><br />
            <asp:LinkButton ID="lbn_sortLink_Private" Runat="server" OnCommand="lbn_sortLink_Private_Click" CommandArgument="PRIVATE" ToolTip="Sort Links by Private/Public Status">Private</asp:LinkButton>
        </div> 

<!--END HIDDEN PANELS-->

<!--END HEADER-->

<!-- BEGIN LIST UPDATE PROGRESS -->

    <asp:UpdateProgress ID="UpdateProgress_Links" runat="server">
        <ProgressTemplate>
            <span style="color:Green;">UPDATE IN PROGRESS...</span>
        </ProgressTemplate>
    </asp:UpdateProgress>

<!-- END LIST UPDATE PROGRESS -->

<!--BEGIN MAIN AREA-->

	<div id="editpage_main">

		<div id="editpage_leftcol">

            <div id="editpage_linkpage">

		        

<!--BEGIN HEADER DATALIST-->
            <asp:DataList runat="server" id="Head_Datalist" OnEditCommand="Head_Datalist_Edit" OnUpdateCommand="Head_Datalist_Update" OnCancelCommand="Head_Datalist_Cancel" OnItemDataBound="Head_Datalist_ItemDataBound" OnPreRender="Head_Datalist_PreRender" >
                <ItemTemplate>
                    <div id="viewpage_header">			        
                        <div id="viewpage_header_area">
				            <div id="viewpage_header_icon">
                                <asp:Image ID="img_list_display" runat="server" ImageUrl="~/Images/Interface/logo_star.png" />
                            </div>
				                <div id="viewpage_header_text">
					                <div class="viewpage_linkpage_title"><asp:Label ID="lbl_ListName" runat="server" /></div>
					                <div class="viewpage_linkpage_desc">
						                <asp:Label ID="lbl_ListDescription" runat="server" />                        
					                </div>
				                </div>
			                </div>

                        <div id="viewpage_header_commands">
				            <img src="Images/interface/more.gif" id="viewpage_header_morebutton" onclick="toggle_more('viewpage_header_more', this.id)" style="cursor:pointer;" alt="More Option" />
			            </div>
                    
                    <!--BEGIN HIDDEN PANELS-->
		
			            <div class="viewpage_hiddenpanel" id="viewpage_header_more">
                            <ul>
				            <li><a href="#" title="Coming Soon...">RATING</a></li>
                            <li><a href="#" title="Coming Soon...">COMMENTS</a></li>
                            <li><a href="#" title="Coming Soon...">FOLLOWING</a></li>
                            </ul>
			            </div>
		
                    <!--END HIDDEN PANES-->  
                    </div>         
                </ItemTemplate>

                <EditItemTemplate>
                    <div id="viewpage_header_edit">	
                        <div id="viewpage_header_area">
				            <div id="viewpage_header_icon">
                                <asp:Image ID="img_list_edit" runat="server" ImageUrl="~/Images/Interface/logo_star.png" />
                            </div>
				                <div id="viewpage_header_text">
                                    <strong>Folder:</strong>
                                    <asp:DropDownList ID="ddl_folderMove" runat="server" ValidationGroup="EditList" />  
					                <br /><br />
                                    <div class="viewpage_linkpage_title">
                                        <asp:TextBox ID="tb_EditListTitle" runat="server" Text='<%#Eval("ListName")%>' Width="100%"></asp:TextBox>
                                    </div>

                                    Upload Image:
                                        <asp:FileUpload ID="upl_listimage" runat="server" />
                                        <asp:RegularExpressionValidator ID="val_validimage" ControlToValidate="upl_listimage" ValidationExpression="^.*\.((j|J)(p|P)(e|E)?(g|G)|(g|G)(i|I)(f|F)|(p|P)(n|N)(g|G))$" Text="Invalid image type" runat="server" ValidationGroup="EditList" />

					                <div class="viewpage_linkpage_desc">
						                <asp:TextBox ID="tb_EditListDescription" runat="server" Height="115px" Width="100%" TextMode="MultiLine" Text='<%#Eval("ListDescription")%>' />                          
					                </div>
				                </div>
			                </div>

                        <div id="viewpage_header_commands">
                       
                            <asp:LinkButton ID="lbn_SubmitEditList" runat="server" CommandName="Update" CommandArgument='<%#Eval("ListId")%>' ToolTip="Submit Changes" ValidationGroup="EditList"><strong>YES</strong></asp:LinkButton>
                            &nbsp;|&nbsp;
                            <asp:LinkButton ID="lbn_CancelEditList" runat="server" CommandName="Cancel" ToolTip="Cancel Changes"><strong>NO</strong></asp:LinkButton>
 				            
			            </div>

       
                    </div>
                
                </EditItemTemplate>

            </asp:DataList>

		<div class="viewpage_hr"></div>

<!--END LINKPAGE HEADER-->


<!--BEGIN LINK DATALIST-->
            <asp:DataList runat="server" id="Link_Datalist" OnEditCommand="Link_Datalist_Edit" OnUpdateCommand="Link_Datalist_Update" OnDeleteCommand="Link_Datalist_Delete" OnCancelCommand="Link_Datalist_Cancel" OnItemDataBound="Link_Datalist_ItemDataBound" >
                
                <ItemTemplate>

                <div class="viewpage_link">
                    <div class="viewpage_link_area">
                        <div class="viewpage_link_icon">
                            <a href="<%#Eval("LinkUrl")%>" title="<%#Eval("LinkName")%>" target="_blank"> 
                            <asp:Image ID="img_link_display" runat="server" ImageUrl="~/Images/Interface/logo_link.png" />
                            </a>
                        </div>
 				        
                        <div class="viewpage_link_text">
                            <div class="viewpage_link_title">                         
                                <a href="<%#Eval("LinkUrl")%>" title="<%#Eval("LinkName")%>: &#13&#13 <%#Eval("LinkDescription")%>" target="_blank"> 
                                <asp:label id="lbl_LinkName" runat="server" />
                                </a>
                        </div>    

                        <div class="viewpage_link_desc">
                            <asp:label id="lbl_LinkDescription" runat="server" />
                        </div>
                    </div>
                </div>
                
                <div class="viewpage_link_commands"> 
                    
                   
                    <asp:Image ID="img_isPrivate" runat="server" ImageUrl="~/Images/Interface/link_lock.png" Visible="false" ToolTip="Private" />

                    <asp:LinkButton ID="lbn_editlink" runat="server" CommandName="Edit" CommandArgument='<%#Eval("LinkId")%>' Visible="false">EDIT</asp:LinkButton>                       
                    
                    <img src="/Images/interface/more.gif" id="viewpage_link_morebutton<%# Container.ItemIndex + 1 %>" onclick="toggle_more('viewpage_link_more<%# Container.ItemIndex + 1 %>', this.id)" style="cursor:pointer;" alt="MORE" />
                </div> 
                
<!--BEGIN HIDDEN PANELS-->

			            <div class="viewpage_hiddenpanel" id="viewpage_link_more<%# Container.ItemIndex + 1 %>">
                            <ul>
				            <li><a href="#" title="Coming Soon...">RATING</a></li>
                            <li><a href="#" title="Coming Soon...">COMMENTS</a></li>
                            <li><a href="#" title="Coming Soon...">FOLLOWING</a></li>
                            </ul>
			            </div>

<!--END HIDDEN PANES-->

                </div>  

            <div class="viewpage_hr"></div>
                                                                                             
                </ItemTemplate>


                <EditItemTemplate>
                

                <div class="viewpage_link_edit">
                    <div class="viewpage_link_area">
                        <div class="viewpage_link_icon">
                            <a href="<%#Eval("LinkUrl")%>" title="<%#Eval("LinkName")%>" target="_blank"> 
                            <asp:Image ID="img_link_edit" runat="server" ImageUrl="~/Images/Interface/logo_link.png" />
                            </a>
                        </div>
 				        
                        <div class="viewpage_link_text">
                            <div class="viewpage_link_title">                             
                            <asp:TextBox ID="tb_EditLinkTitle" runat="server" Text='<%#Eval("LinkName")%>' Width="100%"></asp:TextBox>                              
                            </div>
                            Upload Image:    
                            <asp:FileUpload ID="upl_linkimage" runat="server" />
                            <asp:RegularExpressionValidator ID="val_validimage" ControlToValidate="upl_linkimage" ValidationExpression="^.*\.((j|J)(p|P)(e|E)?(g|G)|(g|G)(i|I)(f|F)|(p|P)(n|N)(g|G))$" Text="Invalid image type" runat="server" ValidationGroup="EditLink" />

                        <div class="viewpage_link_desc">
                            <asp:TextBox ID="tb_EditLinkDescription" runat="server" Height="115px" Width="100%" TextMode="MultiLine" Text='<%#Eval("LinkDescription")%>' />
                        </div>
                        
                       <img src="Images/Interface/link_lock.png" alt="Make Private" style="vertical-align:middle;"/>
                      <asp:CheckBox ID="cb_editprivate" runat="server" oncheckedchanged="cb_editprivate_CheckedChanged" Checked='<%#Eval("LinkPrivate")%>'/>&nbsp;Make Private
                        </div>
                       
                     
                </div>
                
                <div class="viewpage_link_commands">                        
                   <asp:LinkButton ID="lbn_SubmitEditLink" runat="server" CommandName="Update" CommandArgument='<%#Eval("LinkId")%>' ToolTip="Submit Changes" ValidationGroup="EditLink"><strong>YES</strong></asp:LinkButton>
                   &nbsp;|&nbsp;
                   <asp:LinkButton ID="lbn_CancelEditLink" runat="server" CommandName="Cancel" ToolTip="Cancel Changes"><strong>NO</strong></asp:LinkButton>
                </div> 
                
<!--BEGIN HIDDEN PANELS-->



<!--END HIDDEN PANES-->

                </div>  

            <div class="viewpage_hr"></div>


                </EditItemTemplate>


            </asp:DataList>

<!--END LINKPAGE DATALIST-->	

            </div>

		</div>

<!--BEGIN LINKPAGE COMMANDS-->

        <div id="editpage_rightcol">

			<div id="editpage_headercommands">
	
				<p><asp:LinkButton ID="lbn_edit_linkpage" runat="server" onclick="lbn_edit_linkpage_Click" CommandArgument='0' ToolTip="Edit Linkpage"><strong>EDIT LINKPAGE</strong></asp:LinkButton></p>
				<p><asp:LinkButton ID="lbn_delete_linkpage" runat="server" onclick="lbn_delete_linkpage_Click" OnClientClick="return confirm('Are you sure that you want to DELETE this LIST and all associated LINKS?');" ToolTip="Delete Linkpage"><strong>DELETE LINKPAGE</strong></asp:LinkButton></p>

			</div>

<!--BEGIN COMMAND REPEATER-->

            <asp:Repeater ID="Commands_Repeater" runat="server" OnItemCommand="Commands_Repeater_OnItemCommand">
                <ItemTemplate>

			        <div id="editpage_linkcommands<%#(((RepeaterItem)Container).ItemIndex+1).ToString() %>" class="editpage_linkcommands">	
				    <p><asp:LinkButton ID="btn_editlink" runat="server" CommandName="EditLink" CommandArgument='<%#Eval("LinkId")%>' ToolTip="Edit Link"><strong>EDIT LINK</strong></asp:LinkButton></p>
                    <p><asp:LinkButton ID="btn_deletelink" runat="server" OnClientClick="return confirm('Are you sure that you want to DELETE this Link?');" CommandName="DeleteLink" CommandArgument='<%#Eval("LinkId")%>' ToolTip="Delete Link"><strong>DELETE LINK</strong></asp:LinkButton></p>
			    
                </div>

            </ItemTemplate>
        </asp:Repeater>

<!--END COMMAND REPEATER-->
		
        </div>

<!--END LINKPAGE COMMANDS-->

		<div style="clear:both;"></div>
        
    </div>

        </ContentTemplate>

        <Triggers>
        
            <asp:PostBackTrigger ControlID="btn_uplinkimage" />

            

        </Triggers>

    </asp:UpdatePanel>

<!--END UPDATE PANEL-->

<!--BEGIN HIDDEN LABELS-->

<asp:Label ID="lb_UserId" runat="server" Visible="false"></asp:Label> <asp:Label ID="lb_AuthorId" runat="server" Visible="false"></asp:Label>

<!--END HIDDEN LABELS-->

<!--BEGIN ASYNC POST EVENT-->

    <script type="text/javascript">

        Sys.Application.add_init(appl_init);

        function appl_init() {
            var pgRegMgr = Sys.WebForms.PageRequestManager.getInstance();
            pgRegMgr.add_endRequest(EndHandler);
        }

        function EndHandler() {

            var value = document.getElementById('<%=tb_LinkUrl.ClientID%>').value;
            if (value.length > 3) {
                toggle_generalpanel('editpage_addlink');
            }
        }    

    </script>

<!--END ASYNC POST EVENT-->

</asp:Content>
