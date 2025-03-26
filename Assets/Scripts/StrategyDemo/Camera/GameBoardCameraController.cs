using Base.Input;
using StrategyDemo.Entity_NS;
using StrategyDemo.Navigation_NS;
using StrategyDemo.Tile_NS;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StrategyDemo.GameBoard_NS
{
    public sealed class GameBoardCameraController : InputController
    {
        public static event Action<(int xCoordinate, int yCoordinate)> PointerCoordinate;

        private Camera _camera;
        private GameBoardController _controller;

        private void Start()
        {
            _camera = Camera.main;
            _controller = GameBoardController.Instance;
        }

        protected override void OnPointerHeld()
        {

        }

        protected override void OnPointerClicked()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(GetPointerPosition());
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null)
            {
                if(hit.collider.TryGetComponent(out BasePlaceableEntityController clickedPlaceable))
                {
                    _controller.ClickedToEntity(clickedPlaceable);
                    return;
                }
            }

            _controller.Clicked(GetPointerCoordinate());
        }

        protected override void OnMoveAndAttackPointerClicked()
        {

        }

        protected override void Update()
        {
            if (_controller.buildPlacing)
            {

                PointerCoordinate?.Invoke(GetPointerCoordinate());
            }
            base.Update();
        }

        private (int x, int y) GetPointerCoordinate()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(GetPointerPosition());

            Vector3Int coordinate = GameBoardCellShape.Instance.GetTileCoordinateByPointerPosition(mousePosition);
            return (coordinate.x, coordinate.y);
        }
    }
}