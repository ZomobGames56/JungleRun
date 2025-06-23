using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopPopulator : MonoBehaviour
{
    public bool jumpingLevel;
    public List<ShopCharacter> characters;
    public List<ShopClothes> clothes;
    public List<ShopAccessories> accessories;
    public GameObject characterItemPrefab;
    public GameObject clothItemPrefab;
    public GameObject accessoriesItemPrefab;
    public Transform characterContentParent;
    public Transform clothesContentParent;
    public Transform accessoriesContentParent;

    void OnEnable()
    {
        if (jumpingLevel)
        {
            GameManager_Jumping.shopPopulate += PopulateShop;
            GameManager_Jumping.buyCheck += BuyCheck;
        }
        else
        {
            GameManager_BridgeLevel.shopPopulate += PopulateShop;
            GameManager_BridgeLevel.buyCheck += BuyCheck;
        }
    }

    void OnDisable()
    {
        if (jumpingLevel)
        {
            GameManager_Jumping.shopPopulate -= PopulateShop;
            GameManager_Jumping.buyCheck -= BuyCheck;
        }
        else
        {
            GameManager_BridgeLevel.shopPopulate -= PopulateShop;
            GameManager_BridgeLevel.buyCheck -= BuyCheck;
        }
    }

    void PopulateShop()
    {
        foreach (ShopCharacter character in characters)
        {
            GameObject item = Instantiate(characterItemPrefab, characterContentParent);
            ShopCharacterUI ui = item.GetComponent<ShopCharacterUI>();
            ui.jumpingLevel = jumpingLevel;
            ui.Setup(character);
        }
        foreach (ShopClothes cloth in clothes)
        {
            GameObject item = Instantiate(clothItemPrefab, clothesContentParent);
            ShopClothesUI ui = item.GetComponent<ShopClothesUI>();
            ui.jumpingLevel = jumpingLevel;
            ui.Setup(cloth);
        }
        foreach (ShopAccessories access in accessories)
        {
            GameObject item = Instantiate(accessoriesItemPrefab, accessoriesContentParent);
            ShopAccessoriesUI ui = item.GetComponent<ShopAccessoriesUI>();
            ui.jumpingLevel = jumpingLevel;
            ui.Setup(access);
        }
    }

    void BuyCheck(int coins)
    {
        foreach (Transform child in characterContentParent)
        {
            var ui = child.GetComponent<ShopCharacterUI>();
            ui?.BuyCheck(coins);
        }
        foreach (Transform child in clothesContentParent)
        {
            var ui = child.GetComponent<ShopClothesUI>();
            ui?.BuyCheck(coins);
        }
        foreach (Transform child in accessoriesContentParent)
        {
            var ui = child.GetComponent<ShopAccessoriesUI>();
            ui?.BuyCheck(coins);
        }
    }
}
