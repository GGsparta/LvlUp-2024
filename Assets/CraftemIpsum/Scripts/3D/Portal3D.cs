using UnityEngine;

namespace CraftemIpsum._3D
{
    public class Portal3D : MonoBehaviour
    {
        [SerializeField] private AudioSource depotSound;
        [field: SerializeField] public PortalColor Color { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.name.Contains("Waste"))
            {
                depotSound.Play();
            }
        }
    }
}
