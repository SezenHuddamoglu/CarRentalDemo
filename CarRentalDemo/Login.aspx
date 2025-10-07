<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CarRentalDemo.Login" %>

<!DOCTYPE html>
<html lang="tr">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Giriş Yap - Rent a Car</title>
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
                    <img src="Assets/car.svg" width="50" height="50" alt="Car" />
                </div>
                <h1 class="auth-title">Car Rental Demo</h1>
                <p class="auth-subtitle">
                    <asp:Literal ID="litWelcome" runat="server" />
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
            </div>

            <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-primary btn-full"
                OnClick="btnLogin_Click" />

            <div class="auth-actions">
                <p class="auth-link">
                    <asp:Literal ID="litNoAccount" runat="server" />
                    <a href="Register.aspx" class="auth-register-link">
                        <asp:Literal ID="litRegisterLink" runat="server" />
                    </a>
                </p>
            </div>

            <div class="demo-info">
                <p><strong>
                    <asp:Literal ID="litDemoAccount" runat="server" /></strong></p>
                <p>Email: admin@rentacar.com</p>
                <p>
                    <asp:Literal ID="litPasswordLabel" runat="server" />: Admin123</p>
            </div>
        </div>
    </form>


</body>
</html>
