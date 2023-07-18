using UnityEngine;

namespace DumbRide
{

    [CreateAssetMenu(fileName = "CarData", menuName = "ScriptableObjects/SpawnCarData", order = 1)]
    public class GarageDataSO : ScriptableObject
    {
        public int[] engineLevel;
        public int[] gearLevel;
        public int[] wheelLevel;
        public int[] bladeLevel;
        public int[] gunLevel;
        public int[] turboLevel;
        public int[] fuelLevel;
    }
}
