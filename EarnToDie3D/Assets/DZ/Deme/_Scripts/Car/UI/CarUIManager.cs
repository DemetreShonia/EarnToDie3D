using UnityEngine;
namespace DumbRide
{
    public class CarUIManager : MonoBehaviourSingleton<CarUIManager>
    {
        [SerializeField] NeedleMeter _fuelMeter;
        [SerializeField] NeedleMeter _speedoMeter;
        [SerializeField] NeedleMeter _turboMeter;
        
        public void UpdateTurboMeter(float percent)
        {
            _turboMeter.UpdateCurrentAngle(percent);
        }
        public void UpdateFuelMeter(float percent)
        {
            _fuelMeter.UpdateCurrentAngle(percent);
        }
        public void UpdateSpeedoMeter(float percent)
        {
            _speedoMeter.UpdateCurrentAngle(percent);
        }
    }
}
