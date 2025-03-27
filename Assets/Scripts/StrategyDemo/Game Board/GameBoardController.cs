using Base.Util;
using UnityEngine;
using StrategyDemo.Tile_NS;
using StrategyDemo.Navigation_NS;
using System.Collections.Generic;
using Base.Core;
using StrategyDemo.Entity_NS;
using Base.UI;
using Base.Addressable;
using System.Linq;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace StrategyDemo.GameBoard_NS
{
    public class GameBoardController : Singleton<GameBoardController> // change
    {
        [SerializeField] private GameBoardModel _gameBoardModel;

        [SerializeField] private InfiniteScrollViewController _productableScrollViewController;
        [SerializeField] private InfiniteScrollViewController _itemInfoScrollViewController;

        [SerializeField] private EntityView _itemInfo;

        public BasePlaceableEntityController _selectedEntity;
        public BasePlaceableEntityController _chosen;
        public BasePlaceableEntityController _spawner;

        [HideInInspector] public bool buildPlacing;

        private List<AsyncOperationHandle> _asyncOperations = new();

        public void SetSelectedEntity(SO_BasePlaceableEntityData entity)
        {
            PlaceableFactory factory = new PlaceableFactory();
            _selectedEntity = factory.GetPlaceableEntity(entity);
            buildPlacing = true; // prop
            //Close UIS
        }

        public void ObjectDestroyed(BasePlaceableEntityController controller)
        {
            _gameBoardModel.RemovePlaceable(controller);
        }

        private void Start()
        {
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
            GameBoardCameraController.PointerCoordinateChanged += UpdateByPointerPosition;
        }

        private void OnDisable()
        {
            GameBoardCameraController.PointerCoordinateChanged -= UpdateByPointerPosition;
        }

        public void Clicked((int xCoordinate, int yCoordinate) coordinate)
        {

            if (_itemInfoScrollViewController.gameObject.activeSelf)
            {
                _itemInfoScrollViewController.gameObject.SetActive(false);
            }
            if (_chosen && _chosen.flag)
            {
                _chosen.flag.SetActive(false);
            }

            if (buildPlacing && _selectedEntity && _selectedEntity.placeable)
            {
                _gameBoardModel.SetPlaceable(_selectedEntity);
                buildPlacing = false;
                _selectedEntity = null;
            }
        }

        public void ClickedToEntity(BasePlaceableEntityController controller)
        {
            _chosen = controller;
            if (_chosen._produceAbility)
            {
                if (!_itemInfoScrollViewController.gameObject.activeSelf)
                {
                    _itemInfo.UpdateView(controller._data);
                    _itemInfoScrollViewController.gameObject.SetActive(true);
                    _spawner = _chosen;
                }
                if (_chosen.flag)
                {
                    _chosen.flag.SetActive(true);
                }
                _itemInfoScrollViewController.LoadScrollView(new List<SO_BaseEntityData>(controller._produceAbility.Producables));
            }
        }

        public void AttackOrMoveClick(BasePlaceableEntityController controller, (int xCoordinate, int yCoordinate) coordinate)
        {
            if (_chosen == null) return;
            if (controller == null)
            {
                if (_chosen is BaseUnitEntityController)
                {
                    _gameBoardModel.MoveUnit(_chosen as BaseUnitEntityController, coordinate);
                    //Move To Empty Space
                }
                else if (_chosen._produceAbility && _chosen._produceAbility.Flag)
                {
                    _chosen.flag.transform.position = GameBoardCellShape.Instance.GetTilePositionByCoordinate(new Vector3Int(coordinate.xCoordinate, coordinate.yCoordinate));
                    _chosen.defaultPosition = coordinate;
                }
            }
            else
            {
                if (_chosen is BaseUnitEntityController)
                {
                    _gameBoardModel.Attack(_chosen as BaseUnitEntityController, controller);
                    //Follow And Attack
                }
                else if (_chosen._attackAbility || _chosen._produceAbility && _chosen._produceAbility.Flag)
                {
                    _chosen.flag.transform.position = GameBoardCellShape.Instance.GetTilePositionByCoordinate(new Vector3Int(coordinate.xCoordinate, coordinate.yCoordinate));
                    _chosen.defaultPosition = coordinate;
                    //Not Developed But Buildings Can Attack if in range
                    //If we listen other units we can also auto attack
                }
            }
        }

        public void SpawnUnit(SO_BaseUnitEntityData unit)
        {
            _gameBoardModel.SpawnUnit(unit, _spawner);
        }

        private void UpdateByPointerPosition((int xCoordinate, int yCoordinate) coordinate)
        {
            if (buildPlacing && _selectedEntity)
            {
                _gameBoardModel.UpdateByPointerPosition(coordinate, _selectedEntity);
            }
        }

        private void OnValidate()
        {
            if (!_gameBoardModel)
            {
                ObjectFinder.FindObjectInChilderenWithType(ref _gameBoardModel, transform);
            }
        }
    }
}