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

        public void Initialize(Sprite sprite, int curLvl, int maxLvl, int price, int id)
        {
            _slotId = id;
            _price = price;
            _img.sprite = sprite;
            

            _lvlContainer.Initialize(curLvl, maxLvl, price);

            _button = GetComponentInChildren<Button>();
            _economyManager = EconomyManager.Instance;

            _button.onClick.AddListener(HandleUpgradePartButton);
            _economyManager.onLevelChanged += OnLevelChanged;
            //_button.onClick.AddListener
            
        }
        void HandleUpgradePartButton()
        {
            if(_lvlContainer.CanUpgrade)
                _economyManager.TryUpgradePart(_slotId, _price);
        }
        void OnLevelChanged(int slotID)
        {
            if(_slotId == slotID)
            {
                _lvlContainer.IncreaseLevel();
            }
        }
    }
}
