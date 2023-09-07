using UnityEngine;

namespace DumbRide
{
    public class ZombieChunkManager : MonoBehaviour
    {
        ZombieSpawnPoint[] _zombieSpawnPoints;

        ZombieController _zombieManPrefab, _zombieWomanPrefab, _zombieAxePrefab;

        [SerializeField] bool _areSpawnedByDefault = false;
        bool _areZombiesSpawned = false;

        public void Initialize(ZombieController zombieManPrefab, ZombieController zombieWomanPrefab, ZombieController zombieAxePrefab)
        {
            _zombieSpawnPoints = GetComponentsInChildren<ZombieSpawnPoint>();
            _zombieManPrefab = zombieManPrefab;
            _zombieWomanPrefab = zombieWomanPrefab;
            _zombieAxePrefab = zombieAxePrefab;

            if (_areSpawnedByDefault)
            {
                SpawnZombies();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(TagStrings.PLAYER_TAG))
            {
                if(_areZombiesSpawned) return;
                SpawnZombies();
            }
        }
        void SpawnZombies()
        {
            foreach(var point  in _zombieSpawnPoints)
            {
                switch(point.ZombieType)
                {
                    case ZombieType.Woman:
                        Instantiate(_zombieWomanPrefab, point.transform.position, point.transform.rotation);
                        break;
                    case ZombieType.Man:
                        Instantiate(_zombieManPrefab, point.transform.position, point.transform.rotation);
                        break;
                    case ZombieType.Axe:
                        Instantiate(_zombieAxePrefab, point.transform.position, point.transform.rotation);
                        break;
                }
            }
            _areZombiesSpawned = true;
        }
    }
}
