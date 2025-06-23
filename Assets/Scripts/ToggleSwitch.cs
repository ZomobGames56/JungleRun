using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ToggleSwitch : MonoBehaviour
{
    public Slider slider;
    public string type;
    public Image background;
    public Color onColor = Color.green;
    public Color offColor = Color.gray;
    public GameObject reloadButton;
    public bool jungleLevel;

    private void Start()
    {
        if (jungleLevel)
        {
            if (type == "SFX")
            {
                slider.value = PlayerPrefs.GetInt("SFXMuted - Jumping", 0) == 1 ? 0 : 1;
            }
            else if (type == "Music")
            {
                slider.value = PlayerPrefs.GetInt("MusicMuted - Jumping", 0) == 1 ? 0 : 1;
            }
            else if (type == "Tutorial")
            {
                slider.value = PlayerPrefs.GetInt("TutorialEnabled - Jumping", 1) == 1 ? 1 : 0;
            }
        }
        else
        {
            if (type == "SFX")
            {
                slider.value = PlayerPrefs.GetInt("SFXMuted - Bridge", 0) == 1 ? 0 : 1;
            }
            else if (type == "Music")
            {
                slider.value = PlayerPrefs.GetInt("MusicMuted - Bridge", 0) == 1 ? 0 : 1;
            }
            else if (type == "Tutorial")
            {
                slider.value = PlayerPrefs.GetInt("TutorialEnabled - Bridge", 1) == 1 ? 1 : 0;
            }
        }

        if (jungleLevel)
            slider.onValueChanged.AddListener(OnSliderValueChanged_Jungle);
        else
            slider.onValueChanged.AddListener(OnSliderValueChanged_Bridge);

        UpdateToggleVisual();
    }

    private void OnSliderValueChanged_Jungle(float value)
    {
        UpdateToggleVisual();
        if (value == 0)
        {
            switch (type)
            {
                case "SFX":
                    SoundManager_Jumping.Instance.MuteSFX();
                    break;

                case "Music":
                    SoundManager_Jumping.Instance.MuteMusic();
                    break;
 
                case "Tutorial":
                    GameManager_Jumping.Instance.ToggleStart(true);
                    reloadButton.SetActive(true);
                    break;
 
                default:
                    break;
            }
        }
        else
        {
            switch (type)
            {
                case "SFX":
                    SoundManager_Jumping.Instance.UnMuteSFX();
                    break;

                case "Music":
                    SoundManager_Jumping.Instance.UnMuteMusic();
                    break;

                case "Tutorial":
                    GameManager_Jumping.Instance.ToggleStart(false);
                    reloadButton.SetActive(true);
                    break;

                default:
                    break;
            }
        }
    }

    private void OnSliderValueChanged_Bridge(float value)
    {
        UpdateToggleVisual();
        if (value == 0)
        {
            switch (type)
            {
                case "SFX":
                    SoundManager_BridgeLevel.Instance.MuteSFX();
                    break;

                case "Music":
                    SoundManager_BridgeLevel.Instance.MuteMusic();
                    break;
 
                case "Tutorial":
                    GameManager_BridgeLevel.Instance.ToggleStart(true);
                    reloadButton.SetActive(true);
                    break;
 
                default:
                    break;
            }
        }
        else
        {
            switch (type)
            {
                case "SFX":
                    SoundManager_BridgeLevel.Instance.UnMuteSFX();
                    break;

                case "Music":
                    SoundManager_BridgeLevel.Instance.UnMuteMusic();
                    break;

                case "Tutorial":
                    GameManager_Jumping.Instance.ToggleStart(false);
                    reloadButton.SetActive(true);
                    break;

                default:
                    break;
            }
        }
    }

    private void UpdateToggleVisual()
    {
        background.color = (slider.value == 1) ? onColor : offColor;
    }

    public void SmoothToggle()
    {
        float targetValue = (slider.value == 1) ? 0 : 1;
        slider.DOValue(targetValue, 0.1f).SetEase(Ease.InOutSine).SetUpdate(true);
    }
}
