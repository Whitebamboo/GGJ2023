using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NetworkLayerUI : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject LayerNodePrefab;

    public Image containerImage;

    private Color normalColor;
    public Color highlightColor = Color.yellow;
    public Color hintColor = Color.yellow;

    public bool canAdd = true;

    public int layerIndex;

    public void OnEnable()
    {
        if (containerImage != null)
            normalColor = containerImage.color;
    }

    public void OnDrop(PointerEventData data)
    {
        if (!canAdd)
            return;
        
        containerImage.color = normalColor;

        Sprite dropSprite = GetDropSprite(data);

        GameObject obj = Instantiate(LayerNodePrefab, transform);
        obj.transform.GetChild(1).GetComponent<Image>().sprite = dropSprite;

        if(transform.childCount == 5)
        {
            canAdd = false;
        }

        if (transform.childCount == 3 && layerIndex >= 2)
        {
            GetComponentInParent<NetworkUI>().AddLayer();
        }

        SkillConfig config = GetConfig(data);

        Tree.instance.NetworkModule.AddNodeToLayer(layerIndex, new TreeNode(config));

        var originalObj = data.pointerDrag;
        Destroy(originalObj);
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if (containerImage == null || !canAdd)
            return;

        Sprite dropSprite = GetDropSprite(data);
        if (dropSprite != null)
            containerImage.color = highlightColor;
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (containerImage == null || !canAdd)
            return;

        containerImage.color = normalColor;
    }

    private Sprite GetDropSprite(PointerEventData data)
    {
        var originalObj = data.pointerDrag;
        if (originalObj == null)
            return null;

        var dragMe = originalObj.GetComponent<NodeDragUI>();
        if (dragMe == null)
            return null;

        var srcImage = originalObj.GetComponent<Image>();
        if (srcImage == null)
            return null;

        return srcImage.sprite;
    }

    private SkillConfig GetConfig(PointerEventData data)
    {
        var originalObj = data.pointerDrag;
        if (originalObj == null)
            return null;

        var dragMe = originalObj.GetComponent<NodeDragUI>();
        if (dragMe == null)
            return null;

        return dragMe.skillConfig; 
    }
}
