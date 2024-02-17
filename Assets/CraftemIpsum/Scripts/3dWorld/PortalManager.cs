using System.Collections.Generic;
using System.Linq;
using GGL.Extensions;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    private List<Transform> listOfPortals;
    
    // Start is called before the first frame update
    void Start()
    {
        listOfPortals = transform.GetChildren().ToList();
    }

    public List<Transform> getNearPortals(Vector3 position, int number)
    {
        return listOfPortals
            .Where(portal => portal != null)
            .OrderBy(portal => Vector3.Distance(position, portal.transform.position))
            .Take(number).ToList();
    }
}