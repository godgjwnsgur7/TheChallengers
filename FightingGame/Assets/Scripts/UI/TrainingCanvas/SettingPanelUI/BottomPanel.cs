using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class BottomPanel : UIElement
{
    public Slider sizeSlider;
    public Slider opacitySlider;
    public UISettingHelper settingHelper;

    [SerializeField] Text SizeShowText;
    [SerializeField] Text OpacityShowText;
    [SerializeField] KeyPanelArea keyPanelArea;

    private UpdatableUI setBtn;

    // UI 이동버튼용 변수
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
            setBtn.isSelect = false;
            setBtn.ChangeAreaColor();
            setBtn = null;
        }

        base.Close();
    }

    // Call ClickedButton Slider Setting Value
    public void setSlider(UpdatableUI updateUI)
    {
        // 이전 선택했던 UI 드래그 중지
        if (setBtn != null && updateUI != setBtn)
        {
            keyPanelArea.Reset(setBtn);
            setBtn.isSelect = false;
            setBtn.ChangeAreaColor();
        }

        // 선택한 UI 세팅
        setBtn = updateUI;
        settingHelper.SetBtnInit(setBtn);

        // UI 실린더 값 호출
        sizeSlider.value = Managers.Prefs.GetPrefsFloat(ENUM_PLAYERPREFS_TYPE.Size, setBtn.name);
        opacitySlider.value = Managers.Prefs.GetPrefsFloat(ENUM_PLAYERPREFS_TYPE.Opacity, setBtn.name);
        SetSliderText("All");

        // UI 드래그 기능
        setBtn.isSelect = true;
        setBtn.ChangeAreaColor();
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

        settingHelper.SetSize(sizeRatio);

        // % Text Set & Value Save
        SetSliderText("Size");
    }

    // When Opacity Slider Value Change
    public void SettingOpacitySlider()
    {
        opacityRatio = (opacitySlider.value / (opacitySlider.maxValue * 2));

        settingHelper.SetOpacity(opacityRatio);

        // % Text Set & Value Save
        SetSliderText("Opacity");
    }

    // Current Setting size, Opacity Save
    public void SaveSliderValue()
    {
        if (setBtn == null)
            return;

        if (!isUpdatable)
            return;

        // size
        Managers.Prefs.SetPlayerPrefs<float>(sizeSlider.value, ENUM_PLAYERPREFS_TYPE.Size, setBtn.name);
        Managers.Prefs.SetPlayerPrefs<float>(sizeSlider.value, ENUM_PLAYERPREFS_TYPE.ResetSize, setBtn.name);

        // Opacity
        Managers.Prefs.SetPlayerPrefs<float>(opacitySlider.value, ENUM_PLAYERPREFS_TYPE.Opacity, setBtn.name);
        Managers.Prefs.SetPlayerPrefs<float>(opacitySlider.value, ENUM_PLAYERPREFS_TYPE.ResetOpacity, setBtn.name);

        // transform
        Managers.Prefs.SetPlayerPrefs<float>(settingHelper.GetTransform().x, ENUM_PLAYERPREFS_TYPE.TransX, setBtn.name);
        Managers.Prefs.SetPlayerPrefs<float>(settingHelper.GetTransform().y, ENUM_PLAYERPREFS_TYPE.TransY, setBtn.name);

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
                tempRect = new Vector2(settingHelper.GetTransform().x + moveSpeed, settingHelper.GetTransform().y);
                break;
            case "Left":
                tempRect = new Vector2(settingHelper.GetTransform().x - moveSpeed, settingHelper.GetTransform().y);
                break;
            case "Down":
                tempRect = new Vector2(settingHelper.GetTransform().x, settingHelper.GetTransform().y - moveSpeed);
                break;
            case "Up":
                tempRect = new Vector2(settingHelper.GetTransform().x, settingHelper.GetTransform().y + moveSpeed);
                break;
            default:
                Debug.Log("범위 벗어남");
                return;
        }

        settingHelper.SetTransform(tempRect);
    }
}
