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

        public List<(int x, int y)> GetCoordinatesByDimension(Vector2Int dimension) //Can't be minus using for creating entities
        {
            List<(int x, int y)> coordinates = new();

            for (int x = 0; x < dimension.x; x++)
            {
                for (int y = 0; y < dimension.y; y++)
                {
                    coordinates.Add((xCoordinate + x, yCoordinate + y));
                }
            }

            return coordinates;
        }

        //logic change
        public List<(int x, int y)> GetCoordinatesByAddingDimension(Vector2Int startCoord, Vector2Int dimension) //Can be used for effects of entites by specific direction
        {
            List<(int x, int y)> coordinates = new();

            for (int x = 0; x < Mathf.Abs(dimension.x); x++)
            {
                for (int y = 0; y < Mathf.Abs(dimension.y); y++)
                {
                    coordinates.Add((startCoord.x + ((dimension.x > 0) ? x : -x), startCoord.y + ((dimension.y > 0) ? y : -y)));
                }
            }

            return coordinates;
        }

        public HashSet<(int x, int y)> GetNeighbourCoordinates()
        {
            HashSet<(int x, int y)> neighbors = new();


            foreach (Vector2Int coordinate in GameBoardCellShape.Instance.neighborCoordinates)
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