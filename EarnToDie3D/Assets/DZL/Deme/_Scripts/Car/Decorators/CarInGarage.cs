using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public class CarInGarage : MonoBehaviour
    {
        // enable or disable decorators
        [SerializeField] GameObject _blade;
        [SerializeField] GameObject _turbo;
        [SerializeField] GameObject _gun;
        
        public void SwitchDecorators(GarageCarData carData)
        {
            var lvls = carData.partLevels;
            _blade.SetActive(lvls[(int)PartEnum.Blade] > 0);
            _turbo.SetActive(lvls[(int)PartEnum.Turbo] > 0);
            _gun.SetActive(lvls[(int)PartEnum.Gun] > 0);
        }
    }
}
