using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialEnd_Jumping : MonoBehaviour
{
    [SerializeField] GameObject tutorialEndScreen;
    ForwardMovement_Jumping_Tutorial forwardMovement_Jumping_Tutorial;

    void Start()
    {
        forwardMovement_Jumping_Tutorial = GameObject.FindGameObjectWithTag("Player").GetComponent<ForwardMovement_Jumping_Tutorial>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            tutorialEndScreen.SetActive(true);
            forwardMovement_Jumping_Tutorial.enabled = false;
        }
    }

    public void EndTutorialButton()
    {
        SceneManager.LoadScene("Jungle");
    }
}
