using System;
using System.Diagnostics;
using GGL.Singleton;
using UnityEngine;

namespace CraftemIpsum
{
    public class GameManager : CachedBehaviour<GameManager>
    {
        [SerializeField] private int durationInSeconds = 120;
        [SerializeField] private float delayBeforeStart = 4;
        [SerializeField] private Camera j1Camera;
        [SerializeField] private Camera j2Camera;
        
        public event Action OnPlay;
        public event Action OnPause;

        public bool IsPlaying { get; private set; }
        public int Score { get; private set; }
        public int SecondsLeft { get; private set; }
        
        
        private Stopwatch _stopwatch;
        private Rect _leftViewport;
        private Rect _rightViewport;

        
        protected override void Awake()
        {
            base.Awake();
            
            _stopwatch = new Stopwatch();
            SecondsLeft = durationInSeconds;
            _leftViewport = j1Camera.rect;
            _rightViewport = j2Camera.rect;

            SetupLayout();
            Invoke(nameof(Play), delayBeforeStart);
        }

        private void Update()
        {
            SecondsLeft = durationInSeconds - (int)_stopwatch.Elapsed.TotalSeconds;
        }

        private void OnEnable() => Settings.OnSettingsUpdated += SetupLayout;
        private void OnDisable() => Settings.OnSettingsUpdated -= SetupLayout;


        public void Play()
        {
            IsPlaying = true;
            _stopwatch.Start();
            OnPlay?.Invoke();
        }

        public void Pause()
        {
            IsPlaying = false;
            _stopwatch.Stop();
            OnPause?.Invoke();
        }

        public void IncrementScore()
        {
            Score++;
        }
        
        private void SetupLayout()
        {
            switch (Settings.Layout)
            {
                case Layout.J1_J2:
                    j1Camera.rect = _leftViewport;
                    j2Camera.rect = _rightViewport;
                    break;
                case Layout.J2_J1:
                    j1Camera.rect = _rightViewport;
                    j2Camera.rect = _leftViewport;
                    break;
            }
        }
    }
}
