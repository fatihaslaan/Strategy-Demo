using StrategyDemo.Entity_NS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private Image _itemImageUI;

    protected SO_BaseEntityData entityData;

    public void UpdateView(SO_BaseEntityData entityData)
    {
        if (entityData != null)
        {
            this.entityData = entityData;
            _itemName.text = entityData.Name;
            _itemImageUI.sprite = entityData.Icon;
        }
    }
}
