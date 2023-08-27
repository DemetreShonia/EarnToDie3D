using UnityEngine;
using UnityEngine.SceneManagement;

namespace DumbRide
{
    public class SceneSwitchManager : MonoBehaviourSingleton<SceneSwitchManager>
    {
        public InGameCarData InGameCarData { get; private set; }

        [SerializeField] AudioClip[] _bgMusics;
        AudioSource _audioSource;
        int _currentSceneId;
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            _currentSceneId = SceneManager.GetActiveScene().buildIndex;
            _audioSource = GetComponent<AudioSource>();
            StartBgMusic();
        }
        public void SwitchScene(int sceneId)
        {
            _currentSceneId = sceneId;
            SceneManager.LoadScene(sceneId);
            StartBgMusic();
        }

        void StartBgMusic()
        {
            _audioSource.clip = _bgMusics[_currentSceneId];
            _audioSource.PlayDelayed(1);
        }
        public void PassInGameCarData(InGameCarData inGameCarData)
        {
            InGameCarData = inGameCarData;
        }
    }
}
