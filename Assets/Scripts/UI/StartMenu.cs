using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public GameObject tutorialMenu;

    public void OnTutorialChick()
    {
        tutorialMenu.SetActive(true);
    }

    public void OnTutorialClose()
    {
        tutorialMenu.SetActive(false);
    }
}
