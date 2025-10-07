<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="CarRentalDemo.Register" %>


<!DOCTYPE html>
<html lang="tr">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Kayıt Ol - Rent a Car</title>
    <link href="~/Content/Site.css" rel="stylesheet" />
    <link href="~/Content/Auth.css" rel="stylesheet" />


</head>
<body class="auth-page">
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server">
            <Scripts>
                <asp:ScriptReference Name="jquery" />
                <asp:ScriptReference Path="~/Scripts/validation.js" />
            </Scripts>
        </asp:ScriptManager>
        <div class="auth-container">
            <div class="auth-header">
                <div class="auth-logo">
                    <img src="Assets/car.svg" width="60" height="60" alt="Car" />
                </div>
                <h1 class="auth-title">Car Rental Demo</h1>
                <p class="auth-subtitle">
                    <asp:Literal ID="litCreateAccount" runat="server" />
                </p>
            </div>

            <div class="language-switcher">
                <asp:LinkButton ID="btnTurkish" runat="server" CssClass="lang-btn"
                    OnClick="ChangeLanguage_Click" CommandArgument="tr" CausesValidation="false">TR</asp:LinkButton>
                <asp:LinkButton ID="btnEnglish" runat="server" CssClass="lang-btn"
                    OnClick="ChangeLanguage_Click" CommandArgument="en" CausesValidation="false">EN</asp:LinkButton>
            </div>

            <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-error" Visible="false">
                <img src="Assets/error.svg" width="20" height="20" alt="Error" />
                <asp:Literal ID="litError" runat="server" />
            </asp:Panel>

            <asp:Panel ID="pnlSuccess" runat="server" CssClass="alert alert-success" Visible="false">
                <img src="Assets/success.svg" width="20" height="20" alt="Success" />
                <asp:Literal ID="litSuccess" runat="server" />
            </asp:Panel>

            <div class="form-group">
                <label class="form-label">
                    <asp:Literal ID="litFullName" runat="server" />
                    <span class="required">*</span>
                </label>
                <div class="input-wrapper">
                    <img src="Assets/user.svg" class="input-icon" width="20" height="20" alt="User" />
                    <asp:TextBox ID="txtFullName" runat="server" CssClass="form-input"
                        placeholder="Ad Soyad" />
                </div>
                <asp:RequiredFieldValidator ID="rfvFullName" runat="server"
                    ControlToValidate="txtFullName"
                    CssClass="validation-error"
                    Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revFullName" runat="server"
                    ControlToValidate="txtFullName"
                    ValidationExpression="^[a-zA-ZçğıöşüÇĞIİÖŞÜ\s]{2,50}$"
                    CssClass="validation-error"
                    Display="Dynamic" />
            </div>

            <div class="form-group">
                <label class="form-label">
                    <asp:Literal ID="litEmail" runat="server" />
                    <span class="required">*</span>
                </label>
                <div class="input-wrapper">
                    <img src="Assets/email.svg" class="input-icon" width="20" height="20" alt="Email" />
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-input"
                        TextMode="Email" placeholder="email@example.com" />
                </div>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                    ControlToValidate="txtEmail"
                    CssClass="validation-error"
                    Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revEmail" runat="server"
                    ControlToValidate="txtEmail"
                    ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                    CssClass="validation-error"
                    Display="Dynamic" />
            </div>

            <div class="form-group">
                <label class="form-label">
                    <asp:Literal ID="litPassword" runat="server" />
                    <span class="required">*</span>
                </label>
                <div class="input-wrapper">
                    <img src="Assets/lock.svg" class="input-icon" width="20" height="20" alt="Password" />
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-input"
                        TextMode="Password" placeholder="••••••••" />
                </div>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                    ControlToValidate="txtPassword"
                    CssClass="validation-error"
                    Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revPassword" runat="server"
                    ControlToValidate="txtPassword"
                    ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d@$!%*?&]{6,}$"
                    CssClass="validation-error"
                    Display="Dynamic" />
            </div>

            <div class="form-group">
                <label class="form-label">
                    <asp:Literal ID="litConfirmPassword" runat="server" />
                    <span class="required">*</span>
                </label>
                <div class="input-wrapper">
                    <img src="Assets/lock.svg" class="input-icon" width="20" height="20" alt="Confirm Password" />
                    <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-input"
                        TextMode="Password" placeholder="••••••••" />
                </div>
                <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server"
                    ControlToValidate="txtConfirmPassword"
                    CssClass="validation-error"
                    Display="Dynamic" />
                <asp:CompareValidator ID="cvPassword" runat="server"
                    ControlToValidate="txtConfirmPassword"
                    ControlToCompare="txtPassword"
                    CssClass="validation-error"
                    Display="Dynamic" />
            </div>

            <asp:Button ID="btnRegister" runat="server" CssClass="btn btn-primary btn-full"
                OnClick="btnRegister_Click" />

            <div class="auth-footer">
                <p class="auth-link">
                    <asp:Literal ID="litHaveAccount" runat="server" />
                    <a href="Login.aspx">
                        <asp:Literal ID="litLoginLink" runat="server" /></a>
                </p>
            </div>
        </div>
    </form>
</body>
</html>
