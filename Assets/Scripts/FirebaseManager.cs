using UnityEngine;
using Firebase.RemoteConfig;
using Firebase;
using System;

public class FirebaseManager : FBInitializer
{
    private FirebaseRemoteConfig _firebaseRemoteConfig;

    protected override void Awake()
    {
        _firebaseRemoteConfig = FirebaseRemoteConfig.DefaultInstance;
        
        base.Awake();
    }


    /*public async void FetchRemoteConfigAsync()
    {
        await FirebaseApp.CheckAndFixDependenciesAsync();

        var configSettings = new ConfigSettings();
        configSettings.MinimumFetchInternalInMilliseconds = 0;
        await FirebaseRemoteConfig.DefaultInstance.SetConfigSettingsAsync(configSettings);


        System.Collections.Generic.Dictionary<string, object> defaults = new System.Collections.Generic.Dictionary<string, object>();
        defaults.Add("default", "");
        await FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults);

        *//*await FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);

        await FirebaseRemoteConfig.DefaultInstance.ActivateAsync();*//*
        await FirebaseRemoteConfig.DefaultInstance.FetchAndActivateAsync();

        Debug.Log(FirebaseRemoteConfig.DefaultInstance.GetValue("url").StringValue);
        PlayerPrefs.SetString("url", FirebaseRemoteConfig.DefaultInstance.GetValue("url").StringValue);
    }*/

    public string GetStringValue(string param)
    {
        return FirebaseRemoteConfig.DefaultInstance.GetValue(param).StringValue;
    }

    private void OnApplicationQuit()
    {
        Destroy(gameObject);
    }
}