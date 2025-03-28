using UnityEngine;

namespace StrategyDemo.Entity_NS
{
    [CreateAssetMenu(fileName = "Attack_Ability_Data", menuName = "ScriptableObjects/Entity/Ability/Attack_Ability_Data")]
    public class SO_AttackAbilityData : SO_BaseEntityAbilityData
    {
        [Min(1)] [SerializeField] private int _attackRange = 1;
        [Min(1)] [SerializeField] private int _attackPower = 1;
        [Min(1)] [SerializeField] private int _attackRate = 1;

        public int AttackRange { get { return _attackRange; } }
        public int AttackPower { get { return _attackPower; } }
        public int AttackRate { get { return _attackRate; } }

        public override void InitAbility()
        {

        }
    }
}