using System;
using System.Collections.Generic;
using System.Linq;
using CraftemIpsum._3D;
using UnityEngine;

namespace CraftemIpsum.UI
{
    public class ArrowManager : MonoBehaviour
    {
        [SerializeField] private Transform ship;
        [SerializeField] private Camera renderCam;
        [Header("Prefab configuration")]
        [SerializeField] private GameObject arrowPrefab;
        [SerializeField] private RectTransform arrowContainer;
        [SerializeField] private List<Sprite> wasteArrows;
        [SerializeField] private List<Sprite> portalArrows;
        [Header("Managers")]
        [SerializeField] private WasteManager wasteManager;
        [SerializeField] private PortalManager portalManager;

        private Dictionary<WasteType, DirectionalArrow> _wasteArrows;
        private Dictionary<PortalColor, DirectionalArrow> _portalArrows;
        private PortalColor[] _portalColors;
        private WasteType[] _wasteTypes;

        private void OnEnable() => Settings.OnSettingsUpdated += SetupDisplay;
        private void OnDisable() => Settings.OnSettingsUpdated -= SetupDisplay;
        
        private void Awake()
        {
            _wasteTypes = Enum.GetValues(typeof(WasteType)).OfType<WasteType>().ToArray();
            _wasteArrows = new Dictionary<WasteType, DirectionalArrow>();
            foreach (WasteType wasteType in _wasteTypes)
            {
                DirectionalArrow arrow = Instantiate(arrowPrefab, arrowContainer).GetComponent<DirectionalArrow>();
                arrow.Setup(wasteArrows[(int)wasteType], renderCam);
                _wasteArrows[wasteType] = arrow;
            }

            _portalColors = Enum.GetValues(typeof(PortalColor)).OfType<PortalColor>().ToArray();
            _portalArrows = new Dictionary<PortalColor, DirectionalArrow>();
            foreach (PortalColor portalColor in _portalColors)
            {
                DirectionalArrow arrow = Instantiate(arrowPrefab, arrowContainer).GetComponent<DirectionalArrow>();
                arrow.Setup(portalArrows[(int)portalColor], renderCam);
                _portalArrows[portalColor] = arrow;
            }
            
            SetupDisplay();
        }

        private void FixedUpdate()
        {
            foreach (WasteType wasteType in _wasteTypes) 
                _wasteArrows[wasteType].objectToPoint = wasteManager.GetNearestWaste(ship.position, wasteType)?.transform;

            foreach (PortalColor portalColor in _portalColors) 
                _portalArrows[portalColor].objectToPoint = portalManager.GetNearestPortal(ship.position, portalColor)?.transform;
        }

        private void SetupDisplay()
        {
            Rect rect = renderCam.rect;
            arrowContainer.anchorMin = rect.min;
            arrowContainer.anchorMax = rect.max;
        }
    }
}
