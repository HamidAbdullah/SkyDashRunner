using UnityEngine;

namespace SkyDashRunner.Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        
        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        
        [Header("Music")]
        [SerializeField] private AudioClip mainMenuMusic;
        [SerializeField] private AudioClip gameMusic;
        [SerializeField] private float musicVolume = 0.5f;
        
        [Header("SFX")]
        [SerializeField] private AudioClip jumpSound;
        [SerializeField] private AudioClip slideSound;
        [SerializeField] private AudioClip dashSound;
        [SerializeField] private AudioClip collectSound;
        [SerializeField] private AudioClip gameOverSound;
        [SerializeField] private AudioClip powerUpSound;
        [SerializeField] private float sfxVolume = 0.7f;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            // Create audio sources if they don't exist
            if (musicSource == null)
            {
                GameObject musicObj = new GameObject("MusicSource");
                musicObj.transform.SetParent(transform);
                musicSource = musicObj.AddComponent<AudioSource>();
                musicSource.loop = true;
                musicSource.volume = musicVolume;
            }
            
            if (sfxSource == null)
            {
                GameObject sfxObj = new GameObject("SFXSource");
                sfxObj.transform.SetParent(transform);
                sfxSource = sfxObj.AddComponent<AudioSource>();
                sfxSource.volume = sfxVolume;
            }
        }
        
        private void Start()
        {
            PlayMainMenuMusic();
        }
        
        public void PlayMainMenuMusic()
        {
            if (mainMenuMusic != null && musicSource != null)
            {
                musicSource.clip = mainMenuMusic;
                musicSource.Play();
            }
        }
        
        public void PlayGameMusic()
        {
            if (gameMusic != null && musicSource != null)
            {
                musicSource.clip = gameMusic;
                musicSource.Play();
            }
        }
        
        public void PlaySFX(AudioClip clip)
        {
            if (clip != null && sfxSource != null)
            {
                sfxSource.PlayOneShot(clip);
            }
        }
        
        public void PlayJumpSound()
        {
            PlaySFX(jumpSound);
        }
        
        public void PlaySlideSound()
        {
            PlaySFX(slideSound);
        }
        
        public void PlayDashSound()
        {
            PlaySFX(dashSound);
        }
        
        public void PlayCollectSound()
        {
            PlaySFX(collectSound);
        }
        
        public void PlayGameOverSound()
        {
            PlaySFX(gameOverSound);
        }
        
        public void PlayPowerUpSound()
        {
            PlaySFX(powerUpSound);
        }
        
        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            if (musicSource != null)
            {
                musicSource.volume = musicVolume;
            }
        }
        
        public void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
            if (sfxSource != null)
            {
                sfxSource.volume = sfxVolume;
            }
        }
    }
}

