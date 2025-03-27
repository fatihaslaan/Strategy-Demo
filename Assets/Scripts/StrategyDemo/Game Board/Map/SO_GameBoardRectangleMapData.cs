using StrategyDemo.Navigation_NS;
using StrategyDemo.Tile_NS;
using System.Collections.Generic;
using UnityEngine;

namespace StrategyDemo.GameBoard_NS
{
    [CreateAssetMenu(fileName = "Game_Board_Rectangle_Map_Data", menuName = "ScriptableObjects/GameBoard/Game_Board_Rectangle_Map_Data")]
    public class SO_GameBoardRectangleMapData : SO_GameBoardBaseMapData //Rectangle Map Creation Data
    {
        [SerializeField] private SO_TileData _tileData;
        [Min(8)][SerializeField] private int _boardWidth = 8;
        [Min(8)][SerializeField] private int _boardHeight = 8;

        protected override List<TileCoordinate> CreateNewMapTileCoordinates()
        {
            for (int x = 0; x < _boardWidth; x++)
            {
                for (int y = 0; y < _boardHeight; y++)
                {
                    //To centralize tiles around 0,0 we subtracted size / 2 (And of course we can handle coordinates lesser than 0)
                    tileCoordinates.Add(new TileCoordinate(x - (_boardWidth / 2), y - (_boardHeight / 2), _tileData));
                }
            }
            return tileCoordinates;
        }
    }
}