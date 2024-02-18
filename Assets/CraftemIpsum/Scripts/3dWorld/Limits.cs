using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limits : MonoBehaviour
{
    [SerializeField] private BoxCollider topWall;
    [SerializeField] private BoxCollider bottomWall;
    [SerializeField] private BoxCollider forwardWall;
    [SerializeField] private BoxCollider backwardWall;
    [SerializeField] private BoxCollider leftWall;
    [SerializeField] private BoxCollider rightWall;

    private float maxX;
    private float minX;
    private float maxY;
    private float minY;
    private float maxZ;
    private float minZ;
    
    // Start is called before the first frame update
    void Start()
    {
        maxY = topWall.transform.TransformPoint(topWall.center).y - topWall.size.y;
        minY = bottomWall.transform.TransformPoint(bottomWall.center).y + bottomWall.size.y;
        
        maxZ = forwardWall.transform.TransformPoint(forwardWall.center).z - forwardWall.size.z;
        minZ = backwardWall.transform.TransformPoint(backwardWall.center).z + backwardWall.size.z;
        
        maxX = rightWall.transform.TransformPoint(rightWall.center).x - rightWall.size.x;
        minX = leftWall.transform.TransformPoint(leftWall.center).x + leftWall.size.x;
    }

    public (float, float, float, float, float, float) GetWorldSize()
    {
        return (minX, maxX, minY, maxY, minZ, maxZ);
    }
}
