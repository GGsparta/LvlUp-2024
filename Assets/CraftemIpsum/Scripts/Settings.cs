using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CraftemIpsum
{
    public enum Layout
    {
        J1_J2,
        J2_J1
    }

    public class Settings : MonoBehaviour
    {
        #region PLAYER_PREFS
        public static Layout Layout
        {
            get => (Layout)PlayerPrefs.GetInt(nameof(Layout), default);
            set => PlayerPrefs.SetInt(nameof(Layout), (int)value);
        }
        
        public static bool InvertedAxis
        {
            get => Convert.ToBoolean(PlayerPrefs.GetInt(nameof(InvertedAxis), default));
            set => PlayerPrefs.SetInt(nameof(InvertedAxis), Convert.ToInt32(value));
        }
        #endregion

        [SerializeField] private Toggle[] layoutToggles;
        [SerializeField] private Toggle mouseAxisToggle;

        public static event Action OnSettingsUpdated;
        
        private void OnEnable()
        {
            RegisterToggles();
            LoadSettings();
        }
        private void OnDisable()
        {
            UnregisterToggles();
        }


        private void RegisterToggles()
        {
            foreach (Toggle layoutToggle in layoutToggles) 
                layoutToggle.onValueChanged.AddListener(SaveSettings);
            mouseAxisToggle.onValueChanged.AddListener(SaveSettings);
        }
        
        private void UnregisterToggles()
        {
            foreach (Toggle layoutToggle in layoutToggles) 
                layoutToggle.onValueChanged.RemoveListener(SaveSettings);
            mouseAxisToggle.onValueChanged.RemoveListener(SaveSettings);
        }

        private void LoadSettings()
        {
            Layout layout = Layout;
            for (int index = 0; index < layoutToggles.Length; index++) 
                layoutToggles[index].SetIsOnWithoutNotify(layout == (Layout)index);
            
            mouseAxisToggle.SetIsOnWithoutNotify(InvertedAxis);
        }
        
        private void SaveSettings(bool aLayoutToggleHasBeenActivated)
        {
            InvertedAxis = mouseAxisToggle.isOn;

            if (aLayoutToggleHasBeenActivated)
            {
                Toggle activeToggle = layoutToggles.First().group.GetFirstActiveToggle();
                for (int index = 0; index < layoutToggles.Length; index++)
                {
                    if (activeToggle != layoutToggles[index]) continue;
                    Layout = (Layout)index;
                    break;
                }
            }
            
            OnSettingsUpdated?.Invoke();
        }
    }
}