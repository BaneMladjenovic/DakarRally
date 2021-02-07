using System;
using System.Collections.Generic;
using System.Text;

namespace DakarRally.Shared.DTO
{
    public class VehicleFilterDTO
    {
        public string TeamName { get; set; }
        public string VehicleModel { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public string Status { get; set; }
        public int? SortOrder { get; set; }
    }
}
