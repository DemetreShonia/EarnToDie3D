using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public static class DefaultData
    {
        public static LoadData GetLoadData()
        {
            return new LoadData
            {
                gameData = MyGameData,
                carData = GetGarageCarDataArray()
            };
        }

        public static GarageCarData[] GetGarageCarDataArray()
        {
            List<GarageCarData> carDataList = new List<GarageCarData>();
            // default is first

            for (int i = 0; i < Constants.CAR_COUNT_IN_GAME; i++)
            {
                carDataList.Add(new GarageCarData
                {
                    carID = i,
                    isUnlocked = false,
                    isSelected = false,
                    partLevels = new int[]
                    {
                        1, 1, 1, 0, 0, 0, 1
                    }
                });
            }

            // size will be more than 1 always
            carDataList[0].isUnlocked = true; // unlock first car and select too
            carDataList[0].isSelected = true;
            return carDataList.ToArray();
        }
        public static InGameCarData MyIngameCarData
        {
            get
            {
                return new InGameCarData
                {
                    engineTorque = 900,
                    fuelLiter = 1000,
                    gearPower = 18,
                    wheelMass = 20,
                    decoratorDatas = new DecoratorData[]
                    {
                        new DecoratorData
                        {
                            power = 100,
                            quantity = 1,
                            type = DecoratorType.Blade,
                            isUnlocked = true
                        },
                        new DecoratorData
                        {
                            power = 100,
                            quantity = 300,
                            type = DecoratorType.Gun,
                            isUnlocked = true
                        },
                        new DecoratorData
                        {
                            power = 500,
                            quantity = 500,
                            type = DecoratorType.Turbo,
                            isUnlocked = true
                        }
                    }
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
                    money = 10000,
                    selectedCarId = 0
                };
            }
        }
    }
}
