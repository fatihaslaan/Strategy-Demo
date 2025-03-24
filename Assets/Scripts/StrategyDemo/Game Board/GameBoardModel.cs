using UnityEngine;
using StrategyDemo.Tile_NS;
using System.Collections.Generic;
using StrategyDemo.Navigation_NS;

namespace StrategyDemo.GameBoard_NS
{
    public class GameBoardModel : MonoBehaviour
    {
        [SerializeField] private SO_GameBoardData _gameBoardData; // addressable load edicek load bitince eventi tetiklicek sonra controller gridleri yükle dicek viewa, onvalidate backend
        //Scriptable obje, ilerde kayıt edilebilmesi için anlık durum so


        /// <summary>
        /// Didn't use 2d array
        /// </summary>
        public Dictionary<(int xCoordinate, int yCoordinate), Tile> _tiles;

        public SO_GameBoardData GameBoardData {  get { return _gameBoardData; } }

        private void Start()
        {
            //SetTiles();
        }

        public void SetTiles(Dictionary<(int xCoordinate, int yCoordinate), Tile> tiles)
        {
            _tiles = tiles;
        }
    }
}