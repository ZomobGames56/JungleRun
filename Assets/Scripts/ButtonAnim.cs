using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonAnim : MonoBehaviour
{
    Button myButton;

    void Start()
    {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(AnimateButton);
    }
    
    void AnimateButton()
    {
        // myButton.transform.DOKill();
        myButton.transform.DOScale(1.2f, 0.1f).OnComplete(() =>
        {
            myButton.transform.DOScale(1f, 0.1f);
        });
    }
}
