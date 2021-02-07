using System;
using System.Collections.Generic;
using System.Text;

namespace DakarRally.Shared.DTO
{
    public class RaceStatusDTO
    {
        public string Status { get; set; }
        public List<int> VehiclesGroupeByStatus { get; set; }
        public List<int> VehiclesGroupeByVehicleType { get; set; }
    }
}
