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
    public bool isEnded = true;
    private bool isInit = false;

    public GameObject btnArea;
    public Image btnAreaImage;

    protected Color changeColor;

    public SettingPanel settingPanel;
    [SerializeField] UISettingHelper settingHelper;
    CanvasGroup canvasGroup;
    Image DragUIImage;

    bool isDragable = false;

    public float AlphaThreshold = 0.1f;

    ArrayList gos = new ArrayList();
    int index = 0;

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
        gos.Add(collision.gameObject);
    }

    // -------------------------------------------------------------------- Trigger
    // UI 영역 겹치는 동안 수정 불가능, Area영역 색상 수정
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)ENUM_LAYER_TYPE.UI)
        {
            isUpdatable = false;

            ChangeAreaColor();

            if (gos.Contains(collision.gameObject))
            {
                GameObject go = (GameObject)gos[gos.IndexOf(collision.gameObject)];
                go.transform.Find("BtnArea").gameObject.GetComponent<Image>().color =  new Color(255, 0, 0, 0.5f);
            }
        }
    }

    // UI 영역이 겹치지 않게 되면 수정 가능, Area영역 색상 수정
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)ENUM_LAYER_TYPE.UI)
        {
            isUpdatable = true;
            
            ChangeAreaColor();


            if (gos.Contains(collision.gameObject))
            {
                GameObject go = (GameObject)gos[gos.IndexOf(collision.gameObject)];

                go.transform.Find("BtnArea").gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0.0f);

                gos.Remove(collision.gameObject);
            }
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

        DragInit();

        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void DragInit()
    {
        Debug.Log(gos.Count);
        if (gos != null)
        {
            for (int i = gos.Count; i > 0; i++)
            {
                GameObject go = (GameObject)gos[i-1];

                go.transform.Find("BtnArea").gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0.0f);
                gos.RemoveAt(i);
            }
        }

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

        EndDragChk();

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void EndDragChk()
    {
        settingHelper.CheckUITransform();
    }

    public virtual void OnCoolTime() { }
}
