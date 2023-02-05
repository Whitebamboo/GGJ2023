using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class YesNoUI : MonoBehaviour
{
    public Button yes;
    public Button no;
    public GameObject YesImage;
    public GameObject NoImage;

    public void OnYesClick()
    {
        YesImage.SetActive(true);
        NoImage.SetActive(false);
    }

    public void OnNoClick()
    {
        YesImage.SetActive(false);
        NoImage.SetActive(true);
    }

    public void ResetUI()
    {
        EventSystem.current.SetSelectedGameObject(null);
        YesImage.SetActive(false);
        NoImage.SetActive(false);
    }
}
