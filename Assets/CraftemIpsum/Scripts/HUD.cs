using System;
using GGL.UI.Window;
using TMPro;
using UnityEngine;

namespace CraftemIpsum
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerDisplay;
        [SerializeField] private TextMeshProUGUI scoreDisplay;
        [SerializeField] private Popup gameOverPopup;
        [SerializeField] private TextMeshProUGUI goTimerDisplay;
        [SerializeField] private TextMeshProUGUI goScoreDisplay;


        private int _secondsLeftBuffer = -1;
        private int _scoreBuffer = -1;


        private void OnEnable() => GameManager.Instance.OnGameOver += ShowGameOver;
        private void OnDisable() => GameManager.Instance.OnGameOver -= ShowGameOver;

        private void Update()
        {
            if (GameManager.Instance.SecondsLeft != _secondsLeftBuffer)
            {
                _secondsLeftBuffer = GameManager.Instance.SecondsLeft;
                RefreshTimer();
            }
            
            if (GameManager.Instance.Score != _scoreBuffer)
            {
                _scoreBuffer = GameManager.Instance.Score;
                RefreshScore();
            }
        }

        private void RefreshTimer()
        {
            timerDisplay.text = $"{_secondsLeftBuffer / 60:00}:{_secondsLeftBuffer % 60:00}";
        }
        
        private void RefreshScore()
        {
            scoreDisplay.text = $"<b>{_scoreBuffer}</b><i><size=26> déchet(s) recyclé(s)</size></i>";
        }

        private void ShowGameOver()
        {
            goTimerDisplay.text = timerDisplay.text;
            goScoreDisplay.text = scoreDisplay.text;
            gameOverPopup.Open();
        }
    }
}
