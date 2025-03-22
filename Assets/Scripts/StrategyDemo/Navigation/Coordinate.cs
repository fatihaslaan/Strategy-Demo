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

        public HashSet<(int x, int y)> GetNeighbourCoordinates()
        {
            HashSet<(int x, int y)> neighbors = new();
            Vector2Int[] neighborCoordinates =
            {
                new Vector2Int(-1,-1),
                new Vector2Int(1,-1),
                new Vector2Int(1,1),
                new Vector2Int(-1,1),
                new Vector2Int(-1,0),
                new Vector2Int(1,0),
                new Vector2Int(0,-1),
                new Vector2Int(0,1),
            };

            foreach (Vector2Int coordinate in neighborCoordinates)
            {
                neighbors.Add((xCoordinate + coordinate.x, yCoordinate + coordinate.y));
            }

            return neighbors;
        }

        public bool IsOffset()
        {
            return Mathf.Abs(xCoordinate) % 2 != Mathf.Abs(yCoordinate) % 2;
        }
    }
}