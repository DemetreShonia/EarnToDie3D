using System;
using TMPro;
using UnityEngine;

namespace DumbRide
{
    public class GarageManager : MonoBehaviour
    {
        [SerializeField] GarageDataSO[] _garageDataSO;
        [SerializeField] GarageSlot[] _slots;
        [SerializeField]
        CarInGarage[] _carsInGarage;

        GarageCarData[] _carDatas;
        InGameCarData _selectedCarData; // this should be passed to car's controller to be used in car's movement
        int _selectedCarId;

        public GarageCarData SelectedGarageCarData => _carDatas[_selectedCarId];
        public GarageDataSO SelectedCarDataSO => _garageDataSO[_selectedCarId];

        void OnEnable()
        {
            _selectedCarData = DefaultData.MyIngameCarData;
            SaveManager.Instance.onDataLoaded += OnDataLoaded;
            EconomyManager.Instance.onLevelChanged += PartWasUpgraded;
        }
        void OnDisable()
        {
            SaveManager.Instance.onDataLoaded -= OnDataLoaded;
            EconomyManager.Instance.onLevelChanged -= PartWasUpgraded;
        }

       
        public void PartWasUpgraded(int partId, int newLevel)
        {
            SelectedGarageCarData.partLevels[partId] = newLevel;
            SaveManager.Instance.SaveData(_carDatas);
            _slots[partId].UpdatePrice(SelectedCarDataSO.levels[partId].GetPrice(newLevel + 1)); // newlevel + 1 because, we have to show next price
            _carsInGarage[_selectedCarId].SwitchDecorators(SelectedGarageCarData); // update decorators
        }
        public void SelectCar(int id)
        {

            // WE NEED SOME KIND OF..

            // ON SELECTED CAR CHANGED AND WILL BE EASY TO IMPLEMENT
            _selectedCarId = id;
            for (int i = 0; i < _carDatas.Length; i++)
            {
                _carDatas[i].isSelected = false;
            }
            _carDatas[id].isSelected = true;
            SaveManager.Instance.SaveData(_carDatas);
        }
        void OnDataLoaded(LoadData loadedData)
        {
            if(!loadedData.gameData.isNotFirstTime)
            { 
                // first time, use default data and save
                _carDatas = DefaultData.GetGarageCarDataArray();
                SaveManager.Instance.SaveData(_carDatas);
                SaveManager.Instance.SaveData(isNotFirstTime: true);
                print("FIRST TIME");
            }
            else
            {
                _carDatas = loadedData.carData; // not first time, use saved data
                print("NOT FIRST TIME");
            }
            _selectedCarData = TryBuildIngameCarData();
            InitializeSlots();

            _carsInGarage[_selectedCarId].SwitchDecorators(SelectedGarageCarData); // update decorators
        }
        void InitializeSlots()
        {
            var so = SelectedCarDataSO;
            var cd = SelectedGarageCarData;

            for (int i = 0; i < _slots.Length; i++)
            {
                var maxLevelID = so.levels[i].pricesPerLevel.Length - 1;
                var curLevelID = cd.partLevels[i];
                var price = curLevelID + 1 <= maxLevelID ? so.levels[i].pricesPerLevel[curLevelID + 1] : maxLevelID;

                _slots[i].Initialize(so.sprites[i], curLevelID, maxLevelID, price, i);
            }
        }
        InGameCarData TryBuildIngameCarData()
        {
            try
            {
                int id = _selectedCarId;
                GarageCarData loadedCarData = _carDatas[_selectedCarId];

                if (!loadedCarData.isUnlocked)
                {
                    Debug.Log("Car Is Not Unlocked and you can not use it!");

                    return _selectedCarData;
                }
                return DefaultData.MyIngameCarData;
            }
            catch (Exception e)
            {
                Debug.Log("Something Went wrong when creating current car data" + e.Message);
                return _selectedCarData;
            }
        }
    }

}
