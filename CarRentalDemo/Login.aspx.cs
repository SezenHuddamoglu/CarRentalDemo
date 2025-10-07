using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebGrease.Activities;

namespace CarRentalDemo
{
    public partial class Login : System.Web.UI.Page
    {
        private Services.AuthService authService = new Services.AuthService();
        private Services.LanguageService langService = new Services.LanguageService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] != null)
            {
                Response.Redirect("~/RentalRequest.aspx");
            }

            if (!IsPostBack)
            {
                if (Session["Language"] == null)
                {
                    Session["Language"] = "tr";
                }

                UpdateLanguageLabels();
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text.Trim();

                var user = authService.ValidateUser(email, password);

                if (user != null)
                {

                    Session["UserId"] = user.Id;
                    Session["UserEmail"] = user.Email;
                    Session["UserFullName"] = user.FullName;


                    Response.Redirect("~/RentalRequest.aspx");
                }
                else
                {
                    pnlError.Visible = true;
                    string lang = Session["Language"]?.ToString() ?? "tr";
                    litError.Text = langService.GetText("InvalidCredentials", lang);
                }
            }
        }

        protected void ChangeLanguage_Click(object sender, EventArgs e)
        {
            var btn = (System.Web.UI.WebControls.LinkButton)sender;
            Session["Language"] = btn.CommandArgument;
            UpdateLanguageLabels();
        }

        private void UpdateLanguageLabels()
        {
            string lang = Session["Language"]?.ToString() ?? "tr";

            litWelcome.Text = langService.GetText("WelcomeBack", lang);
            litEmail.Text = langService.GetText("Email", lang);
            litPassword.Text = langService.GetText("Password", lang);
            btnLogin.Text = langService.GetText("Login", lang);
            litDemoAccount.Text = langService.GetText("DemoAccount", lang);
            litPasswordLabel.Text = langService.GetText("Password", lang);


            litNoAccount.Text = langService.GetText("NoAccount", lang);
            litRegisterLink.Text = langService.GetText("RegisterNow", lang);

            rfvEmail.ErrorMessage = langService.GetText("EmailRequired", lang);
            rfvPassword.ErrorMessage = langService.GetText("PasswordRequired", lang);
            revEmail.ErrorMessage = langService.GetText("InvalidEmailFormat", lang);


            btnTurkish.CssClass = lang == "tr" ? "lang-btn active" : "lang-btn";
            btnEnglish.CssClass = lang == "en" ? "lang-btn active" : "lang-btn";
        }
    }
}