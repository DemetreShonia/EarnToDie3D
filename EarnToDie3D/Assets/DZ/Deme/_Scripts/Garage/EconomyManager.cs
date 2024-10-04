using System;
using TMPro;
using UnityEngine;

namespace DumbRide
{
    public class EconomyManager : MonoBehaviourSingleton<EconomyManager>
    {
        [SerializeField] TextMeshProUGUI _moneyText;


        protected override void Awake()
        {
            base.Awake();
            UpdateMoneyUI();
        }
        void OnEnable()
        {
            SaveManager.Instance.onDataLoaded += OnDataLoaded;
        }
        void OnDisable()
        {
            SaveManager.Instance.onDataLoaded -= OnDataLoaded;
        }

        void OnDataLoaded(LoadData data)
        {
            Money = data.gameData.money;
            UpdateMoneyUI();
        }

        void UpdateMoneyUI() => _moneyText.text = $"${Money}";
        /// <summary>
        /// Event raised when the level of selected car changes. Id of part (Slot) is passed as an argument. and New Level
        /// </summary>
        public event Action<int, int> onLevelChanged;

        /// <summary>
        /// Event raised when the money amount changes. new amount is passed as an argument.
        /// </summary>
        public event Action<int> onMoneyChanged; // will be useful for particles or something

        public int Money { get; private set; } = 1000;

        public void TryUpgradePart(int slotId, int cost, int currentLevel)
        {
            if (Money >= cost)
            {
                AdjustMoney(-cost);
                onLevelChanged?.Invoke(slotId, currentLevel + 1);
            }
        }

        public void AdjustMoney(int cost)
        {
            bool isSpend = cost < 0; // can be useful for Animating and UI stuff, particles

            Money += cost;
            UpdateMoneyUI();
            onMoneyChanged?.Invoke(cost);
            SaveManager.Instance.SaveData(money: Money);
        }
    }
}
