using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship.Model
{
    internal enum ShipType
    {
        Battleship,
        Destroyer1,
        Destroyer2,
        Submarine1,
        Submarine2,
        Submarine3,
        PatrolBoat1,
        PatrolBoat2,
        PatrolBoat3,
        PatrolBoat4,

    };

    internal class Ship
    {
        private int _health;

        private readonly ShipType _type;

        private static readonly Dictionary<ShipType, int> shipLengths =
            new Dictionary<ShipType, int>()
            {
                {ShipType.Battleship, 4},
                {ShipType.Destroyer1, 3},
                {ShipType.Destroyer2, 3},
                {ShipType.Submarine1, 2},
                {ShipType.Submarine2, 2},
                {ShipType.Submarine3, 2},
                {ShipType.PatrolBoat1, 1},
                {ShipType.PatrolBoat2, 1},
                {ShipType.PatrolBoat3, 1},
                {ShipType.PatrolBoat4, 1}
            };

        public Ship(ShipType type)
        {
            _type = type;
            Reincarnate();
        }

        public void Reincarnate()
        {
            _health = shipLengths[_type];
        }

        public int Length
        {
            get { return shipLengths[_type]; }
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
