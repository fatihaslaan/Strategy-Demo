using StrategyDemo.Navigation_NS;
using System.Collections.Generic;
using UnityEngine;

namespace StrategyDemo.GameBoard_NS
{
    public abstract class GameBoardBaseMapData : ScriptableObject 
    {
        /*
         * We may have different kinds of maps in the future,
         * we can add rocky tiles, non rectangular maps
         * so we need  anabstract base map data class to get list of
         * tiles that created with different algorithms
         */
        public abstract List<TileCoordinate> GetMapTiles();
    }
}