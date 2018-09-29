﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="Linkpager.Account.ForgotPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:PasswordRecovery ID="PasswordRecovery" runat="server">
        <UserNameTemplate>
        <h2>Forgot your Password?</h2>
            <p>Enter your User Name to recover your password.</p>                           
            <p>
            <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
            <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
            <asp:Button ID="SubmitButton" runat="server" CommandName="Submit" Text="Recover" ValidationGroup="PasswordRecovery" BackColor="#F8D63D" BorderStyle="None" Height="25px" Width="75px" ToolTip="Click to Recover Password" CssClass="button_right" />
            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="PasswordRecovery" ForeColor="Red"><strong>X&nbsp;</strong></asp:RequiredFieldValidator>
            </p>    
                                      
             <p><span style="color:Red;"><asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal></span></p>                               
                          
        </UserNameTemplate>

        <QuestionTemplate>

            <h1>Identity Confirmation</h1>
            <p>Answer the following question to receive your password.</p>
                            
            <p>User Name: <asp:Literal ID="UserName" runat="server"></asp:Literal></p>
                             
            <p>Question: <asp:Literal ID="Question" runat="server"></asp:Literal></p>
            <p>
            <asp:Label ID="AnswerLabel" runat="server" AssociatedControlID="Answer">Answer:</asp:Label>
            <asp:TextBox ID="Answer" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="AnswerRequired" runat="server" ControlToValidate="Answer" ErrorMessage="Answer is required." ToolTip="Answer is required." ValidationGroup="PasswordRecovery" ForeColor="Red">*</asp:RequiredFieldValidator>
            </p>
            <p><asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal></p>   
                               
            <asp:Button ID="SubmitButton" runat="server" CommandName="Submit" Text="Submit" ValidationGroup="PasswordRecovery" BackColor="#F8D63D" BorderStyle="None" Height="25px" Width="150px" ToolTip="Click to Recover Password" />                
        </QuestionTemplate>       
        <SuccessTemplate>          
        Your password has been sent to you.          
        </SuccessTemplate>       
    </asp:PasswordRecovery>

     <br /><br />
    <h3>Don't have an account yet?</h3>
    <a href="/Account/Register.aspx" title="Sign Up Today for your Free Account &#13 All the cool kids are doing it!"><img src="/Images/Interface/register.jpg" alt="Sign Up Today for your Free Account" border="0" /></a>
</asp:Content>

