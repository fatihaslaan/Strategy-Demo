using Base.Core;
using Base.Util;
using UnityEngine;

namespace StrategyDemo.GameBoard_NS
{
    public class GameBoardCellShape : Singleton<GameBoardCellShape>
    {
        /// <summary>
        /// Everything about our map, cell chapes and calculations about it
        /// </summary>
        [SerializeField] private Grid _grid;
        private const int STRAIGHT_MOVE_COST = 10;
        private const int DIAGONAL_MOVE_COST = 14;

        public Vector2Int[] neighborCoordinates =
        {
        new Vector2Int(0,-1),
        new Vector2Int(0,1),
        new Vector2Int(-1,0),
        new Vector2Int(1,0),
        //Diagonel Neighbors
        new Vector2Int(1,-1),
        new Vector2Int(-1,1),
        new Vector2Int(-1,-1),
        new Vector2Int(1,1),
    };

        //Map Borders
        public float minXPosition;
        public float maxXPosition;
        public float minYPosition;
        public float maxYPosition;

        public Vector3Int GetTileCoordinateByPointerPosition(Vector2 pointerPos)
        {
            return _grid.WorldToCell(pointerPos);
        }

        public Vector3 GetTilePositionByCoordinate(Vector3Int tileCoordinate)
        {
            Vector3 tilePosition = _grid.CellToWorld(tileCoordinate);
            if (minXPosition > tilePosition.x)
                minXPosition = tilePosition.x;
            else if (maxXPosition < tilePosition.x)
                maxXPosition = tilePosition.x;

            if (minYPosition > tilePosition.y)
                minYPosition = tilePosition.y;
            else if (maxYPosition < tilePosition.y)
                maxYPosition = tilePosition.y;

            return tilePosition;
        }

        public int CalculateTileDistance((int xCoordinate, int yCoordinate) start, (int xCoordinate, int yCoordinate) destination)
        {
            //Diagonel distance
            int x_Difference = Mathf.Abs(start.xCoordinate - destination.xCoordinate);
            int y_Difference = Mathf.Abs(start.yCoordinate - destination.yCoordinate);

            //Horizantal/Vertical Distance
            int remaining = Mathf.Abs(x_Difference - y_Difference);
            return Mathf.Min(x_Difference, y_Difference) + remaining;
        }

        public int CalculateDistance((int xCoordinate, int yCoordinate) start, (int xCoordinate, int yCoordinate) destination)
        {
            //Diagonel distance
            int x_Difference = Mathf.Abs(start.xCoordinate - destination.xCoordinate);
            int y_Difference = Mathf.Abs(start.yCoordinate - destination.yCoordinate);

            //Horizantal/Vertical Distance
            int remaining = Mathf.Abs(x_Difference - y_Difference);
            return (DIAGONAL_MOVE_COST * Mathf.Min(x_Difference, y_Difference)) + (STRAIGHT_MOVE_COST * remaining);
        }

        private void OnValidate()
        {
            if (!_grid)
            {
                ObjectFinder.FindObjectInChilderenWithType(ref _grid, transform);
            }
        }
    }
}