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
                    enginePower = 0,
                    fuelLiter = 0,
                    gearPower = 0,
                    wheelMass = 0,
                    decoratorDatas = new DecoratorData[]
                    {
                        new DecoratorData
                        {
                            power = 100,
                            quantity = 1,
                            type = DecoratorType.Blade,
                            isUnlocked = false
                        },
                        new DecoratorData
                        {
                            power = 100,
                            quantity = 1,
                            type = DecoratorType.Gun,
                            isUnlocked = false
                        },
                        new DecoratorData
                        {
                            power = 100,
                            quantity = 1,
                            type = DecoratorType.Turbo,
                            isUnlocked = false
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
                    money = 1000,
                    selectedCarId = 0
                };
            }
        }
    }
}
