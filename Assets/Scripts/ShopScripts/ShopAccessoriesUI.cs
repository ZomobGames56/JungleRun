using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopAccessoriesUI : MonoBehaviour
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
    int accessCost;
    ShopAccessories currAccess;

    public void Setup(ShopAccessories accessories)
    {
        iconImage.sprite = accessories.accessoriesImage;
        nameText.text = accessories.accessoriesName;
        costText.text = accessories.cost.ToString();
        accessCost = accessories.cost;
        currAccess = accessories;

        switch (accessories.costType)
        {
            case CostTypeAccessories.Coins:
                costTypeImage.sprite = coinSprite;
                break;
            case CostTypeAccessories.Diamonds:
                costTypeImage.sprite = diamondSprite;
                break;
        }
    }

    public void BuyCheck(int coins)
    {
        if (!currAccess.purchased)
        {
            if (coins >= accessCost)
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
        if (jumpingLevel) GameManager_Jumping.Instance.ShopAccessoriesBuyButton(currAccess);
        else GameManager_BridgeLevel.Instance.ShopAccessoriesBuyButton(currAccess);
    }
}
