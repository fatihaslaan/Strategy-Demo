using Base.Util;
using UnityEngine;
using System.Collections.Generic;
using Base.Core;
using StrategyDemo.Entity_NS;
using Base.Addressable;
using System.Linq;
using UnityEngine.ResourceManagement.AsyncOperations;
using StrategyDemo.UI_NS;
using StrategyDemo.Factory_NS;


namespace StrategyDemo.GameBoard_NS
{
    public class GameBoardController : Singleton<GameBoardController>
    {
        [SerializeField] private GameBoardModel _gameBoardModel;

        [SerializeField] private InfiniteScrollViewController _productableScrollViewController;
        [SerializeField] private InfiniteScrollViewController _itemInfoScrollViewController;
        [SerializeField] private EntityView _itemInfo;

        public BasePlaceableEntityController _selectedEntity;

        public BasePlaceableEntityController _currentConstruction;
        public BasePlaceableEntityController _selectedSpawner;

        [HideInInspector] public bool buildPlacing;

        private List<AsyncOperationHandle> _asyncOperations = new();


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
            GameBoardCameraController.PointerClicked += Clicked;
            GameBoardCameraController.EntitySelected += ClickedToEntity;
            GameBoardCameraController.TargetSelected += AttackOrMoveClick;
        }

        private void OnDisable()
        {
            GameBoardCameraController.PointerCoordinateChanged -= UpdateByPointerPosition;
            GameBoardCameraController.PointerClicked -= Clicked;
            GameBoardCameraController.EntitySelected -= ClickedToEntity;
            GameBoardCameraController.TargetSelected -= AttackOrMoveClick;
        }
        public void SetSelectedEntity(SO_BasePlaceableEntityData entity)
        {
            PlaceableFactory factory = new PlaceableFactory();
            _currentConstruction = factory.GetPlaceableEntity(entity);
            buildPlacing = true; // prop
        }

        public void ObjectDestroyed(BasePlaceableEntityController controller)
        {
            _gameBoardModel.RemovePlaceable(controller);
        }

        public void Clicked((int xCoordinate, int yCoordinate) coordinate)
        {
            EmptyClick();
            TryToBuildSelecrtedStructure();
            void EmptyClick()
            {
                if (_itemInfoScrollViewController.gameObject.activeSelf)
                {
                    _itemInfoScrollViewController.gameObject.SetActive(false);
                }
                if (_selectedEntity && _selectedEntity.flag)
                {
                    _selectedEntity.flag.SetActive(false);
                }
            }
            void TryToBuildSelecrtedStructure()
            {
                if (buildPlacing && _currentConstruction && _currentConstruction.placeable)
                {
                    _gameBoardModel.SetPlaceable(_currentConstruction);
                    buildPlacing = false;
                    _currentConstruction = null;
                }
            }
        }

        public void ClickedToEntity(BasePlaceableEntityController controller)
        {
            _selectedEntity = controller;
            CheckProduceAbilityOfSelectedEntity();
            void CheckProduceAbilityOfSelectedEntity()
            {
                if (_selectedEntity._produceAbility)
                {
                    if (!_itemInfoScrollViewController.gameObject.activeSelf)
                    {
                        _itemInfo.UpdateView(controller._data);
                        _itemInfoScrollViewController.gameObject.SetActive(true);
                        _selectedSpawner = _selectedEntity;
                    }
                    if (_selectedEntity.flag)
                    {
                        _selectedEntity.flag.SetActive(true);
                    }
                    _itemInfoScrollViewController.LoadScrollView(new List<SO_BaseEntityData>(controller._produceAbility.Producables));
                }
            }
        }

        public void AttackOrMoveClick(BasePlaceableEntityController controller, (int xCoordinate, int yCoordinate) coordinate)
        {
            if (_selectedEntity == null || _selectedEntity == controller) return;
            if (controller == null)
            {
                if (_selectedEntity is BaseUnitEntityController)
                {
                    _gameBoardModel.MoveUnit(_selectedEntity as BaseUnitEntityController, coordinate);
                    //Move To Empty Space
                }
                else if (_selectedEntity._produceAbility && _selectedEntity._produceAbility.Flag)
                {
                    _selectedEntity.flag.transform.position = GameBoardCellShape.Instance.GetTilePositionByCoordinate(new Vector3Int(coordinate.xCoordinate, coordinate.yCoordinate));
                    _selectedEntity.defaultPosition = coordinate;
                }
            }
            else
            {
                if (_selectedEntity is BaseUnitEntityController)
                {
                    _gameBoardModel.Attack(_selectedEntity as BaseUnitEntityController, controller);
                    //Follow And Attack
                }
                else if (_selectedEntity._attackAbility || _selectedEntity._produceAbility && _selectedEntity._produceAbility.Flag)
                {
                    _selectedEntity.flag.transform.position = GameBoardCellShape.Instance.GetTilePositionByCoordinate(new Vector3Int(coordinate.xCoordinate, coordinate.yCoordinate));
                    _selectedEntity.defaultPosition = coordinate;
                    //Not Developed But Buildings Can Attack if in range
                    //If we listen other units we can also auto attack
                }
            }
        }

        public void SpawnUnit(SO_BaseUnitEntityData unit)
        {
            _gameBoardModel.SpawnUnit(unit, _selectedSpawner);
        }

        private void UpdateByPointerPosition((int xCoordinate, int yCoordinate) coordinate)
        {
            if (buildPlacing && _currentConstruction)
            {
                _gameBoardModel.UpdateByPointerPosition(coordinate, _currentConstruction);
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