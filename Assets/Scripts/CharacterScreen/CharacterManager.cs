using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour
{
    public string levelType;
    public List<Characters> characters;
    public GameObject charItemPrefab;
    public Transform contentPanel;
    public Transform player;
    GameObject child = null;
    GameObject childRagdoll = null;
    public Image costType;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI charName;
    public GameObject buyButton;
    public GameObject selectButton;
    public GameObject selectedText;
    public GameObject notEnoughPanel;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI staminaText;

    public Sprite coinSprite;
    public Sprite diamondSprite;
    public static Func<string, Characters> OnFindCharacter;
    int coinsCurr;

    void OnEnable()
    {
        if (levelType == "Jumping")
        {
            GameManager_Jumping.charManager += DeselectCharacters;
            GameManager_Jumping.charCheck += OnCharacterSelect;
            GameManager_Jumping.characterSave += SaveCharacter;
        }
        else if (levelType == "Tutorial")
        {
            GameManager_Jumping_Tutorial.charManager += DeselectCharacters;
            GameManager_Jumping_Tutorial.charCheck += OnCharacterSelect;
            GameManager_Jumping_Tutorial.characterSave += SaveCharacter;
        }
        else
        {
            GameManager_BridgeLevel.charManager += DeselectCharacters;
            GameManager_BridgeLevel.charCheck += OnCharacterSelect;
            GameManager_BridgeLevel.characterSave += SaveCharacter;
        }
        OnFindCharacter = FindCharacter;
    }

    void OnDisable()
    {
        if (levelType == "Jumping")
        {
            GameManager_Jumping.charManager -= DeselectCharacters;
            GameManager_Jumping.charCheck -= OnCharacterSelect;
            GameManager_Jumping.characterSave -= SaveCharacter;
        }
        else
        {
            GameManager_BridgeLevel.charManager -= DeselectCharacters;
            GameManager_BridgeLevel.charCheck -= OnCharacterSelect;
            GameManager_BridgeLevel.characterSave -= SaveCharacter;
        }
        OnFindCharacter = null;
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("Coins"))
        {
            coinsCurr = PlayerPrefs.GetInt("Coins", 0);
        }
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // if (PlayerPrefs.HasKey("CurrentCharacter"))
        // {
        string savedName = "Bear";
        if (PlayerPrefs.HasKey("CurrentCharacter"))
        {
            savedName = PlayerPrefs.GetString("CurrentCharacter");
        }
        Characters selectedCharacter = FindCharacter(savedName);
        OnCharacterSelect(selectedCharacter, coinsCurr);
        // }

        foreach (var charData in characters)
        {
            if (charData.name == "Bear")
            {
                charData.selected = true;
                charData.purchased = true;
                charData.Save();
            }
            else
            {
                charData.Load();
            }
            GameObject item = Instantiate(charItemPrefab, contentPanel);
            CharacterItemUI ui = item.GetComponent<CharacterItemUI>();
            ui.Setup(charData, OnCharacterSelect, coinsCurr);
            // speedText.text = "Speed : " + charData.speed.ToString();
            // strengthText.text = "Strength : " + charData.strength.ToString();
            // agilityText.text = "Agility : " + charData.agility.ToString();
            // staminaText.text = "Stamina : " + charData.stamina.ToString();
        }
    }

    void OnCharacterSelect(Characters character, int coins)
    {
        Debug.Log("Selected: " + character.characterName);
        coinsCurr = coins;
        if (child != null)
        {
            Destroy(child);
            Destroy(childRagdoll);
        }
        child = Instantiate(character.charPrefab, player);
        childRagdoll = Instantiate(character.ragdollPrefab, player);
        childRagdoll.SetActive(false);
        costType.sprite = character.costTypeSprite;
        cost.text = " " + character.cost.ToString();
        charName.text = character.characterName;
        speedText.text = "Speed : " + character.speed.ToString();
        strengthText.text = "Strength : " + character.strength.ToString();
        staminaText.text = "Stamina : " + character.stamina.ToString();
        if (levelType == "Jumping")
        {
            GameManager_Jumping.Instance.currChar = character;
            GameManager_Jumping.Instance.UpdateChild(child, childRagdoll);
        }
        else if (levelType == "Tutorial")
        {
            GameManager_Jumping_Tutorial.Instance.currChar = character;
            GameManager_Jumping_Tutorial.Instance.UpdateChild(child, childRagdoll);
        }
        else
        {
            GameManager_BridgeLevel.Instance.currChar = character;
            GameManager_BridgeLevel.Instance.UpdateChild(child, childRagdoll);
        }
        if (character.purchased)
        {
            buyButton.SetActive(false);
            notEnoughPanel.SetActive(false);
            selectButton.SetActive(true);
            if (character.selected)
            {
                selectButton.SetActive(false);
                selectedText.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("COINS - " + coinsCurr.ToString() + "\nCOST - " + character.cost.ToString());
            if (coinsCurr < character.cost)
            {
                notEnoughPanel.SetActive(true);
            }
            else
            {
                notEnoughPanel.SetActive(false);
            }
            buyButton.SetActive(true);
            selectButton.SetActive(false);
            selectedText.SetActive(false);
        }
    }

    void DeselectCharacters(Characters currChar)
    {
        foreach (var charData in characters)
        {
            if (!(charData == currChar))
            {
                charData.selected = false;
            }
        }
    }

    void SaveCharacter()
    {
        foreach (var charData in characters)
        {
            charData.Save();
        }
    }

    Characters FindCharacter(string savedName)
    {
        return characters.Find(c => c.characterName == savedName);
    }
}
