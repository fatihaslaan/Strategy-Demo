using UnityEngine;
using UnityEngine.InputSystem;

namespace Base.Input
{
    public abstract class InputController : MonoBehaviour
    {
        protected InputActions _inputActions;
        protected bool _isPointerDown = false;

        private Vector2 _pointerDownPosition;
        private Vector2 _moveAndAttackPointerDownPosition;

        protected virtual void Awake()
        {
            _inputActions = InputHandler.Instance.InputActions;
        }

        protected virtual void OnEnable()
        {
            _inputActions.Player.Drag.performed += OnDragperformed;
            _inputActions.Player.Drag.canceled += OnDragCancelled;
            _inputActions.Player.MoveAndAttack.performed += OnMoveAndAttackPointerDown;
            _inputActions.Player.MoveAndAttack.canceled += OnMoveAndAttackPointerUp;
        }

        protected virtual void OnDisable()
        {
            _inputActions.Player.Drag.performed -= OnDragperformed;
            _inputActions.Player.Drag.canceled -= OnDragCancelled;
            _inputActions.Player.MoveAndAttack.performed -= OnMoveAndAttackPointerDown;
            _inputActions.Player.MoveAndAttack.canceled -= OnMoveAndAttackPointerUp;
        }

        private void OnDragperformed(InputAction.CallbackContext obj)
        {
            _pointerDownPosition = GetPointerPosition();
            _isPointerDown = true;
        }

        private void OnDragCancelled(InputAction.CallbackContext obj)
        {
            if (Vector2.Distance(_pointerDownPosition, GetPointerPosition()) < ((Screen.width + Screen.height) / 2) / 50)
            {
                OnPointerClicked();
            }
            _isPointerDown = false;
        }

        protected virtual void Update()
        {
            if (_isPointerDown)
            {
                OnPointerHeld();
            }
        }

        protected Vector2 GetPointerPosition()
        {
            return _inputActions.Player.PointerPosition.ReadValue<Vector2>();
        }

        protected abstract void OnPointerClicked();

        protected abstract void OnPointerHeld();

        protected abstract void OnMoveAndAttackPointerClicked();

        protected void OnMoveAndAttackPointerDown(InputAction.CallbackContext obj)
        {
            _moveAndAttackPointerDownPosition = GetPointerPosition();
        }
        protected void OnMoveAndAttackPointerUp(InputAction.CallbackContext obj)
        {
            if (Vector2.Distance(_moveAndAttackPointerDownPosition, GetPointerPosition()) < ((Screen.width + Screen.height) / 2) / 100)
            {
                OnMoveAndAttackPointerClicked();
            }
        }
    }
}