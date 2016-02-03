using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship.Model
{
    internal enum VehicleType
    {
        Tank,
        Carrier1,
        Carrier2,
        Cannon1,
        Cannon2,
        Cannon3

    };

    internal class Vehicle
    {
        private int _health;

        private readonly VehicleType _type;

        private static readonly Dictionary<VehicleType, int> vehicleLengths =
            new Dictionary<VehicleType, int>()
            {
                {VehicleType.Tank, 4},
                {VehicleType.Carrier1, 3},
                {VehicleType.Carrier2, 3},
                {VehicleType.Cannon1, 2},
                {VehicleType.Cannon2, 2},
                {VehicleType.Cannon3, 2}
            };

        public Vehicle(VehicleType type)
        {
            _type = type;
            Reincarnate();
        }

        public void Reincarnate()
        {
            _health = vehicleLengths[_type];
        }

        public int Length
        {
            get { return vehicleLengths[_type]; }
        }

        public bool IsDestroyed
        {
            get { return _health == 0 ? true : false; }
        }

        public bool FiredAt()
        {
            _health--;
            return IsDestroyed;
        }

    }
}
