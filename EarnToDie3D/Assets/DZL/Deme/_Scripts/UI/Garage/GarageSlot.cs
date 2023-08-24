using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DumbRide
{
    public class GarageSlot : MonoBehaviour
    {
        [SerializeField] Transform _lvlBarContainer;
        [SerializeField] Image _img;
        [SerializeField] LvlBar _lvlBar;
        [SerializeField] TextMeshProUGUI _priceText;

        List<LvlBar> _lvlBarList = new List<LvlBar>();
        Button _button;
        EconomyManager _economyManager;

        int _slotId;
        int _price;
        int _currentLvl;
        int _maxLvl;

        // Start is called before the first frame update
        
        public void Initialize(Sprite sprite, int curLvl, int maxLvl, int price, int id)
        {
            _slotId = id;
            _price = price;
            _img.sprite = sprite;
            _maxLvl = maxLvl;
            _currentLvl = curLvl;
            
            CheckIsUnlocked(curLvl);

            for (int i = 0; i < _maxLvl; i++)
            {
                var bar = Instantiate(_lvlBar, _lvlBarContainer);
                _lvlBarList.Add(bar);
            }

            SanityCheck();

            _button = GetComponentInChildren<Button>();
            _economyManager = EconomyManager.Instance;

            _button.onClick.AddListener(HandleUpgradePartButton);
            _economyManager.onLevelChanged += OnLevelChanged;
            //_button.onClick.AddListener
            
        }
            
        void CheckIsUnlocked(int curLvl) => _img.color = curLvl == 0 ? Color.gray : Color.white;
        void HandleUpgradePartButton()
        {
            if(_currentLvl < _maxLvl)
                _economyManager.TryUpgradePart(_slotId, _price, _currentLvl);
        }
        void OnLevelChanged(int slotID, int newLevel)
        {
            if(_slotId == slotID)
            {
                IncreaseLevel();
                CheckIsUnlocked(newLevel);
            }
        }

        public void UpdatePrice(int price)
        {
            _priceText.SetText($"${price.ToString()}");
            _priceText.ForceMeshUpdate(true);
            print(price + "  IN UPDATE PRICE");
        }
        public void IncreaseLevel()
        {
            _currentLvl++; // this is stored, no worries
            SanityCheck();
            print(_currentLvl);
        }
        void SanityCheck()
        {
            if (_currentLvl == _maxLvl)
                _priceText.text = "MAX";
            else
                _priceText.text = $"${_price.ToString()}";

            FillBarsTillCurrentLevel();
        }

        void FillBarsTillCurrentLevel()
        {
            for (int i = 0; i < _currentLvl; i++)
            {
                _lvlBarList[i].EnableBar();
            }
        }
    }
}
