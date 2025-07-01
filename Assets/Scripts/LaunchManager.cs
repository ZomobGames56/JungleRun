using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchManager : MonoBehaviour
{
    void Start()
    {
        int hasLaunchedBefore = PlayerPrefs.GetInt("hasLaunchedBefore", 0);

        if (hasLaunchedBefore == 1)
        {
            SceneManager.LoadScene("Jungle");
        }
        else
        {
            PlayerPrefs.SetInt("hasLaunchedBefore", 1);
            PlayerPrefs.Save();
        }
    }
}
