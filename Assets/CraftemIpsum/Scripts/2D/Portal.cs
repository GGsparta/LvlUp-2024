using System.Linq;
using UnityEngine;

namespace CraftemIpsum._2D
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private Waste[] wastePrefabs;

        public void SpawnWaste(WasteType type)
        {
            GameObject go = Instantiate(wastePrefabs.First(w => w.Type == type).gameObject);
            go.transform.position = transform.position;
        }
    }
}
