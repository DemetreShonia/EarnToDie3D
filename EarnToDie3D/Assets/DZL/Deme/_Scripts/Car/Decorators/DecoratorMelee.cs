using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public class DecoratorMelee : Decorator
    {

        // this can be called in the car controller
        public override void Animate()
        {
            // KNIFE ANIMATION... 
        }

        public override void PlaySound()
        {
            // ALSO SOUND TOO...
        }

        void OnCollisionEnter(Collision collision)
        {
            // target Hit Logic
            print(collision.gameObject);
        }
    }
}
