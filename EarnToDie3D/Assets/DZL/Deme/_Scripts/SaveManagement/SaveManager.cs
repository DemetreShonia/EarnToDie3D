using System;
using System.IO;
using UnityEngine;

namespace DumbRide
{
    public class SaveManager : MonoBehaviourSingleton<SaveManager>
    {
        string _savePath;
        LoadData _storedData;

        public event Action<LoadData> onDataLoaded;

        protected override void Awake()
        {
            base.Awake();
            _savePath = Application.persistentDataPath + "/garageData.json";
            LoadData();
            print(Instance);
        }

        public void SaveData(GarageCarData[] carDatas)
        {
            _storedData.carData = carDatas;
            SaveData(_storedData);
        }
        public void SaveData(GarageCarData newCarData)
        {
            _storedData.carData[newCarData.carID] = newCarData; // might throw error if indexes are not correctly assigned
            SaveData(_storedData);
        }
        public void SaveData(GameData gameData)
        {
            _storedData.gameData = gameData;
            SaveData(_storedData);
        }
        public void SaveData(LoadData storedData)
        {
            string json = JsonUtility.ToJson(storedData);
            print(json);
            File.WriteAllText(_savePath, json);
        }
        public void LoadData()
        {
            if (File.Exists(_savePath))
            {
                string json = File.ReadAllText(_savePath).Replace("\n", "");
                print(json);
                _storedData = JsonUtility.FromJson<LoadData>(json);
                onDataLoaded?.Invoke(_storedData);
            }
            else
            {
                using (FileStream fileStream = new FileStream(_savePath, FileMode.Create)) { }
                SaveData(DefaultData.GetStoredData(2)); // we have only two cars in game
            }
        }
        
    }
}
