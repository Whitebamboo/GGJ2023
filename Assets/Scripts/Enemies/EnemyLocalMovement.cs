using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyLocalMovement : MonoBehaviour
{
    private Vector3 scale0;

    // Start is called before the first frame update
    void Start()
    {
        scale0 = transform.localScale;
        transform.localPosition =  new Vector3(0, 0.05f, 0);
        transform.localScale = new Vector3(0.9f * scale0.x, 0.9f * scale0.y, 0.9f * scale0.z);
        StartCoroutine(StartTween());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
     IEnumerator StartTween()
     {
         yield return new WaitForSeconds(Random.value * 0.6f);
         transform.DOScale(1f * scale0.x, 0.3f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
         transform.DOLocalMoveY(-0.05f, 0.3f, false).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
     }
}
