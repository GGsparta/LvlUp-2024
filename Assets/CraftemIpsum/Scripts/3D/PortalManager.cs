using System.Collections.Generic;
using System.Linq;
using CraftemIpsum._2D;
using GGL.Extensions;
using UnityEngine;

namespace CraftemIpsum._3D
{
    public class PortalManager : MonoBehaviour
    {
        private List<Portal3D> _listOfPortals;

        private void Start() => 
            _listOfPortals = transform.GetChildren().Select(t => t.GetComponent<Portal3D>()).ToList();

        public Portal3D GetNearestPortal(Vector3 position, PortalColor color)
        {
            return _listOfPortals
                .Where(portal => portal != null && portal.Color == color)
                .OrderBy(portal => Vector3.Distance(position, portal.transform.position))
                .FirstOrDefault();
        }
    }
}