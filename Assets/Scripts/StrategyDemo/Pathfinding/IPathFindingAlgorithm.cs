using StrategyDemo.Navigation_NS;
using System.Collections.Generic;
using UnityEngine;

namespace StrategyDemo.PathFinding_NS
{
    public interface IPathFindingAlgorithm
    {
        public List<(int xCoordinate, int yCoordinate)> GetPath((int xCoordinate, int yCoordinate) start, (int xCoordinate, int yCoordinate) destination, Vector2Int dimension, bool getClose = false);
    }
}