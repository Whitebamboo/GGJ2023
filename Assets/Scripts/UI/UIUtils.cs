using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIUtils : CSingletonMono<UIUtils>
{
    public void hideUi(GameObject ui)
    {
        CanvasGroup c =  ui.GetComponent<CanvasGroup>();
        c.alpha = 0;
        c.interactable = false;
        c.blocksRaycasts = false;

    }

    public void showUi(GameObject ui)
    {
        CanvasGroup c = ui.GetComponent<CanvasGroup>();
        c.alpha = 1;
        c.interactable = true;
        c.blocksRaycasts = true;

    }
}
