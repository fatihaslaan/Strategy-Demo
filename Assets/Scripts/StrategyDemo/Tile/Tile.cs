using Base.Util;
using StrategyDemo.Navigation_NS;
using UnityEngine;

namespace StrategyDemo.Tile_NS
{
    public class Tile : MonoBehaviour
    {
        [HideInInspector] public bool isOccupied;

        [SerializeField] private SpriteRenderer _spriteRenderer;

        private TileCoordinate _tileCoordinate;

        public void SetTile(TileCoordinate tileCoordinate, Color baseColor, Color offsetColor)
        {
            _tileCoordinate = tileCoordinate;
            transform.localPosition = new Vector3(_tileCoordinate.xCoorddinate, _tileCoordinate.yCoordinate, 0);
            _spriteRenderer.color = IsOffSet() ? offsetColor : baseColor;
        }

        private bool IsOffSet()
        {
            return _tileCoordinate.xCoorddinate % 2 != _tileCoordinate.yCoordinate % 2;
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