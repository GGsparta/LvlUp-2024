using System.Collections.Generic;
using System.Linq;
using GGL.Extensions;
using GGL.Pooling;
using UnityEngine;
using UnityEngine.UI;

public class MoneyCarryer : MonoBehaviour
{
    [SerializeField] private Sprite lowMoney;
    [SerializeField] private Sprite normalMoney;
    [SerializeField] private Sprite highMoney;
    [SerializeField] private MoneyAnim moneyAnim;
    [Space]
    [SerializeField] private int money;
    [SerializeField] private float slope = 1;
    [SerializeField] private int normalMoneyRef;

    private float _minSize;
    private Image _prefab;
    private HashSet<Image> _items = new();
    private int[] _moneyStacks;

    public void Start()
    {
        _prefab = transform.GetChild(0).GetComponent<Image>();
        _minSize = ((RectTransform)_prefab.transform).GetHeight();
        _prefab.gameObject.SetActive(false);
    }

    public void SetMoney(int newMoney)
    {
        if(!enabled || newMoney == money) return;
        Instantiate(moneyAnim, transform.parent).Pop(Vector2.up * 50 + Random.insideUnitCircle * 50, newMoney - money);
        money = newMoney;
        ComputeCurve();
        RefreshGraphics();
    }

    private void ComputeCurve()
    {
        /* Well let's explain this one. Brief resume:
         * 
         * WHAT WE NEED : A stack of money representing our money
         * WHAT WE HAVE : A money count - let's say it's the area of the curve
         *
         *               _ _ _ _ _  
         *         _ _ ⟋     |     ⟍ _ _
         *     _ ⟋          h|           ⟍ _ 
         *   ⟋_______________|_______________⟍
         * -a   -2    -1     0     1     2    a
         * 
         * The curve of this stack is defined by:
         *
         *   f(x) = h - sx²                                 (where s defines the slope) 
         *
         * And f=0 tells us the end of our stack: a and -a.
         *
         *   a = sqrt( h/s )
         *
         * Each stack will resolve f(x) to get the value to display. The only variable missing is h and it can be found
         * thanks to the area which is the integral of the curve between -a and a. We will need to find the primitive of
         * f(x) so in order to express the area A.
         *
         *   F*(x) = hx - sx³/3 + C                         (where C is a constant - but you can ignore it in this case)
         *
         * As the curve has the same area on each side of h, let's just compute one side and make it double:
         * 
         *   A = 2( ha - sa³/3 )
         *
         * The whole system is set up, we can replace a by its definition (see above) to find h.
         *
         *   A = 2( h * sqrt( h/s ) - s * sqrt( h/s )³/3 )
         *   h = (1/2) * 3^(2/3) * (s/2)^(1/3) * A^(2/3)    (don't hurt yourself, give it to wolfram alpha :) )
         */
        
        float
            area = money,
            s = slope,
            h = 0.5f * Mathf.Pow(3, 2f/3) * Mathf.Pow(s/2, 1f/3) * Mathf.Pow(area, 2f / 3),
            a = Mathf.Sqrt(h / s);
        int
            length = 1 + 2 * (int)(a + 0.5f),
            mid = length / 2;
        _moneyStacks = new int[length];
        
        // Integral between [x-0.5,x+0.5] with: x ∈ ℤ
        float AreaAt(int x)
        {
            float 
                a0 = (x - 0.5f).Clamp(-a, a), 
                a1 = (x + 0.5f).Clamp(-a, a);
            return h * a1 - h * a0 + (s / 3) * (Mathf.Pow(a0, 3) - Mathf.Pow(a1, 3));
        } 
        
        for (int i = 0; i < length; i++) 
            _moneyStacks[i] = Mathf.RoundToInt(AreaAt(i - mid));

        int diff = money - _moneyStacks.Sum();
        for (int i = Random.Range(0, _moneyStacks.Length); diff != 0; i++)
        {
            int increment = diff < 0 ? 1 : -1;
            _moneyStacks[i] -= increment;
            diff += increment;
            if (i >= _moneyStacks.Length - 1)
                i = -1;
        }
    }

    private void RefreshGraphics()
    {
        foreach (Image child in _items) Destroy(child.gameObject);
        _items.Clear();

        for (int index = 0; index < _moneyStacks.Length; index++)
        {
            int stack = _moneyStacks[index];
            Image item = Instantiate(_prefab, transform);
            item.gameObject.SetActive(true);
            item.transform.SetAsLastSibling();
            ((RectTransform)item.transform).SetHeight(_minSize + (stack - normalMoneyRef) * _minSize / normalMoneyRef);
            item.sprite = stack < normalMoneyRef * .75f ? lowMoney : index % 2 != 0 ? normalMoney : highMoney;

            _items.Add(item);
        }
    }
}