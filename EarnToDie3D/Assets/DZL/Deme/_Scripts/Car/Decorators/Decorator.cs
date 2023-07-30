using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public abstract class Decorator : MonoBehaviour
    {
        [SerializeField] protected DecoratorData _data;
        [SerializeField] protected GameObject _visual;
        [SerializeField] protected AudioClip _soundClip;
        public DecoratorType Type => _data.type;

        public virtual void Initialize(CurrentCarData carData)
        {
            switch (Type)
            {
                case DecoratorType.Blade:
                    _data = carData.bladeData;
                    break;
                case DecoratorType.Gun:
                    _data = carData.gunData;
                    break;
                case DecoratorType.Turbo:
                    _data = carData.turboData;
                    break;
            }
            SetActive(_data.isUnlocked);
        }

        public abstract void Animate();
        public abstract void PlaySound();
        public virtual void SetActive(bool activeSelf)
        {
            _visual.SetActive(activeSelf);
        }
    }
}
