<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RentalRequest.aspx.cs" Inherits="CarRentalDemo.RentalRequest" %>

<!DOCTYPE html>
<html lang="tr">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Kiralama Talebi - RentACar</title>
    
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    
    <link href="~/Content/Site.css" rel="stylesheet" />
    <link href="~/Content/RentalRequest.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">

        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
        
        <div class="header">
            <div class="container">
                <div class="header-content">
                    <a href="RentalRequest.aspx" class="logo">
                        <img src="Assets/car.svg" width="32" height="32" alt="Car" />
                        Car Rental Demo
                    </a>
                    <div class="nav">
                        <a href="RentalRequest.aspx" class="nav-link active">
                            <img src="Assets/car.svg" width="16" height="16" alt="Request" />
                            <asp:Literal ID="litNavRequest" runat="server" />
                        </a>
                        <a href="RentalList.aspx" class="nav-link">
                            <img src="Assets/list.svg" width="16" height="16" alt="List" />
                            <asp:Literal ID="litNavList" runat="server" />
                        </a>
                        <div class="user-welcome">
                            <img src="Assets/user.svg" width="16" height="16" alt="User" />
                            <asp:Literal ID="litUserWelcome" runat="server" />
                        </div>
                        <div class="language-switcher">
                            <asp:LinkButton ID="btnTurkish" runat="server" CssClass="lang-btn" 
                                OnClick="ChangeLanguage_Click" CommandArgument="tr" CausesValidation="false">TR</asp:LinkButton>
                            <asp:LinkButton ID="btnEnglish" runat="server" CssClass="lang-btn" 
                                OnClick="ChangeLanguage_Click" CommandArgument="en" CausesValidation="false">EN</asp:LinkButton>
                        </div>
                        <asp:LinkButton ID="btnLogout" runat="server" CssClass="btn btn-secondary" 
                            OnClick="btnLogout_Click" CausesValidation="false" />
                    </div>
                </div>
            </div>
        </div>

        <div class="container" style="padding-top: 2rem;">
            <div class="page-header">
                <h1 class="page-title">
                    <img src="Assets/car.svg" width="32" height="32" alt="Request" />
                    <asp:Literal ID="litPageTitle" runat="server" />
                </h1>
            </div>

            <div class="form-card">
                <asp:Panel ID="pnlSuccess" runat="server" CssClass="alert alert-success" Visible="false">
                    <img src="Assets/success.svg" width="20" height="20" alt="Success" />
                    <asp:Literal ID="litSuccess" runat="server" />
                </asp:Panel>

                <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-error" Visible="false">
                    <img src="Assets/error.svg" width="20" height="20" alt="Error" />
                    <asp:Literal ID="litError" runat="server" />
                </asp:Panel>

               <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="date-selection">
                            <div class="form-grid">
                                <div class="form-group">
                                    <label class="form-label">
                                        <asp:Literal ID="litStart" runat="server" />
                                        <span class="required">*</span>
                                    </label>
                                    <div class="input-wrapper">
                                        <img src="Assets/calendar.svg" class="input-icon" width="20" height="20" alt="Calendar" />
                                        <asp:TextBox ID="txtStart" runat="server" CssClass="form-input" />
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvStart" runat="server" 
                                        ControlToValidate="txtStart" 
                                        CssClass="validation-error" 
                                        Display="Dynamic" 
                                        EnableClientScript="false" />
                                    <asp:CustomValidator ID="cvStart" runat="server" 
                                        ControlToValidate="txtStart" 
                                        OnServerValidate="ValidateStart" 
                                        CssClass="validation-error" 
                                        Display="Dynamic" 
                                        EnableClientScript="false" />
                                </div>
                            
                                <div class="form-group">
                                    <label class="form-label">
                                        <asp:Literal ID="litEnd" runat="server" />
                                        <span class="required">*</span>
                                    </label>
                                    <div class="input-wrapper">
                                        <img src="Assets/calendar.svg" class="input-icon" width="20" height="20" alt="Calendar" />
                                        <asp:TextBox ID="txtEnd" runat="server" CssClass="form-input" />
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvEnd" runat="server" 
                                        ControlToValidate="txtEnd" 
                                        CssClass="validation-error" 
                                        Display="Dynamic" 
                                        EnableClientScript="false" />
                                    <asp:CustomValidator ID="cvEnd" runat="server" 
                                        ControlToValidate="txtEnd" 
                                        OnServerValidate="ValidateEnd" 
                                        CssClass="validation-error" 
                                        Display="Dynamic" 
                                        EnableClientScript="false" />
                                </div>
                            </div>
                        </div>

                        <div class="vehicle-selection">
                            <div class="form-grid">
                                <div class="form-group">
                                <label class="form-label">
                                    <asp:Literal ID="litVehicleType" runat="server" />
                                    <span class="required">*</span>
                                </label>
                                <div class="input-wrapper">
                                    <img src="Assets/car.svg" class="input-icon" width="20" height="20" alt="Vehicle" />
                                    <asp:DropDownList ID="ddlVehicleType" runat="server" CssClass="form-select" 
                                        AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlVehicleType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvVehicleType" runat="server" 
                                    ControlToValidate="ddlVehicleType" 
                                    InitialValue="" 
                                    CssClass="validation-error" 
                                    Display="Dynamic" 
                                    EnableClientScript="false" />
                            </div>

                            <div class="form-group">
                                <label class="form-label">
                                    <asp:Literal ID="litVehicleModel" runat="server" />
                                    <span class="required">*</span>
                                </label>
                                <div class="input-wrapper">
                                    <img src="Assets/car.svg" class="input-icon" width="20" height="20" alt="Model" />
                                    <asp:DropDownList ID="ddlVehicleModel" runat="server" CssClass="form-select">
                                    </asp:DropDownList>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvVehicleModel" runat="server" 
                                    ControlToValidate="ddlVehicleModel" 
                                    InitialValue="" 
                                    CssClass="validation-error" 
                                    Display="Dynamic" 
                                    EnableClientScript="false" />
                            </div>
                                                        </div>
                        </div>
                    </ContentTemplate>
                     <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlVehicleType" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>

                <div class="form-actions">
                    <asp:Button ID="btnClear" runat="server" CssClass="btn btn-secondary" 
                        OnClick="btnClear_Click" CausesValidation="false" />
                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" 
                        OnClick="btnSave_Click" />
                </div>
            </div>
        </div>
    </form>
    <script src="Scripts/CalendarValidation.js"></script>

</body>
</html>