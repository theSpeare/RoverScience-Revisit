using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoverScience
{
    public class Coord
    {
        public double longitude;
        public double latitude;

        public Coord (double _long, double _lat)
        {
            longitude = _long;
            latitude = _lat;
        }

        public void Set (double _long, double _lat)
        {
            longitude = _long;
            latitude = _lat;
        }
    }
}
