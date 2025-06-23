using UnityEngine;
using Firebase;
using Firebase.Extensions;

public class FireBaseManager : MonoBehaviour
{
    private FirebaseApp app;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                app = FirebaseApp.DefaultInstance;
                Debug.Log("Firebase is ready to use.");
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }
}
