using StrategyDemo.Navigation_NS;
using System.Collections.Generic;
using UnityEngine;

namespace StrategyDemo.GameBoard_NS
{
    public abstract class SO_GameBoardBaseMapData : ScriptableObject
    {
        public List<TileCoordinate> tileCoordinates = new();
        /*
        * We may have different kinds of maps in the future,
        * We may get this maps via backend (Since TileCoordinate is Serializable we can get json data and convert it to list, although we must first getresponse and turn it to SO because, Or we can get id and download it via addressables this func isn't async)
        * we can add rocky tiles, non rectangular maps
        * so we need  an abstract base map data class to get list of
        * tiles that created with different algorithms
        */

        public List<TileCoordinate> GetTileCoordinates()
        {
            return (tileCoordinates.Count > 0) ? tileCoordinates : CreateNewMapTileCoordinates();
        }

        protected abstract List<TileCoordinate> CreateNewMapTileCoordinates();
    }
}