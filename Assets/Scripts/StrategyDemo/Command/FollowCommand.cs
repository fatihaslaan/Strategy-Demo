using StrategyDemo.Entity_NS;
using StrategyDemo.PathFinding_NS;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace StrategyDemo.Command_NS
{
    public class FollowCommand : MoveCommand
    {
        private BaseUnitEntityController _unitToFollow;
        private IPathFindingAlgorithm _pathFinder;

        public FollowCommand(BaseUnitEntityController unit, BaseUnitEntityController unitToFollow, Action<(int x, int y), BaseUnitEntityController> onNextStep, IPathFindingAlgorithm pathfinder)
            : base(unit, new List<(int x, int y)>(), onNextStep)
        {
            _unitToFollow = unitToFollow;
            _pathFinder = pathfinder;

            path = pathfinder.GetPath(unit.coordinates[0], unitToFollow.coordinates[0], unit.GetDimension(), true);
            _unitToFollow.unitMoved += UpdatePath;
        }

        private void UpdatePath((int x, int y) newCoordinate)
        {
            if (!_unitToFollow || !_unitToFollow.gameObject.activeSelf)
            {
                _unitToFollow.unitMoved -= UpdatePath;
                Terminate();
                return;
            }
            path = _pathFinder.GetPath(unit.coordinates[0], newCoordinate, unit.GetDimension(), true);
            if ((path.Count>1))
            {
                Execute();
            }
        }

        public FollowCommand AddAttackCommand(SO_AttackAbilityData data)
        {
            if (data != null)
            {
                attackCommand = new AttackCommand(data, _unitToFollow, unit);
            }
            else
            {
                attackCommand = null;
            }
            return this;
        }
    }
}