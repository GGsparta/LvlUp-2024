using System;
using UnityEngine;
using UnityEngine.UI;

namespace CraftemIpsum.UI
{
    public class DirectionalArrow : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Image graphics;
        [SerializeField] private Image cursor;
        [SerializeField] private float marginX;
        [SerializeField] private float marginY;
    
        [Header("Controls")]
        [SerializeField] private Camera targetCamera;

        public Transform objectToPoint;

        private Rect _display;
        
        
        private void OnEnable() => Settings.OnSettingsUpdated += SetupDisplay;
        private void OnDisable() => Settings.OnSettingsUpdated -= SetupDisplay;

        private void Update()
        {
            if (!objectToPoint)
            {
                graphics.enabled = false;
                cursor.enabled = false;
                return;
            }
            
            // Compute position and bounds depending on the type of display (fullscreen or in-canvas rect)
            Vector3 canvasPosition = targetCamera.WorldToScreenPoint(objectToPoint.position);
            Vector2 minPoint = _display.min;
            Vector2 maxPoint = _display.max;
            Vector2 minMarginPoint = new(minPoint.x + marginX, minPoint.y + marginY);
            Vector2 maxMarginPoint = new(maxPoint.x - marginX, maxPoint.y - marginY);
            Vector2 centerPoint = (minPoint + maxPoint) / 2;


            // Checks if the POI is visible on screen
            bool isOutsideOfScreen = canvasPosition.z < 0 ||
                                     canvasPosition.x < minMarginPoint.x || canvasPosition.x > maxMarginPoint.x ||
                                     canvasPosition.y < minMarginPoint.y || canvasPosition.y > maxMarginPoint.y;

            if (isOutsideOfScreen)
            {
                // In this case, an arrow will appear to help the user to find the POI. We should compute to place the arrow on screen

                // Shortest angle won't work because a POI behind you might pop on upper/bottom screen. This case should be computed separately.
                // Note: when target is behind the camera, canvas positions are inverted.
                Vector3
                    behindPosition = new(
                        canvasPosition.x > centerPoint.x ? minMarginPoint.x : maxMarginPoint.x,
                        Mathf.Clamp(maxPoint.y - canvasPosition.y, minMarginPoint.y, maxMarginPoint.y)),
                    shortestAnglePosition = new(
                        Mathf.Clamp(canvasPosition.x, minMarginPoint.x, maxMarginPoint.x),
                        Mathf.Clamp(canvasPosition.y, minMarginPoint.y, maxMarginPoint.y)),
                    finalCanvasPosition = canvasPosition.z < 0 ? behindPosition : shortestAnglePosition;

                transform.position = finalCanvasPosition;


                // Finally, we can set the rotation of the arrow, calculated from the offset between the center of the screen and the arrow position
                Vector2 direction = (Vector2)finalCanvasPosition - centerPoint;
                cursor.transform.localEulerAngles =
                    Vector3.forward * Vector2.SignedAngle(Vector2.right, direction);
            }
            else
            {
                transform.position = canvasPosition;
            }

            // Refresh graphic state (display the poi or a direction tip)
            graphics.enabled = cursor.enabled = isOutsideOfScreen;
        }

        public void Setup(Sprite icon, Camera cam)
        {
            targetCamera = cam;
            graphics.sprite = icon;
            SetupDisplay();
        }

        private void SetupDisplay() => 
            _display = new Rect(targetCamera!.pixelWidth * Convert.ToInt32(Settings.Layout == Layout.J1_J2), 0, targetCamera.pixelWidth, targetCamera.pixelHeight);
    }
}
