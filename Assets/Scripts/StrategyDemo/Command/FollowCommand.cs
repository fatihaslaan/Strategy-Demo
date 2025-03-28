using StrategyDemo.Entity_NS;
using StrategyDemo.PathFinding_NS;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace StrategyDemo.Command_NS
{
    public class FollowCommand : MoveCommand
    {
        private BaseUnitEntityController _target;
        private IPathFindingAlgorithm _pathFinder;

        public FollowCommand(BaseUnitEntityController unit, BaseUnitEntityController target, Action<(int x, int y), BaseUnitEntityController> onNextStep, IPathFindingAlgorithm pathfinder)
            : base(unit, new List<(int x, int y)>(), onNextStep)
        {
            _target = target;
            _pathFinder = pathfinder;

            path = pathfinder.GetPath(unit.coordinates[0], target.coordinates[0], unit.GetDimension(), true); //pathfinder to chase our target
            _target.unitMoved += UpdatePath;
        }

        private void UpdatePath((int x, int y) newCoordinate)
        {
            if (!_target || !_target.gameObject.activeSelf) //If target available
            {
                Terminate();
                return;
            }
            path = _pathFinder.GetPath(unit.coordinates[0], newCoordinate, unit.GetDimension(), true);
        }

        public override void Terminate()
        {
            base.Terminate();
            if(_target)
            {
                _target.unitMoved -= UpdatePath;
                _target = null;
            }
        }

        public FollowCommand AddAttackCommand(SO_AttackAbilityData data) //We can also attack to our targer
        {
            if (data != null)
            {
                attackCommand = new AttackCommand(data, _target, unit);
            }
            else
            {
                attackCommand = null;
            }
            return this;
        }
    }
}