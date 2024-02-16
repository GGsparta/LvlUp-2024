using DG.Tweening;
using GGL.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyAnim : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private Image image;
    [SerializeField] private int trembleGap;
    [SerializeField] private float trembleForce = .5f;
    [SerializeField] private float duration = 2f;
    [SerializeField] private float entrance = .3f;

    public void Pop(Vector2 pos, int money)
    {
        transform.localScale = Vector3.one * .001f;
        ((RectTransform)transform).anchoredPosition = pos;
        
        text.text = $"{(money > 0 ? '+' : '-')} <b>{money.Abs()}</b> $";
        image.color = money > 0 ? Color.white : Color.red;
        Vector3 scale = Vector3.one * Mathf.LerpUnclamped(0.7f, 1f, Mathf.Abs((float)money) / trembleGap);

        transform.DOScale(scale, entrance).SetEase(Ease.OutBack);
        canvas.DOFade(0, duration).SetDelay(entrance).OnComplete(() => Destroy(gameObject));
        image.transform.DOShakePosition(duration + entrance, trembleForce + (money.Abs() * trembleForce / trembleGap).Clamp(1));
        text.transform.parent.DOShakePosition(duration + entrance, trembleForce + (money.Abs() * trembleForce / trembleGap).Clamp(1));
        transform.DOLocalMoveY(duration * 60, duration).SetDelay(entrance).SetEase(Ease.InSine);
    }
}
