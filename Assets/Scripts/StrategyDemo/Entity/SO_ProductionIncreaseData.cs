using UnityEngine;

namespace StrategyDemo.Entity_NS
{
    [CreateAssetMenu(fileName = "Production_Increase_Data", menuName = "ScriptableObjects/Entity/Production_Increase_Data")]
    public class SO_ProductionIncreaseData : SO_BaseEntityData
    {
        public override void Use()
        {
            Debug.Log("Production Increased");
        }
    }
}