using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRentalDemo.Services
{
    public class LanguageService
    {
        private Dictionary<string, Dictionary<string, string>> translations;

        public LanguageService()
        {
            InitializeTranslations();
        }

        private void InitializeTranslations()
        {
            translations = new Dictionary<string, Dictionary<string, string>>();


            translations["tr"] = new Dictionary<string, string>
            {
                { "WelcomeBack", "Hesabınıza giriş yapın" },
                { "Welcome", "Hoş geldiniz" },
                { "WelcomeMessage", "Hesabınıza giriş yapın" },
                { "Email", "E-posta" },
                { "Password", "Şifre" },
                { "Login", "Giriş Yap" },
                { "Logout", "Çıkış Yap" },
                { "DemoAccount", "Demo Hesap Bilgileri:" },
                { "InvalidCredentials", "Geçersiz e-posta veya şifre!" },
                { "EmailRequired", "E-posta adresi gereklidir" },
                { "PasswordRequired", "Şifre gereklidir" },
                { "InvalidEmailFormat", "Geçerli bir e-posta adresi giriniz" },
                { "PasswordComplexity", "Şifre en az 6 karakter, 1 büyük harf, 1 küçük harf ve 1 rakam içermelidir" },
                { "NewRequest", "Yeni Talep" },
                { "RequestList", "Talep Listesi" },
                { "NewRentalRequest", "Yeni Kiralama Talebi" },
                { "StartDate", "Başlangıç Tarihi" },
                { "EndDate", "Bitiş Tarihi" },
                { "VehicleType", "Araç Tipi" },
                { "VehicleModel", "Araç Modeli" },
                { "Save", "Kaydet" },
                { "Clear", "Temizle" },
                { "SelectVehicleType", "Araç tipi seçiniz..." },
                { "SelectVehicleModel", "Araç modeli seçiniz..." },
                { "StartDateRequired", "Başlangıç tarihi gereklidir" },
                { "EndDateRequired", "Bitiş tarihi gereklidir" },
                { "VehicleTypeRequired", "Araç tipi seçilmelidir" },
                { "VehicleModelRequired", "Araç modeli seçilmelidir" },
                { "StartDatePast", "Başlangıç tarihi bugünden önce olamaz" },
                { "EndDateBeforeStart", "Bitiş tarihi başlangıç tarihinden sonra olmalıdır" },
                { "InvalidDate", "Geçersiz tarih formatı" },
                { "InvalidDateFormat", "Tarih formatı GG/AA/YYYY şeklinde olmalıdır" },
                { "RequestSaved", "Kiralama talebiniz başarıyla kaydedildi!" },
                { "ErrorSaving", "Kayıt sırasında bir hata oluştu:" },
                { "RentalRequestsList", "Kiralama Talepleri Listesi" },
                { "LoadRequests", "Talepleri Listele" },
                { "NoRequestsFound", "Henüz kayıtlı talep bulunmamaktadır." },
                { "RequestDate", "Talep Tarihi" },
                { "User", "Kullanıcı" },
                { "RentalPeriod", "Kiralama Dönemi" },
                { "Vehicle", "Araç" },
                { "Duration", "Süre" },
                { "Days", "gün" },
                { "DateFormatPlaceholder", "GG/AA/YYYY" },
                
                //auth
                { "CreateAccount", "Hesap Oluşturun" },
                { "FullName", "Ad Soyad" },
                { "ConfirmPassword", "Şifre Tekrarı" },
                { "Register", "Kayıt Ol" },
                { "HaveAccount", "Zaten hesabınız var mı?" },
                { "LoginNow", "Giriş yapın" },
                { "FullNameRequired", "Ad soyad gereklidir" },
                { "ConfirmPasswordRequired", "Şifre tekrarı gereklidir" },
                { "PasswordMismatch", "Şifreler eşleşmiyor" },
                { "InvalidNameFormat", "Ad soyad sadece harf ve boşluk içerebilir (2-50 karakter)" },
                { "RegistrationSuccess", "Kayıt başarılı! Giriş sayfasına yönlendiriliyorsunuz..." },
                { "EmailExists", "Bu e-posta adresi zaten kullanılıyor" },
                { "NoAccount", "Hesabınız yok mu?" },
                { "RegisterNow", "Kayıt Ol" }
            };


            translations["en"] = new Dictionary<string, string>
            {
                { "WelcomeBack", "Sign in to your account" },
                { "Welcome", "Welcome" },
                { "WelcomeMessage", "Sign in to your account" },
                { "Email", "Email" },
                { "Password", "Password" },
                { "Login", "Login" },
                { "Logout", "Logout" },
                { "DemoAccount", "Demo Account Information:" },
                { "InvalidCredentials", "Invalid email or password!" },
                { "EmailRequired", "Email address is required" },
                { "PasswordRequired", "Password is required" },
                { "InvalidEmailFormat", "Please enter a valid email address" },
                { "PasswordComplexity", "Password must contain at least 6 characters, 1 uppercase, 1 lowercase and 1 number" },
                { "NewRequest", "New Request" },
                { "RequestList", "Request List" },
                { "NewRentalRequest", "New Rental Request" },
                { "StartDate", "Start Date" },
                { "EndDate", "End Date" },
                { "VehicleType", "Vehicle Type" },
                { "VehicleModel", "Vehicle Model" },
                { "Save", "Save" },
                { "Clear", "Clear" },
                { "SelectVehicleType", "Select vehicle type..." },
                { "SelectVehicleModel", "Select vehicle model..." },
                { "StartDateRequired", "Start date is required" },
                { "EndDateRequired", "End date is required" },
                { "VehicleTypeRequired", "Vehicle type must be selected" },
                { "VehicleModelRequired", "Vehicle model must be selected" },
                { "StartDatePast", "Start date cannot be in the past" },
                { "EndDateBeforeStart", "End date must be after start date" },
                { "InvalidDate", "Invalid date format" },
                { "InvalidDateFormat", "Date format should be DD/MM/YYYY" },
                { "RequestSaved", "Your rental request has been saved successfully!" },
                { "ErrorSaving", "An error occurred while saving:" },
                { "RentalRequestsList", "Rental Requests List" },
                { "LoadRequests", "Load Requests" },
                { "NoRequestsFound", "No requests found yet." },
                { "RequestDate", "Request Date" },
                { "User", "User" },
                { "RentalPeriod", "Rental Period" },
                { "Vehicle", "Vehicle" },
                { "Duration", "Duration" },
                { "Days", "days" },
                { "DateFormatPlaceholder", "DD/MM/YYYY" },

                // Auth
                { "CreateAccount", "Create Your Account" },
                { "FullName", "Full Name" },
                { "ConfirmPassword", "Confirm Password" },
                { "Register", "Register" },
                { "HaveAccount", "Already have an account?" },
                { "LoginNow", "Sign in" },
                { "FullNameRequired", "Full name is required" },
                { "ConfirmPasswordRequired", "Confirm password is required" },
                { "PasswordMismatch", "Passwords do not match" },
                { "InvalidNameFormat", "Name should only contain letters and spaces (2-50 characters)" },
                { "RegistrationSuccess", "Registration successful! Redirecting to login page..." },
                { "EmailExists", "This email address is already in use" },
                { "NoAccount", "Don't have an account?" },
                { "RegisterNow", "Register Now" }
            };
        }

        public string GetText(string key, string language)
        {
            if (translations.ContainsKey(language) && translations[language].ContainsKey(key))
            {
                return translations[language][key];
            }


            if (translations["tr"].ContainsKey(key))
            {
                return translations["tr"][key];
            }

            return key;
        }
    }
}