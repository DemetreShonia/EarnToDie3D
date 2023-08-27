using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DumbRide
{
    public class GarageManager : MonoBehaviour
    {
        [SerializeField] SceneSwitchManager _sceneSwitchManager;
        [SerializeField] GarageDataSO[] _garageDataSO;
        [SerializeField] GarageSlot[] _slots;
        [SerializeField]
        CarInGarage[] _carsInGarage;

        GarageCarData[] _carDatas;
        [SerializeField] InGameCarData _inGameCarData; // this should be passed to car's controller to be used in car's movement
        int _selectedCarId;


        [SerializeField] Button _returnToMapButton;
        [SerializeField] Button _goToGameButton;

        public GarageCarData SelectedGarageCarData => _carDatas[_selectedCarId];
        public GarageDataSO SelectedCarDataSO => _garageDataSO[_selectedCarId];

        void OnEnable()
        {
            if (SceneSwitchManager.Instance == null)
            {
                Instantiate(_sceneSwitchManager);
            }
            _returnToMapButton.onClick.AddListener(HandleMenuButton);
            _goToGameButton.onClick.AddListener(HandleGoButton);

            SaveManager.Instance.onDataLoaded += OnDataLoaded;
            EconomyManager.Instance.onLevelChanged += PartWasUpgraded;
        }
        void OnDisable()
        {
            _returnToMapButton.onClick.RemoveListener(HandleMenuButton);
            _goToGameButton.onClick.RemoveListener(HandleGoButton);

            SaveManager.Instance.onDataLoaded -= OnDataLoaded;
            EconomyManager.Instance.onLevelChanged -= PartWasUpgraded;
        }

        void HandleMenuButton()
        {
            SceneSwitchManager.Instance.SwitchScene(0);
        }
        void HandleGoButton()
        {
            SceneSwitchManager.Instance.SwitchScene(2);
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
                SaveManager.Instance.SaveData(isNotFirstTime: true);
                SaveManager.Instance.SaveData(_carDatas);
                print("FIRST TIME");
            }
            else
            {
                _carDatas = loadedData.carData; // not first time, use saved data
                print("NOT FIRST TIME");
            }


            _selectedCarId = loadedData.gameData.selectedCarId; // load selected car id
            // Call function which shows selected car

            InitializeSlots(); // and it's slots

            _carsInGarage[_selectedCarId].SwitchDecorators(SelectedGarageCarData); // update decorators

            _inGameCarData = BuildInGameCarData();
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
        public InGameCarData BuildInGameCarData()
        {
            var so = SelectedCarDataSO;
            var cd = SelectedGarageCarData;

            InGameCarData data = new InGameCarData
            {
                fuelLiter = so.GetLevelData(PartEnum.Fuel).GetStats(cd.GetLevel(PartEnum.Fuel)),
                wheelMass = so.GetLevelData(PartEnum.Wheel).GetStats(cd.GetLevel(PartEnum.Wheel)),
                enginePower = so.GetLevelData(PartEnum.Engine).GetStats(cd.GetLevel(PartEnum.Engine)),
                gearPower = so.GetLevelData(PartEnum.Gear).GetStats(cd.GetLevel(PartEnum.Gear)),
            };

            var partToDecoratorMap = new Dictionary<PartEnum, DecoratorType>
            {
                { PartEnum.Gun, DecoratorType.Gun },
                { PartEnum.Blade, DecoratorType.Blade },
                { PartEnum.Turbo, DecoratorType.Turbo }
            };

            // check if part is unlocked, if it is, unlock decorator
            foreach (var item in partToDecoratorMap)
                if (cd.GetLevel(item.Key) > 0)
                    data.UnLockDecorator(item.Value);

            for (int i = 0; i < data.decoratorDatas.Length; i++)
            {
                var curData = data.decoratorDatas[i];
                if (!curData.isUnlocked)
                    continue;

                switch(curData.type)
                {
                    case DecoratorType.Gun:
                        var gunDamage = so.GetLevelData(PartEnum.Gun).GetStats(0);
                        var ammoCount = cd.GetLevel(PartEnum.Gun);
                        curData.power = gunDamage;
                        curData.quantity = ammoCount;
                        break;
                    case DecoratorType.Blade:
                        var bladeDamage = so.GetLevelData(PartEnum.Blade).GetStats(cd.GetLevel(PartEnum.Blade));
                        curData.power = bladeDamage;
                        curData.quantity = 1;
                        break;
                    case DecoratorType.Turbo:
                        var turboLiter = so.GetLevelData(PartEnum.Turbo).GetStats(cd.GetLevel(PartEnum.Turbo));
                        var turboPower = cd.GetLevel(PartEnum.Gun);
                        curData.power = turboPower;
                        curData.quantity = turboLiter;
                        break;
                }
            }

            return data;
        }
        
    }

}
