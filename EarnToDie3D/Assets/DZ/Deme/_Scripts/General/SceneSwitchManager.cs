using UnityEngine;
using UnityEngine.SceneManagement;

namespace DumbRide
{
    public class SceneSwitchManager : MonoBehaviourSingleton<SceneSwitchManager>
    {
        [SerializeField] AudioClip[] _bgMusics;
        AudioSource _audioSource;
        int _currentSceneId;
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
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
    }
}
