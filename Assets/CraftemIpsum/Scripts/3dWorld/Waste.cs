using CraftemIpsum;
using UnityEngine;

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
    

    private void Start()
    {
        initRotation = Quaternion.Euler(wasteType == WasteType.SUSPENSION ? -90 : 0, 0, 0);
        isMoving = false;
        manager = gameObject.GetComponentInParent<WasteManager>();
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

                transform.position += transform.forward * (Time.deltaTime * MOVING_MAX_SPEED *
                                      Mathf.Cos(coeff * Mathf.PI / 2));
                movingDuration += Time.deltaTime;
            }
        }
    }

    public void Fire()
    {
        isMoving = true;
        movingDuration = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("WastePortal"))
        {
            manager.EmitWasteEvent(new WasteData
            {
                type = wasteType,
                portalColor = default // TODO pour des portails de différentes couleur, tu peux setter ici
            });
            GameObject.Destroy(gameObject);
            IsDestroyedWaste = true;
        }
    }
}
