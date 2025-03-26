using StrategyDemo.Navigation_NS;
using StrategyDemo.Tile_NS;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StrategyDemo.PathFinding_NS
{
    public sealed class AStarPathfindingAlgorithm : IPathFindingAlgorithm
    {

        private Dictionary<(int xCoordinate, int yCoordinate), Tile> _tiles;

        public AStarPathfindingAlgorithm(Dictionary<(int xCoordinate, int yCoordinate), Tile> tiles)
        {
            _tiles = tiles;
        }

        public List<(int xCoordinate, int yCoordinate)> GetPath((int xCoordinate, int yCoordinate) start, (int xCoordinate, int yCoordinate) destination)
        {
            //Dictionary to track the path that we move
            Dictionary<(int xCoordinate, int yCoordinate), (int xCoordinate, int yCoordinate)> cameFrom = new();

            //Dictionary that holds distance data for each tile
            Dictionary<(int xCoordinate, int yCoordinate), TileDistanceData> tilesDistanceData = new();

            TileDistanceData startData = new TileDistanceData(0, GameBoardCellShape.Instance.CalculateDistance(start, destination));
            tilesDistanceData[start] = startData;

            SortedDictionary<int, (int xCoordinate, int yCoordinate)> tilesToCalculate = new();
            tilesToCalculate.Add(startData.f_TotalDistance, start);

            //Keep track of visited tiles (used hashset since we only add and check if it containes that tile)
            HashSet<(int xCoordinate, int yCoordinate)> calculatedTiles = new();
            while (tilesToCalculate.Count > 0)
            {
                (int xCoordinate, int yCoordinate) currentTile = tilesToCalculate.First().Value;
                tilesToCalculate.Remove(tilesToCalculate.First().Key);

                if (currentTile == destination)
                    return ReversePath(cameFrom, currentTile);

                calculatedTiles.Add(currentTile);

                foreach ((int xCoordinate, int yCoordinate) neighbor in _tiles[currentTile].TileCoordinate.GetNeighbourCoordinates(_tiles.Keys.ToList()))
                {
                    if (calculatedTiles.Contains(neighbor)) continue;

                    //Check if the tile is occupied or not movable.
                    if (_tiles[neighbor].isOccupied || !_tiles[neighbor].TileCoordinate.tileData.IsMovable)
                        continue;

                    int tempG = tilesDistanceData[currentTile].g_WalkingDistance + GameBoardCellShape.Instance.CalculateDistance(currentTile, neighbor);

                    if (!tilesDistanceData.ContainsKey(neighbor) || tempG < tilesDistanceData[neighbor].g_WalkingDistance)
                    {
                        cameFrom[neighbor] = currentTile;

                        TileDistanceData neighborData = new TileDistanceData(tempG, GameBoardCellShape.Instance.CalculateDistance(neighbor, destination));
                        tilesDistanceData[neighbor] = neighborData;

                        if (!tilesToCalculate.Values.Contains(neighbor))
                            tilesToCalculate.Add(neighborData.f_TotalDistance, neighbor);
                    }
                }
            }

            //Path doesn't exist
            return null;
        }

        private List<(int xCoordinate, int yCoordinate)> ReversePath(Dictionary<(int xCoordinate, int yCoordinate), (int xCoordinate, int yCoordinate)> cameFrom, (int xCoordinate, int yCoordinate) current)
        {
            List<(int xCoordinate, int yCoordinate)> path = new() { current };

            while (cameFrom.ContainsKey(current))
            {
                path.Add(cameFrom[current]);
            }

            path.Reverse();
            return path;
        }
    }
}