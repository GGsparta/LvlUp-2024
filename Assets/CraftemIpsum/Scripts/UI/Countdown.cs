using GGL.Audio.Player;
using GGL.UI.Window;
using TMPro;
using UnityEngine;

namespace CraftemIpsum.UI
{
    public class Countdown : MonoBehaviour
    {
        [SerializeField] private GameObject bg;
        [SerializeField] private TextMeshProUGUI display;
        [SerializeField] private AudioClip countdownFX;

        private int _countdown;

        public void StartCountdown()
        {
            _countdown = 3;
            SoundPlayer.Play(countdownFX);
            Invoke(nameof(Decrement), 1f);
        }

        private void Decrement()
        {
            if (_countdown == 0)
            {
                GetComponent<Popup>().Close();
                Destroy(gameObject);
                Destroy(bg);
                return;
            }

            _countdown--;
            display.text = _countdown.ToString();
            SoundPlayer.Play(countdownFX);
            Invoke(nameof(Decrement), 1f);
        }
    }
}
