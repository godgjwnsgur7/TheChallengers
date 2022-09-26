using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public SettingPanel settingPanel;
    [SerializeField] UISettingHelper settingHelper;
    CanvasGroup canvasGroup;
    Image DragUIImage;

    bool isDragable = false;

    public float AlphaThreshold = 0.1f;

    public void init()
    {
        if (this.gameObject.transform.root.Find("SettingPanel") != null)
            settingPanel = this.gameObject.transform.root.Find("SettingPanel").GetComponent<SettingPanel>();

        if (this.gameObject.transform.root.Find("UpdateUIHelper") != null)
            settingHelper = this.gameObject.transform.root.Find("UpdateUIHelper").GetComponent<UISettingHelper>();

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
        if(this.GetComponent<UpdatableUI>() != null)
            settingPanel.PushKey(this.GetComponent<UpdatableUI>());
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (settingPanel != null)
            isDragable = settingPanel.isUpdate;

        if (!isDragable)
            return;

        // 이전 이동과 비교해서 얼마나 이동했는지를 보여줌
        settingHelper.SetTransform(settingHelper.GetTransform() + eventData.delta);
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
        settingHelper.CheckUITransform();
    }
}
