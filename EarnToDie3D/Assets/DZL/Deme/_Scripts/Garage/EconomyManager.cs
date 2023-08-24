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
        }

        // we might not need two similar... but for now it's ok
        public void TrySpendMoney(int money)
        {
            if (Money >= money)
            {
                Money -= money;
                onMoneyChanged?.Invoke(-money);
            }
        }

    }
}
