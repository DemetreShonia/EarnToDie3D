using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace DumbRide
{
    public class GarageSlot : MonoBehaviour
    {
        [SerializeField] LvlContainer _lvlContainer;
        [SerializeField] Image _img;
        Button _button;
        EconomyManager _economyManager;
        int _slotId;
        int _price;
        
        // Start is called before the first frame update
        public void UpdatePrice(int price)
        {
            _price = price;
            _lvlContainer.UpdatePrice(price);
        }
        public void Initialize(Sprite sprite, int curLvl, int maxLvl, int price, int id)
        {
            _slotId = id;
            _price = price;
            _img.sprite = sprite;
            
            CheckIsUnlocked(curLvl);

            _lvlContainer.Initialize(curLvl, maxLvl, price);

            _button = GetComponentInChildren<Button>();
            _economyManager = EconomyManager.Instance;

            _button.onClick.AddListener(HandleUpgradePartButton);
            _economyManager.onLevelChanged += OnLevelChanged;
            //_button.onClick.AddListener
            
        }
            
        void CheckIsUnlocked(int curLvl) => _img.color = curLvl == 0 ? Color.gray : Color.white;
        void HandleUpgradePartButton()
        {
            if(_lvlContainer.CanUpgrade)
                _economyManager.TryUpgradePart(_slotId, _price, _lvlContainer.CurrentLevel);
        }
        void OnLevelChanged(int slotID, int newLevel)
        {
            if(_slotId == slotID)
            {
                _lvlContainer.IncreaseLevel();
                CheckIsUnlocked(newLevel);
            }
        }
    }
}
