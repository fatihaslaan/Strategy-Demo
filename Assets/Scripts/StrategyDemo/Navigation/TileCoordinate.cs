using StrategyDemo.Tile_NS;

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
    }
}