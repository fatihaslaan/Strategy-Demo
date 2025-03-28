using DG.Tweening;
using StrategyDemo.Entity_NS;
using StrategyDemo.GameBoard_NS;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace StrategyDemo.Command_NS
{
    public class MoveCommand : ICommand
    {
        private event Action<(int x, int y), BaseUnitEntityController> _onNextStep;

        protected BaseUnitEntityController unit;
        protected Stack<(int x, int y)> executedSteps = new(); //For undo
        protected List<(int x, int y)> path;
        protected AttackCommand attackCommand;

        protected Action OnMove;

        private (int x, int y)? _nextPos;

        public MoveCommand(BaseUnitEntityController unit, List<(int x, int y)> path, Action<(int x, int y), BaseUnitEntityController> onNextStep)
        {
            this.unit = unit;
            this.path = new List<(int x, int y)>(path);
            _onNextStep = onNextStep;
        }

        public void Execute()
        {
            if (path == null || !(path.Count > 1)) return;
            MoveStep();

        }

        protected void MoveStep()
        {
            if (!(path.Count > 1))
            {
                Terminate();
                return;
            }

            path.RemoveAt(0);
            if (_nextPos != null && _nextPos == path[0])
            {
                MoveStep();
                return;
            }
            _nextPos = path[0];

            if (unit && unit.gameObject.activeSelf)
            {
                unit.transform.DOMove(GameBoardCellShape.Instance.GetTilePositionByCoordinate(new Vector3Int(_nextPos.Value.x, _nextPos.Value.y)), 1f / unit.MoveSpeed).OnComplete(() =>
                {
                    _onNextStep?.Invoke(_nextPos.Value, unit); //Move to next step and check if it is available via model
                    OnMove?.Invoke();
                    executedSteps.Push(_nextPos.Value);
                    Execute();
                });
            }
            else
            {
                Terminate();
            }
        }
        public MoveCommand AddAttackCommand(SO_AttackAbilityData data, BasePlaceableEntityController target)
        {
            if (data != null)
            {
                attackCommand = new AttackCommand(data, target, unit);
            }
            else
            {
                attackCommand = null;
            }
            return this;
        }

        public void Undo()
        {
            if (executedSteps.Count == 0) return;

            (int x, int y) lastPos = executedSteps.Pop();
            unit.transform.position = GameBoardCellShape.Instance.GetTilePositionByCoordinate(new Vector3Int(lastPos.x, lastPos.y));
            Terminate();
        }

        public virtual void Terminate()
        {
            if (path != null)
            {
                path.Clear();
            }

            if (attackCommand != null)
            {
                attackCommand.Terminate();
                attackCommand = null;
            }
        }
    }

}