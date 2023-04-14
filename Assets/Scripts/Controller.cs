using Firebase.RemoteConfig;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    private FirebaseManager _firebaseManager;
    FirebaseRemoteConfig remoteConfig;
    private UniWebView _webView;
    [SerializeField] private GameObject _internetPopUp;
    [SerializeField] private Text _internetTextPopUp;
    private string _url;


    private void Awake()
    {
        //StartCoroutine(WaitBegin());
        if (PlayerPrefs.HasKey("url"))
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                ShowInternetAbsence();
            }
            else
            {
                CreateWebView();
            }
        }
        else
        {
            var url = FirebaseRemoteConfig.DefaultInstance.GetValue("url").StringValue;
            PlayerPrefs.SetString("url", url);

            if (string.IsNullOrEmpty(url) || SystemInfo.deviceModel.ToLower().Contains("google") || SystemInfo.deviceName.ToLower().Contains("google") || SystemInfo.deviceName.ToLower().Contains("emulator"))
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                CreateWebView();
            }
        }
    }

    private void CreateFirebaseAsync()
    {
        var firebaseGameObject = new GameObject("FirebaseManager");
        _firebaseManager = firebaseGameObject.AddComponent<FirebaseManager>();

        //_firebaseManager.SetDefault();
        //_firebaseManager.FetchRemoteConfigAsync();
    }

    public void CreateWebView()
    {
        var webViewGameObject = new GameObject("UniWebView");
        _webView = webViewGameObject.AddComponent<UniWebView>();
        _webView.AddComponent<WebViewController>();
    }

    private void ShowInternetAbsence()
    {
        _internetTextPopUp.text = "The application needs an internet connection to work";
        _internetPopUp.SetActive(true);
        StartCoroutine(ApplicationClose());
    }

    private void CloseWebView()
    {
        Destroy(_webView);
        _webView = null;
    }

    private void OnApplicationQuit()
    {
        Destroy(_firebaseManager);
        _firebaseManager = null;
        CloseWebView();
    }

    private IEnumerator WaitBegin()
    {
        Debug.Log("Some waiting before work");
        _internetPopUp = GameObject.FindGameObjectWithTag("InternerPopUp");
        _internetTextPopUp = GameObject.FindGameObjectWithTag("InternerPopUpText").GetComponent<Text>();
        yield return new WaitForSeconds(5f);
    }

    private IEnumerator ApplicationClose()
    {
        yield return new WaitForSeconds(5f);
        Application.Quit();
    }
}
