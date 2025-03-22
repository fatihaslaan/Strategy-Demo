using StrategyDemo.Navigation_NS;
using System.Collections.Generic;

namespace StrategyDemo.PathFinding_NS
{
    public interface IPathFindingAlgorithm<TCoordinate> where TCoordinate : Coordinate
    {
        public List<TCoordinate> GetPath(TCoordinate startingCoordinate, TCoordinate destinationCoordinate);
    }
}