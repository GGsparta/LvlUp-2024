using CraftemIpsum._3D;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CraftemIpsum.UI
{
    public class BoostDisplayer : MonoBehaviour
    {
        [SerializeField] private Ship ship;
        [SerializeField] private Image boostFill;

        private Outline _outline;
        
        private void Awake()
        {
            _outline = boostFill.GetComponent<Outline>();
            LaunchIdle();
            
            ship.OnBoost += LaunchFill;
        }

        private void LaunchIdle()
        {
            DOTween.Kill(this);
            boostFill.fillAmount = 1;
            _outline.enabled = true;
        }

        private void LaunchFill(float duration)
        {
            DOTween.Kill(this);
            
            boostFill.fillAmount = 0;
            _outline.enabled = false;

            boostFill.DOFillAmount(1, duration)
                .SetEase(Ease.OutSine)
                .OnComplete(LaunchIdle)
                .SetTarget(this);
        }
    }
}