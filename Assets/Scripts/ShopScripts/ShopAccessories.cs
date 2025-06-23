using UnityEngine;


[CreateAssetMenu(fileName = "ShopAccessories", menuName = "Scriptable Objects/ShopAccessories")]
public class ShopAccessories : ScriptableObject
{
    public string accessoriesName;
    public int cost;
    public CostTypeAccessories costType;
    public Sprite accessoriesImage;
    public bool purchased;
}

public enum CostTypeAccessories
{
    Coins,
    Diamonds
}