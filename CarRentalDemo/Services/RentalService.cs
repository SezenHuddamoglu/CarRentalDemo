using CarRentalDemo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace CarRentalDemo.Services
{
    public class RentalService
    {

        private string dataFilePath;
        private JavaScriptSerializer serializer;

        public RentalService()
        {
            dataFilePath = HttpContext.Current.Server.MapPath("~/App_Data/rentals.json");
            serializer = new JavaScriptSerializer();


            string directory = Path.GetDirectoryName(dataFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }


            if (!File.Exists(dataFilePath))
            {
                File.WriteAllText(dataFilePath, "[]");
            }
        }

        public void SaveRentalRequest(CarRentalDemo.Models.RentalRequest request)
        {
            var requests = GetAllRentalRequests();
            requests.Add(request);

            string json = serializer.Serialize(requests);
            File.WriteAllText(dataFilePath, json);
        }

        public List<CarRentalDemo.Models.RentalRequest> GetAllRentalRequests()
        {
            try
            {
                string json = File.ReadAllText(dataFilePath);
                var requests = serializer.Deserialize<List<CarRentalDemo.Models.RentalRequest>>(json);
                return requests ?? new List<CarRentalDemo.Models.RentalRequest>();
            }
            catch
            {
                return new List<CarRentalDemo.Models.RentalRequest>();
            }
        }


    }
}
