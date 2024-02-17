using System;
using System.Collections;
using System.Collections.Generic;
using GGL.Extensions;
using UnityEditor;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private const float MIN_MOVEMENT_MAGNITUDE = 0.1f;
    private const float MAX_VELOCITY_GAP = 0.05f;
    private const float VELOCITY = 15f;
    private const float ANGULAR_MOVEMENT_COEFF = 5f;
   
    //cas gimbal lock
    private const float GB_MOVEMENT_COEFF = 50f;
    private const float MOVEMENT_COEFF = 1f;
    
    
    // private const ForceMode FORCE_MODE = ForceMode.VelocityChange;
    //private const ForceMode FORCE_MODE = ForceMode.Impulse;
    private const ForceMode FORCE_MODE = ForceMode.Acceleration;
    
    private Rigidbody body;
    private Vector3 targetDir;
    private Quaternion targetRot;

    private Quaternion rotation = Quaternion.identity;
    
    [SerializeField]
    private Camera camera;

    
    
    private void Start()
    {
        body = GetComponent<Rigidbody>(); 
        //body.velocity = new Vector3(VELOCITY, 0, 0);

        targetDir = transform.forward * VELOCITY;
        body.velocity = targetDir;
        targetRot = transform.rotation;
    }
    private void FixedUpdate()
    {
        // [0, 1]
        var mousePosition = camera.ScreenToViewportPoint(Input.mousePosition);
        //Debug.Log(mousePosition);
        
        // [-1, 1]
        mousePosition.x = mousePosition.x.Clamp(0,1).Remap(0,1,-1,1);
        mousePosition.y = mousePosition.y.Clamp(0,1).Remap(0,1,-1,1);
        mousePosition.z = 0f;

        // rotation uniquement
        //mousePosition = new Vector3(mousePosition.y, mousePosition.x, 0);
        ////mousePosition = new Vector3(0, mousePosition.x, mousePosition.y);

        // gimbal lock
        /*targetDir = Quaternion.AngleAxis(mousePosition.y* (GB_MOVEMENT_COEFF * Time.fixedDeltaTime) , transform.right) * targetDir;
        targetDir = Quaternion.AngleAxis(mousePosition.x * (GB_MOVEMENT_COEFF * Time.fixedDeltaTime) , transform.up) * targetDir;
       // targetDir = Quaternion.AngleAxis(mousePosition.y* (GB_MOVEMENT_COEFF * Time.fixedDeltaTime) , transform.right) * targetDir;
        body.velocity = targetDir;
        body.rotation = Quaternion.LookRotation(targetDir);*/

       // gimbal lock 2
      /* var newDirection = targetDir.normalized;
       newDirection += transform.right * mousePosition.x * (MOVEMENT_COEFF * Time.fixedDeltaTime);
       newDirection += transform.up * mousePosition.y * (MOVEMENT_COEFF * Time.fixedDeltaTime);
       targetDir = newDirection * VELOCITY;
       body.velocity = targetDir;
       body.rotation = Quaternion.LookRotation(targetDir);*/

      /*var rotRight = Quaternion.AngleAxis(mousePosition.x * (GB_MOVEMENT_COEFF * Time.fixedDeltaTime), transform.up) *
                     transform.right;
      
     targetRot = targetRot *
                  Quaternion.AngleAxis(mousePosition.x * (GB_MOVEMENT_COEFF * Time.fixedDeltaTime), transform.up)
                  * Quaternion.AngleAxis(mousePosition.y * (GB_MOVEMENT_COEFF * Time.fixedDeltaTime), rotRight);
      body.rotation = targetRot;
      */
      rotation *= Quaternion.Euler(new Vector3(mousePosition.y, mousePosition.x, 0) * (GB_MOVEMENT_COEFF * Time.fixedDeltaTime));
      transform.localRotation = rotation;
      
      //transform.rotation *= Quaternion.AngleAxis(mousePosition.x * (GB_MOVEMENT_COEFF * Time.fixedDeltaTime) , transform.up);
      body.velocity = transform.forward;

      //body.velocity = transform.forward * VELOCITY;
      //targetDir = targetRot * targetDir;
      /*body.velocity = targetDir;
        body.rotation = Quaternion.LookRotation(targetDir);*/

      // ignorer velocity

      /* targetDir += mousePosition * (MOVEMENT_COEFF * Time.fixedDeltaTime);
       targetDir = targetDir.normalized * VELOCITY;
       body.velocity = targetDir;*/

      //targetDir = targetDir + 


      /*if (mousePosition.magnitude >= MIN_MOVEMENT_MAGNITUDE)
      {*/
      /*Quaternion deltaRotation = Quaternion.Euler(mousePosition * (ANGULAR_MOVEMENT_COEFF * Time.fixedDeltaTime));
      //Debug.Log(mousePosition);
      body.MoveRotation(body.rotation * deltaRotation);
      */

      //
      //body.AddForce(mousePosition, FORCE_MODE);
      //}

      /* if ((body.velocity.magnitude - VELOCITY).Abs() < MAX_VELOCITY_GAP)
       {
           float factor = VELOCITY / body.velocity.magnitude;
           body.velocity *= factor;
       }*/
    }
}
