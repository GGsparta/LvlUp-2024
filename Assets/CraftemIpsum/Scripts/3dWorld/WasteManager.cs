using System.Collections.Generic;
using System.Linq;
using CraftemIpsum;
using UnityEngine;
using UnityEngine.Events;

public class WasteManager : MonoBehaviour
{
    [SerializeField] private GameObject exhaustPrefab;
    [SerializeField] private GameObject suspensionPrefab;
    [SerializeField] private GameObject barrelPrefab;
    
    private const int NUMBER_OF_INIT_WASTE = 12;
    private const int TIME_BEFORE_NEW_WASTE = 5;
    
    public UnityEvent<WasteData> wasteInPortal = new();

    private List<Waste> listOfWaste;

    [SerializeField] private Limits limits;
   
    private float elaspedTime;
    
    // Start is called before the first frame update
    void Start()
    {
        listOfWaste = GetComponentsInChildren<Waste>().ToList();

        CreateWaste(NUMBER_OF_INIT_WASTE);

        elaspedTime = 0;
    }

    private void Update()
    {
        // TODO Create waste randomnly

        if (elaspedTime >= TIME_BEFORE_NEW_WASTE)
        {
            elaspedTime = 0;
            CreateWaste(1);
        }
        
        elaspedTime += Time.deltaTime;
    }

    public void EmitWasteEvent(WasteData waste)
    {
        wasteInPortal.Invoke(waste);
    }

    public List<Waste> getNearWaste(Vector3 position, int number)
    {
        return listOfWaste
            .Where(waste => waste != null && !waste.IsDestroyedWaste && waste.gameObject.activeSelf)
            .OrderBy(waste => Vector3.Distance(position, waste.transform.position))
            .Take(number).ToList();
    }

    private void CreateWaste(int number)
    {
        var worldLimits = limits.GetWorldSize();
        for (int i = 0; i < number; i++)
        {
            int prefabId = Random.Range(0, 3);
            GameObject prefab;
            if (prefabId == 0) prefab = exhaustPrefab;
            else if (prefabId == 1) prefab = suspensionPrefab;
            else prefab = barrelPrefab;
            
            float x = Random.Range(worldLimits.Item1, worldLimits.Item2);
            float y = Random.Range(worldLimits.Item3, worldLimits.Item4);
            float z = Random.Range(worldLimits.Item5, worldLimits.Item6);
            
            //GameObject newObject = Instantiate(prefab, new Vector3(x, y, z), prefab.transform.rotation, transform);
            GameObject newObject = Instantiate(prefab, new Vector3(x, y, z), Random.rotation, transform);
            
            listOfWaste.Add(newObject.GetComponent<Waste>());
        }
        
    }
}
