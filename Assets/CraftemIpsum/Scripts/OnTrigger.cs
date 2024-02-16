using UnityEngine;
using UnityEngine.Events;

namespace CraftemIpsum
{
    [RequireComponent(typeof(Collider))]
    public class OnTrigger : MonoBehaviour
    {
        [SerializeField]
        private Collider trigger;

        public UnityEvent onTrigger = new();

        private void OnTriggerEnter(Collider col)
        {
            if(col.CompareTag("Player"))
                onTrigger.Invoke();
        }
    }
}
