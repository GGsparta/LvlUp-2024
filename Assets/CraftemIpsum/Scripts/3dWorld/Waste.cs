using UnityEngine;

public class Waste : MonoBehaviour
{
    [SerializeField]
    private WasteType wasteType;
    
    public WasteType TypeOfWaste => wasteType;
    
    private void OnTriggerEnter(Collider other)
    {
        
    }
}
