<%@ Page Title="" Language="C#" MasterPageFile="~/Private.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="Linkpager.Profile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h1><asp:LoginName ID="Profile_LoginName" runat="server" />'s Profile </h1>
<ul>
<li><a href="/Account/ChangePassword.aspx" title="Change Password">Change Password</a></li>
<br />
<li><a href="/Account/ForgotPassword.aspx" title="Recover Password">Recover Password</a></li>
</ul>
</asp:Content>
