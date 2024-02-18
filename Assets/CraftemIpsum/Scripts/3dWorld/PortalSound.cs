using UnityEngine;

public class PortalSound : MonoBehaviour
{
    [SerializeField] private AudioSource depotSound;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Waste"))
        {
            depotSound.Play();
        }
    }
}
