using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class UpdatableUI : UIElement
{
    public bool isUpdatable = true;
    public bool isSelect = false;
    public bool isEnded = true;
    private bool isInit = false;

    public GameObject btnArea;
    public Image btnAreaImage;

    protected Color changeColor;

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
    }

    // -------------------------------------------------------------------- Trigger
    // UI 영역 겹치는 동안 수정 불가능, Area영역 색상 수정
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)ENUM_LAYER_TYPE.UI)
        {
            changeColor = btnAreaImage.color;

            isUpdatable = false;

            ChangeAreaColor();
        }
    }

    // UI 영역이 겹치지 않게 되면 수정 가능, Area영역 색상 수정
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)ENUM_LAYER_TYPE.UI)
        {
            isUpdatable = true;

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

    public virtual void OnCoolTime() { }
}
