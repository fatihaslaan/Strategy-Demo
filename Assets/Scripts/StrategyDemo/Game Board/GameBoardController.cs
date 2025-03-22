using Base.Util;
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
            GameBoardData gameBoardData = _gameBoardModel.GameBoardData;
            Dictionary<TileCoordinate, Tile> tiles = new();
            foreach (TileCoordinate tileCoordinate in gameBoardData.GameBoardMapData.GetTileCoordinates())
            {
                Tile tile = _gameBoardView.InstantiateTile(gameBoardData.TilePrefab);
                tile.TileCoordinate = tileCoordinate;
                tiles.Add(tileCoordinate, tile);
            }
            return tiles;
        }

        private void OnValidate()
        {
            if (!_gameBoardModel)
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