using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public struct Constants
    {
        public const int CAR_COUNT_IN_GAME = 2;
    }
    public struct InputStrings
    {
        public const string HORIZONTAL = "Horizontal";
        public const string VERTICAL = "Vertical";
    }
    public struct AnimationStrings
    {
        public const string STAND_UP_FRONT = "StandUpFront";
        public const string STAND_UP_BACK = "StandUpBack";
        public const string FIRE = "Fire";
    }
    public struct TagStrings
    {
        public const string CAR_TAG = "Car";
        public const string WHEEL_TAG = "Wheel";
        public const string PLAYER_TAG = "Player";
        public const string MAIN_CAMERA_TAG = "MainCamera";
    }
    public struct LayerStrings
    {
        public const string GROUND_LAYER = "Ground";
    }
}
