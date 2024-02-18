using UnityEngine;
using UnityEngine.Events;

namespace CraftemIpsum._2D
{
    public class TriManager : MonoBehaviour
    {
        [SerializeField] private MachineTri[] machines;
        [SerializeField] private UnityEvent<WasteType> recycledEvent;

        private void OnEnable()
        {
            foreach (MachineTri machineTri in machines)
            {
                machineTri.OnRecycled += recycledEvent.Invoke;
            }
        }

        private void OnDisable()
        {
            foreach (MachineTri machineTri in machines)
            {
                machineTri.OnRecycled -= recycledEvent.Invoke;
            }
        }
    }
}
