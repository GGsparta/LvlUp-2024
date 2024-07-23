using System;

namespace CraftemIpsum
{
    [Serializable]
    public enum WasteType
    {
        EXHAUST, BARREL, SUSPENSION
    }

    [Serializable]
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