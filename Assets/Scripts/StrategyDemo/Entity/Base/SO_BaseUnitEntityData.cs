using StrategyDemo.GameBoard_NS;
using UnityEngine;

namespace StrategyDemo.Entity_NS
{
    [CreateAssetMenu(fileName = "Base_Unit_Entity_Data", menuName = "ScriptableObjects/Entity/Base_Unit_Entity_Data")]
    public class SO_BaseUnitEntityData : SO_BasePlaceableEntityData
    {
        [Min(1)][SerializeField] private int _moveSpeed = 1;
        public int MoveSpeed { get { return _moveSpeed; } }

        public override void Use()
        {
            GameBoardController.Instance.SpawnUnit(this);
        }
    }
}