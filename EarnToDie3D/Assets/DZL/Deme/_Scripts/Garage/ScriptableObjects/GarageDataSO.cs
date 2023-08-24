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
        }
        public LevelData[] levels; // 6 because, there is 6 car parts
        public Sprite[] sprites;

        // will be used to calculate default data, the level will be set to 1 by default in car
        public bool IsPartUnlockedByDefault(int partId)
        {
            return levels[partId].pricesPerLevel[0] == 0; // first level's price is 0, means it's unlocked by default
        }
    }
}
