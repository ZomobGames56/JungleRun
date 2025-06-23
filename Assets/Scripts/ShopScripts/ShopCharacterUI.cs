using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopCharacterUI : MonoBehaviour
{
    public bool jumpingLevel;
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI byLineText;
    public TextMeshProUGUI costText;
    public Image costTypeImage;
    public Button buyButton;
    public GameObject buyButtonObj;
    public GameObject purchasedText;

    public Sprite coinSprite;
    public Sprite diamondSprite;
    int charCost;
    ShopCharacter currChar;

    public void Setup(ShopCharacter character)
    {
        iconImage.sprite = character.characterImage;
        nameText.text = character.characterName;
        byLineText.text = character.byLine;
        costText.text = character.cost.ToString();
        charCost = character.cost;
        currChar = character;

        switch (character.costType)
        {
            case CostType.Coins:
                costTypeImage.sprite = coinSprite;
                break;
            case CostType.Diamonds:
                costTypeImage.sprite = diamondSprite;
                break;
        }
    }

    public void BuyCheck(int coins)
    {
        if (!currChar.purchased)
        {
            if (coins >= charCost)
            {
                buyButton.interactable = true;
                buyButtonObj.transform.GetChild(0).gameObject.SetActive(false);
                buyButtonObj.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                buyButton.interactable = false;
                buyButtonObj.transform.GetChild(0).gameObject.SetActive(true);
                buyButtonObj.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        else
        {
            buyButtonObj.SetActive(false);
            purchasedText.SetActive(true);
        }
    }

    public void BuyButton()
    {
        if (jumpingLevel) GameManager_Jumping.Instance.ShopCharacterBuyButton(currChar);
        else GameManager_BridgeLevel.Instance.ShopCharacterBuyButton(currChar);
    }
}
