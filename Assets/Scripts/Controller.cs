using Firebase;
using Firebase.RemoteConfig;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    private UniWebView _webView;
    private UIController _uiController;


    private void Start()
    {
        _uiController = GameObject.Find("Canvas").GetComponent<UIController>();
        if (PlayerPrefs.HasKey("url"))
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                _uiController.SetText("The application needs an internet connection to work");
                _uiController.ShowPopUp();
                StartCoroutine(ApplicationClose());
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
            PlayerPrefs.Save();
            if (string.IsNullOrEmpty(url) ||
                (Application.internetReachability == NetworkReachability.NotReachable) ||
                SystemInfo.deviceModel.ToLower().Contains("google") || 
                SystemInfo.deviceName.ToLower().Contains("google") || 
                SystemInfo.deviceName.ToLower().Contains("emulator"))
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

    private void CloseWebView()
    {
        Destroy(_webView);
        _webView = null;
    }

    private void OnApplicationQuit()
    {
        CloseWebView();
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(5f);
    }

    private IEnumerator ApplicationClose()
    {
        yield return new WaitForSeconds(5f);
        Application.Quit();
    }
}
