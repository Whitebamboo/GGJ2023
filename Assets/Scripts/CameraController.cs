using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraController : CSingletonMono<CameraController>
{
    public Transform StartPoint;//when start menu
    public Transform InGamePoint;
    public Transform CameraUpRoot;
    public float dis = 5;
    public float min_dis = 5;
    public float max_dis = 40;
    private Vector3 target_position;
    public float move_distance_mult = 10;
    float x;//mouse scroll value
    float y;//a distance depend on x how much to go down


    public GameObject startMenu;//temporary

    public void Start()
    {
        CameraUpRoot.position = StartPoint.position;
    }

    private void Update()
    {
        if(GameManager.instance.state == GameState.InGame)
        {
            SetCameraPosition();
        }
            
    }
    public void OnClickStart()
    {
        if(GameManager.instance.state == GameState.StartMenu)
        {
            //HideStartMenu
            UIUtils.instance.hideUi(startMenu);
            StartCoroutine(ZoomInCamera());
        }
    }

    IEnumerator ZoomInCamera()
    {
        CalculateTargetPosition(dis);
        Tween myTween = CameraUpRoot.DOMove(target_position, 2);
        yield return myTween.WaitForCompletion();
        // This log will happen after the tween has completed
        Debug.Log("Game Started!");
        GameManager.instance.SetState(GameState.InGame);
    }

    /// <summary>
    /// get a target position
    /// </summary>
    /// <param name="dis"></param>
    private void CalculateTargetPosition(float dis)
    {
        target_position = InGamePoint.position - new Vector3(0, 0, dis);
    }


    /// <summary>
    /// actually zoom in zoom out
    /// </summary>
    private void SetCameraPosition()
    {
        x = Input.GetAxis("Mouse ScrollWheel");
        dis -= x * move_distance_mult;
        dis = Mathf.Clamp(dis,min_dis, max_dis);
        CalculateTargetPosition(dis);
        CameraUpRoot.position = Vector3.Lerp(CameraUpRoot.position, target_position, 0.1f);
    }
}
