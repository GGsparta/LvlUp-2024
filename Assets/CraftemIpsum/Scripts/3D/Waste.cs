using DG.Tweening;
using UnityEngine;

namespace CraftemIpsum._3D
{
    public class Waste : MonoBehaviour
    {
        private const float MOVING_DURATION = 5;
        private const float MOVING_MAX_SPEED = 35;
    
        [SerializeField] private WasteType wasteType;
        public WasteType Type => wasteType;
        public bool IsDestroyedWaste { get; private set; } = false;

        
        private Quaternion _initRotation;
        private bool _isMoving;
        private float _movingDuration;
        private WasteManager _manager;
        private Rigidbody _wasteBody;
        

        private void Start()
        {
            _initRotation = Quaternion.Euler(0, 0, 0);
            _isMoving = false;
            _manager = gameObject.GetComponentInParent<WasteManager>();
            _wasteBody = GetComponent<Rigidbody>();
        }

        public void SetRotation(Quaternion rotation) => transform.rotation = _initRotation * rotation;

        private void Update()
        {
            if (!_isMoving) return;
            
            if (_movingDuration >= MOVING_DURATION)
            {
                _isMoving = false;
                _movingDuration = 0;
            }
            else
            {
                float coeff = _movingDuration / MOVING_DURATION;
                transform.position += transform.forward * (Time.deltaTime * MOVING_MAX_SPEED *
                                                           Mathf.Cos(coeff * Mathf.PI / 2));
                _movingDuration += Time.deltaTime;
            }
        }

        public void Fire()
        {
            if (_wasteBody != null)
            {
                _wasteBody.velocity = Vector3.zero;
                _wasteBody.angularVelocity = Vector3.zero;
            }
            _isMoving = true;
            _movingDuration = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.name.Contains("WastePortal"))
            {
                PortalColor color = other.name switch
                {
                    "Red" => PortalColor.RED,
                    "Blue" => PortalColor.BLUE,
                    _ => PortalColor.GREEN
                };

                _manager.EmitWasteEvent(new WasteData
                {
                    type = wasteType,
                    portalColor = color
                });
                
                Destroy(gameObject);
                IsDestroyedWaste = true;
            }
            
            if (other.name.Contains("Wall"))
            {
                GetComponent<Collider>().enabled = false;

                transform.DOScale(Vector3.one * 0.001f, 4f)
                    .OnComplete(() => Destroy(gameObject));
            }
        }
    }
}
