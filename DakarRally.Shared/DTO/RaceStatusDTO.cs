using System;
using System.Collections.Generic;
using System.Text;

namespace DakarRally.Shared.DTO
{
    public class RaceStatusDTO
    {
        public string Status { get; set; }
        public List<string> VehiclesGroupeByStatus { get; set; }
        public List<string> VehiclesGroupeByVehicleType { get; set; }
    }
}
