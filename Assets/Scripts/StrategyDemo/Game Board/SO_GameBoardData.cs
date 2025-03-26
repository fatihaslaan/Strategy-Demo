using StrategyDemo.Entity_NS;
using StrategyDemo.Tile_NS;
using UnityEngine;

namespace StrategyDemo.GameBoard_NS
{
    [CreateAssetMenu(fileName = "Game_Board_Data", menuName = "ScriptableObjects/GameBoard/Game_Board_Data")]
    public class SO_GameBoardData : ScriptableObject
    {
        [SerializeField] private SO_GameBoardBaseMapData _gameBoardMapData; // assetreference
        [SerializeField] private Tile _tilePrefab; //same

        public SO_GameBoardBaseMapData GameBoardMapData { get { return _gameBoardMapData; } }
        public Tile TilePrefab { get { return _tilePrefab; } }

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