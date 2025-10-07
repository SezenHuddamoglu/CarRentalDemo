<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RentalList.aspx.cs" Inherits="CarRentalDemo.RentalList" %>

<!DOCTYPE html>
<html lang="tr">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Kiralama Talepleri - RentACar</title>
    <link href="~/Content/Site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="header">
            <div class="container">
                <div class="header-content">
                    <a href="RentalRequest.aspx" class="logo">
                        <img src="Assets/car.svg" width="32" height="32" alt="Car" />
                        Car Rental Demo
                    </a>
                    <div class="nav">
                        <a href="RentalRequest.aspx" class="nav-link">
                            <img src="Assets/car.svg" width="16" height="16" alt="Request" />
                            <asp:Literal ID="litNavRequest" runat="server" />
                        </a>
                        <a href="RentalList.aspx" class="nav-link active">
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
                    <img src="Assets/list.svg" width="32" height="32" alt="List" />
                    <asp:Literal ID="litPageTitle" runat="server" />
                </h1>

                <div style="margin-top: 1rem;">
                    <asp:Button ID="btnLoadRequests" runat="server"
                        CssClass="btn btn-primary"
                        OnClick="btnLoadRequests_Click"
                        CausesValidation="false" />
                </div>
            </div>

            <div class="table-card">
                <asp:Panel ID="pnlNoData" runat="server" CssClass="alert alert-info" Visible="false">
                    <img src="Assets/info.svg" width="20" height="20" alt="Info" />
                    <asp:Literal ID="litNoData" runat="server" />
                </asp:Panel>

                <asp:Panel ID="pnlTable" runat="server" Visible="false">
                    <div class="table-wrapper">
                        <asp:GridView ID="gvRentalRequests" runat="server"
                            CssClass="data-table"
                            AutoGenerateColumns="false"
                            GridLines="None"
                            EmptyDataText="No rental requests found.">

                            <HeaderStyle CssClass="table-header" />
                            <RowStyle CssClass="table-row" />
                            <AlternatingRowStyle CssClass="table-row table-row-alt" />
                            <Columns>
                                <asp:TemplateField HeaderText="Request Date" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <div style="font-weight: 600; color: #475569;">
                                            <%# Eval("RequestDate", "{0:dd/MM/yyyy}") %><br />
                                            <small style="color: #64748b; font-weight: 400;"><%# Eval("RequestDate", "{0:HH:mm}") %></small>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="User" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div style="font-weight: 600; color: #334155;">
                                            <%# Eval("UserFullName") %>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Rental Period" HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <div style="display: flex; flex-direction: column; gap: 4px;">
                                            <div style="font-weight: 600; color: #059669;">
                                                <%# Eval("StartDate", "{0:dd/MM/yyyy}") %>
                                            </div>
                                            <div style="font-weight: 600; color: #dc2626;">
                                                <%# Eval("EndDate", "{0:dd/MM/yyyy}") %>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Vehicle" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div style="display: flex; flex-direction: column; gap: 2px;">
                                            <div style="font-weight: 700; color: #6366f1; font-size: 0.95rem;">
                                                <%# Eval("VehicleType") %>
                                            </div>
                                            <div style="color: #64748b; font-weight: 500; font-size: 0.9rem;">
                                                <%# Eval("VehicleModel") %>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Duration" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <span class="duration-badge">
                                            <%# ((DateTime)Eval("EndDate") - (DateTime)Eval("StartDate")).Days %>
                                            <asp:Literal ID="litDays" runat="server" />
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </form>
</body>
</html>
