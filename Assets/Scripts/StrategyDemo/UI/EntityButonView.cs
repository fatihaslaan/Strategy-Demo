using Base.Util;
using UnityEngine;
using UnityEngine.UI;

namespace StrategyDemo.UI_NS
{
    public class EntityButonView : EntityView
    {
        /// <summary>
        /// Button view for items
        /// </summary>
        [SerializeField] private Button _button;

        [HideInInspector] public RectTransform rect;

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonPressed);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonPressed);
        }

        private void OnButtonPressed()
        {
            entityData.Use();
        }

        private void OnValidate()
        {
            if (!_button)
            {
                ObjectFinder.FindObjectInChilderenWithType(ref _button, transform);
            }
        }
    }
}