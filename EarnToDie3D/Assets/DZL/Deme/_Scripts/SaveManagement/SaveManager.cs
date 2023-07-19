using System;
using UnityEngine;

namespace DumbRide
{
    public class SaveManager : MonoBehaviourSingleton<SaveManager>
    {
        string _savePath;
        StoredData _storedData;

        public event Action<StoredData> onDataLoaded;

        protected override void Awake()
        {
            base.Awake();
            _savePath = Application.persistentDataPath + "/garageData.json";
            LoadGarageData();
        }

        public void SaveData(CarData newCarData)
        {
            _storedData.carData[newCarData.carID] = newCarData; // might throw error if indexes are not correctly assigned
            SaveData(_storedData);
        }
        public void SaveData(GameData gameData)
        {
            _storedData.gameData = gameData;
            SaveData(_storedData);
        }
        public void SaveData(StoredData storedData)
        {
            string json = JsonUtility.ToJson(storedData);
            System.IO.File.WriteAllText(_savePath, json);
        }

        public void LoadGarageData()
        {
            if (System.IO.File.Exists(_savePath))
            {
                string json = System.IO.File.ReadAllText(_savePath);
                _storedData = JsonUtility.FromJson<StoredData>(json);
                onDataLoaded?.Invoke(_storedData);
            }
            else
            {
                System.IO.File.Create(_savePath); 
                SaveData(DefaultData.GetStoredData(2)); // we have only two cars in game
            }
        }
        
    }
}
