using UnityEngine;

namespace AudioManger
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;
        AudioSource audioSource;
        public AudioClip coinSound;
        public AudioClip lvl1Track;
        public AudioClip enemyDeathSound;
        public AudioClip zapDamagedSound;
        public AudioClip zapDeathSound;
        public AudioClip zapShoots;
        public AudioClip zapCharging;


        void Awake()
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayCoinSound()
        {
            audioSource.PlayOneShot(coinSound, 0.5f);
        }

        public void PlayEnemyDeathSound()
        {
            audioSource.PlayOneShot(enemyDeathSound, 0.7f);
        }

        public void PlayCharacterDamagedSound()
        {
            audioSource.PlayOneShot(zapDamagedSound, 0.5f);
        }

        public void PlayCharacterShootingSound()
        {
            audioSource.PlayOneShot(zapShoots, 0.5f);
        }

        public void PlayCharacterChargingSound()
        {
            audioSource.PlayOneShot(zapCharging, 0.9f);
        }

        public void PlayCharacterDiesSound()
        {

            audioSource.PlayOneShot(zapDeathSound, 0.5f);
        }

        // Play the level 1 track and loop it
        public void PlayLvl1Track()
        {
            audioSource.clip = lvl1Track; // Set the track as the current clip
            audioSource.loop = true;     // Enable looping
            audioSource.volume = 0.05f;  // Adjust the volume
            audioSource.Play();          // Play the audio
        }

        // Stop the current audio
        public void StopAudio()
        {
            audioSource.Stop();
        }
    }
}
