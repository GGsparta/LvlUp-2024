using UnityEngine;

namespace CraftemIpsum._3D
{
    public class Limits : MonoBehaviour
    {
        [SerializeField] private BoxCollider topWall;
        [SerializeField] private BoxCollider bottomWall;
        [SerializeField] private BoxCollider forwardWall;
        [SerializeField] private BoxCollider backwardWall;
        [SerializeField] private BoxCollider leftWall;
        [SerializeField] private BoxCollider rightWall;

        private float _maxX;
        private float _minX;
        private float _maxY;
        private float _minY;
        private float _maxZ;
        private float _minZ;
    
        // Start is called before the first frame update
        private void Start()
        {
            _maxY = topWall.transform.TransformPoint(topWall.center).y - topWall.bounds.size.y/2;
            _minY = bottomWall.transform.TransformPoint(bottomWall.center).y + bottomWall.bounds.size.y/2;
        
            _maxZ = forwardWall.transform.TransformPoint(forwardWall.center).z - forwardWall.bounds.size.z/2;
            _minZ = backwardWall.transform.TransformPoint(backwardWall.center).z + backwardWall.bounds.size.z/2;
        
            _maxX = rightWall.transform.TransformPoint(rightWall.center).x - rightWall.bounds.size.x/2;
            _minX = leftWall.transform.TransformPoint(leftWall.center).x + leftWall.bounds.size.x/2;
        }

        public (float, float, float, float, float, float) GetWorldSize()
        {
            return (_minX, _maxX, _minY, _maxY, _minZ, _maxZ);
        }
    }
}
