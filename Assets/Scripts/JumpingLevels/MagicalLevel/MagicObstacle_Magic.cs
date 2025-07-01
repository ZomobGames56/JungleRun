using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class MagicObstacle_Magic : MonoBehaviour
{
    float originalTimeScale = 0f;
    [SerializeField] TextMeshProUGUI magicText;
    public enum CollisionEffectType
    {
        GainCoins,
        LoseCoins,
        DoubleScore,
        InvertControls,
        ObstacleRush,
        SpeedSlow,
    }

    void Start()
    {
        magicText.text = "";
    }

    void OnEnable()
    {
        MagicStone_Magic.magicObstacle_Magic += ApplyRandomEffect;
    }

    void OnDisable()
    {
        MagicStone_Magic.magicObstacle_Magic -= ApplyRandomEffect;
    }

    public void ApplyRandomEffect()
    {
        CollisionEffectType effect = (CollisionEffectType)Random.Range(0, System.Enum.GetNames(typeof(CollisionEffectType)).Length);
        ApplyEffect(effect);
    }

    public void ApplyEffect(CollisionEffectType effect)
    {
        switch (effect)
        {
            case CollisionEffectType.GainCoins:
                magicText.text = "Gained 10 Coins!!";
                Invoke("HideCanvas", 2f);
                GameManager_Jumping.Instance.MagicPause(0f);
                GameManager_Jumping.Instance.UpdateCoins(10);
                break;

            case CollisionEffectType.LoseCoins:
                magicText.text = "Lost 10 Coins!!";
                Invoke("HideCanvas", 2f);
                GameManager_Jumping.Instance.MagicPause(0f);
                GameManager_Jumping.Instance.UpdateCoins(-10);
                break;

            case CollisionEffectType.SpeedSlow:
                magicText.text = "Sloth Mode!!";
                originalTimeScale = Time.timeScale;
                Time.timeScale = 0.5f;
                GameManager_Jumping.Instance.MagicPause(5f);
                Invoke("SpeedSlow", 5f);
                break;

            case CollisionEffectType.InvertControls:
                magicText.text = "Inverted Controls!! \nRight = Left \nLeft = Right";
                GameManager_Jumping.Instance.MagicPause(10f);
                GameManager_Jumping.Instance.isInverted = true;
                Invoke("Invert", 10f);
                break;

            case CollisionEffectType.ObstacleRush:
                magicText.text = "Obstacle Rush!!";
                GameManager_Jumping.Instance.MagicPause(0f);
                GameManager_Jumping.Instance.obstacleRush = true;
                Invoke("Obstacle", 10f);
                break;

            case CollisionEffectType.DoubleScore:
                magicText.text = "Earn Double Score!!";
                GameManager_Jumping.Instance.MagicPause(10f);
                GameManager_Jumping.Instance.doubled = true;
                Invoke("Doubled", 10f);
                break;

            default:
                break;
        }
        Debug.Log("Applied Effect: " + effect);
    }

    void HideCanvas()
    {
        magicText.text = "";
    }

    void SpeedSlow()
    {
        Debug.Log("AAAAAAA");
        Time.timeScale = originalTimeScale;
        magicText.text = "";
    }

    void Invert()
    {
        Debug.Log("BBBBBB");
        GameManager_Jumping.Instance.isInverted = false;
        magicText.text = "";
    }

    void Doubled()
    {
        Debug.Log("CCCCCCC");
        GameManager_Jumping.Instance.doubled = false;
        magicText.text = "";
    }

    void Obstacle()
    {
        Debug.Log("DDDDDDDDD");
        GameManager_Jumping.Instance.obstacleRush = false;
        magicText.text = "";
    }
}
