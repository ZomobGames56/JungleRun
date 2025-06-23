using UnityEngine;
using UnityEngine.SceneManagement;

public class ExploreScreenManager : MonoBehaviour
{
    public void LevelButton(string level)
    {
        switch (level)
        {
            case "Jungle":
                SceneManager.LoadScene("Jungle");
                break;
                
            case "Lava":
                SceneManager.LoadScene("Lava");
                break;

            case "Magical":
                SceneManager.LoadScene("Magical");
                break;

            case "FloatingBridge":
                SceneManager.LoadScene("FloatingBridge");
                break;

            case "Ice":
                SceneManager.LoadScene("Ice");
                break;

            case "Horror":
                SceneManager.LoadScene("Horror");
                break;

            default:
                break;
        }
    }
}
