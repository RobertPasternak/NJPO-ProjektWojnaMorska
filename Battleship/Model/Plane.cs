using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship.Model
{
    internal enum PlaneType
    {
        Plane1,
        Plane2,
        Plane3
    };

    internal class Plane
    {

        private int _health;

        private readonly PlaneType _type;

        private static readonly Dictionary<PlaneType, int> planeLengths =
            new Dictionary<PlaneType, int>()
            {
                {PlaneType.Plane1, 4},
                {PlaneType.Plane2, 5},
                {PlaneType.Plane3, 5}
            };

        public Plane(PlaneType type)
        {
            _type = type;
            Reincarnate();
        }

        public void Reincarnate()
        {
            _health = planeLengths[_type];
        }

        public int Length
        {
            get { return planeLengths[_type]; }
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
