<%@ Page Title="" Language="C#" MasterPageFile="~/Private.Master" AutoEventWireup="true" CodeBehind="Linkpages.aspx.cs" Inherits="Linkpager.Linkpages" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

<!--ADD PAGE SPECIFIC CSS-->
    <link href="Styles/linkpages.css" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<!--BEGIN UPDATE PANEL FOLDERS-->

    <asp:UpdatePanel ID="udp_FolderDataList" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

    <div id="linkpages_folderarea">

			<div id="linkpages_folders_header">

				<div id="linkpages_folders_title">FOLDERS</div>
				<div id="linkpages_folders_commands">
					<img src="Images/interface/addfolder.gif" class="linkpages_headerbutton" id="linkpages_addfolderbutton" onclick="toggle_generalpanel('linkpages_folders_addfolderpanel','linkpages_addfolderbutton')" style="cursor:pointer;" alt="Add Folder" />
					<img src="Images/interface/sort.gif" class="linkpages_headerbutton" id="linkpages_sortfolderbutton" onclick="toggle_generalpanel('linkpages_folders_sortpanel','linkpages_addfolderbutton')" style="cursor:pointer;" alt="Sort" />
				</div>
			
            </div>

			<div id="linkpages_folders_addfolderpanel">

<!--BEGIN ADD FOLDER-->

            New Folder Name:<br /> 
            <asp:TextBox ID="tb_addfolder" runat="server" width="200" ValidationGroup="AddFolder"></asp:TextBox>
            <br />
            <asp:RequiredFieldValidator ID="val_addfolder" runat="server" ErrorMessage="* Required" ControlToValidate="tb_addfolder" ForeColor="Red" ValidationGroup="AddFolder"></asp:RequiredFieldValidator>
            <br /><br />
            <asp:Button ID="btn_addfolder" Text="ADD" runat="server" OnClick="btn_addfolder_Click" ValidationGroup="AddFolder" BackColor="#F8D63D" BorderStyle="None" Height="20px" Width="75px" ToolTip="Click to Add New Folder" />
           
<!--END ADD FOLDER-->			
            
            </div>

			<div id="linkpages_folders_sortpanel">

            <strong>Sort Folders</strong>
            <br /><br />
				<asp:LinkButton ID="lbn_sortFolderAZ" Runat="server" OnCommand="lbn_sortFolder_AZ_Click" CommandArgument="ASC" ToolTip="Sort Folders Alphabetically from A to Z">A to Z</asp:LinkButton><br />
				<asp:LinkButton ID="lbn_sortFolderZA" Runat="server" OnCommand="lbn_sortFolder_ZA_Click" CommandArgument="DESC" ToolTip="Sort Folders Alphabetically from Z to A">Z to A</asp:LinkButton><br />
                <asp:LinkButton ID="lbn_sortFolder_New" Runat="server" OnCommand="lbn_sortFolder_New_Click" CommandArgument="NEW" ToolTip="Sort Folders by Date from Newest to Oldest">Newest</asp:LinkButton><br />
                <asp:LinkButton ID="lbn_sortFolder_Old" Runat="server" OnCommand="lbn_sortFolder_Old_Click" CommandArgument="OLD" ToolTip="Sort by Folders from Oldest to Newest">Oldest</asp:LinkButton>
               
			</div>

<!--BEGIN FOLDER LIST-->

<!-- BEGIN FOLDER UPDATE PROGRESS -->

    <asp:UpdateProgress ID="UpdateProgress_Folders" runat="server" AssociatedUpdatePanelID="udp_FolderDataList">
        <ProgressTemplate>
            <span style="color:Green;">UPDATE IN PROGRESS...</span>
        </ProgressTemplate>
    </asp:UpdateProgress>

<!-- END FOLDER UPDATE PROGRESS -->

