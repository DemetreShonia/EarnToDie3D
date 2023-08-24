using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

namespace DumbRide
{
    public class LvlContainer : MonoBehaviour
    {
        [SerializeField] LvlBar _lvlBar;
        [SerializeField] TextMeshProUGUI _priceText;

        List<LvlBar> _lvlBarList = new List<LvlBar>();

        public int CurrentLevel => _currentLvl;
        int _currentLvl = 1;
        int _maxLvl = 1;
        int _price = 0;

        public bool CanUpgrade => _currentLvl < _maxLvl;
        public void Initialize(int currentLvl, int maxLvl, int price)
        {
            _currentLvl = currentLvl;
            _maxLvl = maxLvl;
            _price = price;

            for (int i = 0; i < _maxLvl; i++)
            {
                var bar = Instantiate(_lvlBar, transform);
                _lvlBarList.Add(bar);
            }

            SanityCheck();
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
