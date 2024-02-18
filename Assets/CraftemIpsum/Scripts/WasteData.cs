using System;
using UnityEngine.Serialization;

namespace CraftemIpsum
{
    public enum WasteType
    {
        EXHAUST, BARREL, SUSPENSION
    }

    public enum PortalColor
    {
        RED, GREEN, BLUE
    }

    [Serializable]
    public struct WasteData
    {
        public WasteType type;
        public PortalColor portalColor;
    }
}