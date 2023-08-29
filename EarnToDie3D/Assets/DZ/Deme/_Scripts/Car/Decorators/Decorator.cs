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
        [SerializeField] protected DecoratorType _type;

        public bool IsUnlocked => _data != null && _data.isUnlocked;
        public DecoratorType Type => _type;

        public virtual void Initialize(DecoratorData data)
        {
            _data = data;
            SetActive(_data.isUnlocked);
        }

        public abstract void Animate();
        public abstract void PlaySound();
        public virtual void SetActive(bool activeSelf)
        {
            _visual.SetActive(activeSelf);
        }
        public virtual void CheckForInputs() { }
    }
}
