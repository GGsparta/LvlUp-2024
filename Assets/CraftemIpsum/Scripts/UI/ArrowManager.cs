using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CraftemIpsum.UI;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    [SerializeField]
    private List<DirectionalArrow> wasteArrows;
    
    [SerializeField]
    private List<DirectionalArrow> portalArrows;
    
    [SerializeField]
    private WasteManager wasteManager;
    
    [SerializeField]
    private PortalManager portalManager;
    
    [SerializeField]
    private Transform objectToFollow;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        List<Waste> wasteList = wasteManager.getNearWaste(objectToFollow.position, wasteArrows.Count);
        for (int i=0; i < wasteArrows.Count; i++)
        {
            if (i < wasteList.Count())
            {
                wasteArrows[i].objectToPoint = wasteList[i].transform;
            }
            else
            {
                wasteArrows[i].objectToPoint = null;
            }
        }
        
        List<Transform> portalList = portalManager.getNearPortals(objectToFollow.position, portalArrows.Count);
        for (int i=0; i < portalArrows.Count; i++)
        {
            if (i < portalList.Count())
            {
                portalArrows[i].objectToPoint = portalList[i].transform;
            }
            else
            {
                portalArrows[i].objectToPoint = null;
            }
        }
    }
}
