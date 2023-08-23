using System;
using System.IO;
using UnityEngine;

namespace DumbRide
{
    public class SaveManager : MonoBehaviourSingleton<SaveManager>
    {
        string _loadDataPath;

        LoadData _storedData;
        GarageShopData _shopData;

        public event Action<LoadData, GarageShopData> onDataLoaded;

        void Start()
        {
            _loadDataPath = Application.persistentDataPath + "/garageData.json";
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
            File.WriteAllText(_loadDataPath, json);
        }
        public void LoadData()
        {
            _shopData = DefaultData.MyGarageShopData;

            if (File.Exists(_loadDataPath))
            {
                string json = File.ReadAllText(_loadDataPath);
                _storedData = JsonUtility.FromJson<LoadData>(json);
                onDataLoaded?.Invoke(_storedData, _shopData);
            }
            else
            {
                using (FileStream fileStream = new FileStream(_loadDataPath, FileMode.Create)) { }
                SaveData(DefaultData.GetLoadData(_shopData.carPriceDatas.Length)); // we have only two cars in game
            }
        }
        
    }
}
