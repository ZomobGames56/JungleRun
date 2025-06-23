using UnityEngine;

public class SoundManager_BridgeLevel : MonoBehaviour
{
    public static SoundManager_BridgeLevel Instance { get; set; }
    [SerializeField] private AudioClip jumpingSound;
    [SerializeField] private AudioClip drowningSound;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip coinsSound;
    [SerializeField] private AudioClip exploSound;
    [SerializeField] private AudioClip snowSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSource2;
    [SerializeField] private AudioSource bgAudioSource;
    [SerializeField] private AudioSource otherAudioSource;
    [SerializeField] private AudioSource otherAudioSource2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1);
        float music = PlayerPrefs.GetFloat("MusicVolume", 1);

        SetSFX(sfx);
        SetMusic(music);
    }

    public void PlayJumpingSound()
    {
        audioSource.PlayOneShot(jumpingSound);
    }

    public void PlayLoseSound(string type)
    {
        if (type == "drown") audioSource.PlayOneShot(drowningSound);
        else audioSource.PlayOneShot(hurtSound);
    }

    public void PlayCoinsSound()
    {
        audioSource.PlayOneShot(coinsSound);
    }

    public void PlayExploSound()
    {
        audioSource2.PlayOneShot(exploSound);
    }

    public void PlaySnowSound()
    {
        audioSource.PlayOneShot(snowSound);
    }

    public void SetSFX(float value)
    {
        audioSource.volume = value;
        audioSource2.volume = value;
        if (otherAudioSource != null) otherAudioSource.volume = value;
        if (otherAudioSource2 != null) otherAudioSource2.volume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }

    public void SetMusic(float value)
    {
        bgAudioSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }

    public void MuteMusic()
    {
        
    }
    public void UnMuteMusic()
    {
        
    }
    public void MuteSFX()
    {
        
    }
    public void UnMuteSFX()
    {
        
    }
}
