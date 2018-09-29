<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Viewpage.aspx.cs" Inherits="Linkpager.Viewpage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

<!--LOAD STYLESHEETS-->
        <link href="~/Styles/viewpage.css" rel="stylesheet" type="text/css" />

<!--LOAD JAVASCRIPT-->
        <script src="/Scripts/global.js" type="text/javascript"></script>
        <script type="text/javascript" src="http://w.sharethis.com/button/buttons.js"></script><script type="text/javascript">                                                                                                   stLight.options({ publisher: '20fca839-abad-4612-86aa-9ccaeb1bb0e0' });</script>

<!-- BEGIN Google Analytics -->
<script type="text/javascript">

    var _gaq = _gaq || [];
    _gaq.push(['_setAccount', 'UA-20506889-1']);
    _gaq.push(['_trackPageview']);

    (function () {
        var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
        ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
        var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
    })();

</script>
<!-- END Google Analytics -->

</head>
<body>
    <form id="form1" runat="server">

<div id="viewpage_outer">

	<div id="viewpage_leftcol">

<!--BEGIN LINKPAGE HEADER-->

		<div id="viewpage_header">

			<div id="viewpage_header_area">

				<div id="viewpage_header_icon"><img id="header_logo" src="Images/interface/logo_star.png" alt="" /></div>

				<div id="viewpage_header_text">

					<div class="viewpage_linkpage_title"><asp:Label ID="lbl_ListTitle" runat="server" /></div>
		
					<div class="viewpage_linkpage_desc">
                    <asp:Label ID="lbl_ListDescription" runat="server" />
					</div>
				                 
                </div>

			</div>

			<div id="viewpage_header_commands">
				<img src="Images/interface/share.gif" id="viewpage_header_sharebutton" onclick="toggle_generalpanel('viewpage_header_share')" style="cursor:pointer;" alt="" />
				<img src="Images/interface/more.gif" id="viewpage_header_morebutton" onclick="toggle_more('viewpage_header_more', this.id)" style="cursor:pointer;" alt="" />
			</div>

<!--BEGIN HIDDEN PANELS-->
			
			<div id="viewpage_header_share">
	
    <br />	
    		
<!-- BEGIN SHARETHIS BUTTON -->
Share this Linkpage with others anyway you like:<br /><br />

        <span class="st_email_large"></span>
        <span class="st_twitter_large"></span>
        <span class="st_facebook_large"></span>
        <span class="st_myspace_large"></span>
        <span class="st_digg_large"></span>
        <span class="st_reddit_large"></span>
        <span class="st_delicious_large"></span>
        <span class="st_stumbleupon_large"></span>
        <span class="st_myspace_large"></span>
        <span class="st_ybuzz_large"></span>
        <span class="st_gbuzz_large"></span>
        <span class="st_blogger_large"></span>
        <span class="st_technorati_large"></span>

<!-- END SHARETHIS BUTTON -->

			</div>

			<div id="viewpage_header_more">
				more panel
			</div>
		
<!--END HIDDEN PANES-->

		</div>

		<div class="viewpage_hr"></div>

<!--END LINKPAGE HEADER-->


<!--BEGIN LINK REPEATER-->

        <asp:Repeater ID="Link_Repeater" runat="server" OnItemDataBound="Link_Repeater_OnItemDataBound">
            <ItemTemplate>		
            

		<div class="viewpage_link">

			<div class="viewpage_link_area">

				<div class="viewpage_link_icon">
                
                <a href="<%#Eval("LinkUrl")%>" title="<%#Eval("LinkName")%>" target="_blank"> 
                <img src="Images/interface/logo_link.png" alt='<%#Eval("LinkName")%>' />
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
				<img src="Images/interface/share.gif"  id="viewpage_link_sharebutton<%#(((RepeaterItem)Container).ItemIndex+1).ToString() %>" onclick="toggle_generalpanel('viewpage_link_share<%#(((RepeaterItem)Container).ItemIndex+1).ToString() %>')" style="cursor:pointer;" alt="" />
				<img src="Images/interface/more.gif" id="viewpage_link_morebutton<%#(((RepeaterItem)Container).ItemIndex+1).ToString() %>" onclick="toggle_more('viewpage_link_more<%#(((RepeaterItem)Container).ItemIndex+1).ToString() %>', this.id)" style="cursor:pointer;" alt="" />
			</div>

<!--BEGIN HIDDEN PANELS-->
			
			<div class="viewpage_link_share" id="viewpage_link_share<%#(((RepeaterItem)Container).ItemIndex+1).ToString() %>">

    <br />	
    		
<!-- BEGIN SHARETHIS BUTTON -->
Share this Linkpage with others anyway you like:<br /><br />

        <span class="st_email_large"></span>
        <span class="st_twitter_large"></span>
        <span class="st_facebook_large"></span>
        <span class="st_myspace_large"></span>
        <span class="st_digg_large"></span>
        <span class="st_reddit_large"></span>
        <span class="st_delicious_large"></span>
        <span class="st_stumbleupon_large"></span>
        <span class="st_myspace_large"></span>
        <span class="st_ybuzz_large"></span>
        <span class="st_gbuzz_large"></span>
        <span class="st_blogger_large"></span>
        <span class="st_technorati_large"></span>

<!-- END SHARETHIS BUTTON -->

			</div>

			<div class="viewpage_link_more" id="viewpage_link_more<%#(((RepeaterItem)Container).ItemIndex+1).ToString() %>">
				more panel
			</div>
		
<!--END HIDDEN PANES-->

		</div>


 
            </ItemTemplate>

            <SeparatorTemplate>

		        <div class="viewpage_hr"></div>
            
            </SeparatorTemplate>
            
            <FooterTemplate>
            
		        <div class="viewpage_hr"></div>            
            
            </FooterTemplate>
            
            </asp:Repeater>		

<!--END LINKPAGE REPEATER-->			

	</div>

	<div id="viewpage_rightcol">

		<div id="viewpage_marketingblock">

     <p><strong>This Linkpage by <asp:Label ID="lbl_Author" runat="server" /></strong></p>
     <br />
     Want to make & share your own <a href="http://www.linkpager.com/" title="Linkpager.com" target="_blank">Linkpages</a>?</p>
     <p  style="float:right">
        <a href="/Account/Register.aspx" title="Sign Up Today for your Free Account &#13 All the cool kids are doing it!" target="_blank"><img src="/Images/Interface/register.jpg" alt="Sign Up Today for your Free Account" border="0" /></a>
        </p>
		</div>

		<div class="viewpage_ad">

<!-- BEGIN ADSENSE -->

            <script type="text/javascript"><!--
            google_ad_client = "ca-pub-9807806089947208";
            /* Linkpager Large Rectangle */
            google_ad_slot = "7527826326";
            google_ad_width = 336;
            google_ad_height = 280;
            //-->
            </script>
            <script type="text/javascript" src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
            </script>

<!-- END ADSENSE -->

        </div>

		<div class="viewpage_ad">

<!-- BEGIN ADSENSE -->

            <script type="text/javascript"><!--
                google_ad_client = "ca-pub-9807806089947208";
                /* Linkpager Large Rectangle */
                google_ad_slot = "7527826326";
                google_ad_width = 336;
                google_ad_height = 280;
            //-->
            </script>
            <script type="text/javascript" src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
            </script>

<!-- END ADSENSE -->

        </div>

	</div>

	<div></div>

</div>



    </form>
</body>
</html>
