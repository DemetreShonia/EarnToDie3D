using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public class CarDataWrapper
    {
        public CarData[] carData;
    }

    public class SaveManager : MonoBehaviourSingleton<SaveManager>
    {
        string _savePath;
        CarData[] _carData;

        public event Action<CarData[]> onDataLoaded;

        protected override void Awake()
        {
            base.Awake();
            _savePath = Application.persistentDataPath + "/garageData.json";
            //SaveGarageData();
            LoadGarageData();
        }

        public void SaveGarageData()
        {
            string json = JsonUtility.ToJson(new CarDataWrapper { carData = _carData });
            System.IO.File.WriteAllText(_savePath, json);
        }

        public void LoadGarageData()
        {
            if (System.IO.File.Exists(_savePath))
            {
                string json = System.IO.File.ReadAllText(_savePath);
                _carData = JsonUtility.FromJson<CarDataWrapper>(json).carData;
                onDataLoaded?.Invoke(_carData);
            }
            else
            {
                System.IO.File.Create(_savePath); // TODO: Check if this works
            }
        }
    }
}
