namespace StrategyDemo.PathFinding_NS
{
    public struct TileDistanceData
    {
        public int f_TotalDistance;
        public int g_WalkingDistance;

        public TileDistanceData(int g, int h)
        {
            g_WalkingDistance = g;
            f_TotalDistance = g + h;
        }
    }
}