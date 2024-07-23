using GGL.UI.Window;
using TMPro;
using UnityEngine;

namespace CraftemIpsum.UI
{
    public class Countdown : MonoBehaviour
    {
        [SerializeField] private GameObject bg;
        [SerializeField] private TextMeshProUGUI display;

        private int _countdown;

        public void StartCountdown()
        {
            _countdown = 3;
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
            Invoke(nameof(Decrement), 1f);
        }
    }
}
