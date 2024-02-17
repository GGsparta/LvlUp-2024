using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class WasteManager : MonoBehaviour
{
    public UnityEvent<WasteType> wasteInPortal = new();

    private List<Waste> listOfWaste;
    
    // Start is called before the first frame update
    void Start()
    {
        listOfWaste = GetComponentsInChildren<Waste>().ToList();
    }

    private void Update()
    {
        // TODO Create waste randomnly
    }

    public void EmitWasteEvent(WasteType type)
    {
        wasteInPortal.Invoke(type);
    }

    public List<Waste> getNearWaste(Vector3 position, int number)
    {
        return listOfWaste
            .Where(waste => waste != null && !waste.IsDestroyedWaste && waste.gameObject.activeSelf)
            .OrderBy(waste => Vector3.Distance(position, waste.transform.position))
            .Take(number).ToList();
    }
}
