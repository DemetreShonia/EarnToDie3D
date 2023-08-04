using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public class LvlContainer : MonoBehaviour
    {
        [SerializeField] LvlBar _lvlBar;

        int _currentLvl = 1;
        int _maxLvl = 1;
        public void Initialize(int currentLvl, int maxLvl)
        {
            _currentLvl = currentLvl;
            _maxLvl = maxLvl;
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
