using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterItemUI : MonoBehaviour
{
    public Button button;
    public Image image;
    private Characters character;

    public void Setup(Characters data, System.Action<Characters, int> onClicked, int coins)
    {
        character = data;
        image.sprite = data.charIcon;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClicked?.Invoke(character, coins));
    }
}
