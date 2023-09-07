using UnityEngine;

namespace DumbRide
{
    public class ZombieManager : MonoBehaviour
    {
        [SerializeField] ZombieController _zombieManPrefab, _zombieWomanPrefab, _zombieAxePrefab;
        [SerializeField] Transform _chunksParent;
        ZombieChunkManager[] _zombieChunkManagers;


        void Awake()
        {
            _zombieChunkManagers = _chunksParent.GetComponentsInChildren<ZombieChunkManager>();
            foreach (var manager in _zombieChunkManagers)
            {
                manager.Initialize(_zombieManPrefab, _zombieWomanPrefab, _zombieAxePrefab);
            }
        }
    }
}
