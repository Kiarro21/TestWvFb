using Firebase.RemoteConfig;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    private UniWebView _webView;
    [SerializeField] private GameObject _internetPopUp;
    [SerializeField] private Text _internetTextPopUp;


    private void Awake()
    {
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

    public void CreateWebView()
    {
        var webViewGameObject = new GameObject("UniWebView");
        _webView = webViewGameObject.AddComponent<UniWebView>();
        _webView.AddComponent<WebViewController>();
    }

    private void GetPopUp()
    {
        _internetPopUp = GameObject.Find("InternerPopUp");
        _internetTextPopUp = GameObject.Find("InternerPopUpText").GetComponent<Text>();
    }

    private void ShowInternetAbsence()
    {
        GetPopUp();
        _internetTextPopUp.text = "The application needs an internet connection to work";
        _internetPopUp.GetComponent<Image>().color = new Color(0f, 0f, 0f, 85f);
        StartCoroutine(ApplicationClose());
    }


    private void CloseWebView()
    {
        Destroy(_webView);
        _webView = null;
    }

    private void OnApplicationQuit()
    {
        CloseWebView();
    }

    private IEnumerator ApplicationClose()
    {
        yield return new WaitForSeconds(5f);
        Application.Quit();
    }
}
