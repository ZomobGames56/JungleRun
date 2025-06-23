using UnityEngine;
using UnityEngine.UI;

public class SettingsScreenManager : MonoBehaviour
{
    [SerializeField] string type;
    [SerializeField] bool jungleLevel;
    [SerializeField] Slider slider;

    private void Start()
    {
        float initialValue = type == "SFX" ? PlayerPrefs.GetFloat("SFXVolume", 1) : PlayerPrefs.GetFloat("MusicVolume", 1);

        slider.value = initialValue;

        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        if (jungleLevel)
        {
            if (type == "SFX")
                SoundManager_Jumping.Instance.SetSFX(value);
            else if (type == "Music")
                SoundManager_Jumping.Instance.SetMusic(value);
        }
        else
        {
            if (type == "SFX")
                SoundManager_BridgeLevel.Instance.SetSFX(value);
            else if (type == "Music")
                SoundManager_BridgeLevel.Instance.SetMusic(value);
        }
    }

    public void PlayButton()
    {
        if (jungleLevel)
        {
            GameManager_Jumping.Instance.ToggleStart(false);
            GameManager_Jumping.Instance.RestartButton();
        }
        else
        {
            GameManager_BridgeLevel.Instance.ToggleStart(false);
            GameManager_BridgeLevel.Instance.RestartButton();
        }
    }

    public void RightButton()
    {
        slider.value = Mathf.Clamp(slider.value + 0.1f, slider.minValue, slider.maxValue);
    }

    public void LeftButton()
    {
        slider.value = Mathf.Clamp(slider.value - 0.1f, slider.minValue, slider.maxValue);
    }
}
