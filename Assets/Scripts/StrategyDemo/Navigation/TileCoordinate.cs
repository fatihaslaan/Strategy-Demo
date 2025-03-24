using StrategyDemo.Tile_NS;
using System.Collections.Generic;

namespace StrategyDemo.Navigation_NS
{
    [System.Serializable]
    public class TileCoordinate : Coordinate
    {
        public SO_TileData tileData;

        public TileCoordinate(int x, int y, SO_TileData tileData) : base(x, y)
        {
            this.tileData = tileData;
        }

        public TileCoordinate(Coordinate coordinate, SO_TileData tileData) : base(coordinate.yCoordinate, coordinate.yCoordinate)
        {
            this.tileData = tileData;
        }

        public List<(int x_Coordinate, int y_Coordinate)> GetNeighbourCoordinates(List<(int xCoordinate, int yCoordinate)> tiles)
        {
            HashSet<(int x, int y)> neighborCoordinates = GetNeighbourCoordinates();

            return tiles.FindAll(tileCoordinate => neighborCoordinates.Contains((tileCoordinate.xCoordinate, tileCoordinate.yCoordinate)));
        }
    }
}