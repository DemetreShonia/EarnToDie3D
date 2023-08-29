using UnityEngine;

namespace DumbRide
{
    public class Fuel
    {
        float _maxFuelCapacity;
        float _currentFuel;
        float _fuelConsumptionRate;
        public float FuelLeftPercent => _maxFuelCapacity != 0  ? _currentFuel / _maxFuelCapacity : 0;
        public bool IsTankEmpty => _currentFuel == 0f;

        public Fuel(float maxFuelCapacity, float currentFuel = 0, float fuelConsumptionRate = 0.0625f)
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
