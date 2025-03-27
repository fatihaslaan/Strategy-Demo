using Base.Input;
using StrategyDemo.Entity_NS;
using StrategyDemo.Navigation_NS;
using StrategyDemo.Tile_NS;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

namespace StrategyDemo.GameBoard_NS
{
    public sealed class GameBoardCameraController : InputController
    {
        public static event Action<(int xCoordinate, int yCoordinate)> PointerCoordinateChanged;

        private GameBoardController _controller;

        private Camera _camera;
        private float _decreaseCameraMovementSpeed = 50f;
        private Vector2 _cameraMovementPointerPosition = Vector2.zero;

        private void Start()
        {
            _camera = Camera.main;
            _controller = GameBoardController.Instance;
        }

        protected override void OnPointerHeld()
        {
            MoveCamera();
        }

        protected override void OnPointerClicked()
        {
            _controller.Clicked(GetPointerCoordinate());

            Vector2 mousePosition = _camera.ScreenToWorldPoint(GetPointerPosition());
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out BasePlaceableEntityController clickedPlaceable))
                {
                    _controller.ClickedToEntity(clickedPlaceable);
                }
            }

        }

        protected override void OnMoveAndAttackPointerClicked()
        {
            Vector2 mousePosition = _camera.ScreenToWorldPoint(GetPointerPosition());
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            BasePlaceableEntityController controller = null;
            if (hit.collider != null)
            {
                controller = hit.collider.GetComponent<BasePlaceableEntityController>();
            }
            _controller.AttackOrMoveClick(controller, GetPointerCoordinate());
        }

        protected override void Update()
        {
            if (_controller.buildPlacing)
            {

                PointerCoordinateChanged?.Invoke(GetPointerCoordinate());
            }
            base.Update();
        }

        private (int x, int y) GetPointerCoordinate()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(GetPointerPosition());

            Vector3Int coordinate = GameBoardCellShape.Instance.GetTileCoordinateByPointerPosition(mousePosition);
            return (coordinate.x, coordinate.y);
        }

        protected override void OnPointerDown()
        {
            _cameraMovementPointerPosition = GetPointerPosition();
        }

        protected override void OnPointerUp()
        {

        }

        private void MoveCamera()
        {
            Vector2 differenceBetweenTouchPositions = (_cameraMovementPointerPosition - GetPointerPosition());
            if (_cameraMovementPointerPosition != Vector2.zero)
            {
                _camera.transform.position += new Vector3(differenceBetweenTouchPositions.x, differenceBetweenTouchPositions.y, 0) / _decreaseCameraMovementSpeed;
            }
            ClampCamera();
            _cameraMovementPointerPosition = GetPointerPosition();
        }

        private void ClampCamera()
        {
            // Get the camera's current position
            Vector3 currentPosition = _camera.transform.position;

            // Clamp the x and y coordinates within the boundaries
            float clampedX = Mathf.Clamp(currentPosition.x, GameBoardCellShape.Instance.minXPosition, GameBoardCellShape.Instance.maxXPosition);
            float clampedY = Mathf.Clamp(currentPosition.y, GameBoardCellShape.Instance.minYPosition, GameBoardCellShape.Instance.maxYPosition);

            // Set the camera's position with the clamped values (retain the z-axis position)
            _camera.transform.position = new Vector3(clampedX, clampedY, currentPosition.z);
        }
    }
}