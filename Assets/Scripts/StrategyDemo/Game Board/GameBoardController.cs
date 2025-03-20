using Base.Util;
using System;
using UnityEngine;
using StrategyDemo.Tile_NS;
using StrategyDemo.Navigation_NS;
using System.Collections.Generic;


namespace StrategyDemo.GameBoard_NS
{
    public class GameBoardController : MonoBehaviour
    {
        [SerializeField] private GameBoardModel _gameBoardModel;
        [SerializeField] private GameBoardView _gameBoardView;

        private void Start()
        {
            _gameBoardModel.SetTiles(CreateTilesInGameBoardView());
        }

        private Dictionary<TileCoordinate, Tile> CreateTilesInGameBoardView()
        {
            return _gameBoardView.CreateTiles(_gameBoardModel.GameBoardData);
        }

        private void OnValidate()
        {
            if(!_gameBoardModel)
            {
                ObjectFinder.FindObjectInChilderenWithType(ref _gameBoardModel, transform);
            }
            if (!_gameBoardView)
            {
                ObjectFinder.FindObjectInChilderenWithType(ref _gameBoardView, transform);
            }
        }
    }
}