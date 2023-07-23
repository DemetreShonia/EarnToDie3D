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

        [SerializeField] Image _imageTest;
        CarData[] _carData;
        CurrentCarData _selectedCarData; // this should be passed to car's controller to be used in car's movement
        int _selectedCarId;
        
        void OnEnable()
        {
            print("FIRST");
            _selectedCarData = DefaultData.MyCurrentCarData;
            SaveManager.Instance.onDataLoaded += OnDataLoaded;
            LoadSprites();
        }
        void OnDisable()
        {
            SaveManager.Instance.onDataLoaded -= OnDataLoaded;
        }

        void LoadSprites()
        {
            var data = _garageDataSO[_selectedCarId];
            _imageTest.sprite = data.engineSprite;  
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
        void OnDataLoaded(StoredData loadedData)
        {
            _carData = loadedData.carData;
            _selectedCarData = TryBuildCurrentCarData();
        }
        CurrentCarData TryBuildCurrentCarData()
        {
            try
            {
                int id = _selectedCarId;
                CarData loadedCarData = _carData[_selectedCarId];

                if (loadedCarData.isUnlocked)
                {
                    Debug.Log("Car Is Not Unlocked and you can not use it!");

                    return _selectedCarData;
                }

                var curCarData = new CurrentCarData
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
                        power = _garageDataSO[id].blade[0] // it is only one blade (Overkill to use FirstOrDefault)
                    },
                    gunData = new DecoratorData
                    {
                        isUnlocked = loadedCarData.isBladeBought,
                        power = _garageDataSO[id].gun[loadedCarData.gunLevel]
                    },
                    turboData = new DecoratorData
                    {
                        isUnlocked = loadedCarData.isBladeBought,
                        power = _garageDataSO[id].turbo[loadedCarData.turboLevel]
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
