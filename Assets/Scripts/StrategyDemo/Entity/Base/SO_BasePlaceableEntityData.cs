using StrategyDemo.Entity_NS;
using StrategyDemo.GameBoard_NS;
using System.Collections.Generic;
using UnityEngine;

namespace StrategyDemo.Entity_NS
{
    [CreateAssetMenu(fileName = "Base_Placeable_Entity_Data", menuName = "ScriptableObjects/Entity/Base_Placeable_Entity_Data")]
    public class SO_BasePlaceableEntityData : SO_BaseEntityData
    {
        [Min(1)][SerializeField] private int _hp = 1;
        [Min(1)][SerializeField] private Vector2Int _dimension = Vector2Int.one;
        [SerializeField] private BasePlaceableEntityController _entityPrefab;
        [SerializeField] private List<SO_BaseEntityAbilityData> _abilities;

        public int Hp { get { return _hp; } }
        public Vector2Int Dimension { get { return _dimension; } }
        public BasePlaceableEntityController EntityPrefab { get { return _entityPrefab; } }
        public List<SO_BaseEntityAbilityData> Abilities { get { return _abilities; } }

        public override void Use()
        {
            if (!GameBoardController.Instance.buildPlacing)
                GameBoardController.Instance.SetSelectedEntity(this);
        }
    }
}