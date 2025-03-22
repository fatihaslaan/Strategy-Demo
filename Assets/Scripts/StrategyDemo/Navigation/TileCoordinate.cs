using StrategyDemo.Tile_NS;
using System.Collections.Generic;

namespace StrategyDemo.Navigation_NS
{
    [System.Serializable]
    public class TileCoordinate : Coordinate
    {
        public TileData tileData;

        public TileCoordinate(int x, int y, TileData tileData) : base(x, y)
        {
            this.tileData = tileData;
        }

        public TileCoordinate(Coordinate coordinate, TileData tileData) : base(coordinate.yCoordinate, coordinate.yCoordinate)
        {
            this.tileData = tileData;
        }

        public List<TileCoordinate> GetNeighbourCoordinates(List<TileCoordinate> tiles)
        {
            HashSet<(int x, int y)> neighborCoordinates = GetNeighbourCoordinates();

            return tiles.FindAll(tileCoordinate => neighborCoordinates.Contains((tileCoordinate.xCoordinate, tileCoordinate.yCoordinate)));
        }
    }
}