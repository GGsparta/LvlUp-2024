using UnityEngine;
using UnityEngine.UI;

public class PlayerDisplay : MonoBehaviour
{
    [SerializeField] private Slider lifebar;
    [SerializeField] private MoneyCarryer carryer;
    [SerializeField] private Image avatar;
    [SerializeField] private Sprite aliveSprite;
    [SerializeField] private Sprite stunSprite;

    public void SetLife(float percentage)
    {
        lifebar.value= percentage;
        avatar.color = percentage <= 0 ? Color.gray : Color.white;
        avatar.sprite = percentage <= 0 ? stunSprite : aliveSprite;
    }

    public void SetMoney(int money) => carryer.SetMoney(money);
}
