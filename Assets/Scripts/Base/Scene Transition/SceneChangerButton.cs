using Base.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Base.SceneTransition
{
    public class SceneChangerButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private SceneNames _sceneName;

        private void Awake()
        {
            _button.onClick.AddListener(OnChangeSceneButtonPressed);
        }

        private void OnChangeSceneButtonPressed()
        {
            SceneTransitionManager.Instance.ChangeScene(_sceneName);
        }

        private void OnValidate()
        {
            if(!_button)
            {
                ObjectFinder.FindObjectInChilderenWithType(ref _button, transform);
            }
        }
    }
}