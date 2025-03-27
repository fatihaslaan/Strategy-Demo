using StrategyDemo.GameBoard_NS;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

            SortedSet<(int fCost, int x, int y)> tilesToCalculate = new();
            tilesToCalculate.Add((startData.f_TotalDistance, start.xCoordinate,start.yCoordinate));

            //Keep track of visited tiles (used hashset since we only add and check if it containes that tile)
            HashSet<(int xCoordinate, int yCoordinate)> calculatedTiles = new();
            while (tilesToCalculate.Count > 0)
            {
                var currentTuple = tilesToCalculate.Min;
                tilesToCalculate.Remove(currentTuple);
                (int xCoordinate, int yCoordinate) currentTile = (currentTuple.x, currentTuple.y);

                if (getClose)
                {
                    //Get Closer Location (Not exact location for following moving towards to structure)
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

                        tilesToCalculate.Add((neighborData.f_TotalDistance, neighbor.xCoordinate, neighbor.yCoordinate));
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