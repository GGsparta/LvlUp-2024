using System;
using System.Collections.Generic;
using GGL.Components;
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
        public event Action<float> OnBoost;
        public event Action OnShoot;
        
        private const float VELOCITY = 15f;
        private const float MOVEMENT_FACTOR = 50f;
        private const float DASH_FACTOR = 3f;
        private const float DASH_DURATION = 2f;
        private const float DASH_DELAY = 2f;
        private static readonly Vector3 MODEL_ROTATION_EFFECT = new(0.89f, 0.56f, 0.33f);

        private Rigidbody _body;
        private Quaternion _rotation = Quaternion.identity;
        private Quaternion _defaultModelRotation;
        private List<Waste> _wasteList;
        private PlayerInput _input;
        private float _lastBoostUsage;
        private FollowPosition _cameraFollow;


        private void Start()
        {
            _wasteList = new List<Waste>();
            _body = GetComponent<Rigidbody>();
            _cameraFollow = camera.GetComponent<FollowPosition>();
            _rotation = transform.rotation;
            _defaultModelRotation = model.localRotation;
            _body.velocity = transform.forward * VELOCITY;
            _lastBoostUsage = Time.timeSinceLevelLoad;

            _input = GetComponent<PlayerInput>();
            _input.actions["Shoot"].performed += DoShoot;
            _input.actions["Boost"].performed += DoBoost;
        }

        private void Update()
        {
            if (GameManager.Exists && !GameManager.Instance.IsPlaying)
            {
                _body.velocity = Vector3.zero;
                return;
            }

            // Input in [0, 1] range
            Vector2 rawInput = camera.ScreenToViewportPoint(Input.mousePosition);
            rawInput.x = Settings.InvertedAxis ? 1 - rawInput.x : rawInput.x;
            rawInput.y = Settings.InvertedAxis ? rawInput.y : 1 - rawInput.y;

            // Input in [-1, 1] range
            Vector2 mouseInput = new(
                rawInput.x.Clamp(0, 1).Remap(0, 1, -1, 1),
                rawInput.y.Clamp(0, 1).Remap(0, 1, -1, 1));

            // Rotate ship
            _rotation *= Quaternion.Euler(new Vector3(mouseInput.y, mouseInput.x, 0) * (MOVEMENT_FACTOR * Time.deltaTime));
            _body.MoveRotation(_rotation);

            // Effect rotation on model
            model.localRotation = _defaultModelRotation
                                  * Quaternion.Euler(Vector3.up * Mathf.SmoothStep(-90f * MODEL_ROTATION_EFFECT.y,
                                      90f * MODEL_ROTATION_EFFECT.y, rawInput.y))
                                  * Quaternion.Euler(Vector3.forward * Mathf.SmoothStep(-90f * MODEL_ROTATION_EFFECT.z,
                                      90f * MODEL_ROTATION_EFFECT.z, rawInput.x))
                                  * Quaternion.Euler(Vector3.right * Mathf.SmoothStep(90f * MODEL_ROTATION_EFFECT.x,
                                      -90f * MODEL_ROTATION_EFFECT.x, rawInput.x));
            
            // Velocity & boost
            float boost = 1 - Mathf.Pow((Time.timeSinceLevelLoad - _lastBoostUsage) / DASH_DURATION, 3);
            _cameraFollow.flexibility = Mathf.SmoothStep(0, .9f, boost);
            float velocityScale = Mathf.SmoothStep(1f, DASH_FACTOR, boost);
            _body.velocity = transform.forward * (VELOCITY * velocityScale);
        }


        private void DoBoost(InputAction.CallbackContext obj)
        {
            if (Time.timeSinceLevelLoad - _lastBoostUsage < DASH_DURATION + DASH_DELAY) return;
            
            _lastBoostUsage = Time.timeSinceLevelLoad;
            OnBoost?.Invoke(DASH_DELAY + DASH_DURATION);
        }
        
        private void DoShoot(InputAction.CallbackContext obj)
        {
            if (_wasteList.Count <= 0) return;

            Waste waste = _wasteList[0];
            _wasteList.RemoveAt(0);

            Quaternion rot = model.rotation * Quaternion.Inverse(_defaultModelRotation);
            if (waste.Type == WasteType.EXHAUST)
            {
                rot *= Quaternion.Euler(0, 0, 90);
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
            
            OnShoot?.Invoke();
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
            if (_input) _input.actions["Shoot"].Enable();
        }

        private void OnDisable()
        {
            if (_input) _input.actions["Shoot"].Disable();
        }
    }
}