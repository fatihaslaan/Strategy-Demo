using Base.Util;
using StrategyDemo.Navigation_NS;
using UnityEngine;

namespace StrategyDemo.Tile_NS
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Color _baseTileColor = Color.white;
        [SerializeField] private Color _offsetTileColor = Color.gray;
        private TileCoordinate _tileCoordinate;

        [HideInInspector] public bool isOccupied;
        public TileCoordinate TileCoordinate
        {
            get { return _tileCoordinate; }
            set
            {
                _tileCoordinate = value;
                transform.localPosition = new Vector3(value.xCoordinate, value.yCoordinate, 0);
                _spriteRenderer.color = value.IsOffset() ? _offsetTileColor : _baseTileColor;
                _spriteRenderer.sprite = value.tileData.Sprite;
            }
        }

        private void OnValidate()
        {
            if (!_spriteRenderer)
            {
                ObjectFinder.FindObjectInChilderenWithType(ref _spriteRenderer, transform);
            }
        }
    }
}