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

    private float pHalfWidth;
    private float pHalfHeight;

    // UI 이동버튼용 변수
    private float tempX;
    private float tempY;
    private Vector2 tempRect;

    private float sizeRatio;
    private float opacityRatio;

    private bool isPushMoveBtn = false;
    public bool isUpdatable;
    private float moveSpeed;

    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    public override void Close()
    {
        if (setBtn != null)
        {
            setBtn.OnOffUIArea();
            setBtn = null;
        }

        base.Close();
    }

    // Call ClickedButton Slider Setting Value
    public void setSlider(UpdatableUI updateUI)
    {
        // 이전 선택했던 UI 드래그 중지
        if (setBtn != null)
            setBtn.OnOffUIArea();

        // 선택한 UI 세팅
        setBtn = updateUI;

        // 부모UI
        parent = setBtn.transform.parent.gameObject;
        parentRect = parent.GetComponent<RectTransform>();

        // 부모 절반길이
        pHalfWidth = parentRect.sizeDelta.x / 2;
        pHalfHeight = parentRect.sizeDelta.y / 2;

        // UI 실린더 값 호출
        sizeSlider.value = PlayerPrefs.GetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.Size);
        opacitySlider.value = PlayerPrefs.GetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.Opacity);
        SetSliderText("All");

        // UI 드래그 기능
        setBtn.OnOffUIArea();
    }


    // % 표기 Text 설정
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
        sizeRatio = (sizeSlider.value + 50) / sizeSlider.maxValue;

        setBtn.SetSize(sizeRatio);

        // % Text Set & Value Save
        SetSliderText("Size");
        PlayerPrefs.SetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.Size, sizeSlider.value);
    }

    // When Opacity Slider Value Change
    public void SettingOpacitySlider()
    {
        opacityRatio = (opacitySlider.value / (opacitySlider.maxValue * 2));

        setBtn.SetOpacity(opacityRatio);

        // % Text Set & Value Save
        SetSliderText("Opacity");
        PlayerPrefs.SetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.Opacity, opacitySlider.value);
    }

    // Current Setting size, Opacity Save
    public void SaveSliderValue()
    {
        if (setBtn == null)
            return;

        if (!isUpdatable)
            return;

        // size
        PlayerPrefs.SetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.ResetSize, PlayerPrefs.GetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.Size));
        PlayerPrefs.SetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.Size, sizeSlider.value);

        // resetOpacity
        PlayerPrefs.SetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity,
            PlayerPrefs.GetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.Opacity));

        // Opacity
        PlayerPrefs.SetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.Opacity, opacitySlider.value);

        // transform
        PlayerPrefs.SetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.TransX, setBtn.GetTransform().x);
        PlayerPrefs.SetFloat($"{setBtn.name}" + ENUM_PLAYERPREFS_TYPE.TransY, setBtn.GetTransform().y);

        PlayerPrefs.Save();
        Close();
    }

    public void CheckIsUpdatable()
    {
        isUpdatable = setBtn.isUpdatable;
    }

    // setBtn TransForm move
    public void MoveBtnDown(string direction)
    {
        isPushMoveBtn = true;
        moveSpeed = 0.5f;
        StartCoroutine(MoveKeyPanelUI(direction));
    }

    public void MoveBtnUp(string direction)
    {
        isPushMoveBtn = false;
    }

    IEnumerator MoveKeyPanelUI(string direction)
    {
        while (isPushMoveBtn)
        {
            MoveKeyPanelUISub(direction);

            if(moveSpeed <= 5.0f)
                moveSpeed += 0.5f * Time.deltaTime;

            yield return null;
        }
    }

    public void MoveKeyPanelUISub(string direction)
    {
        switch (direction)
        {
            case "Right":
                tempRect = new Vector2(setBtn.GetTransform().x + moveSpeed, setBtn.GetTransform().y);
                break;
            case "Left":
                tempRect = new Vector2(setBtn.GetTransform().x - moveSpeed, setBtn.GetTransform().y);
                break;
            case "Down":
                tempRect = new Vector2(setBtn.GetTransform().x, setBtn.GetTransform().y - moveSpeed);
                break;
            case "Up":
                tempRect = new Vector2(setBtn.GetTransform().x, setBtn.GetTransform().y + moveSpeed);
                break;
            default:
                Debug.Log("범위 벗어남");
                return;
        }

        tempX = Mathf.Clamp(tempRect.x, -pHalfWidth + setBtn.GetHalfWidth() * setBtn.GetSize().x, pHalfWidth - setBtn.GetHalfWidth() * setBtn.GetSize().x);
        tempY = Mathf.Clamp(tempRect.y, -pHalfHeight + setBtn.GetHalfHeight() * setBtn.GetSize().y, pHalfHeight - setBtn.GetHalfHeight() * setBtn.GetSize().y);
        tempRect = new Vector2(tempX, tempY);

        setBtn.SetTransform(tempRect);
    }
}
