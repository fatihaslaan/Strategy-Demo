using StrategyDemo.Tile_NS;
using UnityEngine;

namespace StrategyDemo.GameBoard_NS
{
    [CreateAssetMenu(fileName = "Game_Board_Data", menuName = "ScriptableObjects/Game_Board_Data")]
    public class GameBoardData : ScriptableObject
    {
        [SerializeField] private GameBoardBaseMapData _gameBoardMapData; // assetreference
        [SerializeField] private Tile _tilePrefab; //same
        [SerializeField] private Color _baseTileColor = Color.white;
        [SerializeField] private Color _offsetTileColor = Color.gray;

        public GameBoardBaseMapData GameBoardMapData { get { return _gameBoardMapData; } }
        public Tile TilePrefab { get { return _tilePrefab; } }
        public Color BaseTileColor { get { return _baseTileColor; } }
        public Color OffsetTileColor { get { return _offsetTileColor; } }

        private void OnValidate()
        {
            if(!_gameBoardMapData)
            {
                Debug.LogError("No Map Data Selected For Board Data");
            }
            if(!_tilePrefab)
            {
                Debug.LogError("No Tile Prefab Setted For Board Data");
            }
        }
    }
}