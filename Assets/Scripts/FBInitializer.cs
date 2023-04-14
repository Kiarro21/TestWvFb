using System;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Extensions;
using Firebase.RemoteConfig;

public class FBInitializer : MonoBehaviour
{
    //public GUISkin fb_GUISkin;
    private Vector2 controlsScrollViewVector = Vector2.zero;
    private Vector2 scrollViewVector = Vector2.zero;
    bool UIEnabled = true;
    private string logText = "";
    const int kMaxLogSize = 16382;
    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    public FirebaseStatus FbStatus = FirebaseStatus.Waiting;

    protected virtual void Awake()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
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

    // Initialize remote config, and set the default values.
    void InitializeFirebase()
    {
        System.Collections.Generic.Dictionary<string, object> defaults = new System.Collections.Generic.Dictionary<string, object>();
        defaults.Add("default", "");

        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults).ContinueWithOnMainThread(task =>
        {
            DebugLog("RemoteConfig configured and ready!");
            FetchDataAsync();
        });

    }

    public Task FetchDataAsync()
    {
        var configSettings = new ConfigSettings();
        configSettings.MinimumFetchInternalInMilliseconds = 0;
        FirebaseRemoteConfig.DefaultInstance.SetConfigSettingsAsync(configSettings);
        DebugLog("Fetching data...");
        Task fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);

        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    void FetchComplete(Task fetchTask)
    {
        if (fetchTask.IsCanceled)
        {
            DebugLog("Fetch canceled.");
            FbStatus = FirebaseStatus.Failed;
        }
        else if (fetchTask.IsFaulted)
        {
            DebugLog("Fetch encountered an error.");
            FbStatus = FirebaseStatus.Failed;
        }
        else if (fetchTask.IsCompleted)
        {
            DebugLog("Fetch completed successfully!");
            FbStatus = FirebaseStatus.Connected;
        }

        var info = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Info;
        switch (info.LastFetchStatus)
        {
            case Firebase.RemoteConfig.LastFetchStatus.Success:
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(task =>
                {
                    DebugLog(String.Format("Remote data loaded and ready (last fetch time {0}).", info.FetchTime));
                    DebugLog(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("url").StringValue);
                    PlayerPrefs.SetString("url", FirebaseRemoteConfig.DefaultInstance.GetValue("url").StringValue);
                    PlayerPrefs.Save();
                });
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case Firebase.RemoteConfig.FetchFailureReason.Error:
                        DebugLog("Fetch failed for unknown reason");
                        break;
                    case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                        DebugLog("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Pending:
                DebugLog("Latest Fetch call still pending.");
                break;
        }
    }

    // Output text to the debug log text field, as well as the console.
    public void DebugLog(string s)
    {
        print(s);
        logText += s + "\n";

        while (logText.Length > kMaxLogSize)
        {
            int index = logText.IndexOf("\n");
            logText = logText.Substring(index + 1);
        }

        scrollViewVector.y = int.MaxValue;
    }

    public enum FirebaseStatus
    {
        Waiting = 0,
        Connected = 1,
        Failed = 2
    }
}
