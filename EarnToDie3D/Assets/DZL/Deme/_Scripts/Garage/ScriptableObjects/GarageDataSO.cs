using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    [CreateAssetMenu(fileName = "CarData", menuName = "ScriptableObjects/SpawnCarData", order = 1)]
    public class GarageDataSO : ScriptableObject
    {
        [System.Serializable]
        public class LevelData
        { 
            public int[] pricesPerLevel;
        }
        public LevelData[] levels; // 6 because, there is 6 car parts
        public Sprite[] sprites;
    }
}
