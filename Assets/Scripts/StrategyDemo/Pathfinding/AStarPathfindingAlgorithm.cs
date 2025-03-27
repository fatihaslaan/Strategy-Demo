using StrategyDemo.GameBoard_NS;
using StrategyDemo.Navigation_NS;
using StrategyDemo.Tile_NS;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace StrategyDemo.PathFinding_NS
{
    public sealed class AStarPathfindingAlgorithm : IPathFindingAlgorithm
    {

        private TileCalculator _tileCalculator;

        public AStarPathfindingAlgorithm(TileCalculator tileCalculator)
        {
            _tileCalculator = tileCalculator;
        }

        public List<(int xCoordinate, int yCoordinate)> GetPath((int xCoordinate, int yCoordinate) start, (int xCoordinate, int yCoordinate) destination, Vector2Int dimension, bool getClose = false)
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

                if (getClose)
                {
                    //Get Close Location

                    List<(int xCoordinate, int yCoordinate)> movableNeighbors = _tileCalculator.GetMovableNeighborsByDimension(dimension, destination);
                    if (movableNeighbors.Count > 0)
                        if (movableNeighbors.Contains(currentTile))
                            return ReversePath(cameFrom, currentTile);
                }
                if (currentTile == destination)
                    return ReversePath(cameFrom, currentTile);
                calculatedTiles.Add(currentTile);

                foreach ((int xCoordinate, int yCoordinate) neighbor in _tileCalculator.GetMovableNeighborsByDimension(dimension, currentTile))
                {
                    if (calculatedTiles.Contains(neighbor)) continue;

                    int tempG = tilesDistanceData[currentTile].g_WalkingDistance + GameBoardCellShape.Instance.CalculateDistance(currentTile, neighbor);

                    if (!tilesDistanceData.ContainsKey(neighbor) || tempG < tilesDistanceData[neighbor].g_WalkingDistance)
                    {
                        cameFrom[neighbor] = currentTile;

                        TileDistanceData neighborData = new TileDistanceData(tempG, GameBoardCellShape.Instance.CalculateDistance(neighbor, destination));
                        tilesDistanceData[neighbor] = neighborData;

                        tilesToCalculate[neighborData.f_TotalDistance] = neighbor;
                    }
                }
            }
            Debug.Log("Path Doesn't Exist");
            //Path doesn't exist
            return null;
        }

        private List<(int xCoordinate, int yCoordinate)> ReversePath(Dictionary<(int xCoordinate, int yCoordinate), (int xCoordinate, int yCoordinate)> cameFrom, (int xCoordinate, int yCoordinate) current)
        {
            List<(int xCoordinate, int yCoordinate)> path = new() { current };

            while (cameFrom.ContainsKey(current))
            {
                path.Add(cameFrom[current]);
                current = cameFrom[current];
            }

            path.Reverse();
            return path;
        }
    }
}