using Firebase;
using Firebase.Analytics;
using UnityEngine;

public class FirebaseInit : MonoBehaviour
{
    public static bool isFirebaseReady { get; private set; } = false;
    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            isFirebaseReady = true;
            Debug.Log("Firebase Analytics Initialized");
        });
    }
}




