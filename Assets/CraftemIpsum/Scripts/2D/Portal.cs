using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using GGL.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CraftemIpsum._2D
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private Waste[] wastePrefabs;
        [SerializeField] private Light spotlight;

        [Header("Color")] public PortalColor color;
        [SerializeField] private Color color1;
        [SerializeField] private Color color2;

        [Header("Animation")] [SerializeField] private float breathingDuration;


        [ContextMenu(nameof(SpawnWaste))]
        public void SpawnWaste() => StartCoroutine(ESpawnWaste());

        private IEnumerator ESpawnWaste()
        {
            yield return new WaitForSeconds(.5f);
            SpawnWaste(Enum.GetValues(typeof(WasteType)).OfType<WasteType>().Shuffle().First());
        }

        private void Start()
        {
            spotlight.color = color1;
            spotlight.DOColor(color2, breathingDuration)
                .SetEase(Ease.InOutFlash)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public void SpawnWaste(WasteType type)
        {
            GameObject go = Instantiate(wastePrefabs.First(w => w.Type == type).gameObject);
            go.transform.position = transform.position;
            go.transform.Rotate(Vector3.forward * Random.Range(-180f, 180f));
            go.GetComponent<Rigidbody2D>().velocity = -transform.up * 10f;

            DOTween.Kill(this, true);
            transform.localScale = Vector3.one * 1.2f;
            transform.DOScale(Vector3.one, .5f)
                .SetEase(Ease.OutBack)
                .SetTarget(this);
        }
    }
}