using Base.Util;
using StrategyDemo.Entity_NS;
using StrategyDemo.GameBoard_NS;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityButonView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private Image _itemImageUI;
    [SerializeField] private Button _button;

    private SO_BaseEntityData a; //Test refactor

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
        a.Use();
    }

    public void UpdateView(SO_BaseEntityData entityData)
    {
        if (entityData != null)
        {
            a = entityData;
            _itemName.text = entityData.Name;
            _itemImageUI.sprite = entityData.Icon;
        }
    }

    private void OnValidate()
    {
        if(!_itemName)
        {
            ObjectFinder.FindObjectInChilderenWithType(ref _itemName, transform);
        }
        if (!_button)
        {
            ObjectFinder.FindObjectInChilderenWithType(ref _button, transform);
        }
    }
}
