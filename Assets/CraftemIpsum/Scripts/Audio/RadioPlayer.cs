using System;
using System.Collections;
using System.Collections.Generic;
using GGL.Audio.Player;
using UnityEngine;

namespace CraftemIpsum.Audio
{
    public class RadioPlayer : MonoBehaviour
    {
        [Serializable]
        private class RadioClip
        {
            public AudioClip audioClip;
            public float volumeForClip = 1f;
        }

        [SerializeField] private AudioClip startingAudioClip;
        [SerializeField] private List<RadioClip> items;
        
        private AudioSource _player;

        private IEnumerator Start()
        {
            _player = MusicPlayer.Instance.GetComponent<AudioSource>();
            _player.loop = false;
            _player.clip = startingAudioClip;
            _player.volume = 1f;
            yield return StartCoroutine(Watch());
        }

        private void OnEnable()
        {
            GameManager.Instance.OnPlay += Play;
            GameManager.Instance.OnPause += Pause;
            GameManager.Instance.OnGameOver += GoNext;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnPlay -= Play;
            GameManager.Instance.OnPause -= Pause;
            GameManager.Instance.OnGameOver -= GoNext;
        }

        private void OnDestroy()
        {
            MusicPlayer.Stop();
        }

        private void Play()
        {
            _player.Play();
        }

        private void Pause()
        {
            _player.Pause();
        }

        private IEnumerator Watch()
        {
            while (items.Count > 0)
            {
                yield return new WaitUntil(() => !_player.isPlaying && _player.time >= _player.clip.length - 0.05f);
                GoNext();
            }
            
            _player.loop = true;
        }

        private void GoNext()
        {
            if(items.Count == 0) return;
            RadioClip item = items[0];
            _player.clip = item.audioClip;
            _player.volume = item.volumeForClip;
            _player.Play();
            items.RemoveAt(0);
        }
    }
}
