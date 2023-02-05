using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DescriptionUI : MonoBehaviour
{
    public GameObject box;

    public TextMeshProUGUI tmp;

    public void SetText(string description)
    {
        gameObject.SetActive(true);
        box.SetActive(true);
        tmp.text = description;
    }

    public void DisableBox()
    {
        box.SetActive(false);
    }
}
