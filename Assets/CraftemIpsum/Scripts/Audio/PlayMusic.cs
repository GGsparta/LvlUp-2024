using UnityEngine;

namespace CraftemIpsum.Audio
{
    /// <summary>
    /// Play a music with the defined settings
    /// </summary>
    public class PlayMusic : MonoBehaviour
    {
        [SerializeField]
        private AudioClip clip;

        [SerializeField]
        private float volume = 1f;
    
        [SerializeField]
        private float fade = 0.2f;
    
        [SerializeField]
        private bool loop;

        public void PlayLater(float delay) => Invoke(nameof(Play), delay);
    
        public void Play() => ManuPlayer.Play(clip, volume, fade, loop);
    }
}
