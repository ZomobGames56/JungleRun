using UnityEngine;

public class SoundManager_Jumping : MonoBehaviour
{
    public static SoundManager_Jumping Instance { get; set; }

    [SerializeField] private string levelType;
    [SerializeField] private AudioClip jumpingSound;
    [SerializeField] private AudioClip drowningSound;
    [SerializeField] private AudioClip burningSound;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip coinsSound;
    [SerializeField] private AudioClip exploSound;
    [SerializeField] private AudioClip riverSound;
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
        Debug.Log("SFX : " + sfx);
        Debug.Log("Music : " + music);

        SetSFX(sfx);
        SetMusic(music);
    }

    public void PlayJumpingSound()
    {
        audioSource.PlayOneShot(jumpingSound);
    }

    public void PlayLoseSound(string type)
    {
        if (type == "fire") audioSource.PlayOneShot(burningSound);
        else if (type == "stab") audioSource.PlayOneShot(hurtSound);
        else
        {
            if (levelType == "Lava")
                audioSource.PlayOneShot(burningSound);
            else
                audioSource.PlayOneShot(drowningSound);
        }
    }

    public void PlayCoinsSound()
    {
        audioSource.PlayOneShot(coinsSound);
    }

    public void PlayRiverSound()
    {
        audioSource2.PlayOneShot(riverSound);
    }

    public void PlayExplosionSound()
    {
        audioSource.PlayOneShot(exploSound);
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
