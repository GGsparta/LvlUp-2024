using System;
using System.Collections.Generic;
using GGL.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

namespace CraftemIpsum._2D
{
    public class Player : MonoBehaviour
    {
        
        [Header("Parameters")]
        [SerializeField] private float speed = 2f;
        [SerializeField] private float inAirMovingAbility = 0.75f;
        [SerializeField] private float jumpForce = 3f;
        [SerializeField] private float maxSpeed = 5f;
        [SerializeField] private float groundedDamping = 0;
        [Header("Content")]
        [SerializeField] private Transform transportSpot;
        [SerializeField] private Animator animator;
        [Header("Sounds")]
        [SerializeField] private AudioSource jumpSound;
        [SerializeField] private AudioSource collectSound;


        private InputAction _walk, _jump, _pickUp;
        private Rigidbody2D _rigidbody;
        private List<GameObject> _wastesAround = new();
        private bool _grounded;
        private Waste _current;

        private static readonly int Grounded = Animator.StringToHash("Grounded");
        private static readonly int Moving = Animator.StringToHash("Moving");
        private float _input;


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ground")) animator.SetBool(Grounded, _grounded = true);
            else if (other.CompareTag("Waste"))
            {
                DoPickUp(other.gameObject);
                _wastesAround.Add(other.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Ground")) animator.SetBool(Grounded, _grounded = false);
            else if (other.CompareTag("Waste"))
            {
                _wastesAround.Remove(other.gameObject);
            }
        }

        private void Awake()
        {
            PlayerInput input = GetComponent<PlayerInput>();
            _walk = input.actions["Walk"];
            _pickUp = input.actions["PickUp"];
            _jump = input.actions["Jump"];
            _rigidbody = GetComponent<Rigidbody2D>();
            _grounded = false;

            _jump.performed += DoJump;
            _pickUp.performed += DoCast;
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
            _rigidbody.simulated = !GameManager.Exists || GameManager.Instance.IsPlaying;
            if (GameManager.Exists && !GameManager.Instance.IsPlaying)
                return;
            DoWalk();
            UpdateGraphics();
        }

        private void DoPickUp(GameObject go)
        {
            if (GameManager.Exists && !GameManager.Instance.IsPlaying)
                return;
            if (_current) return;
            _current = go.GetComponentInParent<Waste>();
            go.GetComponent<SpriteRenderer>().sortingOrder = 0;

            _current.GetComponent<Rigidbody2D>().simulated = false;
            _current.transform.SetParent(transform);
            _current.transform.localScale = Vector3.one;
            _current.transform.position = transportSpot.position;
            _current.transform.rotation = transportSpot.rotation;
            collectSound.Play();
        }

        private void DoCast(InputAction.CallbackContext obj)
        {
            if (GameManager.Exists && !GameManager.Instance.IsPlaying)
                return;
            if (!_current)
            {
                if(_wastesAround.Count > 0)
                    DoPickUp(_wastesAround[UnityEngine.Random.Range(0, _wastesAround.Count)]);
                return;
            }

            if (IsInvoking(nameof(ReleaseObject)))
            {
                CancelInvoke(nameof(ReleaseObject));
                ReleaseObject();
                return;
            }

            var r = _current.GetComponent<Rigidbody2D>();
            r.velocity = _rigidbody.velocity;
            r.simulated = true;
            _current.transform.SetParent(null);
            _current.GetComponentInChildren<SpriteRenderer>().sortingOrder = 2;

            Invoke(nameof(ReleaseObject), .5f);
        }

        private void ReleaseObject() => _current = null;

        private void DoJump(InputAction.CallbackContext callbackContext)
        {
            if (GameManager.Exists && !GameManager.Instance.IsPlaying)
            {
                return;
            }

            if (!_grounded)
                return;

            _rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            _grounded = false;
            jumpSound.Play();
        }

        private void DoWalk()
        {
            _input = _walk.ReadValue<float>();
            transform.up = transform.position.normalized;

            if (_grounded)
            {
                _rigidbody.AddForce(transform.right * (_input * speed), ForceMode2D.Impulse);
                _rigidbody.velocity = _rigidbody.velocity.normalized * (Mathf.Min(_rigidbody.velocity.magnitude, maxSpeed) * (1 - groundedDamping));
            }
            else
            {
                _rigidbody.AddForce(transform.right * (_input * inAirMovingAbility), ForceMode2D.Force);
                _rigidbody.velocity = _rigidbody.velocity.normalized * Mathf.Min(_rigidbody.velocity.magnitude, maxSpeed);
            }
        }

        private void UpdateGraphics()
        {
            animator.SetBool(Moving, _rigidbody.velocity.magnitude > .5f);
            if(_input != 0f)
                transform.localScale = new Vector3(_input < 0 ? -1 : 1, 1, 1);
        }
    }
}
