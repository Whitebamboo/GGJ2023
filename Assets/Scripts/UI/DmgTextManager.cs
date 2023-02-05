using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum DmgType {
    EnemyWeak,//xuruo
    EnemyNormal,
    EnemyRestraint,//kezhi
    EnemyCritical,//chengfabaoji
    PlayerWeak,
    PlayerNormal,
    PlayerCritical,
    
}

public class DmgTextManager : CSingletonMono<DmgTextManager>
{
    [SerializeField] private GameObject normalTextObject;
    [SerializeField] private GameObject criticalTextObject;
    [SerializeField] private float textLifeTime;
    private List<DmgText> texts = new List<DmgText>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddDmgText(float dmg, DmgType type, Vector3 pos)
    {
        var dmgInt = (int) dmg;
        var dmgString = dmgInt.ToString();
        DmgText dmgText;
        int i = texts.FindIndex(text => !text.gameObject.activeSelf && text.dmgType == type);
        if (i != -1)
        {
            dmgText = texts[i];
            dmgText.gameObject.SetActive(true);
            dmgText.textComponent.alpha = 1;
        }
        else
        {
            var go = CreateTextPrefab(type);
            go.transform.position = pos;
            dmgText = go.GetComponent<DmgText>();
            dmgText.dmgType = type;
            texts.Add(dmgText);
        }
        dmgText.gameObject.transform.position = pos;
        dmgText.textComponent.text = "-" + dmgString;
        //dmgText.transform.DOLocalJump(new Vector3(0, 0.2f, 0), 0.1f, 1, textLifeTime);
        //dmgText.transform.DOLocalMoveY()
        dmgText.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), textLifeTime);
        dmgText.textComponent.DOFade(0, textLifeTime).SetEase(Ease.InCirc).OnComplete(() => OnTextFaded(dmgText));
    }

    private void OnTextFaded(DmgText dmgText)
    {
        dmgText.gameObject.SetActive(false);
    }

    private GameObject CreateTextPrefab(DmgType dmgType)
    {
        switch (dmgType)
        {
            case DmgType.EnemyNormal:
                return Instantiate(normalTextObject, transform);
            case DmgType.EnemyCritical:
                return Instantiate(criticalTextObject, transform);
            default:
                return Instantiate(normalTextObject, transform);
        }
    }
}
