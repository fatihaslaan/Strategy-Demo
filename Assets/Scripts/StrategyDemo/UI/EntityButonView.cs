using Base.Item;
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

    private BaseEntityController a; //Test refactor

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
        GameBoardController.Instance.SetSelectedEntity(a);
    }

    public void UpdateView(SO_EntityData entityData)
    {
        if (entityData != null)
        {
            a = entityData.EntityPrefab;
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
        if(!_itemImageUI)
        {
            ObjectFinder.FindObjectInChilderenWithType(ref _itemImageUI, transform);
        }
        if (!_button)
        {
            ObjectFinder.FindObjectInChilderenWithType(ref _button, transform);
        }
    }
}
