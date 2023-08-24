using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    using UnityEngine;

    public class Timer
    {
        float _timeRemaining;
        bool _isTimerRunning;
        System.Action _onTimerComplete;

        // Constructor
        public Timer()
        {
            _timeRemaining = 0f;
            _isTimerRunning = false;
            _onTimerComplete = null;
        }

        // Start the timer with the specified duration in seconds
        public void StartTimer(float duration)
        {
            _timeRemaining = duration;
            _isTimerRunning = true;
        }

        // Stop the timer
        public void StopTimer()
        {
            _isTimerRunning = false;
        }

        // Update the timer manually
        public void UpdateTimer()
        {
            if (_isTimerRunning)
            {
                _timeRemaining -= Time.deltaTime;

                if (_timeRemaining <= 0f)
                {
                    _isTimerRunning = false;
                    _onTimerComplete?.Invoke();
                }
            }
        }

        // Get the current remaining time in seconds
        public float GetTimeRemaining()
        {
            return _timeRemaining;
        }

        // Check if the timer is currently running
        public bool IsTimerRunning()
        {
            return _isTimerRunning;
        }

        // Subscribe to the OnTimerComplete event
        public void SetOnTimerCompleteCallback(System.Action callback)
        {
            _onTimerComplete = callback;
        }
    }

}
