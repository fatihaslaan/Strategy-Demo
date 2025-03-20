using StrategyDemo.Navigation_NS;
using StrategyDemo.Tile_NS;
using System.Collections.Generic;
using UnityEngine;

namespace StrategyDemo.GameBoard_NS
{
    public class GameBoardView : MonoBehaviour
    {
        public Dictionary<TileCoordinate, Tile> CreateTiles(GameBoardData gameBoardData)
        {
            Dictionary<TileCoordinate,Tile> tiles = new ();
            foreach (TileCoordinate tileCoordinate in gameBoardData.GameBoardMapData.GetMapTiles())
            {
                Tile tile = Instantiate(gameBoardData.TilePrefab, transform);
                tile.SetTile(tileCoordinate, gameBoardData.BaseTileColor, gameBoardData.OffsetTileColor);
                tiles.Add(tileCoordinate, tile);
            }
            return tiles;
        }
    }
}