using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HPBar : MonoBehaviour
{
    
    public Image HP_lerp;
    public Image HP_bar;


    private float defaultHP;
    private float currenHP;
    private float previousHP;//
    private void Update()
    {
        if (previousHP > currenHP)
        {
            previousHP = Mathf.Lerp(previousHP, currenHP, 0.2f);
            HP_lerp.fillAmount = previousHP / defaultHP;
        }
    }
    public void InitialHP(float health)
    {
        defaultHP = health;
        currenHP = defaultHP;
        previousHP = defaultHP;
    }

    public void UpdateHP(float newHP)
    {
        currenHP = newHP;
        HP_bar.fillAmount = currenHP / defaultHP;
    }

}
