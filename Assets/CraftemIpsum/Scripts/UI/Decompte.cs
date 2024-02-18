using GGL.UI.Window;
using TMPro;
using UnityEngine;

public class Decompte : MonoBehaviour
{
    [SerializeField] private GameObject bg;
    [SerializeField] private TextMeshProUGUI display;

    private int _compte;

    public void StartDecompte()
    {
        _compte = 3;
        Invoke(nameof(Decrement), 1f);
    }

    private void Decrement()
    {
        if (_compte == 0)
        {
            GetComponent<Popup>().Close();
            Destroy(gameObject);
            Destroy(bg);
            return;
        }

        _compte--;
        display.text = _compte.ToString();
        Invoke(nameof(Decrement), 1f);
    }
}
