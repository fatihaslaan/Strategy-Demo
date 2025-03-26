using StrategyDemo.Navigation_NS;
using System.Collections.Generic;

namespace StrategyDemo.PathFinding_NS
{
    public interface IPathFindingAlgorithm
    {
        public List<(int xCoordinate, int yCoordinate)> GetPath((int xCoordinate, int yCoordinate) startingCoordinate, (int xCoordinate, int yCoordinate) destinationCoordinate);
    }
}