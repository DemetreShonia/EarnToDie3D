using System;
using System.IO;
using UnityEngine;

namespace DumbRide
{
    public class SaveManager : MonoBehaviourSingleton<SaveManager>
    {
        string _loadDataPath;

        LoadData _storedData;

        public event Action<LoadData> onDataLoaded;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }
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
        public GameData TheGameData
        {
            get => _storedData.gameData;
            set
            {
                _storedData.gameData = value;
                SaveData(_storedData);
            }
        }
        public void SaveData(bool? isNotFirstTime = null, int? highScore = null, int? money = null, int? selectedCarId = null)
        {
            if (isNotFirstTime.HasValue)
                _storedData.gameData.isNotFirstTime = isNotFirstTime.Value;

            if (highScore.HasValue)
                _storedData.gameData.highScore = highScore.Value;

            if (money.HasValue)
                _storedData.gameData.money = money.Value;

            if (selectedCarId.HasValue)
                _storedData.gameData.selectedCarId = selectedCarId.Value;

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
            print("Loaded Data");
            if (File.Exists(_loadDataPath))
            {
                string json = File.ReadAllText(_loadDataPath);
                _storedData = JsonUtility.FromJson<LoadData>(json);
            }
            else
            {
                using (FileStream fileStream = new FileStream(_loadDataPath, FileMode.Create)) { }
                _storedData = DefaultData.GetLoadData();
                SaveData(_storedData); // we have only two cars in game
            }
            onDataLoaded?.Invoke(_storedData);
        }

    }
}
