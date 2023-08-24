using UnityEngine;

namespace DumbRide
{
    public class LvlBar : MonoBehaviour
    {
        [SerializeField] UnityEngine.UI.Image _bar;

        
        public void EnableBar()
        {
            _bar.enabled = true;
        }
    }
}
