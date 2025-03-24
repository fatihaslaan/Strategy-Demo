using StrategyDemo.Navigation_NS;
using StrategyDemo.Tile_NS;
using System.Collections.Generic;
using UnityEngine;

namespace StrategyDemo.GameBoard_NS
{
    [CreateAssetMenu(fileName = "Game_Board_Rectangle_Map_Data", menuName = "ScriptableObjects/Game_Board_Rectangle_Map_Data")]
    public class SO_GameBoardRectangleMapData : SO_GameBoardBaseMapData
    {
        [SerializeField] private SO_TileData _tileData;
        [Min(10)][SerializeField] private int _boardWidth = 10;
        [Min(10)][SerializeField] private int _boardHeight = 10;

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