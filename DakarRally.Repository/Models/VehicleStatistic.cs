using System;
using System.Collections.Generic;
using System.Text;

namespace DakarRally.Repository.Models
{
    public class VehicleStatistic
    {
        public int Id { get; set; }
        public int Distance { get; set; }
        public string Status { get; set; }
        public MalfunctionStatistic MalfunctionStatistic { get; set; }
        public DateTime FinishTime { get; set; }

        public int VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
}
