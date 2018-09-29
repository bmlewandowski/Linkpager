<%@ Page Title="" Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Linkpager.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

<!--LOAD STYLESHEETS-->
    <link href="Styles/home.css" rel="stylesheet" type="text/css" />

<!--LOAD JAVASCRIPT-->
    <script type="text/javascript" src="http://w.sharethis.com/button/buttons.js"></script><script type="text/javascript">                                                                                                   stLight.options({ publisher: '20fca839-abad-4612-86aa-9ccaeb1bb0e0' });</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

		<div id="home_maintitle">SAVE FAVORITES THE WAY YOU WANT TO</div>

		<div id="home_linkpagearea">

<!--BEGIN LINKPAGELIST-->

			<div class="global_linkpage" id="linkpage1">
            <a href="http://www.linkpager.com/Viewpage.aspx?ListId=38" target="_blank" title='Linkpage: "Sites for People that Love Film"'>
            <img src="Images/Interface/logo_star.png" alt='Linkpage: "Sites for People that Love Film"' />
            </a>
            </div>

			<div class="global_linkpage" id="linkpage2">
            <a href="http://www.linkpager.com/Viewpage.aspx?ListId=9" target="_blank" title='Linkpage: "Pontiac Motors"'>
            <img src="Images/Interface/logo_star.png" alt='Linkpage: "Pontiac Motors"' />
            </a>
            </div>

			<div class="global_linkpage" id="linkpage3">
            <a href="http://www.linkpager.com/Viewpage.aspx?ListId=39" target="_blank" title='Linkpage: "Contemporary Art Sites and Magazines"'>
            <img src="Images/Interface/logo_star.png" alt='Linkpage: "Contemporary Art Sites and Magazines"' />
            </a>
            </div>

			<div class="global_linkpage" id="linkpage4">
            <a href="http://www.linkpager.com/Viewpage.aspx?ListId=23" target="_blank" title='Linkpage: "All About Shoes!"'>
            <img src="Images/Interface/logo_star.png" alt='Linkpage: "All About Shoes!"' />
            </a>
            </div>

			<div class="global_linkpage" id="linkpage5">
            <a href="http://www.linkpager.com/Viewpage.aspx?ListId=24" target="_blank" title='Linkpage: "Need A Shirt?"'>
            <img src="Images/Interface/logo_star.png" alt='Linkpage: "Need A Shirt?"' />
            </a>
            </div>

			<div class="global_linkpage" id="linkpage6">
            <a href="FAQ.aspx" title="Learn More...">
            <img src="Images/Interface/learn_more.png" alt="FAQ" />
            </a>
            </div>


<!--END LINKPAGE LIST-->

		</div>

		<div id="home_messagearea">

			<a href="/Account/Register.aspx" title="Sign Up Today for your Free Account &#13 All the cool kids are doing it!"><img src="Images/Interface/register.jpg" alt="Sign Up Today for your Free Account" border="0" /></a>
			<p><strong>LINKPAGES</strong> are web pages of your favorite places that you can share with your friends. Organize your favories into lists and keep them grouped YOUR way.</p>
			<p>Forget Syncing. Store and Share!</p>
       
        		<ul>
        			<li><strong>Social Bookmarking</strong></li>
       				<li><strong>Private Bookmarking</strong></li>
        			<li><strong>ANYWHERE Bookmarking</strong></li>
        		</ul>
        
        		<p><a href="http://twitter.com/linkpager" title="Follow Linkpager on Twitter... &#13 See how long it takes us to botch the English language" target="_blank"><img src="Images/Interface/followtwitter.jpg" alt="Follow LINKPAGER on Twitter" border="0" /></a></p>
		
        		<iframe src="http://www.facebook.com/plugins/likebox.php?href=http%3A%2F%2Fwww.facebook.com%2Fpages%2FInterfave%2F156694957698009&amp;width=200&amp;colorscheme=light&amp;connections=0&amp;stream=false&amp;header=false&amp;height=50" scrolling="no" frameborder="0" style="border:none; overflow:hidden; width:200px; height:68px;" allowtransparency="true" title="Not 'Like-like', but y'know, like"  ></iframe>
		</div>

		<div style="clear:both;"></div>

		<div id="home_sharearea">
			<p>YOU LIKE THE SAUCE? THE SAUCE IS GOOD RIGHT? PASS THE SAUCE!!</p>
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
		</div>


</asp:Content>
