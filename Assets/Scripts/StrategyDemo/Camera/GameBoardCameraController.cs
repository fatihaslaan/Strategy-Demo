using Base.Input;
using StrategyDemo.Entity_NS;
using System;
using UnityEngine;

namespace StrategyDemo.GameBoard_NS
{
    public sealed class GameBoardCameraController : InputController
    {
        public static event Action<(int xCoordinate, int yCoordinate)> PointerCoordinateChanged;
        public static event Action<(int xCoordinate, int yCoordinate)> PointerClicked;
        public static event Action<BasePlaceableEntityController> EntitySelected;
        public static event Action<BasePlaceableEntityController, (int xCoordinate, int yCoordinate)> TargetSelected;

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

        protected override void OnPointerClicked() //Left Click
        {
            PointerClicked?.Invoke(GetPointerCoordinate());

            Vector2 mousePosition = _camera.ScreenToWorldPoint(GetPointerPosition());
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out BasePlaceableEntityController clickedPlaceable))
                {
                    EntitySelected?.Invoke(clickedPlaceable);
                }
            }
        }

        protected override void OnMoveAndAttackPointerClicked() //Right Click
        {
            Vector2 mousePosition = _camera.ScreenToWorldPoint(GetPointerPosition());
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            BasePlaceableEntityController controller = null;
            if (hit.collider != null)
            {
                controller = hit.collider.GetComponent<BasePlaceableEntityController>();
            }
            TargetSelected?.Invoke(controller, GetPointerCoordinate());
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

        private void ClampCamera() //Clamp camera between map borders
        {
            Vector3 currentPosition = _camera.transform.position;

            float clampedX = Mathf.Clamp(currentPosition.x, GameBoardCellShape.Instance.minXPosition, GameBoardCellShape.Instance.maxXPosition);
            float clampedY = Mathf.Clamp(currentPosition.y, GameBoardCellShape.Instance.minYPosition, GameBoardCellShape.Instance.maxYPosition);

            _camera.transform.position = new Vector3(clampedX, clampedY, currentPosition.z);
        }
    }
}