using UnityEngine;

namespace StrategyDemo.Tile_NS
{
    [CreateAssetMenu(fileName = "Tile_Data", menuName = "ScriptableObjects/Tile_Data")]
    public class TileData : ScriptableObject
    {
        [SerializeField] private TileType _tileType;
        [SerializeField] private Sprite _sprite;

        public TileType TileType { get { return _tileType; } }
        public Sprite Sprite { get { return _sprite; } }
    }
}