using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Scriptable Objects/Character")]
public class Characters : ScriptableObject
{
    public string characterName;
    public Sprite charIcon;
    public GameObject charPrefab;
    public GameObject ragdollPrefab;
    public int speed;
    public int strength;
    public int stamina;
    public int cost;
    public CostTypeChar costType;
    public Sprite costTypeSprite;
    public bool purchased;
    public bool selected;

    public void Save()
    {
        PlayerPrefs.SetInt(characterName + "_selected", selected ? 1 : 0);
        PlayerPrefs.SetInt(characterName + "purchased", purchased ? 1 : 0);
    }
}

public enum CostTypeChar
{
    Coins,
    Diamonds
}
