using StrategyDemo.Tile_NS;

namespace StrategyDemo.Navigation_NS
{
    [System.Serializable]
    public class TileCoordinate:Coordinate
    {
        public TileData tileData;

        public TileCoordinate(int x, int y, TileData tileData)
        {
            xCoorddinate = x;
            yCoordinate = y;
            this.tileData = tileData;
        }
    }
}