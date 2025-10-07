using CarRentalDemo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Newtonsoft.Json;

namespace CarRentalDemo.Services
{
    public class AuthService
    {
        private string usersFilePath;

        public AuthService()
        {
            usersFilePath = HostingEnvironment.MapPath("~/App_Data/users.json");

            
            if (!File.Exists(usersFilePath))
            {
                var defaultUsers = new List<User>
                {
                    new User
                    {
                        Id = "1",
                        Email = "admin@rentacar.com",
                        Password = "Admin123",
                        FullName = "Admin User",
                        CreatedDate = DateTime.Now
                    },
                    new User
                    {
                        Id = "2",
                        Email = "user@rentacar.com",
                        Password = "user123",
                        FullName = "Test User",
                        CreatedDate = DateTime.Now
                    }
                };

                SaveUsers(defaultUsers);
            }
        }

        private List<User> LoadUsers()
        {
            try
            {
                if (File.Exists(usersFilePath))
                {
                    string json = File.ReadAllText(usersFilePath);
                    return JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
                }
            }
            catch (Exception)
            {
                
            }

            return new List<User>();
        }

        private void SaveUsers(List<User> users)
        {
            try
            {
                string json = JsonConvert.SerializeObject(users, Formatting.Indented);
                File.WriteAllText(usersFilePath, json);
            }
            catch (Exception)
            {
                
            }
        }

        public User ValidateUser(string email, string password)
        {
            var users = LoadUsers();
            return users.FirstOrDefault(u =>
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);
        }

        public User RegisterUser(string fullName, string email, string password)
        {
            var users = LoadUsers();

           
            if (users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            {
                return null;
            }

            var newUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                Password = password,
                FullName = fullName,
                CreatedDate = DateTime.Now
            };

            users.Add(newUser);
            SaveUsers(users);

            return newUser;
        }
    }
}
