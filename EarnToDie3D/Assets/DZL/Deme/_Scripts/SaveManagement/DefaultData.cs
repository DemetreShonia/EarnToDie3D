using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public static class DefaultData
    {
        public static LoadData GetStoredData(int carAmount)
        {
            return new LoadData
            {
                gameData = MyGameData,
                carData = GetCarDataArray(carAmount)
            };
        }

        public static GarageCarData[] GetCarDataArray(int size)
        {
            List<GarageCarData> carDataList = new List<GarageCarData>() { MyCarData }; // default is first
            GarageCarData iteratorData = MyCarData;
            iteratorData.isUnlocked = iteratorData.isSelected = false;

            for (int i = 1; i < size; i++)
            {
                iteratorData.carID = i;
                carDataList.Add(iteratorData);
            }
            return carDataList.ToArray();
        }
        public static InGameCarData MyCurrentCarData
        {
            get
            {
                return new InGameCarData
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
        public static GarageCarData MyCarData
        {
            get
            {
                return new GarageCarData
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
