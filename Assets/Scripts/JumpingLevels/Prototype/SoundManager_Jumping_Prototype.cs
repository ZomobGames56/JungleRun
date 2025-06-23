using UnityEngine;

public class SoundManager_Jumping_Prototype : MonoBehaviour
{
    public static SoundManager_Jumping_Prototype Instance{get; set; }
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip drownSound;
    [SerializeField] private AudioClip burnSound;
    [SerializeField] private AudioClip stabbedSound;
    [SerializeField] private AudioClip coinsSound;
    [SerializeField] private AudioClip riverAudio;
    [SerializeField] private AudioSource riverAudioSource;
    [SerializeField] private AudioSource audioSource;

    private void Awake(){
        if(Instance == null){
            Instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }

    public void PlayJumpingSound(){
        audioSource.PlayOneShot(jumpSound);
    }

    public void PlayLoseSound(string type){
        if(type == "fire")  audioSource.PlayOneShot(burnSound);
        else if(type == "stab")  audioSource.PlayOneShot(stabbedSound);
        else  audioSource.PlayOneShot(drownSound);
    }

    public void PlayCoinsSound(){
        audioSource.PlayOneShot(coinsSound);
    }

    public void PlayRiverSound(){
        riverAudioSource.PlayOneShot(riverAudio);
    }
}
