using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public static class DefaultData
    {
        public static StoredData GetStoredData(int carAmount)
        {
            return new StoredData
            {
                gameData = MyGameData,
                carData = GetCarDataArray(carAmount)
            };
        }

        public static CarData[] GetCarDataArray(int size)
        {
            List<CarData> carDataList = new List<CarData>() { MyCarData }; // default is first
            CarData iteratorData = MyCarData;
            iteratorData.isUnlocked = iteratorData.isSelected = false;

            for (int i = 1; i < size; i++)
            {
                iteratorData.carID = i;
                carDataList.Add(iteratorData);
            }
            return carDataList.ToArray();
        }
        public static CurrentCarData MyCurrentCarData
        {
            get
            {
                return new CurrentCarData
                {
                    bladeData = new DecoratorData { isUnlocked = false, power = 0 },
                    enginePower = 0,
                    fuelLiter = 0,
                    gearPower = 0,
                    gunData = new DecoratorData { isUnlocked = false, power = 0 },
                    turboData = new DecoratorData { isUnlocked = false, power = 0 },
                    wheelData = new WheelData { friction = 0, power = 0 }
                };
            }
        }
        public static GameData MyGameData
        {
            get
            {
                return new GameData
                {
                    highScore = 0,
                    money = 0,
                    selectedCarId = 0
                };
            }
        }
        public static CarData MyCarData
        {
            get
            {
                return new CarData
                {
                    carID = 0,
                    isUnlocked = true,
                    isSelected = true,
                    engineLevel = 0,
                    gearLevel = 0,
                    wheelLevel = 0,
                    isBladeBought = false,
                    gunLevel = -1,
                    turboLevel = -1,
                    fuelLevel = 0
                };
            }
        }   
    }
}
