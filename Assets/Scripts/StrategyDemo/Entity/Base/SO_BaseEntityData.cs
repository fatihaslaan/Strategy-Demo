using UnityEngine;

namespace StrategyDemo.Entity_NS
{
    [CreateAssetMenu(fileName = "Base_Entity_Data", menuName = "ScriptableObjects/Entity/Base_Entity_Data")]
    public abstract class SO_BaseEntityData : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _icon;

        public string Name { get { return _name; } }
        public Sprite Icon { get { return _icon; } }

        public abstract void Use();
    }
}