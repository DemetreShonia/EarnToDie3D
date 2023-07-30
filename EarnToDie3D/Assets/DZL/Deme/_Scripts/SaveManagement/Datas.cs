namespace DumbRide
{
    [System.Serializable]
    public struct GameData
    {
        public int highScore;
        public int money;
        public int selectedCarId;
    }
    [System.Serializable]
    public class StoredData
    {
        public CarData[] carData;
        public GameData gameData;
    }

    [System.Serializable]
    public struct CarData
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
    public struct WheelData
    {
        public float friction;
        public float power;
    }
    public struct DecoratorData
    {
        public bool isUnlocked;
        public float power;
        public DecoratorType type;
    }
    public enum DecoratorType
    {
        Blade,
        Gun,
        Turbo
    }
    public struct CurrentCarData
    {
        public float enginePower;
        public float gearPower;
        public WheelData wheelData;
        public DecoratorData bladeData;
        public DecoratorData gunData;
        public DecoratorData turboData;
        public float fuelLiter;
    }

}
