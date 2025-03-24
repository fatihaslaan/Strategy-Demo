using StrategyDemo.Navigation_NS;
using StrategyDemo.Tile_NS;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StrategyDemo.PathFinding_NS
{
    public sealed class AStarPathfindingAlgorithm : IPathFindingAlgorithm<TileCoordinate>
    {
        private const int STRAIGHT_MOVE_COST = 10;
        private const int DIAGONAL_MOVE_COST = 14;

        private Dictionary<TileCoordinate, Tile> _tiles;

        public AStarPathfindingAlgorithm(Dictionary<TileCoordinate, Tile> tiles)
        {
            _tiles = tiles;
        }

        public List<TileCoordinate> GetPath(TileCoordinate start, TileCoordinate destination)
        {
            //Dictionary to track the path that we move
            Dictionary<TileCoordinate, TileCoordinate> cameFrom = new();

            //Dictionary that holds distance data for each tile
            Dictionary<TileCoordinate, TileDistanceData> tilesDistanceData = new();

            TileDistanceData startData = new TileDistanceData(0, CalculateDistance(start, destination));
            tilesDistanceData[start] = startData;

            SortedDictionary<int, TileCoordinate> tilesToCalculate = new();
            tilesToCalculate.Add(startData.f_TotalDistance, start);

            //Keep track of visited tiles (used hashset since we only add and check if it containes that tile)
            HashSet<TileCoordinate> calculatedTiles = new();
            while (tilesToCalculate.Count > 0)
            {
                TileCoordinate currentTile = tilesToCalculate.First().Value;
                tilesToCalculate.Remove(tilesToCalculate.First().Key);

                if (currentTile == destination)
                    return ReversePath(cameFrom, currentTile);

                calculatedTiles.Add(currentTile);

                foreach (TileCoordinate neighbor in currentTile.GetNeighbourCoordinates(_tiles.Keys.ToList()))
                {
                    if (calculatedTiles.Contains(neighbor)) continue;

                    //Check if the tile is occupied or not movable.
                    if (_tiles[neighbor].isOccupied || !neighbor.tileData.IsMovable)
                        continue;

                    int tempG = tilesDistanceData[currentTile].g_WalkingDistance + CalculateDistance(currentTile, neighbor);

                    if (!tilesDistanceData.ContainsKey(neighbor) || tempG < tilesDistanceData[neighbor].g_WalkingDistance)
                    {
                        cameFrom[neighbor] = currentTile;

                        TileDistanceData neighborData = new TileDistanceData(tempG, CalculateDistance(neighbor, destination));
                        tilesDistanceData[neighbor] = neighborData;

                        if (!tilesToCalculate.Values.Contains(neighbor))
                            tilesToCalculate.Add(neighborData.f_TotalDistance, neighbor);
                    }
                }
            }

            //Path doesn't exist
            return null;
        }

        private int CalculateDistance(TileCoordinate start, TileCoordinate destination)
        {
            //Diagonel distance
            int x_Difference = Mathf.Abs(start.xCoordinate - destination.xCoordinate);
            int y_Difference = Mathf.Abs(start.yCoordinate - destination.yCoordinate);

            //Horizantal/Vertical Distance
            int remaining = Mathf.Abs(x_Difference - y_Difference);
            return (DIAGONAL_MOVE_COST * Mathf.Min(x_Difference, y_Difference)) + (STRAIGHT_MOVE_COST * remaining);
        }

        private List<TileCoordinate> ReversePath(Dictionary<TileCoordinate, TileCoordinate> cameFrom, TileCoordinate current)
        {
            List<TileCoordinate> path = new() { current };

            while (cameFrom.ContainsKey(current))
            {
                path.Add(cameFrom[current]);
            }

            path.Reverse();
            return path;
        }
    }
}