using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DakarRally.Repository.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }
        public string TeamName { get; set; }
        public string VehicleModel { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public int Speed { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }

        public int RaceId { get; set; }
    }
}
