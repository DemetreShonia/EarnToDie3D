using UnityEngine;

namespace DumbRide
{
    public class CarManager : MonoBehaviourSingleton<CarManager>
    {
        [SerializeField] Transform _carTransform;
        public Transform CarTransform => _carTransform;
    }
}
