using StrategyDemo.Command_NS;
using StrategyDemo.GameBoard_NS;
using StrategyDemo.Pooling_NS;
using System;
using UnityEngine;

namespace StrategyDemo.Entity_NS
{

    public class BaseUnitEntityController : BasePlaceableEntityController
    {
        private int _moveSpeed;

        public int MoveSpeed { get { return _moveSpeed; } }

        public ICommand currentCommand;

        public Action<(int x, int y)> unitMoved;

        public void SetEntity(SO_BaseUnitEntityData data)
        {
            base.SetEntity(data);
            _moveSpeed = data.MoveSpeed;
        }

        public new void ExecuteCommand(ICommand moveCommand)
        {
            if (currentCommand != null)
            {
                currentCommand.Terminate();
                currentCommand = null;
            }
            currentCommand = moveCommand;
            currentCommand.Execute();
        }

        public void Undo()
        {
            currentCommand.Undo();
            currentCommand = null;
        }

        public override void ReturnObject()
        {
            GameBoardController.Instance.ObjectDestroyed(this);
            _hpBar.localScale = Vector3.one;
            EntityObjectPooler.Instance.ReturnUnit(this);
        }
    }
}