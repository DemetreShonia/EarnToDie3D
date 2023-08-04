using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public class Fuel
    {
        float _maxFuelCapacity = 100f;
        float _currentFuel = 100f;
        float _fuelConsumptionRate = 1f;
        public float FuelLeftPercent => _maxFuelCapacity != 0  ? _currentFuel / _maxFuelCapacity : 0;
        public bool IsTankEmpty => _currentFuel == 0f;

        public Fuel(float maxFuelCapacity, float currentFuel = 0, float fuelConsumptionRate = 1)
        {
            if (maxFuelCapacity == 0)
                Debug.LogWarning("Fuel capacity is 0");

            _maxFuelCapacity = maxFuelCapacity;
            _currentFuel = currentFuel == 0 ? maxFuelCapacity : _currentFuel;
            _fuelConsumptionRate = fuelConsumptionRate;
        }

        public void ConsumeFuel(float distanceTraveled)
        {
            _currentFuel -= distanceTraveled * _fuelConsumptionRate;
            _currentFuel = Mathf.Clamp(_currentFuel, 0f, _maxFuelCapacity);
        }

        public void RefillFuel()
        {
            _currentFuel = _maxFuelCapacity;
        }
    }
}
