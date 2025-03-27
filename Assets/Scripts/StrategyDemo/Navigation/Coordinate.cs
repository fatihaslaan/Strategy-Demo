using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace StrategyDemo.Navigation_NS
{
    [System.Serializable]
    public class Coordinate
    {
        public int xCoordinate;
        public int yCoordinate;

        public Coordinate(int x, int y)
        {
            xCoordinate = x;
            yCoordinate = y;
        }

        public bool IsOffset()
        {
            return Mathf.Abs(xCoordinate) % 2 != Mathf.Abs(yCoordinate) % 2;
        }
    }
}