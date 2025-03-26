using System.Collections.Generic;
using UnityEngine;

namespace StrategyDemo.Entity_NS
{
    [CreateAssetMenu(fileName = "Produce_Ability_Data", menuName = "ScriptableObjects/Entity/Ability/Produce_Ability_Data")]
    public class SO_ProduceAbilityData : SO_BaseEntityAbilityData
    {
        [SerializeField] private GameObject _flag;

        [SerializeField] private List<SO_BaseEntityData> _producables;

        public GameObject Flag {  get { return _flag; } }
        public List<SO_BaseEntityData> Producables { get { return _producables; } }

        public override void InitAbility()
        {

        }
    }
}