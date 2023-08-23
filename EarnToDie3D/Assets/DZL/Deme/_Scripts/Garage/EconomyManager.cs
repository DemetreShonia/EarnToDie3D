using System;

namespace DumbRide
{
    public class EconomyManager : MonoBehaviourSingleton<EconomyManager>
    {
        /// <summary>
        /// Event raised when the level of selected car changes. Id of part (Slot) is passed as an argument.
        /// </summary>
        public event Action<int> onLevelChanged;

        /// <summary>
        /// Event raised when the money amount changes. new amount is passed as an argument.
        /// </summary>
        public event Action<int> onMoneyChanged; // will be useful for particles or something

        public int Money { get; private set; } = 1000;

        public void AddMoney(int amount)
        {
            Money += amount;
            onMoneyChanged?.Invoke(amount);
        }
        public void TryUpgradePart(int slotId, int cost)
        {
            if (Money >= cost)
            {
                Money -= cost;
                onMoneyChanged?.Invoke(-cost);
                onLevelChanged?.Invoke(slotId);
            }
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
