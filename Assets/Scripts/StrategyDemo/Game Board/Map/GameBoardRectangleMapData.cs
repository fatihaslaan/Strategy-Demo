using StrategyDemo.Navigation_NS;
using StrategyDemo.Tile_NS;
using System.Collections.Generic;
using UnityEngine;

namespace StrategyDemo.GameBoard_NS
{
    [CreateAssetMenu(fileName = "Game_Board_Rectangle_Map_Data", menuName = "ScriptableObjects/Game_Board_Rectangle_Map_Data")]
    public class GameBoardRectangleMapData : GameBoardBaseMapData
    {
        [SerializeField] private TileData _tileData;
        [Min(10)][SerializeField] private int _boardWidth = 10;
        [Min(10)][SerializeField] private int _boardHeight = 10;
        public override List<TileCoordinate> GetMapTiles()
        {
            List<TileCoordinate> mapTiles = new();
            for (int x = 0; x < _boardWidth; x++)
            {
                for (int y = 0; y < _boardHeight; y++)
                {
                    //To centralize tiles around 0,0 we subtracted size / 2
                    mapTiles.Add(new TileCoordinate(x - (_boardWidth / 2), y - (_boardHeight / 2), _tileData));
                }
            }
            return mapTiles;
        }
    }
}