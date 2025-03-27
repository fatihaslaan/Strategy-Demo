using StrategyDemo.Entity_NS;
using System.Collections;
using UnityEngine;

namespace StrategyDemo.Command_NS
{
    public class AttackCommand : ICommand
    {
        private int _range;
        private int _damage;
        private int _rate;
        private BasePlaceableEntityController _target;
        private BasePlaceableEntityController _unit;

        public int currentRange = int.MaxValue;

        public AttackCommand(SO_AttackAbilityData data, BasePlaceableEntityController target, BaseUnitEntityController unit)
        {
            _range = data.AttackRange;
            _damage = data.AttackPower;
            _rate = data.AttackRate;
            _target = target;
            _unit = unit;
            _unit.StartCoroutine(Attack());
        }

        public void Execute()
        {

        }

        IEnumerator Attack()
        {
            while (_target && _target.gameObject.activeSelf && _unit && _unit.gameObject.activeSelf)
            {
                Debug.Log("Current Range " + currentRange + " --- " + _range);
                if(currentRange <= _range)
                {
                    _target.RecieveDamage(_damage);
                }
                yield return new WaitForSeconds(1f / _rate);
            }
            Terminate();
        }

        public void Undo()
        {

        }

        public void Terminate()
        {
            Debug.Log("Terminated");
            _unit.StopCoroutine(Attack());
            _target = null;
        }
    }
}