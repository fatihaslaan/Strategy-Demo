using UnityEngine;

namespace StrategyDemo.Tile_NS
{
    [CreateAssetMenu(fileName = "Tile_Data", menuName = "ScriptableObjects/Tile/Tile_Data")]
    public class SO_TileData : ScriptableObject
    {
        [SerializeField] private TileType _tileType;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private bool _isMovable = true;
        [SerializeField] private bool _isConstructable = true;

        public TileType TileType { get { return _tileType; } }
        public Sprite Sprite { get { return _sprite; } }
        public bool IsMovable { get { return _isMovable; } }
        public bool IsConstructable { get { return _isConstructable; } }
    }
}