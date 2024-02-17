using System;
using GGL.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

namespace CraftemIpsum._2D
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float speed = 2f;
        [SerializeField] private float inAirMovingAbility = 0.75f;
        [SerializeField] private float jumpForce = 3f;
        [SerializeField] private float maxSpeed = 5f;
        [SerializeField] private float groundedDamping = 0;


        private InputAction _walk, _jump, _pickUp;
        private Rigidbody2D _rigidbody;
        private bool _grounded;
        
        public bool CanMove => true;


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ground")) _grounded = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Ground")) _grounded = false;
        }

        private void Awake()
        {
            PlayerInput input = GetComponent<PlayerInput>();
            _walk = input.actions["Walk"];
            _pickUp = input.actions["PickUp"];
            _jump = input.actions["Jump"];
            _rigidbody = GetComponent<Rigidbody2D>();
            _grounded = true;

            _jump.performed += DoJump;
        }

        private void OnEnable()
        {
            _walk.Enable();
            _pickUp.Enable();
            _jump.Enable();
        }

        private void OnDisable()
        {
            _walk.Disable();
            _pickUp.Disable();
            _jump.Disable();
        }

        public void Update()
        {
            if (CanMove)
            {
                DoWalk();
                //DoJump();
                DoPickUp();
            }

            UpdateGraphics();
        }

        private void DoPickUp()
        {
            // todo
        }

        private void DoJump(InputAction.CallbackContext callbackContext)
        {
            if (!_grounded)
                return;

            _rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            _grounded = false;
        }

        private void DoWalk()
        {
            float input = _walk.ReadValue<float>();

            if (_grounded)
            {
                _rigidbody.AddForce(transform.right * (input * speed), ForceMode2D.Impulse);
                _rigidbody.velocity = _rigidbody.velocity.normalized * (Mathf.Min(_rigidbody.velocity.magnitude, maxSpeed) * (1 - groundedDamping));
            }
            else
            {
                _rigidbody.AddForce(transform.right * (input * inAirMovingAbility), ForceMode2D.Force);
                _rigidbody.velocity = _rigidbody.velocity.normalized * Mathf.Min(_rigidbody.velocity.magnitude, maxSpeed);
            }
        }

        private void UpdateGraphics()
        {
            transform.up = transform.position.normalized;
        }
    }
}
