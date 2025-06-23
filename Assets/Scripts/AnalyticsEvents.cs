using Firebase.Analytics;
using UnityEngine;

public class AnalyticsEvents : MonoBehaviour
{
    private static AnalyticsEvents instance;
    public static bool gameStarted = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void GameStartEvent()
    {
        if (FirebaseInit.isFirebaseReady)
        {
            gameStarted = true;
            FirebaseAnalytics.LogEvent("Game_Start", new Parameter("GameStarted", gameStarted ? 1 : 0));
            Debug.Log("GameStartEvent Called");
        }
        else
        {
            Debug.Log("Failed to load");
            Debug.Log(FirebaseInit.isFirebaseReady);
        }
    }

    public static void LevelStartEvent(string level)
    {
        if (FirebaseInit.isFirebaseReady)
        {
            FirebaseAnalytics.LogEvent("Level_Start", new Parameter("Level", level));
            Debug.Log("LevelStartEvent Called - " + level);
        }
        else
        {
            Debug.Log("Failed to load");
            Debug.Log(FirebaseInit.isFirebaseReady);
        }
    }

    // public static void LevelNameEvent(string name)
    // {
    //     if (FirebaseInit.isFirebaseReady)
    //     {
    //         FirebaseAnalytics.LogEvent("Level_Name", new Parameter("Level", name));
    //         Debug.Log("LevelNameEvent Called");
    //     }
    // }

    public static void LevelCompleteEvent(string level)
    {
        if (FirebaseInit.isFirebaseReady)
        {
            FirebaseAnalytics.LogEvent("Level_Completed", new Parameter("Level", level));
            // FirebaseAnalytics.LogEvent("Status", new Parameter("Status", status));
            Debug.Log("LevelCompletedEvent Called - " + level);
        }
    }

    public static void GameOverEvent()
    {
        if (FirebaseInit.isFirebaseReady)
        {
            gameStarted = false;
            FirebaseAnalytics.LogEvent("Game_Start", new Parameter("GameStarted", gameStarted ? 1 : 0));
            Debug.Log("GameOverEvent Called");
        }
    }
}
