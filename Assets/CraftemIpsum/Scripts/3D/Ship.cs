using System.Collections.Generic;
using GGL.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CraftemIpsum._3D
{
    public class Ship : MonoBehaviour
    {
        [SerializeField] private AudioSource collectSound;
        [SerializeField] private AudioSource shootSound;
        [SerializeField] private Transform model;
        [SerializeField] private new Camera camera;
    
        private const bool INVERSE_CONTROL = false;
        
        // ReSharper disable once HeuristicUnreachableCode
        private const int UP_MODIFIER = INVERSE_CONTROL ? 1 : -1;
        private const float VELOCITY = 15f;
        private const float MOVEMENT_FACTOR = 50f;
    
        private Rigidbody _body;
        private Quaternion _rotation = Quaternion.identity;
        private Quaternion _defaultModelRotation;
        private List<Waste> _wasteList;
        private PlayerInput _input;
        
    
        private void Start()
        {
            _wasteList = new List<Waste>();
            _body = GetComponent<Rigidbody>();
            _rotation = transform.rotation;
            _defaultModelRotation = model.localRotation;
            _body.velocity = transform.forward * VELOCITY;
        
            _input = GetComponent<PlayerInput>();
            _input.actions["Shoot"].performed += DoShoot;
        }

        private void DoShoot(InputAction.CallbackContext obj)
        {
            if (_wasteList.Count <= 0) return;
            
            Waste waste = _wasteList[0];
            _wasteList.RemoveAt(0);

            Quaternion rot = model.rotation * Quaternion.Inverse(_defaultModelRotation);
            if (waste.Type == WasteType.EXHAUST)
            {
                rot *= Quaternion.Euler(0 ,0, 90);
            }
            waste.SetRotation(rot);

            float xOffset = 5;
            switch (waste.Type)
            {
                case WasteType.EXHAUST:
                case WasteType.SUSPENSION:
                    xOffset = 4;
                    break;
            }

            waste.transform.position = transform.position + model.right * xOffset;
            waste.gameObject.SetActive(true);
            waste.Fire();
            shootSound.Play();
        }

        private void Update()
        {
            if (GameManager.Exists && !GameManager.Instance.IsPlaying)
            {
                _body.velocity = Vector3.zero;
                return;
            }

            // [0, 1]
            Vector3 mousePosition = camera.ScreenToViewportPoint(Input.mousePosition);
        
            // [-1, 1]
            mousePosition.x = mousePosition.x.Clamp(0,1).Remap(0,1,-1,1);
            mousePosition.y = mousePosition.y.Clamp(0,1).Remap(0,1,-1,1) * UP_MODIFIER;
            mousePosition.z = 0f;
        
            // Rotate ship
            _rotation *= Quaternion.Euler(new Vector3(mousePosition.y, mousePosition.x, 0) * (MOVEMENT_FACTOR * Time.deltaTime));
            _body.MoveRotation(_rotation);
            _body.velocity = transform.forward * VELOCITY;
            
            // Effect rotation on model
            model.localRotation = _defaultModelRotation
                                  * Quaternion.Euler(Vector3.up * Mathf.SmoothStep(-50f, 50f, mousePosition.y.Remap(-1,1,0,1)))
                                  * Quaternion.Euler(Vector3.forward * Mathf.SmoothStep(-30f, 30f, mousePosition.x.Remap(-1,1,0,1)))
                                  * Quaternion.Euler(Vector3.right * Mathf.SmoothStep(80f, -80f, mousePosition.x.Remap(-1,1,0,1)))
                                  ;
        }

        private void OnTriggerEnter(Collider other)
        {
            Waste waste = other.GetComponent<Waste>();
            if (!waste) return;
            
            _wasteList.Add(waste);
            waste.gameObject.SetActive(false);
            
            collectSound.Play();
        }
    
        private void OnEnable()
        {
            if(_input) _input.actions["Shoot"].Enable();
        }

        private void OnDisable()
        {
            if(_input) _input.actions["Shoot"].Disable();
        }
    }
}
