using UnityEngine;
using StrategyDemo.Tile_NS;
using System.Collections.Generic;
using StrategyDemo.Navigation_NS;

namespace StrategyDemo.GameBoard_NS
{
    public class GameBoardModel : MonoBehaviour
    {
        [SerializeField] private GameBoardData _gameBoardData; // addressable load edicek load bitince eventi tetiklicek sonra controller gridleri yükle dicek viewa, onvalidate backend
        //Scriptable obje, ilerde kayıt edilebilmesi için anlık durum so

        private Dictionary<TileCoordinate, Tile> _tiles;

        public GameBoardData GameBoardData {  get { return _gameBoardData; } }

        public void SetTiles(Dictionary<TileCoordinate, Tile> tiles)
        {
            _tiles = tiles;
        }
    }
}