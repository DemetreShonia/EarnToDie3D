using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public class GarageSlot : MonoBehaviour
    {
        [SerializeField] LvlContainer _lvlContainer;
        [SerializeField] UnityEngine.UI.Image _img;
        // Start is called before the first frame update

        public void Initialize(Sprite sprite, int curLvl, int maxLvl)
        {
            _img.sprite = sprite;
            _lvlContainer.Initialize(curLvl, maxLvl);
        }
    }
}
