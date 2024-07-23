using System.Collections.Generic;
using UnityEngine;

namespace CraftemIpsum._2D
{
    public class Planet : MonoBehaviour
    {
        [SerializeField] private float gravityScale;
        [SerializeField] private float gravityDamping;
        [SerializeField] private CircleCollider2D gravityZone;

        private readonly List<Rigidbody2D> _listOfAttractedBodies = new();


        private void OnTriggerEnter2D(Collider2D other)
        {
            Rigidbody2D body = other.GetComponentInParent<Rigidbody2D>();
            if(body) 
                _listOfAttractedBodies.Add(body);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Rigidbody2D body = other.GetComponentInParent<Rigidbody2D>();
            if(body) 
                _listOfAttractedBodies.Remove(body);
        }

        public void FixedUpdate()
        {
            if (GameManager.Exists && !GameManager.Instance.IsPlaying)
                return;

            foreach (Rigidbody2D body in _listOfAttractedBodies)
            {
                Vector3 offset = body.transform.position - transform.position;
                float damping = 1 - (offset.magnitude / (gravityZone.radius * transform.lossyScale.magnitude) * gravityDamping);

                body.AddForce(-offset.normalized * (gravityScale * body.mass * damping), ForceMode2D.Force);
            }
        }
    }
}
