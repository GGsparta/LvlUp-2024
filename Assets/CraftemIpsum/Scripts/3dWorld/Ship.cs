using System;
using System.Collections;
using System.Collections.Generic;
using CraftemIpsum;
using GGL.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ship : MonoBehaviour
{
    private const bool inverseControl = false;
    private const int upModifier = inverseControl ? 1 : -1;
    
    
    private const float MIN_MOVEMENT_MAGNITUDE = 0.1f;
    private const float VELOCITY = 15f;
   
    //cas gimbal lock
    private const float MOVEMENT_COEFF = 50f;
    
    private Rigidbody body;

    //private Quaternion rotation = Quaternion.identity;
    private Vector3 rotation = Vector3.zero;
    
    [SerializeField]
    private Camera camera;

    private List<Waste> wasteList;
    private PlayerInput input;
    

    
    
    private void Start()
    {
        wasteList = new List<Waste>();
        body = GetComponent<Rigidbody>();
        rotation = transform.eulerAngles;
        body.velocity = transform.forward * VELOCITY;
        
        input = GetComponent<PlayerInput>();
        input.actions["Shoot"].performed += DoShoot;
    }

    private void DoShoot(InputAction.CallbackContext obj)
    {
        if (wasteList.Count > 0)
        {
            Waste waste = wasteList[0];
            wasteList.RemoveAt(0);

            Quaternion rotation = transform.rotation;
            if (waste.Type == WasteType.EXHAUST)
            {
                rotation *= Quaternion.Euler(0 ,0, 90);
            }
            waste.SetRotation(rotation);

            float xDecalage = 3;
            if (waste.Type == WasteType.EXHAUST) xDecalage = 4;
            
            waste.transform.position = transform.position + transform.forward * xDecalage;
            waste.gameObject.SetActive(true);
            waste.Fire();
        }
    }

    private void Update()
    {
        // [0, 1]
        var mousePosition = camera.ScreenToViewportPoint(Input.mousePosition);
        
        // [-1, 1]
        mousePosition.x = mousePosition.x.Clamp(0,1).Remap(0,1,-1,1);
        mousePosition.y = mousePosition.y.Clamp(0,1).Remap(0,1,-1,1) * upModifier;
        mousePosition.z = 0f;
        
     /* rotation *= Quaternion.Euler(new Vector3(mousePosition.y, mousePosition.x, 0) * (MOVEMENT_COEFF * Time.deltaTime));
      transform.localRotation = rotation;*/
     
     if ((Vector3.Angle(transform.forward, Vector3.up) > 5 && Vector3.Angle(transform.forward, -Vector3.up) > 5) ||
         (Vector3.Angle(transform.forward, Vector3.up) < 5 && mousePosition.y > 0) ||
         (Vector3.Angle(transform.forward, -Vector3.up) < 5 && mousePosition.y < 0))
     {
         rotation += new Vector3(mousePosition.y, mousePosition.x, 0) * (MOVEMENT_COEFF * Time.deltaTime);
     }
     else
     {
         rotation += new Vector3(0, mousePosition.x, 0) * (MOVEMENT_COEFF * Time.deltaTime);
     }
     
     body.MoveRotation(Quaternion.Euler(rotation));
      

      body.velocity = transform.forward * VELOCITY;

    }

    private void OnTriggerEnter(Collider other)
    {
        Waste waste = other.GetComponent<Waste>();
        if (waste != null)
        {
            wasteList.Add(waste);
            //waste.enabled = false;
            waste.gameObject.SetActive(false);
        }
    }
    
    private void OnEnable()
    {
        if(input != null) input.actions["Shoot"].Enable();
    }

    private void OnDisable()
    {
        if(input != null) input.actions["Shoot"].Disable();
    }
}
