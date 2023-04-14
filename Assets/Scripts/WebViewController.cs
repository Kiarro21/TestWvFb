using UnityEngine;

public class WebViewController : MonoBehaviour
{
    private UniWebView _webView;
    private UniWebViewInterface _webViewInterface;

    void Start()
    {
        _webView = GetComponent<UniWebView>();
        _webView.Frame = new Rect(0f, 0f, Screen.width, Screen.height);

        _webView.OnPageFinished += OnPageFinished;
        _webView.OnOrientationChanged += OrientationChanging;
        _webView.OnPageErrorReceived += OnPageErrorReceived;
        _webView.OnShouldClose += OnShouldClose;

        _webView.Load(PlayerPrefs.GetString("url"));
        _webView.Show();
        //UniWebViewInterface.SetShowSpinnerWhileLoading("", true);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_webView.CanGoBack)
            {
                _webView.GoBack();
            }
        }
    }

    private void OnPageFinished(UniWebView webView, int statusCode, string url)
    {
        Debug.Log($"Page {url} is loaded with {statusCode} code");
    }

    void OnPageErrorReceived(UniWebView webView, int errorCode, string errorMessage)
    {
        Debug.Log("Page failed to load with error code: " + errorCode + " and error message: " + errorMessage);
    }

    private bool OnShouldClose(UniWebView webView)
    {
        Destroy(webView.gameObject);
        Application.Quit();
        return true;
    }

    private void OrientationChanging(UniWebView view, ScreenOrientation orientation)
    {
        _webView.Frame = new Rect(0, 0, Screen.width, Screen.height);
    }
}
