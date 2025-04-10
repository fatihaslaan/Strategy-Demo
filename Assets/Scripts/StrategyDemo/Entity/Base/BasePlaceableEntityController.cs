﻿using Base.Util;
using StrategyDemo.Command_NS;
using StrategyDemo.GameBoard_NS;
using StrategyDemo.Navigation_NS;
using StrategyDemo.Pooling_NS;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace StrategyDemo.Entity_NS
{
    public class BasePlaceableEntityController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;

        [SerializeField] protected Transform _hpBar;
        private int _hp;

        private Vector2Int _dimension;

        [HideInInspector] public bool placeable;

        [HideInInspector] public SO_BasePlaceableEntityData data;
        [HideInInspector] public SO_AttackAbilityData attackAbility;
        [HideInInspector] public SO_ProduceAbilityData produceAbility;

        [HideInInspector] public List<(int x, int y)> coordinates = new();
        [HideInInspector] public List<(int x, int y)> movableNeighbors = new();

        [HideInInspector] public (int x, int y)? defaultPosition = null;
        [HideInInspector] public GameObject flag;

        public event Action<BasePlaceableEntityController> OnDestruction;

        private ICommand _currentCommand;

        public void RecieveDamage(int damage)
        {
            _hp -= damage;
            _hpBar.localScale = new Vector3((float)_hp / data.Hp, 1,1);
            if(_hp<=0)
            {
                OnDestruction?.Invoke(this);
                ReturnObject();
            }
        }
        public void ExecuteCommand(ICommand command) //If recieved a command while completing previous terminate previous command
        {
            if (_currentCommand != null)
            {
                _currentCommand.Terminate();
                _currentCommand = null;
            }
            _currentCommand = command;
            _currentCommand.Execute();
        }

        public void TerminateCommand()
        {
            if(_currentCommand != null)
            {
                _currentCommand.Terminate();
            }
        }

        public void Undo()
        {
            if(_currentCommand != null)
            {
                _currentCommand.Undo();
                _currentCommand = null;
            }
        }

        public void UpdateView(bool constructable) // prop
        {
            placeable = constructable;
            _renderer.color = constructable ? Color.white : new Color(1, 0, 0, 0.5f);
        }

        public virtual void ReturnObject()
        {
            GameBoardController.Instance.ObjectDestroyed(this);
            _hpBar.localScale = Vector3.one;
            EntityObjectPooler.Instance.ReturnBuilding(this);
        }

        public void SetEntity(SO_BasePlaceableEntityData data)
        {
            _renderer.sprite = data.Icon;
            _dimension = data.Dimension;
            transform.localScale = new Vector3(_dimension.x, _dimension.y,1);
            this.data = data;
            attackAbility = null;
            produceAbility = null;
            _hp = data.Hp;
            GetComponent<BoxCollider2D>().enabled = false;
        }

        public Vector2Int GetDimension()
        {
            return _dimension;
        }

        public void Select(List<(int x, int y)> updatedMovableNeighbors)
        {
            movableNeighbors = updatedMovableNeighbors;
        }

        public void Place(List<(int x, int y)> updatedMovableNeighbors)
        {
            GetComponent<BoxCollider2D>().enabled = true;
            movableNeighbors = updatedMovableNeighbors;
            //Set Abilities
            foreach (SO_BaseEntityAbilityData ability in data.Abilities)
            {
                if (ability is SO_AttackAbilityData)
                {
                    attackAbility = ability as SO_AttackAbilityData;
                }

                if (ability is SO_ProduceAbilityData)
                {
                    SetProduceAbility(ability as SO_ProduceAbilityData);
                }
            }
        }

        private void SetProduceAbility(SO_ProduceAbilityData produceAbility)
        {
            this.produceAbility = produceAbility;
            if (produceAbility.Flag && produceAbility.Producables.Exists(p => p is SO_BaseUnitEntityData))
            {
                if (movableNeighbors.Count > 0)
                {
                    flag = Instantiate(produceAbility.Flag);
                    flag.transform.SetParent(transform);
                    flag.transform.position = GameBoardCellShape.Instance.GetTilePositionByCoordinate(new Vector3Int(movableNeighbors[0].x, movableNeighbors[0].y, 0));
                    flag.gameObject.SetActive(false);
                    defaultPosition = movableNeighbors[0];
                }
            }
        }

        public void UpdatePosition(Coordinate coord)
        {
            transform.position = GameBoardCellShape.Instance.GetTilePositionByCoordinate(new Vector3Int(coord.xCoordinate, coord.yCoordinate, 0));
        }

        private void OnValidate()
        {
            if (!_renderer)
            {
                ObjectFinder.FindObjectInChilderenWithType(ref _renderer, transform);
            }
        }
    }
}