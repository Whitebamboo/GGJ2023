using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyLocalMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition =  new Vector3(0, 0.05f, 0);
        transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        StartCoroutine(StartTween());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
     IEnumerator StartTween()
     {
         yield return new WaitForSeconds(Random.value * 0.6f);
         transform.DOScale(1f, 0.3f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
         transform.DOLocalMoveY(-0.05f, 0.3f, false).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
     }
}
