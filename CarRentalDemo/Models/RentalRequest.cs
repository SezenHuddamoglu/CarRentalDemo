using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRentalDemo.Models
{
    public class RentalRequest
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string VehicleType { get; set; }
        public string VehicleModel { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Now;

    }
}