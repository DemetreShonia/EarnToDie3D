using UnityEngine;

namespace DumbRide
{
    [CreateAssetMenu(fileName = "CarData", menuName = "ScriptableObjects/SpawnCarData", order = 1)]
    public class GarageDataSO : ScriptableObject
    {
        // if you ask why I didn't use custom class for each part, it's because
        // sprites are not serializable and reconstructing each would be overkill

        public int[] engine;
        public Sprite engineSprite;

        public int[] gear;
        public Sprite gearSprite;

        public int[] wheel;
        public Sprite wheelSprite;

        public int[] blade;
        public Sprite bladeSprite;

        public int[] gun;
        public Sprite gunSprite;

        public int[] turbo;
        public Sprite turboSprite;

        public int[] fuel;
        public Sprite fuelSprite;

    }
}
