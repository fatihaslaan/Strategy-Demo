using Base.Util;
using StrategyDemo.GameBoard_NS;
using StrategyDemo.Navigation_NS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StrategyDemo.Entity_NS
{
    public class BasePlaceableEntityController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        private int _hp;

        private Vector2Int _dimension;

        [HideInInspector] public bool placeable;

        [HideInInspector] public SO_BasePlaceableEntityData _data;
        [HideInInspector] public SO_AttackAbilityData _attackAbility;
        [HideInInspector] public SO_ProduceAbilityData _produceAbility;

        [HideInInspector] public List<(int x, int y)> coordinates = new();
        [HideInInspector] public List<(int x, int y)> movableNeighbors = new ();


        public void UpdateView(bool constructable) // prop
        {
            placeable = constructable;
            _renderer.color = constructable ? Color.white : new Color(1,0,0,0.5f);
        }

        public void SetEntity(SO_BasePlaceableEntityData data)
        {
            _data = data;
            _hp = data.Hp;
            _dimension = data.Dimension;
            GetComponent<BoxCollider2D>().enabled = false;
        }

        public void Select(List<(int x, int y)> updatedMovableNeighbors)
        {
            movableNeighbors = updatedMovableNeighbors;
            //open ui
        }

        public void Place(List<(int x, int y)> updatedMovableNeighbors)
        {
            GetComponent<BoxCollider2D>().enabled = true;
            movableNeighbors = updatedMovableNeighbors;
            foreach (SO_BaseEntityAbilityData ability in _data.Abilities)
            {
                if (ability is SO_AttackAbilityData)
                {
                    _attackAbility = ability as SO_AttackAbilityData;
                }

                if (ability is SO_ProduceAbilityData)
                {
                    SetProduceAbility(ability as SO_ProduceAbilityData);
                }
            }
        }

        private void SetProduceAbility(SO_ProduceAbilityData produceAbility)
        {
            _produceAbility = produceAbility;
            if(produceAbility.Flag && produceAbility.Producables.Exists(p=> p is SO_BaseUnitEntityData))
            {
                if (movableNeighbors.Count > 0)
                {
                    GameObject flag = Instantiate(produceAbility.Flag);
                    flag.transform.SetParent(transform);
                    flag.transform.position = GameBoardCellShape.Instance.GetTilePositionByCoordinate(new Vector3Int(movableNeighbors[0].x, movableNeighbors[0].y, 0));
                    flag.gameObject.SetActive(false);
                }
            }
        }

        public void UpdatePosition(Coordinate coord)
        {
            transform.position = GameBoardCellShape.Instance.GetTilePositionByCoordinate(new Vector3Int(coord.xCoordinate, coord.yCoordinate, 0));

            coordinates = coord.GetCoordinatesByDimension(_dimension);
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