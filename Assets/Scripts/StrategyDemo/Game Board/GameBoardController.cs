using Base.Util;
using UnityEngine;
using StrategyDemo.Tile_NS;
using StrategyDemo.Navigation_NS;
using System.Collections.Generic;
using Base.Core;
using StrategyDemo.Entity_NS;
using System;
using Base.UI;
using Base.Addressable;
using System.Linq;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEngine.EventSystems.EventTrigger;


namespace StrategyDemo.GameBoard_NS
{
    public class GameBoardController : Singleton<GameBoardController> // change
    {
        [SerializeField] private GameBoardModel _gameBoardModel;
        [SerializeField] private GameBoardView _gameBoardView;

        [SerializeField] private InfiniteScrollViewController _productableScrollViewController;
        [SerializeField] private InfiniteScrollViewController _itemInfoScrollViewController;

        public BasePlaceableEntityController _selectedEntity;
        public BasePlaceableEntityController _chosen;



        [HideInInspector] public bool buildPlacing;

        private List<AsyncOperationHandle> _asyncOperations = new();

        public void SetSelectedEntity(SO_BasePlaceableEntityData entity)
        {
            _selectedEntity = Instantiate(entity.EntityPrefab, new Vector3(0, 0, -1), Quaternion.identity);
            _selectedEntity.SetEntity(entity);
            buildPlacing = true;
            //Close UIS
        }

        private void Start()
        {
            _gameBoardModel.SetTiles(CreateTilesInGameBoardView());
            _asyncOperations.Add(AddressableManager.LoadAddressableAssetsAsync<SO_BaseEntityData>(AddressableLabelNames.Productables, ProductableItemsLoaded));
            void ProductableItemsLoaded(IList<SO_BaseEntityData> list)
            {
                _productableScrollViewController.LoadScrollView(list.ToList());
            }
        }

        private void OnDestroy()
        {
            AddressableManager.ReleaseAssets(_asyncOperations);
        }

        private void OnEnable()
        {
            GameBoardCameraController.PointerCoordinate += UpdateByPointerPosition;
        }

        private void OnDisable()
        {
            GameBoardCameraController.PointerCoordinate -= UpdateByPointerPosition;
        }

        public void Clicked((int xCoordinate, int yCoordinate) coordinate)
        {
            //Close ui
            if(_selectedEntity && _selectedEntity.placeable)
            {
                _gameBoardModel.SetPlaceable(_selectedEntity);
                buildPlacing = false;
                _selectedEntity = null;
            }
        }

        public void ClickedToEntity(BasePlaceableEntityController controller)
        {
            if(controller._produceAbility)
            {
                _chosen = controller;
                _itemInfoScrollViewController.LoadScrollView(controller._produceAbility.Producables);
            }
        }

        public void SpawnUnit(SO_BaseUnitEntityData unit)
        {
            _chosen.Select(_gameBoardModel.GetMovableNeighbors(_chosen.coordinates));
            if (_chosen.movableNeighbors.Count > 0)
            {
                BasePlaceableEntityController unitController = Instantiate(unit.EntityPrefab, new Vector3(0, 0, -1), Quaternion.identity);
                unitController.SetEntity(unit);
                foreach (var item in _chosen.movableNeighbors)
                {
                    unitController.UpdatePosition(_gameBoardModel.GetTileCoordinate(item));
                    if(_gameBoardModel.IsTilesAvailableToMove(unitController.coordinates))
                    {
                        _gameBoardModel.SetPlaceable(unitController);
                        return;
                    }
                }
                Destroy(unitController.gameObject);
            }
            

        }

        private void UpdateByPointerPosition((int xCoordinate, int yCoordinate) coordinate)
        {
            if (_selectedEntity)
            {
                if(!_gameBoardModel.IsTileAvailableToConstruct(coordinate)) return;
                _selectedEntity.UpdatePosition(_gameBoardModel.GetTileCoordinate(coordinate));

                if (!_gameBoardModel.IsTilesAvailableToConstruct(_selectedEntity.coordinates))
                {
                    _selectedEntity.UpdateView(false);
                    return;
                }
                _selectedEntity.UpdateView(true);
            }
        }

        private Dictionary<(int xCoordinate, int yCoordinate), Tile> CreateTilesInGameBoardView()
        {
            SO_GameBoardData gameBoardData = _gameBoardModel.GameBoardData;
            Dictionary<(int xCoordinate, int yCoordinate), Tile> tiles = new();
            foreach (TileCoordinate tileCoordinate in gameBoardData.GameBoardMapData.GetTileCoordinates())
            {
                Tile tile = _gameBoardView.InstantiateTile(gameBoardData.TilePrefab);
                tile.TileCoordinate = tileCoordinate;
                tiles.Add((tileCoordinate.xCoordinate, tileCoordinate.yCoordinate), tile);
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