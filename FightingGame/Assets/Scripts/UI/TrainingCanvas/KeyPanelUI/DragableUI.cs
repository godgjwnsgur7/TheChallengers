using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragableUI : UpdatableUI, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public SettingPanel settingPanel;
    CanvasGroup canvasGroup;
    Image DragUIImage;

    bool isDragable = false;

    public float AlphaThreshold = 0.1f;

    public override void init()
    {
        base.init();

        if(this.gameObject.transform.root.Find("SettingPanel") != null)
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
            isDragable = settingPanel.isUpdate;

        if (!isDragable)
            return;

        DragInit();

        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void DragInit()
    {
        settingPanel.PushKey(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (settingPanel != null)
            isDragable = settingPanel.isUpdate;

        if (!isDragable)
            return;

        // 이전 이동과 비교해서 얼마나 이동했는지를 보여줌
        this.SetTransform(this.GetTransform() + eventData.delta);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (settingPanel != null)
            isDragable = settingPanel.isUpdate;

        if (!isDragable)
            return;

        EndDragChk();

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void EndDragChk()
    {
        this.CheckUITransform();
    }
}