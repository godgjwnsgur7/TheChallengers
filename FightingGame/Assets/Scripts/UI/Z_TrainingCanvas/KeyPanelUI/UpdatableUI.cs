using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using UnityEngine.EventSystems;

public class UpdatableUI : UIElement, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public bool isUpdatable = true;
    public bool isSelect = false;
    private bool isInit = false;
    private bool isDragable = false;

    public GameObject btnArea;
    public Image btnAreaImage;

    protected Color changeColor;

    public SettingPanel settingPanel;
    [SerializeField] UISettingHelper settingHelper;
    CanvasGroup canvasGroup;
    Image DragUIImage;

    public float AlphaThreshold = 0.1f;

    int collisionCount = 0;

    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    public override void Close()
    {
        base.Close();
    }

    public virtual void init()
    {
        if (isInit)
            return;

        btnArea = this.transform.Find("BtnArea").gameObject;
        btnAreaImage = btnArea.GetComponent<Image>();
        isInit = true;

        if (this.gameObject.transform.root.Find("SettingPanel") != null)
            settingPanel = this.gameObject.transform.root.Find("SettingPanel").GetComponent<SettingPanel>();

        if (this.gameObject.transform.root.Find("UISettingHelper") != null)
            settingHelper = this.gameObject.transform.root.Find("UISettingHelper").GetComponent<UISettingHelper>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = this.gameObject.AddComponent<CanvasGroup>();

        // Buttom Click 가능 범위 설정
        DragUIImage = GetComponent<Image>();
        DragUIImage.alphaHitTestMinimumThreshold = AlphaThreshold;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)ENUM_LAYER_TYPE.UI)
        {
            // 충돌중인 UI카운트 + 1 
            collisionCount++;

            if (collisionCount == 1)
                settingPanel.unEditCount++;

            if (collisionCount > 0)
                isUpdatable = false;

            ChangeAreaColor();
        }
    }

    // UI 영역이 겹치지 않게 되면 수정 가능, Area영역 색상 수정
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)ENUM_LAYER_TYPE.UI)
        {
            collisionCount--;

            if (collisionCount < 1)
            {
                isUpdatable = true;
                settingPanel.unEditCount--;
            }
            
            ChangeAreaColor();
        }
    }

    // -------------------------------------------------------------------- Drag Area Change
    // 드래그 관련 행위 후 Area 색 변경
    public void ChangeAreaColor() 
    {
        changeColor = btnAreaImage.color;

        // Highlight
        if (isUpdatable && isSelect)
            changeColor = new Color(0, 255, 0, 0.5f);
        else if (!isUpdatable && isSelect)
            changeColor = new Color(255, 0, 0, 0.5f);
        else if (isUpdatable && !isSelect)
            changeColor = new Color(255, 255, 255, 0);
        else
            changeColor = new Color(255, 0, 0, 0.5f);

        btnAreaImage.color = changeColor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (settingPanel != null)
            isDragable = settingPanel.isUpdate;

        if (!isDragable)
            return;

        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;

        DragInit();
    }

    public void DragInit()
    {
        // 드래그와 동시에 선택되도록 설정
        if (this.GetComponent<UpdatableUI>() != null)
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

        // 위치이동 가능 범위 체크
        settingHelper.CheckUITransform();

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}
