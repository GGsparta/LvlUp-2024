using System.Collections;
using System.Linq;
using UnityEngine;

namespace CraftemIpsum._2D
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private Waste[] wastePrefabs;
        [SerializeField] private Light spotlight;
        public PortalColor color;

        public void SpawnWaste(WasteType type)
        {
            GameObject go = Instantiate(wastePrefabs.First(w => w.Type == type).gameObject);
            go.transform.position = transform.position;
            go.transform.Rotate(Vector3.forward * Random.Range(-180f, 180f));
            go.GetComponent<Rigidbody2D>().velocity = -transform.up * 10f;

            StopAllCoroutines();
            StartCoroutine(EBlinkLight());
        }

        private IEnumerator EBlinkLight()
        {
            spotlight.enabled = true;
            yield return new WaitForSeconds(.2f);
            spotlight.enabled = false;
            yield return new WaitForSeconds(.2f);
            spotlight.enabled = true;
            yield return new WaitForSeconds(.2f);
            spotlight.enabled = false;
        }
    }
}
