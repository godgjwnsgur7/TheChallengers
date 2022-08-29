using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    SettingPanel settingPanel;
    UpdatableUI thisUpdatbleUI;

    CanvasGroup canvasGroup;
    Image DragUIImage;

    bool isUpdate = false;

    public float AlphaThreshold = 0.1f;

    public void Init()
    {
        thisUpdatbleUI = this.gameObject.GetComponent<UpdatableUI>();
        if (thisUpdatbleUI == null)
            thisUpdatbleUI = this.gameObject.AddComponent<UpdatableUI>();

        thisUpdatbleUI.parent = this.gameObject.transform.parent.gameObject;
        settingPanel = this.gameObject.transform.root.Find("SettingPanel").GetComponent<SettingPanel>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = this.gameObject.AddComponent<CanvasGroup>();

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
        thisUpdatbleUI.CheckUITransform();
    }
}