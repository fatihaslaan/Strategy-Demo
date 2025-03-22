using UnityEngine;
using UnityEngine.InputSystem;

namespace Base.Input
{
    public abstract class InputController : MonoBehaviour
    {
        protected InputActions _inputActions;
        protected bool _isPointerDown = false;

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
            OnPointerDown();
            _isPointerDown = true;
        }

        private void OnDragCancelled(InputAction.CallbackContext obj)
        {
            OnPointerUp();
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

        protected abstract void OnPointerUp();

        protected abstract void OnPointerDown();

        protected abstract void OnPointerHeld();

        protected abstract void OnMoveAndAttackPointerDown(InputAction.CallbackContext obj);
        protected abstract void OnMoveAndAttackPointerUp(InputAction.CallbackContext obj);
    }
}