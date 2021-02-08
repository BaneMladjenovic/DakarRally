using System;
using System.Collections.Generic;
using System.Text;

namespace DakarRally.Shared.Enums
{
    public enum SortOrder
    {
        ASC,
        DESC
    }

    public enum VehicleType
    {
        Car,
        Motorcycle,
        Truck
    }

    public enum CarType
    {
        Sport,
        Terrain
    }

    public enum MotorcycleType
    {
        Cross,
        Sport
    }

    public enum RaceStatus
    {
        Pending, 
        Running, 
        Finished
    }
}
