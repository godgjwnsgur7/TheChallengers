using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class UISettingHelper : MonoBehaviour
{
    public Button updateUI;

    public GameObject parent;
    public RectTransform parentRect;

    public GameObject btnArea;
    public GameObject backGround;
    public GameObject icon;

    public Image btnAreaImage;
    public Image backGroundImage;
    public Image iconImage;

    public RectTransform thisRect;

    protected BoxCollider2D thisBoxCollider;
    protected Color changeColor;

    protected float xRange;
    protected float yRange;
    protected float scaleSizeX;
    protected float scaleSizeY;

    public void SetBtnInit(Button updateUI)
    {
        if (updateUI == null)
            return;

        this.updateUI = updateUI;

        parent = updateUI.gameObject.transform.parent.gameObject;
        parentRect = parent.GetComponent<RectTransform>();

        btnArea = this.updateUI.transform.Find("BtnArea").gameObject;
        backGround = this.updateUI.transform.Find("BackGround").gameObject;
        icon = this.updateUI.transform.Find("Icon").gameObject;

        btnAreaImage = btnArea.GetComponent<Image>();
        backGroundImage = backGround.GetComponent<Image>();
        iconImage = icon.GetComponent<Image>();

        thisRect = this.updateUI.GetComponent<RectTransform>();

        thisBoxCollider = this.updateUI.GetComponent<BoxCollider2D>();
        if (thisBoxCollider == null)
            thisBoxCollider = this.updateUI.gameObject.AddComponent<BoxCollider2D>();
        SetBoxCollider();
    }

    public void Clear()
    {
        updateUI = null;
        parent = null;
        parentRect = null;

        btnArea = null;
        backGround = null;
        icon = null;

        btnAreaImage = null;
        backGroundImage = null;
        iconImage = null;

        thisRect = null;

        thisBoxCollider = null;
    }

    // -------------------------------------------------------------------- Set
    // UI 크기 조절(50~150%)
    public virtual void SetSize(float size, bool isInit = false)
    {
        thisRect.localScale = new Vector3(size, size, size);

        if (!isInit)
            CheckUITransform();
    }

    // UI 투명도 조절(50~100%)
    public virtual void SetOpacity(float Opacity, bool isInit = false)
    {
        changeColor = btnAreaImage.color;
        changeColor.a = (isInit == true) ? 0.0f : 0.5f;
        btnAreaImage.color = changeColor;

        changeColor = backGroundImage.color;
        changeColor.a = 0.5f + Opacity;
        backGroundImage.color = changeColor;

        changeColor = iconImage.color;
        changeColor.a = 0.5f + Opacity;
        iconImage.color = changeColor;
    }

    // UI 위치 설정
    public void SetTransform(Vector2 rectTrans)
    {
        thisRect.anchoredPosition = rectTrans;

        CheckUITransform();
    }

    // UI 위치 체크
    public void CheckUITransform()
    {
        scaleSizeX = GetHalfWidth() * GetSize().x;
        scaleSizeY = GetHalfHeight() * GetSize().y;

        xRange = Mathf.Clamp(GetTransform().x, -GetHalfParentWidth() + scaleSizeX, GetHalfParentWidth() - scaleSizeX);
        yRange = Mathf.Clamp(GetTransform().y, -GetHalfParentHeight() + scaleSizeY, GetHalfParentHeight() - scaleSizeY);


        thisRect.anchoredPosition = new Vector3(xRange, yRange, 0);
    }

    // 박스 콜라이더 설정
    public void SetBoxCollider()
    {
        thisBoxCollider.size = thisRect.sizeDelta;

        if (!thisBoxCollider.isTrigger)
            thisBoxCollider.isTrigger = true;
    }

    // 스킬 아이콘 변경 (미구현)
    public void SetSkillImage(ENUM_CHARACTER_TYPE characterType)
    {
        // iconImage.sprite = 
    }

    // -------------------------------------------------------------------- Get
    // UI 위치 값
    public Vector2 GetTransform()
    {
        return thisRect.anchoredPosition;
    }

    // UI 가로 세로 길이
    public float GetHalfWidth()
    {
        return thisRect.sizeDelta.x / 2;
    }

    public float GetHalfHeight()
    {
        return thisRect.sizeDelta.y / 2;
    }

    // 부모 UI 가로 세로 길이
    public float GetHalfParentWidth()
    {
        return parentRect.sizeDelta.x / 2;
    }

    public float GetHalfParentHeight()
    {
        return parentRect.sizeDelta.y / 2;
    }

    // UI 스케일 값
    public Vector3 GetSize()
    {
        return thisRect.localScale;
    }
}