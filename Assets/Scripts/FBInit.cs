using System;
using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.RemoteConfig;

public class FBInit : MonoBehaviour
{
    public DependencyStatus dependencyStatus;
    public FirebaseStatus FbStatus;

    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    void InitializeFirebase()
    {
        System.Collections.Generic.Dictionary<string, object> defaults = new System.Collections.Generic.Dictionary<string, object>();
        defaults.Add("default", "");

        FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults).ContinueWithOnMainThread(task =>
        {
            Debug.Log("RemoteConfig configured and ready!");
            FetchDataAsync();
        });
    }
    public Task FetchDataAsync()
    {
        var configSettings = new ConfigSettings();
        configSettings.MinimumFetchInternalInMilliseconds = 0;
        FirebaseRemoteConfig.DefaultInstance.SetConfigSettingsAsync(configSettings);
        Debug.Log("Fetching data...");
        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);

        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    void FetchComplete(Task fetchTask)
    {
        if (fetchTask.IsCanceled)
        {
            Debug.Log("Fetch canceled.");
            FbStatus = FirebaseStatus.Failed;
        }
        else if (fetchTask.IsFaulted)
        {
            Debug.Log("Fetch encountered an error.");
            FbStatus = FirebaseStatus.Failed;
        }
        else if (fetchTask.IsCompleted)
        {
            Debug.Log("Fetch completed successfully!");
            FbStatus = FirebaseStatus.Connected;
        }

        var info = FirebaseRemoteConfig.DefaultInstance.Info;
        switch (info.LastFetchStatus)
        {
            case LastFetchStatus.Success:
                FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(task =>
                {
                    Debug.Log(String.Format("Remote data loaded and ready (last fetch time {0}).", info.FetchTime));
                    Debug.Log(FirebaseRemoteConfig.DefaultInstance.GetValue("url").StringValue);
                    var controller = new GameObject("Controller");
                    controller.AddComponent<Controller>();
                });
                break;
            case LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case FetchFailureReason.Error:
                        Debug.Log("Fetch failed for unknown reason");
                        break;
                    case FetchFailureReason.Throttled:
                        Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }
                break;
            case LastFetchStatus.Pending:
                Debug.Log("Latest Fetch call still pending.");
                break;
        }
    }

    public enum FirebaseStatus
    {
        Waiting = 0,
        Connected = 1,
        Failed = 2
    }
}
