using UnityEngine;

namespace CraftemIpsum._3D
{
    public class Waste : MonoBehaviour
    {
        private const float MOVING_DURATION = 5;
        private const float MOVING_MAX_SPEED = 35;
    
        private Quaternion initRotation;
        private bool isMoving;
        private float movingDuration;

        public bool IsDestroyedWaste { get; private set; } = false;

        private WasteManager manager;
    
        [SerializeField]
        private WasteType wasteType;

        private Rigidbody wasteBody;
    
        public WasteType Type
        {
            get { return wasteType; }
        }
    

        private void Start()
        {
            //initRotation = Quaternion.Euler(wasteType == WasteType.SUSPENSION ? -90 : 0, 0, 0);
            initRotation = Quaternion.Euler(0, 0, 0);
            isMoving = false;
            manager = gameObject.GetComponentInParent<WasteManager>();
            wasteBody = GetComponent<Rigidbody>();
        }

        public void SetRotation(Quaternion q)
        {
            transform.rotation = initRotation * q;
        }

        private void Update()
        {
            if (isMoving)
            {
                if (movingDuration >= MOVING_DURATION)
                {
                    isMoving = false;
                    movingDuration = 0;
                }
                else
                {
                    var coeff = movingDuration / MOVING_DURATION;

                    /*if (wasteBody == null) 
                {*/
                    transform.position += transform.forward * (Time.deltaTime * MOVING_MAX_SPEED *
                                                               Mathf.Cos(coeff * Mathf.PI / 2));
                    /*}
                else
                {
                    wasteBody.velocity = transform.forward * (Time.deltaTime * MOVING_MAX_SPEED *
                        Mathf.Cos(coeff * Mathf.PI / 2));
                }*/

                    movingDuration += Time.deltaTime;
                }
            }
        }

        public void Fire()
        {
            if (wasteBody != null)
            {
                wasteBody.velocity = Vector3.zero;
                wasteBody.angularVelocity = Vector3.zero;
            }
            isMoving = true;
            movingDuration = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.name.Contains("WastePortal"))
            {
                // pas beau mais fonctionnel
                PortalColor color;
                if (other.name.Contains("Red")) color = PortalColor.RED;
                else if (other.name.Contains("Blue")) color = PortalColor.BLUE;
                else color = PortalColor.GREEN;
            
                manager.EmitWasteEvent(new WasteData
                {
                    type = wasteType,
                    portalColor = color
                });
                GameObject.Destroy(gameObject);
                IsDestroyedWaste = true;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.name.Contains("Wall"))
            {
                //isMoving = false;
                transform.rotation *= Quaternion.Euler(0, 180, 0);

                if (wasteBody != null)
                {
                    wasteBody.velocity = Vector3.zero;
                    wasteBody.angularVelocity = Vector3.zero;
                }
            }
        }
    }
}
