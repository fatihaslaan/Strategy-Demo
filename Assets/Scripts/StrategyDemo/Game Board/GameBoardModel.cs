using UnityEngine;
using StrategyDemo.Tile_NS;
using System.Collections.Generic;
using StrategyDemo.Navigation_NS;
using StrategyDemo.Entity_NS;

namespace StrategyDemo.GameBoard_NS
{
    public class GameBoardModel : MonoBehaviour
    {
        [SerializeField] private SO_GameBoardData _gameBoardData; // addressable load edicek load bitince eventi tetiklicek sonra controller gridleri yükle dicek viewa, onvalidate backend
        //Scriptable obje, ilerde kayıt edilebilmesi için anlık durum so


        /// <summary>
        /// Didn't use 2d array
        /// </summary>
        private Dictionary<(int xCoordinate, int yCoordinate), Tile> _tiles;

        public SO_GameBoardData GameBoardData { get { return _gameBoardData; } }

        private void Start()
        {
            //SetTiles();
        }

        public void SetTiles(Dictionary<(int xCoordinate, int yCoordinate), Tile> tiles)
        {
            _tiles = tiles;
        }

        public void SetPlaceable(BasePlaceableEntityController placeableEntity)
        {
            foreach (var item in placeableEntity.coordinates)
            {
                _tiles[item].isOccupied = true;
            }
            placeableEntity.Place(GetMovableNeighbors(placeableEntity.coordinates));
            //Set So
            //Move view
        }

        public bool IsTilesAvailableToMove(List<(int xCoordinate, int yCoordinate)> coordinates)
        {
            foreach ((int xCoordinate, int yCoordinate) coordinate in coordinates)
            {
                if (!_tiles.ContainsKey(coordinate)) return false;

                if (_tiles[coordinate].isOccupied) return false;

                if (!_tiles[coordinate].TileCoordinate.tileData.IsMovable) return false;
            }

            return true;
        }

        public bool IsTileAvailableToMove((int xCoordinate, int yCoordinate) coordinate)
        {
            if (!_tiles.ContainsKey(coordinate)) return false;

            if (_tiles[coordinate].isOccupied) return false;

            if (!_tiles[coordinate].TileCoordinate.tileData.IsMovable) return false;

            return true;
        }

        //refactor
        public bool IsTilesAvailableToConstruct(List<(int xCoordinate, int yCoordinate)> coordinates)
        {
            foreach ((int xCoordinate, int yCoordinate) coordinate in coordinates)
            {
                if (!_tiles.ContainsKey(coordinate)) return false;

                if (_tiles[coordinate].isOccupied) return false;

                if (!_tiles[coordinate].TileCoordinate.tileData.IsConstructable) return false;
            }

            return true;
        }

        public bool IsTileAvailableToConstruct((int xCoordinate, int yCoordinate) coordinate)
        {
            if (!_tiles.ContainsKey(coordinate)) return false;

            if (_tiles[coordinate].isOccupied) return false;

            if (!_tiles[coordinate].TileCoordinate.tileData.IsConstructable) return false;

            return true;
        }

        public TileCoordinate GetTileCoordinate((int xCoordinate, int yCoordinate) coordinate)
        {
            return _tiles[coordinate].TileCoordinate;
        }

        //also get constructable neighbors or generatable?
        public List<(int x, int y)> GetMovableNeighbors(List<(int x, int y)> coordinates)
        {
            List<(int x, int y)> neighbors = new();
            foreach ((int x, int y) coordinate in coordinates)
            {
                foreach (Vector2Int n in GameBoardCellShape.Instance.neighborCoordinates)
                {
                    if (coordinates.Contains((coordinate.x + n.x, coordinate.y + n.y)) || neighbors.Contains((coordinate.x + n.x, coordinate.y + n.y)) || !IsTileAvailableToMove((coordinate.x + n.x, coordinate.y + n.y))) continue;
                    neighbors.Add((coordinate.x + n.x, coordinate.y + n.y));
                }
            }

            return neighbors;
        }
    }
}