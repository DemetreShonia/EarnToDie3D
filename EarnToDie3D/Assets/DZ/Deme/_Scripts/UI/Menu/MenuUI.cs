using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DumbRide
{
    public class MenuUI : MonoBehaviour
    {
        [SerializeField] Button _playButton;
        void OnEnable() => _playButton.onClick.AddListener(HandlePlayClick);
        void OnDisable() => _playButton.onClick.RemoveListener(HandlePlayClick);
        void HandlePlayClick() => SceneSwitchManager.Instance.SwitchScene(1);
    }
}
