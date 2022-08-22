using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class BottomPanel : UIElement
{
    public Slider sizeSlider;
    public Slider opacitySlider;
    [SerializeField] Text SizeShowText;
    [SerializeField] Text OpacityShowText;
    [SerializeField] KeyPanelArea keyPanelArea;

    private GameObject parent;
    private RectTransform parentRect;

    private UpdatableUI setBtn;
    private RectTransform setBtnRect;
    private RectTransform setBtnIconRect;
    private RectTransform setBtnAreaRect;

    private Image setBtnImage;
    private Image setBtnIconImage;
    private Image setBtnAreaImage;

    private float halfWidth;
    private float halfHeight;
    private float pHalfWidth;
    private float pHalfHeight;

    private float tempX;
    private float tempY;
    private Vector2 TempRect;
    private Vector2 beforeSize;
    private Vector2 baseSize;
    private float sliderRatio;
    private Color tempColor;

    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    public override void Close()
    {
        if (setBtn != null)
        {
            keyPanelArea.OnOffDrag(setBtn);
            setBtn = null;
        }

        base.Close();
    }

    // Call ClickedButton Slider Setting Value
    public void setSlider(UpdatableUI updateUI)
    {
        // 이전 선택했던 UI 드래그 중지
        if (setBtn != null)
            keyPanelArea.OnOffDrag(setBtn);

        // 선택한 UI 세팅
        setBtn = updateUI;
        setBtnImage = setBtn.backGroundImage;
        setBtnIconImage = setBtn.iconImage;
        setBtnAreaImage = setBtn.btnAreaImage;

        setBtnRect = setBtn.backGroundRect;
        setBtnIconRect = setBtn.iconRect;
        setBtnAreaRect = setBtn.btnAreaRect;
        beforeSize = setBtnRect.sizeDelta;

        parent = setBtn.transform.parent.gameObject;
        parentRect = parent.GetComponent<RectTransform>();

        // 부모, 자신의 절반길이
        halfWidth = setBtnRect.sizeDelta.x / 2;
        halfHeight = setBtnRect.sizeDelta.y / 2;
        pHalfWidth = parentRect.sizeDelta.x / 2;
        pHalfHeight = parentRect.sizeDelta.y / 2;

        // UI 실린더 값 호출
        sizeSlider.value = PlayerPrefs.GetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.Size);
        opacitySlider.value = PlayerPrefs.GetFloat($"{setBtn.backGroundImage.name}" + ENUM_PLAYERPREFS_TYPE.Opacity);
        SetSliderText("All");

        // UI 드래그 기능
        keyPanelArea.OnOffDrag(setBtn);
    }


    public void SetSliderText(string sliderType)
    {
        switch (sliderType)
        {
            case "All":
                SizeShowText.text = (int)sizeSlider.value + "%";
                OpacityShowText.text = (int)opacitySlider.value + "%";
                break;
            case "Size":
                SizeShowText.text = (int)sizeSlider.value + "%";
                break;
            case "Opacity":
                OpacityShowText.text = (int)opacitySlider.value + "%";
                break;
            default:
                break;
        }
    }

    // When Size Slider Value Change
    public void SettingSizeSlider()
    {
        // min/max Ratio Set
        sliderRatio = (sizeSlider.value + 50) / sizeSlider.maxValue;

        // Ratio Update
        baseSize = new Vector2(PlayerPrefs.GetFloat($"{setBtn.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX),
            PlayerPrefs.GetFloat($"{setBtn.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY));
        setBtnRect.sizeDelta = baseSize * sliderRatio;

        baseSize = new Vector2(PlayerPrefs.GetFloat($"{setBtn.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX),
            PlayerPrefs.GetFloat($"{setBtn.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY));
        setBtnIconRect.sizeDelta = baseSize * sliderRatio;

        baseSize = new Vector2(PlayerPrefs.GetFloat($"{setBtn.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX),
            PlayerPrefs.GetFloat($"{setBtn.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY));
        setBtnAreaRect.sizeDelta = baseSize * sliderRatio;

        // % Text Set & Value Save
        SetSliderText("Size");
        PlayerPrefs.SetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.Size, sizeSlider.value);

        /*baseSize = new Vector2(PlayerPrefs.GetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX),
            PlayerPrefs.GetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY));

        sliderRatio = (sizeSlider.value + 50) / sizeSlider.maxValue;
        setBtnRect.sizeDelta = baseSize * sliderRatio;
        setBtnIconRect.sizeDelta = baseSize * sliderRatio;
        setBtnAreaRect.sizeDelta = baseSize * sliderRatio;

        SetSliderText("Size");

        PlayerPrefs.SetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.Size, sizeSlider.value);*/
    }

    // When Opacity Slider Value Change
    public void SettingOpacitySlider()
    {
        tempColor = setBtnImage.color;
        tempColor.a = 0.5f + (opacitySlider.value / (opacitySlider.maxValue * 2));
        setBtnImage.color = tempColor;

        tempColor = setBtnAreaImage.color;
        tempColor.a = 0.5f;
        setBtnAreaImage.color = tempColor;

        tempColor = setBtnIconImage.color;
        tempColor.a = 0.5f + (opacitySlider.value / (opacitySlider.maxValue * 2));
        setBtnIconImage.color = tempColor;

        // % Text Set & Value Save
        SetSliderText("Opacity");
        PlayerPrefs.SetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.Opacity, opacitySlider.value);
    }

    // Current Setting size, Opacity Save
    public void SaveSliderValue()
    {
        if (setBtn == null)
            return;

        // size
        PlayerPrefs.SetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.ResetSize, PlayerPrefs.GetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.Size));
        PlayerPrefs.SetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.Size, sizeSlider.value);

        // resetOpacity
        PlayerPrefs.SetFloat($"{setBtn.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity,
            PlayerPrefs.GetFloat($"{setBtn.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.Opacity));
        PlayerPrefs.SetFloat($"{setBtn.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity,
            PlayerPrefs.GetFloat($"{setBtn.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.Opacity));
        PlayerPrefs.SetFloat($"{setBtn.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity,
            PlayerPrefs.GetFloat($"{setBtn.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.Opacity));
        
        // Opacity
        PlayerPrefs.SetFloat($"{setBtn.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.Opacity, opacitySlider.value);
        PlayerPrefs.SetFloat($"{setBtn.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.Opacity, opacitySlider.value);
        PlayerPrefs.SetFloat($"{setBtn.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.Opacity, opacitySlider.value);

        // transform
        PlayerPrefs.SetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.TransX, setBtnRect.anchoredPosition.x);
        PlayerPrefs.SetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.TransY, setBtnRect.anchoredPosition.y);

        PlayerPrefs.Save();
        Close();
    }

    // setBtn TransForm move
    public void moveUI(string direction)
    {
        switch (direction)
        {
            case "Right":
                TempRect = new Vector2(setBtnRect.anchoredPosition.x+1, setBtnRect.anchoredPosition.y);
                break;
            case "Left":
                TempRect = new Vector2(setBtnRect.anchoredPosition.x - 1, setBtnRect.anchoredPosition.y);
                break;
            case "Down":
                TempRect = new Vector2(setBtnRect.anchoredPosition.x, setBtnRect.anchoredPosition.y - 1);
                break;
            case "Up":
                TempRect = new Vector2(setBtnRect.anchoredPosition.x, setBtnRect.anchoredPosition.y + 1);
                break;
            default:
                Debug.Log("범위 벗어남");
                return;
        }

        tempX = Mathf.Clamp(TempRect.x, -pHalfWidth + halfWidth, pHalfWidth - halfWidth);
        tempY = Mathf.Clamp(TempRect.y, -pHalfHeight + halfHeight, pHalfHeight - halfHeight);
        TempRect = new Vector2(tempX, tempY);
        setBtnRect.anchoredPosition = TempRect;
    }
}
