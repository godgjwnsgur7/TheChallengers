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

    float[] prefsList;

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
            switch (setBtn.name)
            {
                case "LeftMoveBtn":
                    keyPanelArea.Reset(0);
                    break;
                case "RightMoveBtn":
                    keyPanelArea.Reset((ENUM_BTNPREFS_TYPE)1);
                    break;
                case "AttackBtn":
                    keyPanelArea.Reset((ENUM_BTNPREFS_TYPE)2);
                    break;
                case "JumpBtn":
                    keyPanelArea.Reset((ENUM_BTNPREFS_TYPE)3);
                    break;
                case "SkillBtn1":
                    keyPanelArea.Reset((ENUM_BTNPREFS_TYPE)4);
                    break;
                case "SkillBtn2":
                    keyPanelArea.Reset((ENUM_BTNPREFS_TYPE)5);
                    break;
                case "SkillBtn3":
                    keyPanelArea.Reset((ENUM_BTNPREFS_TYPE)6);
                    break;
            }
            setBtn.isSelect = false;
            setBtn.ChangeAreaColor();
            keyPanelArea.RemoveUpdateComponent(setBtn);
        }

        if (updateUI != setBtn)
        {
            // 선택한 UI 세팅
            setBtn = updateUI;
            settingHelper.SetBtnInit(setBtn.GetComponent<Button>());
            switch (setBtn.name)
            {
                case "LeftMoveBtn":
                    prefsList = Managers.Prefs.GetButtonPrefs(0);
                    break;
                case "RightMoveBtn":
                    prefsList = Managers.Prefs.GetButtonPrefs((ENUM_BTNPREFS_TYPE)1);
                    break;
                case "AttackBtn":
                    prefsList = Managers.Prefs.GetButtonPrefs((ENUM_BTNPREFS_TYPE)2);
                    break;
                case "JumpBtn":
                    prefsList = Managers.Prefs.GetButtonPrefs((ENUM_BTNPREFS_TYPE)3);
                    break;
                case "SkillBtn1":
                    prefsList = Managers.Prefs.GetButtonPrefs((ENUM_BTNPREFS_TYPE)4);
                    break;
                case "SkillBtn2":
                    prefsList = Managers.Prefs.GetButtonPrefs((ENUM_BTNPREFS_TYPE)5);
                    break;
                case "SkillBtn3":
                    prefsList = Managers.Prefs.GetButtonPrefs((ENUM_BTNPREFS_TYPE)6);
                    break;
            }

            // UI 실린더 값 호출
            sizeSlider.value = prefsList[1];
            opacitySlider.value = prefsList[2];
            SetSliderText("All");

            // UI 드래그 기능
            setBtn.isSelect = true;
            setBtn.ChangeAreaColor();
        }
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
        Managers.Prefs.SetButtonPrefs((ENUM_BTNPREFS_TYPE)prefsList[0], ENUM_BTNSUBPREFS_TYPE.Size, sizeSlider.value);

        // % Text Set & Value Save
        SetSliderText("Size");
    }

    // When Opacity Slider Value Change
    public void SettingOpacitySlider()
    {
        opacityRatio = (opacitySlider.value / (opacitySlider.maxValue * 2));

        settingHelper.SetOpacity(opacityRatio);
        Managers.Prefs.SetButtonPrefs((ENUM_BTNPREFS_TYPE)prefsList[0], ENUM_BTNSUBPREFS_TYPE.Opacity, opacitySlider.value);

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


        Managers.Prefs.SetButtonPrefs((ENUM_BTNPREFS_TYPE)prefsList[0], ENUM_BTNSUBPREFS_TYPE.ResetSize, prefsList[1]);
        Managers.Prefs.SetButtonPrefs((ENUM_BTNPREFS_TYPE)prefsList[0], ENUM_BTNSUBPREFS_TYPE.ResetOpacity, prefsList[2]);
        Managers.Prefs.SaveButtonPrefs();
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
        moveSpeed = 0.0f;
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
                moveSpeed += 0.1f * (Time.deltaTime * 5);
            Debug.Log(moveSpeed);
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
