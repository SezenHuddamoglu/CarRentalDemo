using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CarRentalDemo
{
    public partial class RentalList : System.Web.UI.Page
    {
        private Services.LanguageService langService = new Services.LanguageService();
        private Services.RentalService rentalService = new Services.RentalService();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }

            if (!IsPostBack)
            {

                if (Session["Language"] == null)
                {
                    Session["Language"] = "tr";
                }
                
                UpdateLanguageLabels();
                UpdateUserWelcome();

                
                pnlTable.Visible = false; 
                pnlNoData.Visible = false; 

            }

        }
        protected void ChangeLanguage_Click(object sender, EventArgs e)
        {
            var btn = (System.Web.UI.WebControls.LinkButton)sender;
            Session["Language"] = btn.CommandArgument;
            UpdateLanguageLabels();
            
            if (pnlTable.Visible)
            {
                LoadRentalRequests();
            }
        }
       
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            System.Web.Security.FormsAuthentication.SignOut();

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
           
            
            Response.Redirect("~/Login.aspx");
        }

        protected void btnLoadRequests_Click(object sender, EventArgs e)
        {
            LoadRentalRequests();
        }

        private void LoadRentalRequests()
        {
            var requests = rentalService.GetAllRentalRequests();

            if (requests != null && requests.Count > 0)
            {
                gvRentalRequests.DataSource = requests.OrderByDescending(r => r.RequestDate).ToList();
                gvRentalRequests.DataBind();

                UpdateGridHeaders();

                string lang = Session["Language"]?.ToString() ?? "tr";
                string daysText = langService.GetText("Days", lang);

                foreach (GridViewRow row in gvRentalRequests.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        Literal litDays = (Literal)row.FindControl("litDays");
                        if (litDays != null)
                        {
                            litDays.Text = daysText;
                        }
                    }
                }

                pnlTable.Visible = true;
                pnlNoData.Visible = false;
            }
            else
            {
                pnlTable.Visible = false;
                pnlNoData.Visible = true;
            }

        }

        private void UpdateGridHeaders()
        {

            string lang = Session["Language"]?.ToString() ?? "tr";
           

            if (gvRentalRequests.HeaderRow != null)
            {
           
                try
                {

                    string requestDateText = langService.GetText("RequestDate", lang);
                    string userText = langService.GetText("User", lang);
                    string rentalPeriodText = langService.GetText("RentalPeriod", lang);
                    string vehicleText = langService.GetText("Vehicle", lang);
                    string durationText = langService.GetText("Duration", lang);

                   
                    if (gvRentalRequests.HeaderRow.Cells.Count > 0)
                        gvRentalRequests.HeaderRow.Cells[0].Text = requestDateText;
                    if (gvRentalRequests.HeaderRow.Cells.Count > 1)
                        gvRentalRequests.HeaderRow.Cells[1].Text = userText;
                    if (gvRentalRequests.HeaderRow.Cells.Count > 2)
                        gvRentalRequests.HeaderRow.Cells[2].Text = rentalPeriodText;
                    if (gvRentalRequests.HeaderRow.Cells.Count > 3)
                        gvRentalRequests.HeaderRow.Cells[3].Text = vehicleText;
                    if (gvRentalRequests.HeaderRow.Cells.Count > 4)
                        gvRentalRequests.HeaderRow.Cells[4].Text = durationText;


                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Hata: {ex.Message}");
                }
            }
        }

        private void UpdateLanguageLabels()
        {
            string lang = Session["Language"]?.ToString() ?? "tr";

            litPageTitle.Text = langService.GetText("RentalRequestsList", lang);
            litNavRequest.Text = langService.GetText("NewRequest", lang);
            litNavList.Text = langService.GetText("RequestList", lang);
            litNoData.Text = langService.GetText("NoRequestsFound", lang);
            btnLogout.Text = langService.GetText("Logout", lang);

            btnLoadRequests.Text = langService.GetText("LoadRequests", lang);

            btnTurkish.CssClass = lang == "tr" ? "lang-btn active" : "lang-btn";
            btnEnglish.CssClass = lang == "en" ? "lang-btn active" : "lang-btn";

            UpdateUserWelcome();
        }
        private void UpdateUserWelcome()
        {
            if (Session["UserFullName"] != null)
            {
                string lang = Session["Language"]?.ToString() ?? "tr";
                litUserWelcome.Text = langService.GetText("Welcome", lang) + " " + Session["UserFullName"].ToString();
            }
        }

    }
}