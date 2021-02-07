using System;
using System.Collections.Generic;
using System.Text;

namespace DakarRally.Repository.Models
{
    public class MalfunctionStatistic
    {
        public int Id { get; set; }
        public int NumberOfLightMalfunctions { get; set; }
        public int NumberOfHeavyMalfunctions { get; set; }

        public int VehicleStatisticId { get; set; }
        public virtual VehicleStatistic VehicleStatistic { get; set; }
    }
}
