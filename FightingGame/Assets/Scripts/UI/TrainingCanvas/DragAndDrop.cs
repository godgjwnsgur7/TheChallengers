using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    GameObject parent;
    SettingPanel settingPanel;
    UpdatableUI thisUpdatbleUI;

    RectTransform parentRectTransform;
    CanvasGroup canvasGroup;
    Image DragUIImage;

    Vector2 currentPosition;

    float parentHalfWidth;
    float parentHalfHeight;
    float xRange;
    float yRange;
    bool isUpdate = false;

    public float AlphaThreshold = 0.1f;

    public void Init()
    {
        thisUpdatbleUI = this.gameObject.GetComponent<UpdatableUI>();

        parent = this.gameObject.transform.parent.gameObject;
        settingPanel = this.gameObject.transform.root.Find("SettingPanel").GetComponent<SettingPanel>();

        canvasGroup = GetComponent<CanvasGroup>();
        parentRectTransform = parent.GetComponent<RectTransform>();

        // Buttom Click 가능 범위 설정
        DragUIImage = GetComponent<Image>();
        DragUIImage.alphaHitTestMinimumThreshold = AlphaThreshold;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (settingPanel != null)
            isUpdate = settingPanel.isUpdate;

        if (!isUpdate)
            return;

        DragInit();

        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void DragInit()
    {
        if(settingPanel != null)
            settingPanel.PushKey(thisUpdatbleUI);

        parentHalfWidth = parentRectTransform.sizeDelta.x / 2;
        parentHalfHeight = parentRectTransform.sizeDelta.y / 2;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (settingPanel != null)
            isUpdate = settingPanel.isUpdate;

        if (!isUpdate)
            return;
        // 이전 이동과 비교해서 얼마나 이동했는지를 보여줌
        thisUpdatbleUI.SetTransform(thisUpdatbleUI.GetTransform() + eventData.delta);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (settingPanel != null)
            isUpdate = settingPanel.isUpdate;

        if (!isUpdate)
            return;

        EndDragChk();

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void EndDragChk()
    {
        xRange = Mathf.Clamp(thisUpdatbleUI.GetTransform().x, -parentHalfWidth + thisUpdatbleUI.GetHalfWidth(), parentHalfWidth - thisUpdatbleUI.GetHalfWidth());
        yRange = Mathf.Clamp(thisUpdatbleUI.GetTransform().y, -parentHalfHeight + thisUpdatbleUI.GetHalfHeight(), parentHalfHeight - thisUpdatbleUI.GetHalfHeight());

        currentPosition = new Vector2(xRange, yRange);
        thisUpdatbleUI.SetTransform(currentPosition);
    }
}