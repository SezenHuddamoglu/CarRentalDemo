using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CarRentalDemo
{
    public partial class Register : System.Web.UI.Page
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

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string fullName = txtFullName.Text.Trim();
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text.Trim();

                try
                {
                    var user = authService.RegisterUser(fullName, email, password);

                    if (user != null)
                    {
                        pnlSuccess.Visible = true;
                        pnlError.Visible = false;
                        string lang = Session["Language"]?.ToString() ?? "tr";
                        litSuccess.Text = langService.GetText("RegistrationSuccess", lang);


                        txtFullName.Text = "";
                        txtEmail.Text = "";
                        txtPassword.Text = "";
                        txtConfirmPassword.Text = "";

                        Response.AddHeader("REFRESH", "2;URL=Login.aspx");
                    }
                    else
                    {
                        pnlError.Visible = true;
                        pnlSuccess.Visible = false;
                        string lang = Session["Language"]?.ToString() ?? "tr";
                        litError.Text = langService.GetText("EmailExists", lang);
                    }
                }
                catch (Exception ex)
                {
                    pnlError.Visible = true;
                    pnlSuccess.Visible = false;
                    litError.Text = ex.Message;
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

            litCreateAccount.Text = langService.GetText("CreateAccount", lang);
            litFullName.Text = langService.GetText("FullName", lang);
            litEmail.Text = langService.GetText("Email", lang);
            litPassword.Text = langService.GetText("Password", lang);
            litConfirmPassword.Text = langService.GetText("ConfirmPassword", lang);
            btnRegister.Text = langService.GetText("Register", lang);
            litHaveAccount.Text = langService.GetText("HaveAccount", lang);
            litLoginLink.Text = langService.GetText("LoginNow", lang);

            rfvFullName.ErrorMessage = langService.GetText("FullNameRequired", lang);
            rfvEmail.ErrorMessage = langService.GetText("EmailRequired", lang);
            rfvPassword.ErrorMessage = langService.GetText("PasswordRequired", lang);
            rfvConfirmPassword.ErrorMessage = langService.GetText("ConfirmPasswordRequired", lang);
            cvPassword.ErrorMessage = langService.GetText("PasswordMismatch", lang);


            revFullName.ErrorMessage = langService.GetText("InvalidNameFormat", lang);
            revEmail.ErrorMessage = langService.GetText("InvalidEmailFormat", lang);
            revPassword.ErrorMessage = langService.GetText("PasswordComplexity", lang);


            btnTurkish.CssClass = lang == "tr" ? "lang-btn active" : "lang-btn";
            btnEnglish.CssClass = lang == "en" ? "lang-btn active" : "lang-btn";
        }
    }
}