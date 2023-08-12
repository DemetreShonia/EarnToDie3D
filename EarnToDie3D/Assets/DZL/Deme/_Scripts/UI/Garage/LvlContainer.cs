using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DumbRide
{
    public class LvlContainer : MonoBehaviour
    {
        [SerializeField] LvlBar _lvlBar;
        [SerializeField] TextMeshProUGUI _priceText;

        int _currentLvl = 1;
        int _maxLvl = 1;
        public void Initialize(int currentLvl, int maxLvl, int price)
        {
            _currentLvl = currentLvl;
            _maxLvl = maxLvl;

            if (currentLvl == maxLvl)
                _priceText.text = "MAX";
            else
                _priceText.text = $"${price.ToString()}";
        }
        void Start()
        {
            for (int i = 0; i < _maxLvl; i++)
            {
                var bar = Instantiate(_lvlBar, transform);

                if(i <= _currentLvl)
                    bar.EnableBar();
            }
        }

    }
}