<!-- BEGIN DATALIST -->
            <asp:DataList runat="server" id="Folder_Datalist" OnEditCommand="Folder_Datalist_Edit" OnUpdateCommand="Folder_Datalist_Update" OnDeleteCommand="Folder_Datalist_Delete" OnCancelCommand="Folder_Datalist_Cancel" >
                <HeaderTemplate>
                    <div class="linkpages_folder" id="linkpages_folder" onclick="" style="cursor:pointer;">
                        <div class="linkpages_folder_title">      
                        <asp:LinkButton ID="lbn_selectHome" Runat="server" OnCommand="lbn_selectHome_Click" CommandArgument="0" ToolTip="HOME">
                        HOME
                        </asp:LinkButton>
                        </div>
                    </div>
                </HeaderTemplate>

                <ItemTemplate>
 			        <div class="linkpages_folder" id="linkpages_folder_<%# Container.ItemIndex + 1 %>" onclick="" style="cursor:pointer;">
				        <div class="linkpages_folder_title">
                            <asp:LinkButton ID="lbn_selectList" Runat="server" OnCommand="lbn_selectList_Click" CommandArgument='<%#Eval("Folderid")%>' Visible="true" ToolTip='<%#Eval("FolderName")%>'>
                            <%#Eval("FolderName")%> 
                            </asp:LinkButton>

                        </div>
				        <div class="linkpages_folder_panelbutton"><img src="Images/Interface/folder_more.gif" id="linkpages_folder_panelbutton_<%# Container.ItemIndex + 1 %>" onclick="toggle_folderpanel('linkpages_folderpanel_<%# Container.ItemIndex + 1 %>',this.id)" style="cursor:pointer;" alt="" /></div>
			            </div>
			            <div class="linkpages_folderpanel" id="linkpages_folderpanel_<%# Container.ItemIndex + 1 %>">
				            <div class="linkpages_folder_commands">
                                <asp:LinkButton ID="lbn_editfolder" runat="server" CommandName="Edit" CommandArgument='<%#Eval("FolderId")%>' ToolTip="Rename Folder">RENAME</asp:LinkButton>
                            </div>
				            <div class="linkpages_folder_dragsort">
                                <asp:LinkButton ID="lbn_deletefolder" runat="server" OnClientClick="return confirm('Are you sure that you want to DELETE this FOLDER?');" CommandName="Delete" CommandArgument='<%#Eval("FolderId")%>' ToolTip="Delete Folder">DELETE</asp:LinkButton>
                            </div>
			            </div>
                </ItemTemplate>

                <EditItemTemplate>
<!--BEGIN FOLDER EDIT-->
                    <div class="linkpages_folder" id="linkpages_folder_<%# Container.ItemIndex + 1 %>" onclick="" style="cursor:pointer;">
                        <asp:TextBox ID="tb_editfolder" runat="server" Text='<%#Eval("FolderName")%>' />
                        <asp:LinkButton ID="lbn_SubmitEditFolder" runat="server" CommandName="Update" CommandArgument='<%#Eval("FolderId")%>'>YES</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="lbn_CancelEditFolder" runat="server" CommandName="Cancel">NO</asp:LinkButton>
                    </div>
<!--END FOLDER EDIT-->
                </EditItemTemplate>

            </asp:DataList>

<!-- END DATALIST -->

<!--END FOLDER LIST-->

    </div>

<!--END UPDATE PANEL FOLDERS-->
        </ContentTemplate>
    </asp:UpdatePanel>

<!--BEGIN UPDATE PANEL LINKPAGE-->

    <asp:UpdatePanel ID="udp_ListRepeater" runat="server" UpdateMode="Conditional" OnPreRender="udp_ListRepeater_PreRender">
        <ContentTemplate>

            <div id="linkpages_linkpagearea">

<div id="linkpages_linkpages_header">
				<div id="linkpages_linkpages_title"><asp:Label ID="lbl_activefolder" runat="server" Text="LINKPAGES" OnPreRender="lbl_activefolder_PreRender"></asp:Label></div>
				<div id="linkpages_linkpages_commands">
					<img src="Images/interface/addlinkpage.png" class="linkpages_headerbutton" id="linkpages_addlinkpagebutton" onclick="toggle_generalpanel('linkpages_linkpages_addlinkpagepanel','linkpages_addlinkpagebutton')" style="cursor:pointer;" alt="" />
					<img src="Images/interface/sort.gif" class="linkpages_headerbutton" id="linkpages_sortlinkpagebutton" onclick="toggle_generalpanel('linkpages_linkpages_sortpanel','linkpages_sortlinkpagebutton')" style="cursor:pointer;" alt="" />
				</div>
			</div>

			<div class="linkpages_linkpages_panel" id="linkpages_linkpages_addlinkpagepanel">

