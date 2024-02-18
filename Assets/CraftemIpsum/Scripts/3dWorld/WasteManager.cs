using System.Collections.Generic;
using System.Linq;
using CraftemIpsum;
using UnityEngine;
using UnityEngine.Events;

public class WasteManager : MonoBehaviour
{
    public UnityEvent<WasteData> wasteInPortal = new();

    private List<Waste> listOfWaste;

    [SerializeField] private Limits limits;
    
    // Start is called before the first frame update
    void Start()
    {
        listOfWaste = GetComponentsInChildren<Waste>().ToList();
    }

    private void Update()
    {
        // TODO Create waste randomnly
    }

    public void EmitWasteEvent(WasteData waste)
    {
        wasteInPortal.Invoke(waste);
    }

    public List<Waste> getNearWaste(Vector3 position, int number)
    {
        return listOfWaste
            .Where(waste => waste != null && !waste.IsDestroyedWaste && waste.gameObject.activeSelf)
            .OrderBy(waste => Vector3.Distance(position, waste.transform.position))
            .Take(number).ToList();
    }

    private void CreateWaste(int number)
    {
        var worldLimits = limits.GetWorldSize();
        for (int i = 0; i < number; i++)
        {
            float x = Random.Range(worldLimits.Item1, worldLimits.Item2);
            float y = Random.Range(worldLimits.Item3, worldLimits.Item4);
            float z = Random.Range(worldLimits.Item5, worldLimits.Item6);
        }
        
    }
}
