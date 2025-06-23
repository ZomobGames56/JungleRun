using UnityEngine;


[CreateAssetMenu(fileName = "ShopCharacter", menuName = "Scriptable Objects/ShopCharacter")]
public class ShopCharacter : ScriptableObject
{
    public string characterName;
    public int cost;
    public string byLine;
    public CostType costType;
    public Sprite characterImage;
    public bool purchased;
}

public enum CostType
{
    Coins,
    Diamonds
}