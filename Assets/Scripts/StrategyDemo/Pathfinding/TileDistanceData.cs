namespace StrategyDemo.PathFinding_NS
{
    public struct TileDistanceData
    {
        public int g_WalkingDistance;
        public int h_DestinationDistance;
        public int f_TotalDistance;

        public TileDistanceData(int g, int h)
        {
            g_WalkingDistance = g; 
            h_DestinationDistance = h;
            f_TotalDistance = g + h;
        }
    }
}