using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Backgorund : MonoBehaviour
{
    [SerializeField] Image _backgroundImage;
    [SerializeField] List<Sprite> _backgroundList;

    void Start()
    {
        _backgroundImage.sprite = _backgroundList[Random.Range(0, _backgroundList.Count)];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
