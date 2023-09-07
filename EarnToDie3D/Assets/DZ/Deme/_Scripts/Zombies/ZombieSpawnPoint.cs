using UnityEngine;

namespace DumbRide
{
    public enum ZombieType
    {
        Man, Woman, Axe
    }
    public class ZombieSpawnPoint : MonoBehaviour
    {
        [SerializeField] ZombieType _zombieType;
        public ZombieType ZombieType => _zombieType;
    }
}
