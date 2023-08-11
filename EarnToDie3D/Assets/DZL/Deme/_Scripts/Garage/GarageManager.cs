using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DumbRide
{
    public class GarageManager : MonoBehaviour
    {
        [SerializeField] GarageDataSO[] _garageDataSO;

        #region Images
        // Here array would not work because we chosen the way to work like this (because sprites are not serializable)
        [Header("Car Part Slots")]
        [SerializeField] GarageSlot _slotEngine;
        [SerializeField] GarageSlot _slotGear;
        [SerializeField] GarageSlot _slotWheel;
        [SerializeField] GarageSlot _slotKnife;
        [SerializeField] GarageSlot _slotGun;
        [SerializeField] GarageSlot _slotTurbo;
        [SerializeField] GarageSlot _slotFuel;
        #endregion

        GarageCarData[] _carData;
        GarageShopData _garageShopData;
        InGameCarData _selectedCarData; // this should be passed to car's controller to be used in car's movement
        int _selectedCarId;

        void OnEnable()
        {
            print("FIRST");
            _selectedCarData = DefaultData.MyIngameCarData;
            SaveManager.Instance.onDataLoaded += OnDataLoaded;

            InitializeSlots();
        }
        void OnDisable()
        {
            SaveManager.Instance.onDataLoaded -= OnDataLoaded;
        }

        void InitializeSlots()
        {
            var data = _garageDataSO[_selectedCarId];
            //var carLvlsData = _carData[_selectedCarId];
            var carLvlsData = DefaultData.MyGarageCarData;

            // max lvl's never change, it is taken from EarnToDie game original
            _slotEngine.Initialize(data.engineSprite, carLvlsData.engineLevel, 3);
            _slotGear.Initialize(data.gearSprite, carLvlsData.gearLevel, 3);
            _slotWheel.Initialize(data.wheelSprite, carLvlsData.wheelLevel, 3);
            _slotKnife.Initialize(data.bladeSprite, carLvlsData.isBladeBought? 1 : 0, 1);
            _slotGun.Initialize(data.gunSprite, carLvlsData.gunLevel, 10);
            _slotTurbo.Initialize(data.turboSprite, carLvlsData.turboLevel, 10);
            _slotFuel.Initialize(data.fuelSprite, carLvlsData.fuelLevel, 10);
        }

        public void SelectCar(int id)
        {
            _selectedCarId = id;
            for (int i = 0; i < _carData.Length; i++)
            {
                _carData[i].isSelected = false;
            }
            _carData[id].isSelected = true;
            SaveManager.Instance.SaveData(_carData);
        }
        void OnDataLoaded(LoadData loadedData, GarageShopData garageShopData)
        {
            _carData = loadedData.carData;
            _selectedCarData = TryBuildCurrentCarData();
            _garageShopData = garageShopData;
        }
        InGameCarData TryBuildCurrentCarData()
        {
            try
            {
                int id = _selectedCarId;
                GarageCarData loadedCarData = _carData[_selectedCarId];

                if (loadedCarData.isUnlocked)
                {
                    Debug.Log("Car Is Not Unlocked and you can not use it!");

                    return _selectedCarData;
                }

                var curCarData = new InGameCarData
                {
                    enginePower = _garageDataSO[id].engine[loadedCarData.engineLevel], // starts from 0!
                    gearPower = _garageDataSO[id].gear[loadedCarData.gearLevel],
                    wheelData = new WheelData
                    {
                        // TODO: Fix this
                        friction = _garageDataSO[id].wheel[loadedCarData.wheelLevel],
                        power = _garageDataSO[id].wheel[loadedCarData.wheelLevel]
                    },
                    bladeData = new DecoratorData
                    {
                        isUnlocked = loadedCarData.isBladeBought,
                        power = _garageDataSO[id].blade[0], // it is only one blade (Overkill to use FirstOrDefault),
                        type = DecoratorType.Blade
                    },
                    gunData = new DecoratorData
                    {
                        isUnlocked = loadedCarData.isBladeBought,
                        power = _garageDataSO[id].gun[loadedCarData.gunLevel],
                        type = DecoratorType.Gun

                    },
                    turboData = new DecoratorData
                    {
                        isUnlocked = loadedCarData.isBladeBought,
                        power = _garageDataSO[id].turbo[loadedCarData.turboLevel],
                        type = DecoratorType.Turbo
                    },
                    fuelLiter = _garageDataSO[id].fuel[loadedCarData.fuelLevel]
                };

                return curCarData;
            }
            catch (Exception e)
            {
                Debug.Log("Something Went wrong when creating current car data" + e.Message);
                return _selectedCarData;
            }
        }
    }

}
