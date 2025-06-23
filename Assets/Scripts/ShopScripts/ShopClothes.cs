using UnityEngine;


[CreateAssetMenu(fileName = "ShopClothes", menuName = "Scriptable Objects/ShopClothes")]
public class ShopClothes : ScriptableObject
{
    public string clothName;
    public int cost;
    public CostTypeCloth costType;
    public Sprite clothImage;
    public bool purchased;
}

public enum CostTypeCloth
{
    Coins,
    Diamonds
}