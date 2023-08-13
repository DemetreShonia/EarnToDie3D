using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DumbRide
{
    public class DecoratorMelee : Decorator
    {
        [SerializeField] CarHitter _meleeHitter;
        [SerializeField] CarHitter _defaultHitter;
        [SerializeField] float _meleeDamage;
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
                
            _defaultHitter.gameObject.SetActive(!activeSelf);
            _meleeHitter.gameObject.SetActive(activeSelf);

            if(activeSelf) _meleeHitter.SetDamageMultiplier(_meleeDamage);


        }
    }
}
