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
        void OnTriggerEnter(Collider other)
        {
            var damage = (int) (_multiplyDamage ? _data.power : _defaultDamage);

            if(other.gameObject.TryGetComponent(out IBodyPart bodyPart))
            {
                bodyPart.AddForce(Vector3.one * 100, 1, damage); // multiplier is just 1 for now
            }
            // target Hit Logic
            print(other.gameObject);
        }
        //void OnCollisionEnter(Collision collision)
        //{
        //    var damage = (int) (_multiplyDamage ? _data.power : _defaultDamage);

        //    if(collision.gameObject.TryGetComponent(out IBodyPart bodyPart))
        //    {
        //        bodyPart.AddForce(collision.relativeVelocity, 1, damage); // multiplier is just 1 for now
        //    }
        //    // target Hit Logic
        //    print(collision.gameObject);
        //}

        public override void SetActive(bool activeSelf)
        {
            base.SetActive(activeSelf);

           // meaning blade is shown and multiply damage when hitting zombies else use default damage
            _multiplyDamage = activeSelf;
        }
    }
}
