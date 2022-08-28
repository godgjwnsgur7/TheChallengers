using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class UpdatableUI : UIElement
{
    private DragAndDrop dragAndDrop;

    public bool isUpdatable = true;
    public bool isSelect = false;

    public GameObject btnArea;
    public GameObject backGround;
    public GameObject icon;

    public Image btnAreaImage;
    public Image backGroundImage;
    public Image iconImage;

    public RectTransform thisRect;
    public RectTransform btnAreaRect;
    public RectTransform backGroundRect;
    public RectTransform iconRect;

    BoxCollider2D thisBoxCollider;

    private Vector2 changeSize;
    private Color changeColor;

    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    public override void Close()
    {
        base.Close();
    }

    public void init()
    {
        dragAndDrop = GetComponent<DragAndDrop>();
        if (dragAndDrop == null)
            dragAndDrop = this.gameObject.AddComponent<DragAndDrop>();
        dragAndDrop.Init();

        btnAreaImage = btnArea.GetComponent<Image>();
        backGroundImage = backGround.GetComponent<Image>();
        iconImage = icon.GetComponent<Image>();

        thisRect = this.GetComponent<RectTransform>();
        btnAreaRect = btnArea.GetComponent<RectTransform>();
        backGroundRect = backGround.GetComponent<RectTransform>();
        iconRect = icon.GetComponent<RectTransform>();

        thisBoxCollider = GetComponent<BoxCollider2D>();
        if (thisBoxCollider == null)
            thisBoxCollider = this.gameObject.AddComponent<BoxCollider2D>();
        SetBoxCollider();
    }

    public void SetSize(float size)
    {
        changeSize = new Vector2(PlayerPrefs.GetFloat($"{backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX),
            PlayerPrefs.GetFloat($"{backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY));
        backGroundRect.sizeDelta = changeSize * size;

        changeSize = new Vector2(PlayerPrefs.GetFloat($"{iconRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX),
            PlayerPrefs.GetFloat($"{iconRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY));
        iconRect.sizeDelta = changeSize * size;

        changeSize = new Vector2(PlayerPrefs.GetFloat($"{btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX),
            PlayerPrefs.GetFloat($"{btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY));
        btnAreaRect.sizeDelta = changeSize * size;

        SetBoxCollider();
    }

    public void SetOpacity(float Opacity, bool isInit = false)
    {
        changeColor = btnAreaImage.color;
        changeColor.a = (isInit == true)? 0.0f : 0.5f;
        btnAreaImage.color = changeColor;

        changeColor = backGroundImage.color;
        changeColor.a = 0.5f + Opacity;
        backGroundImage.color = changeColor;

        changeColor = iconImage.color;
        changeColor.a = 0.5f + Opacity;
        iconImage.color = changeColor;
    }

    public void SetTransform(Vector2 rectTrans)
    {
        thisRect.anchoredPosition = rectTrans;
    }

    public Vector2 GetTransform()
    {
        return thisRect.anchoredPosition;
    }

    public void SetBoxCollider()
    {
        thisBoxCollider.size = thisRect.sizeDelta;

        if (!thisBoxCollider.isTrigger)
            thisBoxCollider.isTrigger = true;
    }

    public float GetHalfWidth()
    {
        return backGroundRect.sizeDelta.x / 2;
    }

    public float GetHalfHeight()
    {
        return backGroundRect.sizeDelta.y / 2;
    }
    
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

    // 드래그 관련 행위 후 Area 색 변경
    private void ChangeAreaColor() 
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

    // UI선택시 Area영역 색 변경
    public void OnOffUIArea()
    {
        isSelect = !isSelect;
        changeColor = btnAreaImage.color;

        // Highlight
        if (isSelect)
            changeColor = new Color(0, 255, 0, 0.5f);
        else
            changeColor = new Color(255, 255, 255, 0);

        btnAreaImage.color = changeColor;
    }
}
