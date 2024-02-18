using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CraftemIpsum._2D
{
    public class MachineTri : MonoBehaviour
    {
        [SerializeField] private WasteType wasteType;
        [SerializeField] private Transform wasteSpot;
        [SerializeField] private float wasteSpotRadius;

        public event Action<WasteType> OnRecycled;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Waste"))
            {
                Waste waste = other.GetComponentInParent<Waste>();
                if(waste && waste.Type == wasteType) 
                    AspireWaste(waste.gameObject);
            }
        }

        private void AspireWaste(GameObject go)
        {
            go.GetComponent<Rigidbody2D>().simulated = false;
            go.transform.SetParent(transform);
            go.transform.position = wasteSpot.position + (Vector3)Random.insideUnitCircle * wasteSpotRadius;
            go.GetComponentInChildren<SpriteRenderer>().material.color = new Color(1, 1, 1, 0.2f);

            OnRecycled?.Invoke(wasteType);
        }
    }
}
