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
            
            System.Diagnostics.Debug.WriteLine($"PAGE_LOAD ÇAĞRILDI");

            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }

            if (!IsPostBack)
            {
                System.Diagnostics.Debug.WriteLine("İLK YÜKLEME");

                if (Session["Language"] == null)
                {
                    Session["Language"] = "tr";
                }

                LoadVehicleTypes();
                UpdateLanguageLabels();
                UpdateUserWelcome();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(" POSTBACK YÜKLEME ");
            }

            string placeholder = langService.GetText("DateFormatPlaceholder", Session["Language"]?.ToString() ?? "tr");
            txtStart.Attributes["placeholder"] = placeholder;
            txtEnd.Attributes["placeholder"] = placeholder;

            UpdateCalendarLanguage();

            System.Diagnostics.Debug.WriteLine($"Vehicle Type Seçili: {ddlVehicleType.SelectedValue}");
            System.Diagnostics.Debug.WriteLine($"Vehicle Model Seçili: {ddlVehicleModel.SelectedValue}");
            System.Diagnostics.Debug.WriteLine($"UpdatePanel UpdateMode: {UpdatePanel1.UpdateMode}");
            System.Diagnostics.Debug.WriteLine($" PAGE_LOAD BİTTİ");
        }

        private void UpdateCalendarLanguage()
        {
            System.Diagnostics.Debug.WriteLine("UpdateCalendarLanguage çağrıldı");

            ScriptManager.RegisterStartupScript(this, GetType(), "UpdateCalendarLang",
                "setTimeout(function() { initializeDatePickers(); }, 100);", true);
        }

        protected void ddlVehicleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"VEHICLE TYPE CHANGED ÇAĞRILDI ");
            System.Diagnostics.Debug.WriteLine($"Seçilen Vehicle Type: {ddlVehicleType.SelectedValue}");
            System.Diagnostics.Debug.WriteLine($"Sayfa IsPostBack: {IsPostBack}");
            System.Diagnostics.Debug.WriteLine($"Sayfa IsCallback: {Page.IsCallback}");

            System.Diagnostics.Debug.WriteLine($"Değişim öncesi Model dropdown item sayısı: {ddlVehicleModel.Items.Count}");

            LoadVehicleModels();

            System.Diagnostics.Debug.WriteLine($"Değişim sonrası Model dropdown item sayısı: {ddlVehicleModel.Items.Count}");

            System.Diagnostics.Debug.WriteLine("UpdatePanel güncelleniyor");
            UpdatePanel1.Update();

            System.Diagnostics.Debug.WriteLine($" VEHICLE TYPE CHANGED BİTTİ ");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"SAVE BUTTON CLICKED");

            if (Page.IsValid)
            {
                try
                {
                    var request = new Models.RentalRequest
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = Session["UserId"].ToString(),
                        UserFullName = Session["UserFullName"].ToString(),
                        StartDate = ParseDateFromDDMMYYYY(txtStart.Text),
                        EndDate = ParseDateFromDDMMYYYY(txtEnd.Text),
                        VehicleType = ddlVehicleType.SelectedValue,
                        VehicleModel = ddlVehicleModel.SelectedValue,
                        RequestDate = DateTime.Now
                    };

                    rentalService.SaveRentalRequest(request);

                    pnlSuccess.Visible = true;
                    pnlError.Visible = false;
                    string lang = Session["Language"]?.ToString() ?? "tr";
                    litSuccess.Text = langService.GetText("RequestSaved", lang);

                    ClearForm();
                    System.Diagnostics.Debug.WriteLine("Kayıt başarılı!");
                }
                catch (Exception ex)
                {
                    pnlError.Visible = true;
                    pnlSuccess.Visible = false;
                    string lang = Session["Language"]?.ToString() ?? "tr";
                    litError.Text = langService.GetText("ErrorSaving", lang) + " " + ex.Message;
                    System.Diagnostics.Debug.WriteLine($"Kayıt hatası: {ex.Message}");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Sayfa valid değil");
            }

            System.Diagnostics.Debug.WriteLine($"AVE BUTTON BİTTİ");
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("CLEAR BUTTON CLICKED ");
            ClearForm();
            pnlSuccess.Visible = false;
            pnlError.Visible = false;
            System.Diagnostics.Debug.WriteLine(" CLEAR BUTTON BİTTİ ");
        }

        protected void ChangeLanguage_Click(object sender, EventArgs e)
        {
            var btn = (System.Web.UI.WebControls.LinkButton)sender;
            System.Diagnostics.Debug.WriteLine($"LANGUAGE CHANGED: {btn.CommandArgument}");
            Session["Language"] = btn.CommandArgument;
            LoadVehicleTypes();
            UpdateLanguageLabels();
            System.Diagnostics.Debug.WriteLine($"LANGUAGE CHANGED BİTTİ");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(" LOGOUT CLICKED");
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
            System.Diagnostics.Debug.WriteLine("ValidateStart çağrıldı");
            DateTime startDate;
            string lang = Session["Language"]?.ToString() ?? "tr";

            if (TryParseDateFromDDMMYYYY(args.Value, out startDate))
            {
                if (startDate < DateTime.Today)
                {
                    cvStart.ErrorMessage = langService.GetText("StartDatePast", lang);
                    args.IsValid = false;
                    System.Diagnostics.Debug.WriteLine("Start date geçmiş tarih - VALID: false");
                }
                else
                {
                    args.IsValid = true;
                    System.Diagnostics.Debug.WriteLine("Start date geçerli - VALID: true");
                }
            }
            else
            {
                cvStart.ErrorMessage = langService.GetText("InvalidDateFormat", lang);
                args.IsValid = false;
                System.Diagnostics.Debug.WriteLine("Start date format hatası - VALID: false");
            }
        }

        protected void ValidateEnd(object source, ServerValidateEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("ValidateEnd çağrıldı");
            DateTime startDate, endDate;
            string lang = Session["Language"]?.ToString() ?? "tr";

            if (TryParseDateFromDDMMYYYY(txtStart.Text, out startDate) &&
                TryParseDateFromDDMMYYYY(args.Value, out endDate))
            {
                if (endDate <= startDate)
                {
                    cvEnd.ErrorMessage = langService.GetText("EndDateBeforeStart", lang);
                    args.IsValid = false;
                    System.Diagnostics.Debug.WriteLine("End date start'tan önce - VALID: false");
                }
                else
                {
                    args.IsValid = true;
                    System.Diagnostics.Debug.WriteLine("End date geçerli - VALID: true");
                }
            }
            else
            {
                cvEnd.ErrorMessage = langService.GetText("InvalidDateFormat", lang);
                args.IsValid = false;
                System.Diagnostics.Debug.WriteLine("End date format hatası - VALID: false");
            }
        }

        private DateTime ParseDateFromDDMMYYYY(string dateString)
        {
            System.Diagnostics.Debug.WriteLine($"ParseDateFromDDMMYYYY: {dateString}");
            return DateTime.ParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        private bool TryParseDateFromDDMMYYYY(string dateString, out DateTime date)
        {
            System.Diagnostics.Debug.WriteLine($"TryParseDateFromDDMMYYYY: {dateString}");
            return DateTime.TryParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
        }

        private void LoadVehicleTypes()
        {
            System.Diagnostics.Debug.WriteLine("LoadVehicleTypes çağrıldı");
            string lang = Session["Language"]?.ToString() ?? "tr";

            ddlVehicleType.Items.Clear();
            ddlVehicleType.Items.Add(new ListItem(langService.GetText("SelectVehicleType", lang), ""));
            ddlVehicleType.Items.Add(new ListItem("Sedan", "Sedan"));
            ddlVehicleType.Items.Add(new ListItem("SUV", "SUV"));

            ddlVehicleModel.Items.Clear();
            ddlVehicleModel.Items.Add(new ListItem(langService.GetText("SelectVehicleModel", lang), ""));

            System.Diagnostics.Debug.WriteLine("Vehicle Types yüklendi");
        }

        private void LoadVehicleModels()
        {
            System.Diagnostics.Debug.WriteLine("LoadVehicleModels çağrıldı");
            string lang = Session["Language"]?.ToString() ?? "tr";
            string selectedType = ddlVehicleType.SelectedValue;

            ddlVehicleModel.Items.Clear();
            ddlVehicleModel.Items.Add(new ListItem(langService.GetText("SelectVehicleModel", lang), ""));

            if (selectedType == "Sedan")
            {
                ddlVehicleModel.Items.Add(new ListItem("Fiat Egea", "Fiat Egea"));
                ddlVehicleModel.Items.Add(new ListItem("Renault Symbol", "Renault Symbol"));
                ddlVehicleModel.Items.Add(new ListItem("Renault Megane", "Renault Megane"));
                System.Diagnostics.Debug.WriteLine("Sedan modelleri yüklendi");
            }
            else if (selectedType == "SUV")
            {
                ddlVehicleModel.Items.Add(new ListItem("Fiat Egea Cross", "Fiat Egea Cross"));
                ddlVehicleModel.Items.Add(new ListItem("Peugeot 3008", "Peugeot 3008"));
                ddlVehicleModel.Items.Add(new ListItem("Nissan Qashqai", "Nissan Qashqai"));
                System.Diagnostics.Debug.WriteLine("SUV modelleri yüklendi");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Hiçbir vehicle type seçili değil");
            }
        }

        private void ClearForm()
        {
            System.Diagnostics.Debug.WriteLine("ClearForm çağrıldı");
            txtStart.Text = string.Empty;
            txtEnd.Text = string.Empty;
            ddlVehicleType.SelectedIndex = 0;
            ddlVehicleModel.Items.Clear();
            string lang = Session["Language"]?.ToString() ?? "tr";
            ddlVehicleModel.Items.Add(new ListItem(langService.GetText("SelectVehicleModel", lang), ""));

            string placeholder = langService.GetText("DateFormatPlaceholder", lang);
            txtStart.Attributes["placeholder"] = placeholder;
            txtEnd.Attributes["placeholder"] = placeholder;

            System.Diagnostics.Debug.WriteLine("Form temizlendi");
        }

        private void UpdateUserWelcome()
        {
            System.Diagnostics.Debug.WriteLine("UpdateUserWelcome çağrıldı");
            if (Session["UserFullName"] != null)
            {
                string lang = Session["Language"]?.ToString() ?? "tr";
                litUserWelcome.Text = langService.GetText("Welcome", lang) + " " + Session["UserFullName"].ToString();
                System.Diagnostics.Debug.WriteLine($"User welcome güncellendi: {litUserWelcome.Text}");
            }
        }

        private void UpdateLanguageLabels()
        {
            System.Diagnostics.Debug.WriteLine("UpdateLanguageLabels çağrıldı");
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
            System.Diagnostics.Debug.WriteLine("Language labels güncellendi");
        }
    }
}