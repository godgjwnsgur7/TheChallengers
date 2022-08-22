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

    private GameObject setBtn;
    private GameObject parent;
    private RectTransform setBtnRect;
    private RectTransform parentRect;
    private Image setBtnImage;

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
    public void setSlider(GameObject go)
    {
        // 이전 선택했던 UI 드래그 중지
        if (setBtn != null)
            keyPanelArea.OnOffDrag(setBtn);

        // 선택한 UI 세팅
        setBtn = go;
        parent = setBtn.transform.parent.gameObject;
        setBtnImage = setBtn.GetComponent<Image>();
        setBtnRect = setBtn.GetComponent<RectTransform>();
        parentRect = parent.GetComponent<RectTransform>();
        beforeSize = setBtnRect.sizeDelta;

        // 부모, 자신의 절반길이
        halfWidth = setBtnRect.sizeDelta.x / 2;
        halfHeight = setBtnRect.sizeDelta.y / 2;
        pHalfWidth = parentRect.sizeDelta.x / 2;
        pHalfHeight = parentRect.sizeDelta.y / 2;

        // UI 실린더 값 호출
        sizeSlider.value = PlayerPrefs.GetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.Size);
        opacitySlider.value = PlayerPrefs.GetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.Opacity);
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
        baseSize = new Vector2(PlayerPrefs.GetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX),
            PlayerPrefs.GetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY));

        sliderRatio = (sizeSlider.value + 50) / sizeSlider.maxValue;
        setBtnRect.sizeDelta = baseSize * sliderRatio;
        SetSliderText("Size");

        PlayerPrefs.SetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.Size, sizeSlider.value);
    }

    // When Opacity Slider Value Change
    public void SettingOpacitySlider()
    {
        tempColor = setBtnImage.color;
        tempColor.a = 0.5f + (opacitySlider.value / (opacitySlider.maxValue * 2));
        setBtnImage.color = tempColor;
        SetSliderText("Opacity");

        PlayerPrefs.SetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.Opacity, opacitySlider.value);
    }

    // Current Setting size, Opacity Save
    public void SaveSliderValue()
    {
        if (setBtn == null)
            return;

        PlayerPrefs.SetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.ResetSize, PlayerPrefs.GetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.Size));
        PlayerPrefs.SetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity, PlayerPrefs.GetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.Opacity));

        PlayerPrefs.SetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.Size, sizeSlider.value);
        PlayerPrefs.SetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.Opacity, opacitySlider.value);
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
