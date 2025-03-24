using UnityEngine;

namespace Base.Item
{
    [CreateAssetMenu(fileName = "Base_Item_Data", menuName = "ScriptableObjects/Base_Item_Data")]
    public class SO_BaseItemData : ScriptableObject
    {
        [SerializeField] private string _itemName;
        [SerializeField] private Sprite _itemIcon;

        public string ItemName { get { return _itemName; } }
        public Sprite ItemIcon { get { return _itemIcon; } }
    }
}