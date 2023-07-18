using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    [System.Serializable]
    public class CarData
    {
        public int carID;
        public bool isUnlocked;
        public bool isSelected;
        public int engineLevel;
        public int gearLevel;
        public int wheelLevel;
        public bool isBladeBought;
        public int gunLevel;
        public int turboLevel;
        public int fuelLevel;
    }
}
