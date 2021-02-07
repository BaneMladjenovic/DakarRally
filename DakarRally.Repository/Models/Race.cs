using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DakarRally.Repository.Models
{
    public class Race
    {
        [Key]
        public int Id { get; set; }
        public int Distance { get; set; }
        public string Status { get; set; }
        public int Year { get; set; }

        public List<Vehicle> Vehicles { get; set; }
    }
}
