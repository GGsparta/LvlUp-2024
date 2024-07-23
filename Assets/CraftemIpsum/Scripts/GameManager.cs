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

        private Stopwatch _stopwatch;

        public event Action OnPlay;
        public event Action OnPause;

        public bool IsPlaying { get; private set; }
        public int Score { get; private set; }
        public int SecondsLeft { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            _stopwatch = new Stopwatch();
            SecondsLeft = durationInSeconds;
            Invoke(nameof(Play), delayBeforeStart);
        }

        private void Update()
        {
            SecondsLeft = durationInSeconds - (int)_stopwatch.Elapsed.TotalSeconds;
        }

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
    }
}
