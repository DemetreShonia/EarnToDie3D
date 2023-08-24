using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public static class DefaultData
    {
        public static GarageShopData MyGarageShopData
        {
            get
            {
                return new GarageShopData
                {
                    carPriceDatas = new CarPriceData[]
                    {
                        new CarPriceData // mustang
                        {
                            carID = 0,
                            price = 0, // free
                            lvlUpDatas = new LvlUpData[]
                            {
                                new LvlUpData // engine
                                {
                                    prices = new int[] { 0, 35, 110}
                                },
                                new LvlUpData // gears
                                {
                                    prices = new int[] { 0, 40, 125}
                                },
                                new LvlUpData // wheels
                                {
                                    prices = new int[] { 0, 45, 140}
                                },
                                new LvlUpData // blade
                                {
                                    prices = new int[] { 100}
                                },
                                new LvlUpData // gun
                                {
                                    prices = new int[] { 80, 90, 100, 110, 120, 130, 140, 150, 160, 180}
                                },
                                new LvlUpData // turbo
                                {
                                    prices = new int[] { 60, 70, 80, 90, 100, 120, 140, 160, 180, 200}
                                },
                                new LvlUpData // fuel
                                {
                                    prices = new int[] { 4, 8, 20, 28, 36, 50, 74, 85, 117, }
                                },
                            }
                        },
                        // Truck, not yet!
                        //new CarPriceData
                        //{
                        //    carID = 0,
                        //    price = 0,
                        //    lvlUpDatas = new LvlUpData[]
                        //    {
                        //        new LvlUpData
                        //        {
                        //            prices = new int[] { 0, 0, 0, 0, 0, 0, 0 }
                        //        }
                        //    }
                        //}
                    }
                };
            }
        }
        public static LoadData GetLoadData(int carAmount)
        {
            return new LoadData
            {
                gameData = MyGameData,
                carData = GetGarageCarDataArray(carAmount)
            };
        }

        public static GarageCarData[] GetGarageCarDataArray(int size)
        {
            List<GarageCarData> carDataList = new List<GarageCarData>() { MyGarageCarData }; // default is first
            GarageCarData iteratorData = MyGarageCarData;
            iteratorData.isUnlocked = iteratorData.isSelected = false;

            for (int i = 1; i < size; i++)
            {
                iteratorData.carID = i;
                carDataList.Add(iteratorData);
            }
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
                    money = 0,
                    selectedCarId = 0
                };
            }
        }
        public static GarageCarData MyGarageCarData
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
