using StrategyDemo.Tile_NS;
using UnityEngine;

namespace StrategyDemo.GameBoard_NS
{
    public class GameBoardView : MonoBehaviour
    {
        [SerializeField] private Transform _tilesParent;
        public Tile InstantiateTile(Tile tilePrefab)
        {
            Tile tile = Instantiate(tilePrefab, _tilesParent);
            return tile;
        }
    }
}