<!--BEGIN ADD LINKPAGE-->

                Name: <asp:TextBox ID="tb_addlist" runat="server" width="200" ValidationGroup="AddList"></asp:TextBox>
                <asp:RequiredFieldValidator ID="val_addlist" runat="server" ErrorMessage="* Required" ControlToValidate="tb_addlist" ForeColor="Red" ValidationGroup="AddList"></asp:RequiredFieldValidator>               
                <br /><br />
                <asp:Button ID="btn_addlist" Text="ADD" runat="server" OnClick="btn_addlist_Click" ValidationGroup="AddList" BackColor="#F8D63D" BorderStyle="None" Height="20px" Width="75px" ToolTip="Click to Add New Linkpage to Current Folder" CssClass="button_right" />
                Description:
                <br />
                <asp:TextBox ID="tb_list_descrip" runat="server" Height="40px" Width="400px" TextMode="MultiLine" />
               
<!--END ADD LINKPAGE-->

			</div>

			<div class="linkpages_linkpages_panel" id="linkpages_linkpages_sortpanel">

            <strong>Sort Linkpages</strong>
            <br /><br />
		    <asp:LinkButton ID="lbn_sortListAZ" Runat="server" OnCommand="lbn_sortList_AZ_Click" CommandArgument="ASC" ToolTip="Sort Linkpages from A to Z">A to Z</asp:LinkButton><br />
		    <asp:LinkButton ID="lbn_sortListZA" Runat="server" OnCommand="lbn_sortList_ZA_Click" CommandArgument="DESC" ToolTip="Sort Linkpages Alphabetically from Z to A">Z to A</asp:LinkButton><br />
            <asp:LinkButton ID="lbn_sortList_New" Runat="server" OnCommand="lbn_sortList_New_Click" CommandArgument="NEW" ToolTip="Sort Linkpages by Date from Newest to Oldest">Newest</asp:LinkButton><br />
            <asp:LinkButton ID="lbn_sortList_Old" Runat="server" OnCommand="lbn_sortList_Old_Click" CommandArgument="OLD" ToolTip="Sort Linkpages by Date from Oldest to Newest">Oldest</asp:LinkButton>
			
            </div>
			
<!--BEGIN LINKPAGELIST-->

<!-- BEGIN LIST UPDATE PROGRESS -->

    <asp:UpdateProgress ID="UpdateProgress_Lists" runat="server" AssociatedUpdatePanelID="udp_ListRepeater">
        <ProgressTemplate>
            <span style="color:Green;">UPDATE IN PROGRESS...</span>
        </ProgressTemplate>
    </asp:UpdateProgress>

<!-- END LIST UPDATE PROGRESS -->

<!-- BEGIN LINKPAGE REPEATER -->  

 <asp:Repeater ID="List_Repeater" runat="server" OnItemDataBound="List_Repeater_OnItemDataBound"  OnItemCommand="List_Repeater_OnItemCommand">

 <HeaderTemplate>
 </HeaderTemplate>

 <ItemTemplate>

			<div class="global_linkpage" onclick="location.href='Editpage.aspx?ListId=<%#Eval("ListId")%>';" style="cursor:pointer;">
				<img class="global_linkpage_image" src="Images/Interface/logo_link.png" alt="<%#Eval("ListName")%>" />
				    <div class="global_linkpage_label">
					    <div class="global_linkpage_trans"></div>
				    </div>
				    <div class="global_linkpage_title">
                        <div class="linkpages_delete">
                        <asp:LinkButton ID="lbn_delete_list" runat="server" OnClientClick="return confirm('Are you sure that you want to DELETE this LIST and all associated LINKS?');" CommandName="DeleteList" CommandArgument='<%#Eval("ListId")%>'><strong></strong></asp:LinkButton>
                        </div>
                    <b><%#Eval("ListName")%></b><br><asp:label id="lblListDesc" runat="server" />
                    </div>
            </div>

</ItemTemplate>

  <FooterTemplate>
  </FooterTemplate>

 </asp:Repeater>

<!-- END LINKPAGE REPEATER --> 
 
            </div>
        </ContentTemplate>

        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btn_addlist" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

<!--END UPDATE PANEL LINKPAGE-->

<!--END LINKPAGE LIST-->

		<div style="clear:both;"></div>

</asp:Content>


