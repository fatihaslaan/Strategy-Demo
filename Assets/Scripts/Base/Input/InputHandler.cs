using Base.Core;

namespace Base.Input
{
    public sealed class InputHandler : PersistentSingleton<InputHandler>
    {
        /// <summary>
        /// Created as persistent singleton because we may be need that inputactions
        /// Created as sealed class since this class only creates&enables the inputactions at start and disables on quit so no need to inherit any further
        /// </summary>
        private InputActions _inputActions;
        public InputActions InputActions { get { return _inputActions; } }

        private void OnEnable()
        {
            _inputActions = new InputActions();
            _inputActions.Enable();
            _inputActions.Player.Enable();
            _inputActions.UI.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
            _inputActions.Player.Disable();
            _inputActions.UI.Disable();
        }
    }
}