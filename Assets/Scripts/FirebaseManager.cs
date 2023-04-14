using UnityEngine;
using Firebase.RemoteConfig;
using Firebase;
using System;

public class FirebaseManager : MonoBehaviour
{
    private FirebaseRemoteConfig _firebaseRemoteConfig;

    /*protected override void Awake()
    {
        _firebaseRemoteConfig = FirebaseRemoteConfig.DefaultInstance;
        *//*var configSettings = new ConfigSettings();
        configSettings.MinimumFetchInternalInMilliseconds = 0;
        _firebaseRemoteConfig.SetConfigSettingsAsync(configSettings);*//*
        base.Awake();
    }*/

    public string GetStringValue(string param)
    {
        return FirebaseRemoteConfig.DefaultInstance.GetValue(param).StringValue;
    }

    private void OnApplicationQuit()
    {
        Destroy(gameObject);
    }

    /*
        IEnumerator FirstStart()
        {
            while (_fBInitializer.FbStatus == FBInitializer.FirebaseStatus.Waiting)
            {
                yield return new WaitForSeconds(0.1f);
                Debug.Log("Waiting");
            }
            if (_fBInitializer.FbStatus == FBInitializer.FirebaseStatus.Connected)
            {
                var url = FirebaseRemoteConfig.DefaultInstance.GetValue("url").StringValue;
                if (url != "")
                    PlayerPrefs.SetString("url", url);
            }
        }

    public void SetDefault()
    {

    }*/

    public async void FetchRemoteConfigAsync()
    {
        await FirebaseApp.CheckAndFixDependenciesAsync();

        var configSettings = new ConfigSettings();
        configSettings.MinimumFetchInternalInMilliseconds = 0;
        await FirebaseRemoteConfig.DefaultInstance.SetConfigSettingsAsync(configSettings);


        System.Collections.Generic.Dictionary<string, object> defaults = new System.Collections.Generic.Dictionary<string, object>();
        defaults.Add("default", "");
        await FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults);

        /*await FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);

        await FirebaseRemoteConfig.DefaultInstance.ActivateAsync();*/
        await FirebaseRemoteConfig.DefaultInstance.FetchAndActivateAsync();

        Debug.Log(FirebaseRemoteConfig.DefaultInstance.GetValue("url").StringValue);
        PlayerPrefs.SetString("url", FirebaseRemoteConfig.DefaultInstance.GetValue("url").StringValue);



        /*var configSettings = new ConfigSettings();
        configSettings.MinimumFetchInternalInMilliseconds = 0;
        _firebaseRemoteConfig.SetConfigSettingsAsync(configSettings);

        _firebaseRemoteConfig.FetchAndActivateAsync();

        Debug.Log($"{_firebaseRemoteConfig.GetValue("url").StringValue} is fetched");*/

        /*private IEnumerator FetchAndActivate()
        {
            yield return new WaitForSeconds(0.1f);

            _firebaseRemoteConfig.FetchAsync(TimeSpan.Zero);

            yield return new WaitForSeconds(10f);



            if (_firebaseRemoteConfig.Info.LastFetchStatus == LastFetchStatus.Success)
            {
                Debug.Log($"Last fetch time: {_firebaseRemoteConfig.Info.FetchTime}");
                _firebaseRemoteConfig.ActivateAsync();
                Debug.Log("Remote Config fetch and activate completed.");
            }
        }*/
    }
}