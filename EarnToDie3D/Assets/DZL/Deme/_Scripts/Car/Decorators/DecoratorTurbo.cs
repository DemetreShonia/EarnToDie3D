using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public class DecoratorTurbo : Decorator
    {
        CarEngine _engine;
        public override void Animate()
        {
        }

        public override void PlaySound()
        {

        }
        public override void CheckForInputs()
        {
            _engine.SetIsCarTurboEnabled(Input.GetKey(GlobalInputs.TURBO_KEY));
        }
        public void SetCarEngine(CarEngine engine) => _engine = engine;
    }
}
