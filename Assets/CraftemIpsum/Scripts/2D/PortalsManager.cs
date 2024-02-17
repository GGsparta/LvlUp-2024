using System.Linq;
using UnityEngine;

namespace CraftemIpsum._2D
{
    public class PortalsManager : MonoBehaviour
    {
        [SerializeField] private Portal[] portals;

        public void PopWaste(WasteData waste)
        {
            portals.First(p => p.color == waste.portalColor).SpawnWaste(waste.type);
        }
    }
}
