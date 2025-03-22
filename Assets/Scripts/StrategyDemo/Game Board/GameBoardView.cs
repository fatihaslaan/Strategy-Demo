using StrategyDemo.Tile_NS;
using UnityEngine;

namespace StrategyDemo.GameBoard_NS
{
    public class GameBoardView : MonoBehaviour
    {
        public Tile InstantiateTile(Tile tilePrefab)
        {
            Tile tile = Instantiate(tilePrefab, transform);
            return tile;
        }
    }
}