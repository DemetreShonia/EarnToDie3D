using System;
using UnityEngine.SocialPlatforms.Impl;

namespace DumbRide
{
    /// <summary>
    /// There is a garage shop data in project which is read-only which stores array of price-data for each car.
    /// </summary>
    [System.Serializable]
    public class GarageShopData
    {
        public CarPriceData[] carPriceDatas;
    }

    /// <summary>
    /// Each car's id, price and level up prices are stored in this data
    /// </summary>
    [System.Serializable]
    public class CarPriceData
    {
        public int carID;
        public int price;
        public LvlUpData[] lvlUpDatas; // engine, gear, wheel, blade, gun, turbo, fuel
    }

    /// <summary>
    /// LvlUpData is data for each level up price for each car. There is array which holds price for each level. array id = level
    /// </summary>
    [System.Serializable]
    public class LvlUpData
    {
        public int[] prices;
    }


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
