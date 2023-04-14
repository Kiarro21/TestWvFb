using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject _internetPopUp;

    void Start()
    {

    }

    public void SetText(string text)
    {
        _internetPopUp.GetComponentInChildren<Text>().text = text;
    }

    public void ShowPopUp()
    {
        _internetPopUp.SetActive(true);
    }
}
