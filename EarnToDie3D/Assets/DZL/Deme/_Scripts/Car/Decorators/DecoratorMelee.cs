using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DumbRide
{
    public class DecoratorMelee : Decorator
    {
        [SerializeField] float _defaultDamage;
        bool _multiplyDamage;
        // this can be called in the car controller
        public override void Animate()
        {
            // KNIFE ANIMATION... 
        }

        public override void PlaySound()
        {
            // ALSO SOUND TOO...
        }
       

        public override void SetActive(bool activeSelf)
        {
            base.SetActive(activeSelf);

           // meaning blade is shown and multiply damage when hitting zombies else use default damage
            _multiplyDamage = activeSelf;
        }
    }
}
