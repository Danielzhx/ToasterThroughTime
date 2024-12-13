using UnityEngine;

namespace AudioManger
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;
        AudioSource audioSource;
        public AudioClip coinSound;

        void Awake()
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayCoinSound()
        {
            audioSource.PlayOneShot(coinSound, 0.15f);
        }
    }
}
