using CarRentalDemo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebGrease.Activities;

namespace CarRentalDemo
{
    public partial class RentalRequest : System.Web.UI.Page
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

                LoadVehicleTypes();
                UpdateLanguageLabels();
                UpdateUserWelcome();
            }
            
            string placeholder = langService.GetText("DateFormatPlaceholder", Session["Language"]?.ToString() ?? "tr");
            txtStart.Attributes["placeholder"] = placeholder;
            txtEnd.Attributes["placeholder"] = placeholder;

            UpdateCalendarLanguage();

        }

        private void UpdateCalendarLanguage()
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "UpdateCalendarLang",
                "setTimeout(function() { initializeDatePickers(); }, 100);", true);
        }

        protected void ddlVehicleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadVehicleModels();

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (Page.IsValid)
            {
                try
                {
                    var request = new Models.RentalRequest
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = Session["UserId"].ToString(),
                        UserFullName = Session["UserFullName"].ToString(),
                        StartDate = ParseDateFromDDMMYYYY(txtStart.Text).Date,
                        EndDate = ParseDateFromDDMMYYYY(txtEnd.Text).Date,
                        VehicleType = ddlVehicleType.SelectedValue,
                        VehicleModel = ddlVehicleModel.SelectedValue,
                        RequestDate = DateTime.Today
                    };

                    rentalService.SaveRentalRequest(request);

                    pnlSuccess.Visible = true;
                    pnlError.Visible = false;
                    string lang = Session["Language"]?.ToString() ?? "tr";
                    litSuccess.Text = langService.GetText("RequestSaved", lang);

                    ClearForm();
                }
                catch (Exception ex)
                {
                    pnlError.Visible = true;
                    pnlSuccess.Visible = false;
                    string lang = Session["Language"]?.ToString() ?? "tr";
                    litError.Text = langService.GetText("ErrorSaving", lang) + " " + ex.Message;
                }
            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
            pnlSuccess.Visible = false;
            pnlError.Visible = false;
        }

        protected void ChangeLanguage_Click(object sender, EventArgs e)
        {
            var btn = (System.Web.UI.WebControls.LinkButton)sender;
            Session["Language"] = btn.CommandArgument;
            LoadVehicleTypes();
            UpdateLanguageLabels();
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

        protected void ValidateStart(object source, ServerValidateEventArgs args)
        {
            DateTime startDate;
            string lang = Session["Language"]?.ToString() ?? "tr";

            if (TryParseDateFromDDMMYYYY(args.Value, out startDate))
            {
                if (startDate < DateTime.Today)
                {
                    cvStart.ErrorMessage = langService.GetText("StartDatePast", lang);
                    args.IsValid = false;
                }
                else
                {
                    args.IsValid = true;
                }
            }
            else
            {
                cvStart.ErrorMessage = langService.GetText("InvalidDateFormat", lang);
                args.IsValid = false;
            }
        }

        protected void ValidateEnd(object source, ServerValidateEventArgs args)
        {
            DateTime startDate, endDate;
            string lang = Session["Language"]?.ToString() ?? "tr";

            if (TryParseDateFromDDMMYYYY(txtStart.Text, out startDate) &&
                TryParseDateFromDDMMYYYY(args.Value, out endDate))
            {
                if (endDate <= startDate)
                {
                    cvEnd.ErrorMessage = langService.GetText("EndDateBeforeStart", lang);
                    args.IsValid = false;
                }
                else
                {
                    args.IsValid = true;
                }
            }
            else
            {
                cvEnd.ErrorMessage = langService.GetText("InvalidDateFormat", lang);
                args.IsValid = false;
            }
        }

        private DateTime ParseDateFromDDMMYYYY(string dateString)
        {
            return DateTime.ParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
        }

        private bool TryParseDateFromDDMMYYYY(string dateString, out DateTime date)
        {
            bool success = DateTime.TryParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime tempDate);
            date = success ? tempDate.Date : DateTime.MinValue;
            return success;
        }

        private void LoadVehicleTypes()
        {
            string lang = Session["Language"]?.ToString() ?? "tr";

            ddlVehicleType.Items.Clear();
            ddlVehicleType.Items.Add(new ListItem(langService.GetText("SelectVehicleType", lang), ""));
            ddlVehicleType.Items.Add(new ListItem("Sedan", "Sedan"));
            ddlVehicleType.Items.Add(new ListItem("SUV", "SUV"));

            ddlVehicleModel.Items.Clear();
            ddlVehicleModel.Items.Add(new ListItem(langService.GetText("SelectVehicleModel", lang), ""));

        }

        private void LoadVehicleModels()
        {
            string lang = Session["Language"]?.ToString() ?? "tr";
            string selectedType = ddlVehicleType.SelectedValue;

            ddlVehicleModel.Items.Clear();
            ddlVehicleModel.Items.Add(new ListItem(langService.GetText("SelectVehicleModel", lang), ""));

            if (selectedType == "Sedan")
            {
                ddlVehicleModel.Items.Add(new ListItem("Fiat Egea", "Fiat Egea"));
                ddlVehicleModel.Items.Add(new ListItem("Renault Symbol", "Renault Symbol"));
                ddlVehicleModel.Items.Add(new ListItem("Renault Megane", "Renault Megane"));
            }
            else if (selectedType == "SUV")
            {
                ddlVehicleModel.Items.Add(new ListItem("Fiat Egea Cross", "Fiat Egea Cross"));
                ddlVehicleModel.Items.Add(new ListItem("Peugeot 3008", "Peugeot 3008"));
                ddlVehicleModel.Items.Add(new ListItem("Nissan Qashqai", "Nissan Qashqai"));
            }
           
        }

        private void ClearForm()
        {
            txtStart.Text = string.Empty;
            txtEnd.Text = string.Empty;
            ddlVehicleType.SelectedIndex = 0;
            ddlVehicleModel.Items.Clear();
            string lang = Session["Language"]?.ToString() ?? "tr";
            ddlVehicleModel.Items.Add(new ListItem(langService.GetText("SelectVehicleModel", lang), ""));

            string placeholder = langService.GetText("DateFormatPlaceholder", lang);
            txtStart.Attributes["placeholder"] = placeholder;
            txtEnd.Attributes["placeholder"] = placeholder;

        }

        private void UpdateUserWelcome()
        {
            if (Session["UserFullName"] != null)
            {
                string lang = Session["Language"]?.ToString() ?? "tr";
                litUserWelcome.Text = langService.GetText("Welcome", lang) + " " + Session["UserFullName"].ToString();
            }
        }

        private void UpdateLanguageLabels()
        {
            string lang = Session["Language"]?.ToString() ?? "tr";

            litPageTitle.Text = langService.GetText("NewRentalRequest", lang);
            litNavRequest.Text = langService.GetText("NewRequest", lang);
            litNavList.Text = langService.GetText("RequestList", lang);
            litStart.Text = langService.GetText("StartDate", lang);
            litEnd.Text = langService.GetText("EndDate", lang);
            litVehicleType.Text = langService.GetText("VehicleType", lang);
            litVehicleModel.Text = langService.GetText("VehicleModel", lang);
            btnSave.Text = langService.GetText("Save", lang);
            btnClear.Text = langService.GetText("Clear", lang);
            btnLogout.Text = langService.GetText("Logout", lang);

            rfvStart.ErrorMessage = langService.GetText("StartDateRequired", lang);
            rfvEnd.ErrorMessage = langService.GetText("EndDateRequired", lang);
            rfvVehicleType.ErrorMessage = langService.GetText("VehicleTypeRequired", lang);
            rfvVehicleModel.ErrorMessage = langService.GetText("VehicleModelRequired", lang);

            string placeholder = langService.GetText("DateFormatPlaceholder", lang);
            txtStart.Attributes["placeholder"] = placeholder;
            txtEnd.Attributes["placeholder"] = placeholder;

            btnTurkish.CssClass = lang == "tr" ? "lang-btn active" : "lang-btn";
            btnEnglish.CssClass = lang == "en" ? "lang-btn active" : "lang-btn";

            UpdateUserWelcome();
        }
    }
}