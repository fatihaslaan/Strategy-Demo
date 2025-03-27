using UnityEngine;
using StrategyDemo.Tile_NS;
using System.Collections.Generic;
using StrategyDemo.Navigation_NS;

namespace StrategyDemo.GameBoard_NS
{
    public class TileCalculator
    {
        private readonly Dictionary<(int xCoordinate, int yCoordinate), Tile> _tiles;
        public TileCalculator(Dictionary<(int xCoordinate, int yCoordinate), Tile> tiles)
        {
            _tiles = tiles;
        }


        public bool IsTilesAvailableToMove(List<(int xCoordinate, int yCoordinate)> coordinates)
        {
            foreach ((int xCoordinate, int yCoordinate) coordinate in coordinates)
            {
                if (!_tiles.ContainsKey(coordinate)) return false;

                if (_tiles[coordinate].isOccupied) return false;

                if (!_tiles[coordinate].TileCoordinate.tileData.IsMovable) return false;
            }

            return true;
        }

        public bool IsTileAvailableToMove((int xCoordinate, int yCoordinate) coordinate)
        {
            if (!_tiles.ContainsKey(coordinate)) return false;

            if (_tiles[coordinate].isOccupied) return false;

            if (!_tiles[coordinate].TileCoordinate.tileData.IsMovable) return false;

            return true;
        }

        //refactor
        public bool IsTilesAvailableToConstruct(List<(int xCoordinate, int yCoordinate)> coordinates)
        {
            foreach ((int xCoordinate, int yCoordinate) coordinate in coordinates)
            {
                if (!_tiles.ContainsKey(coordinate)) return false;

                if (_tiles[coordinate].isOccupied) return false;

                if (!_tiles[coordinate].TileCoordinate.tileData.IsConstructable) return false;
            }

            return true;
        }

        public bool IsTileAvailableToConstruct((int xCoordinate, int yCoordinate) coordinate)
        {
            if (!_tiles.ContainsKey(coordinate)) return false;

            if (_tiles[coordinate].isOccupied) return false;

            if (!_tiles[coordinate].TileCoordinate.tileData.IsConstructable) return false;

            return true;
        }

        public TileCoordinate GetTileCoordinate((int xCoordinate, int yCoordinate) coordinate)
        {
            return _tiles[coordinate].TileCoordinate;
        }

        //also get constructable neighbors or generatable?
        public List<(int x, int y)> GetMovableNeighbors(List<(int x, int y)> coordinates)
        {
            List<(int x, int y)> neighbors = new();
            foreach ((int x, int y) coordinate in coordinates)
            {
                foreach (Vector2Int n in GameBoardCellShape.Instance.neighborCoordinates)
                {
                    (int x, int y) temp = (coordinate.x + n.x, coordinate.y + n.y);
                    if (coordinates.Contains(temp) || neighbors.Contains(temp) || !IsTileAvailableToMove(temp)) continue;
                    neighbors.Add(temp);
                }
            }
            return neighbors;
        }

        public List<(int x, int y)> GetMovableNeighborsByDimension(Vector2Int dimension, (int x, int y) current)
        {
            List<(int x, int y)> neighbors = new();
            var t = GetCoordinatesByDimension(current, dimension);
            foreach ((int x, int y) coordinate in t)
            {
                foreach (Vector2Int n in GameBoardCellShape.Instance.neighborCoordinates)
                {
                    (int x, int y) temp = (current.x + n.x, current.y  + n.y);
                    if (t.Contains(temp) || neighbors.Contains(temp) || !IsTileAvailableToMove(temp)) continue;
                    neighbors.Add(temp);
                }
            }
            return neighbors;
        }



        public List<(int x, int y)> GetCoordinatesByDimension((int xCoordinate,int yCoordinate) coordinate, Vector2Int dimension) //Can't be minus using for creating entities
        {
            List<(int x, int y)> coordinates = new();

            for (int x = 0; x < dimension.x; x++)
            {
                for (int y = 0; y < dimension.y; y++)
                {
                    coordinates.Add((coordinate.xCoordinate + x, coordinate.yCoordinate + y));
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
    }
}