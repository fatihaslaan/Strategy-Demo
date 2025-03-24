using UnityEngine;

namespace StrategyDemo.Entity_NS
{
    [CreateAssetMenu(fileName = "Entity_Data", menuName = "ScriptableObjects/Entity_Data")]
    public class SO_EntityData : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _hp;
        [SerializeField] private BaseEntityController _entityPrefab;

        public string Name { get { return _name; } }
        public Sprite Icon { get { return _icon; } }
        public int HP { get { return _hp; } }
        public BaseEntityController EntityPrefab { get { return _entityPrefab; } }
    }
}