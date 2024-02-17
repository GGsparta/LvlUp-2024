using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WasteManager : MonoBehaviour
{
    public UnityEvent<WasteType> wasteInPortal = new();
    
    // Start is called before the first frame update
    void Start()
    {
        // TODO Create waste
    }

    private void Update()
    {
        // TODO Create waste randomnly
    }

    public void EmitWasteEvent(WasteType type)
    {
        wasteInPortal.Invoke(type);
    }
}
