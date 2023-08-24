using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace DumbRide
{
    /// <summary>
    /// Load Data is Main Data To Load and Save
    /// </summary>
    [System.Serializable]
    public class LoadData
    {
        public GameData gameData;
        public GarageCarData[] carData;
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
    public class GarageCarData
    {
        public int carID;
        public int[] partLevels; // what part is on what level
        public bool isUnlocked;
        public bool isSelected;

        public int GetLevel(PartEnum part)
        {
            return partLevels[(int)part];
        }
    }
    public enum PartEnum
    {
        Engine,
        Gear,
        Wheel,
        Blade,
        Gun,
        Turbo,
        Fuel
    }

    /// <summary>
    /// InGameCarData is data which car will be used in game. Fuel liter, ammo amount and so on. It is calculated from GarageCarData
    /// </summary>
    public class InGameCarData
    {
        public int fuelLiter;
        public DecoratorData[] decoratorDatas;
        public int wheelMass;
        public float enginePower;
        public float gearPower;

        public void UnLockDecorator(DecoratorType type)
        {
            decoratorDatas[(int)type].isUnlocked = true;
        }
        public DecoratorData GetDecorator(DecoratorType type)
        {
            return decoratorDatas[(int)type];
        }
        public DecoratorData GetDecorator(int id)
        {
            return decoratorDatas[id];
        }
    }
    
    /// <summary>
    /// Decorator Data is data for each decorator. Calculated from GarageCarData to be used in InGameCarData
    /// </summary>
    public struct DecoratorData
    {
        public bool isUnlocked;
        public int power;
        public int quantity;
        public DecoratorType type;
    }
    public enum DecoratorType
    {
        Blade,
        Gun,
        Turbo
    }
}
