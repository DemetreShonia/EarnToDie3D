namespace DumbRide
{
    /// <summary>
    /// Load Data is Main Data To Load and Save
    /// </summary>
    [System.Serializable]
    public class LoadData
    {
        public GarageCarData[] carData;
        public GameData gameData;
    }

    /// <summary>
    /// Game Data is General Game Data 
    /// </summary>
    [System.Serializable]
    public struct GameData
    {
        public int highScore;
        public int money;
        public int selectedCarId;
    }

    /// <summary>
    /// GarageCarData is Data for Each Car which holds current state of car, what is upgraded, on what level, is it unlocked, and etc
    /// </summary>
    [System.Serializable]
    public struct GarageCarData
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
    /// <summary>
    /// InGameCarData is data which car will be used in game. Fuel liter, ammo amount and so on. It is calculated from GarageCarData
    /// </summary>
    public struct InGameCarData
    {
        public float enginePower;
        public float gearPower;
        public WheelData wheelData;
        public DecoratorData bladeData;
        public DecoratorData gunData;
        public DecoratorData turboData;
        public float fuelLiter;
        public float turboLiter;
    }
    /// <summary>
    /// Wheel Data is data for each wheel. Calculated from GarageCarData to be used in InGameCarData
    /// </summary>
    public struct WheelData
    {
        public float friction;
        public float power;
    }
    /// <summary>
    /// Decorator Data is data for each decorator. Calculated from GarageCarData to be used in InGameCarData
    /// </summary>
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
}
