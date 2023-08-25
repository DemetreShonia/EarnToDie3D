using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    [CreateAssetMenu(fileName = "CarData", menuName = "ScriptableObjects/SpawnCarData", order = 1)]
    public class GarageDataSO : ScriptableObject
    {
        public int carPrice; // this is for unlocking the car, -1 means it's unlocked by default, first car...
        // or we can make client buy it with coins
        [System.Serializable]
        public class LevelData
        { 
            public int[] pricesPerLevel;
            public int[] statsPerLevel;

            public int GetStats(int level)
            {
                if (statsPerLevel.Length > level)
                    return statsPerLevel[level];
                return -1;
            }
            public int GetPrice(int level)
            {
                if (pricesPerLevel.Length > level)
                    return pricesPerLevel[level];
                return -1;
            }
        }
        public LevelData GetLevelData(PartEnum part)
        {
            return levels[(int)part];
        }
        public LevelData[] levels; // 6 because, there is 6 car parts
        public Sprite[] sprites;
    }
}
