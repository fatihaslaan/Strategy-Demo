using UnityEngine;
using StrategyDemo.Tile_NS;
using System.Collections.Generic;
using StrategyDemo.Entity_NS;
using StrategyDemo.PathFinding_NS;
using StrategyDemo.Navigation_NS;
using StrategyDemo.Command_NS;

namespace StrategyDemo.GameBoard_NS
{
    public class GameBoardModel : MonoBehaviour
    {
        [SerializeField] private SO_GameBoardData _gameBoardData;
        [SerializeField] private GameBoardView _gameBoardView;

        /// <summary>
        /// Didn't use 2d array
        /// </summary>
        private Dictionary<(int xCoordinate, int yCoordinate), Tile> _tiles;

        public SO_GameBoardData GameBoardData { get { return _gameBoardData; } }

        private TileCalculator _tileCalculator;
        private AStarPathfindingAlgorithm _pathFinder;
        private PlaceableFactory _placeableFactory;

        private void Start()
        {
            SetTiles(CreateTilesInGameBoardView());
        }

        private Dictionary<(int xCoordinate, int yCoordinate), Tile> CreateTilesInGameBoardView()
        {
            Dictionary<(int xCoordinate, int yCoordinate), Tile> tiles = new();
            foreach (TileCoordinate tileCoordinate in _gameBoardData.GameBoardMapData.GetTileCoordinates())
            {
                Tile tile = _gameBoardView.InstantiateTile(_gameBoardData.TilePrefab);
                tile.TileCoordinate = tileCoordinate;
                tiles.Add((tileCoordinate.xCoordinate, tileCoordinate.yCoordinate), tile);
            }
            return tiles;
        }

        public void SetTiles(Dictionary<(int xCoordinate, int yCoordinate), Tile> tiles)
        {
            _tiles = tiles;
            _tileCalculator = new TileCalculator(_tiles);
            _pathFinder = new AStarPathfindingAlgorithm(_tileCalculator);
            _placeableFactory = new();
        }
        public void RemovePlaceable(BasePlaceableEntityController placeableEntity)
        {
            foreach (var item in placeableEntity.coordinates)
            {
                _tiles[item].isOccupied = false;
            }
        }
        public void SetPlaceable(BasePlaceableEntityController placeableEntity)
        {
            foreach (var item in placeableEntity.coordinates)
            {
                _tiles[item].isOccupied = true;
            }
            placeableEntity.Place(_tileCalculator.GetMovableNeighbors(placeableEntity.coordinates));
            //Set So
            //Move view
        }

        public void MoveUnit(BaseUnitEntityController unit, (int xCoordinate, int yCoordinate) destination)
        {
            List<(int xCoordinate, int yCoordinate)> path = _pathFinder.GetPath(unit.coordinates[0], destination, unit.GetDimension());
            if (path != null)
            {
                unit.ExecuteCommand(new MoveCommand(unit, new List<(int x, int y)>(path), UpdateMovingUnit));
            }
        }

        public void Attack(BaseUnitEntityController unit, BasePlaceableEntityController target)
        {
            //Could Check For Teams
            if (target is BaseUnitEntityController)
            {
                unit.ExecuteCommand(new FollowCommand(unit, (BaseUnitEntityController)target, UpdateMovingUnit, _pathFinder).AddAttackCommand(unit._attackAbility));
            }
            else
            {
                List<(int xCoordinate, int yCoordinate)> path = _pathFinder.GetPath(unit.coordinates[0], target.coordinates[0], unit.GetDimension(), true);
                if (path != null)
                {
                    unit.ExecuteCommand(new MoveCommand(unit, new List<(int x, int y)>(path), UpdateMovingUnit).AddAttackCommand(unit._attackAbility, target));
                }
            }
        }

        public void SpawnUnit(SO_BaseUnitEntityData unit, BasePlaceableEntityController spawner)
        {
            //refact
            spawner.movableNeighbors = _tileCalculator.GetMovableNeighbors(spawner.coordinates);

            if (spawner.movableNeighbors.Count > 0)
            {
                BaseUnitEntityController unitController = _placeableFactory.GetPlaceableEntity(unit) as BaseUnitEntityController;
                unitController.UpdatePosition(_tileCalculator.GetTileCoordinate(spawner.movableNeighbors[0]));
                unitController.coordinates = _tileCalculator.GetCoordinatesByDimension(spawner.movableNeighbors[0], unitController.GetDimension());
                SetPlaceable(unitController);
                if (spawner.defaultPosition != null)
                {
                    List<(int xCoordinate, int yCoordinate)> path = _pathFinder.GetPath(unitController.coordinates[0], (spawner.defaultPosition.Value), unitController.GetDimension(), true);
                    if (path != null)
                    {
                        unitController.ExecuteCommand(new MoveCommand(unitController, new List<(int x, int y)>(path), UpdateMovingUnit));
                    }
                }
            }
        }

        public void UpdateMovingUnit((int xCoordinate, int yCoordinate) coordinate, BaseUnitEntityController movingObject)
        {
            if (!_tileCalculator.IsTileAvailableToMove(coordinate))
            {
                movingObject.Undo();
                return;
            }

            RemovePlaceable(movingObject);
            movingObject.coordinates = _tileCalculator.GetCoordinatesByDimension(coordinate, movingObject.GetDimension());
            if (_tileCalculator.IsTilesAvailableToMove(movingObject.coordinates))
            {
                SetPlaceable(movingObject);
                movingObject.unitMoved?.Invoke(coordinate);
                return;
            }
            movingObject.ReturnObject();
        }

        public void UpdateByPointerPosition((int xCoordinate, int yCoordinate) coordinate, BasePlaceableEntityController movingObject)
        {
            if (!_tileCalculator.IsTileAvailableToConstruct(coordinate)) return;
            movingObject.UpdatePosition(_tileCalculator.GetTileCoordinate(coordinate));
            movingObject.coordinates = _tileCalculator.GetCoordinatesByDimension(coordinate, movingObject.GetDimension());
            if (!_tileCalculator.IsTilesAvailableToConstruct(movingObject.coordinates))
            {
                movingObject.UpdateView(false);
                return;
            }
            movingObject.UpdateView(true);
        }
    }
}