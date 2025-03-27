using Base.Util;
using StrategyDemo.Entity_NS;
using StrategyDemo.GameBoard_NS;
using System;
using UnityEngine;
using UnityEngine.UI;

public class EntityButonView : EntityView
{
    [SerializeField] private Button _button;

    public RectTransform rect;

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
