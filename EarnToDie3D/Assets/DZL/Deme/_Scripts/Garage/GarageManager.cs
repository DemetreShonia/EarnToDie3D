using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    
    public class GarageManager : MonoBehaviour
    {
        [SerializeField] GarageDataSO[] _garageDataSO;

        CarData[] _carData;
        CurrentCarData _selectedCarData; // this should be passed to car's controller to be used in car's movement
        int _selectedCarId;

        public void SelectCar(int id)
        {
            _selectedCarId = id;
        }
        void OnEnable()
        {
            SaveManager.Instance.onDataLoaded += OnDataLoaded;
        }
        void OnDisable()
        {
            SaveManager.Instance.onDataLoaded -= OnDataLoaded;
        }

        void OnDataLoaded(StoredData loadedData)
        {
            _carData = loadedData.carData;
            TryBuildCurrentCarData();
        }
        void TryBuildCurrentCarData()
        {
            try
            {
                int id = _selectedCarId;
                CarData loadedCarData = _carData[_selectedCarId];

                if (loadedCarData.isUnlocked)
                    Debug.Log("Car Is Not Unlocked and you can not use it!");

                var curCarData = new CurrentCarData
                {
                    enginePower = _garageDataSO[id].engineLevel[loadedCarData.engineLevel], // starts from 0!
                    gearPower = _garageDataSO[id].gearLevel[loadedCarData.gearLevel],
                    wheelData = new WheelData
                    {
                        // TODO: Fix this
                        friction = _garageDataSO[id].wheelLevel[loadedCarData.wheelLevel],
                        power = _garageDataSO[id].wheelLevel[loadedCarData.wheelLevel]
                    },
                    bladeData = new DecoratorData
                    {
                        isUnlocked = loadedCarData.isBladeBought,
                        power = _garageDataSO[id].bladeLevel[0] // it is only one blade (Overkill to use FirstOrDefault)
                    },
                    gunData = new DecoratorData
                    {
                        isUnlocked = loadedCarData.isBladeBought,
                        power = _garageDataSO[id].gunLevel[loadedCarData.gunLevel]
                    },
                    turboData = new DecoratorData
                    {
                        isUnlocked = loadedCarData.isBladeBought,
                        power = _garageDataSO[id].turboLevel[loadedCarData.turboLevel]
                    },
                    fuelLiter = _garageDataSO[id].fuelLevel[loadedCarData.fuelLevel]
                };

                _selectedCarData = curCarData;
            }
            catch (Exception e)
            {
                Debug.LogError("Something Went wrong when creating current car data" + e.Message);
            }
        }
    }

}
