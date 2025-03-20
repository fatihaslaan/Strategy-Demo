using StrategyDemo.Tile_NS;

namespace StrategyDemo.Navigation_NS
{
    [System.Serializable]
    public class TileCoordinate:Coordinate
    {
        public TileType tileType;

        public TileCoordinate(int x, int y, TileType tileType)
        {
            xCoorddinate = x;
            yCoordinate = y;
            this.tileType = tileType;
        }
    }
}