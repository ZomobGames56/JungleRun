using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopClothesUI : MonoBehaviour
{
    public bool jumpingLevel;
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public Image costTypeImage;
    public Button buyButton;
    public GameObject buyButtonObj;
    public GameObject purchasedText;

    public Sprite coinSprite;
    public Sprite diamondSprite;
    int clothCost;
    ShopClothes currCloth;

    public void Setup(ShopClothes clothes)
    {
        iconImage.sprite = clothes.clothImage;
        nameText.text = clothes.clothName;
        costText.text = clothes.cost.ToString();
        clothCost = clothes.cost;
        currCloth = clothes;

        switch (clothes.costType)
        {
            case CostTypeCloth.Coins:
                costTypeImage.sprite = coinSprite;
                break;
            case CostTypeCloth.Diamonds:
                costTypeImage.sprite = diamondSprite;
                break;
        }
    }

    public void BuyCheck(int coins)
    {
        if (!currCloth.purchased)
        {
            if (coins >= clothCost)
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
        if (jumpingLevel) GameManager_Jumping.Instance.ShopClothesBuyButton(currCloth);
        else GameManager_BridgeLevel.Instance.ShopClothesBuyButton(currCloth);
    }
    
}
