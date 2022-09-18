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

    float[] currPrefsList;
    float[][] prefsLists = new float[(int)ENUM_BTNPREFS_TYPE.Max][];
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

        currPrefsList = null;
        for (int i = 0; i < prefsLists.Length; i++)
            prefsLists[i] = null;

        base.Close();
    }

    // Call ClickedButton Slider Setting Value
    public void setSlider(UpdatableUI updateUI)
    {
        // 이전 선택했던 UI 드래그 중지
        if (setBtn != null && updateUI != setBtn)
        {
            if(currPrefsList != null)
            {
                // 현재 위치 값 대입
                currPrefsList[5] = settingHelper.GetTransform().x;
                currPrefsList[6] = settingHelper.GetTransform().y;
                // 현재 수정 값 임시 저장
                prefsLists[(int)currPrefsList[0]] = new float[(int)ENUM_BTNSUBPREFS_TYPE.Max];
                prefsLists[(int)currPrefsList[0]] = currPrefsList; 
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
            switch (setBtn.name) // 임시
            {
                case "LeftMoveBtn":
                    currPrefsList = Managers.Prefs.GetPrefsList(0);
                    break;
                case "RightMoveBtn":
                    currPrefsList = Managers.Prefs.GetPrefsList((ENUM_BTNPREFS_TYPE)1);
                    break;
                case "AttackBtn":
                    currPrefsList = Managers.Prefs.GetPrefsList((ENUM_BTNPREFS_TYPE)2);
                    break;
                case "JumpBtn":
                    currPrefsList = Managers.Prefs.GetPrefsList((ENUM_BTNPREFS_TYPE)3);
                    break;
                case "SkillBtn1":
                    currPrefsList = Managers.Prefs.GetPrefsList((ENUM_BTNPREFS_TYPE)4);
                    break;
                case "SkillBtn2":
                    currPrefsList = Managers.Prefs.GetPrefsList((ENUM_BTNPREFS_TYPE)5);
                    break;
                case "SkillBtn3":
                    currPrefsList = Managers.Prefs.GetPrefsList((ENUM_BTNPREFS_TYPE)6);
                    break;
            }

            // UI 실린더 값 호출
            sizeSlider.value = currPrefsList[1];
            opacitySlider.value = currPrefsList[2];
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

    // 사이즈 Slider 변경 시 
    public void SettingSizeSlider()
    {
        // min/max Ratio Set
        sizeRatio = (sizeSlider.value + 50) / sizeSlider.maxValue;

        settingHelper.SetSize(sizeRatio);
        currPrefsList[1] = sizeSlider.value;
        //Managers.Prefs.SetButtonPrefs((ENUM_BTNPREFS_TYPE)prefsList[0], ENUM_BTNSUBPREFS_TYPE.Size, sizeSlider.value);

        // % Text Set & Value Save
        SetSliderText("Size");
    }

    // 투명도 Slider 변경 시
    public void SettingOpacitySlider()
    {
        opacityRatio = (opacitySlider.value / (opacitySlider.maxValue * 2));

        settingHelper.SetOpacity(opacityRatio);
        currPrefsList[2] = opacitySlider.value;
        //Managers.Prefs.SetButtonPrefs((ENUM_BTNPREFS_TYPE)prefsList[0], ENUM_BTNSUBPREFS_TYPE.Opacity, opacitySlider.value);

        // % Text Set & Value Save
        SetSliderText("Opacity");
    }

    // 현재 수정 사항 저장
    public void SaveSliderValue()
    {
        if (setBtn == null)
            return;

        if (!isUpdatable)
            return;

        // 마지막의 수정 UI 리셋 기준 값 변경 후 Prefs 저장
        currPrefsList[5] = settingHelper.GetTransform().x;
        currPrefsList[6] = settingHelper.GetTransform().y;
        prefsLists[(int)currPrefsList[0]] = new float[(int)ENUM_BTNSUBPREFS_TYPE.Max];
        prefsLists[(int)currPrefsList[0]] = currPrefsList;

        // 현재 변경되있는 값들이 다음의 리셋 값이 됨
        for(int i = 0; i < prefsLists.Length; i++)
        {
            if (prefsLists[i] == null)
                continue;

            prefsLists[i][3] = prefsLists[i][1];
            prefsLists[i][4] = prefsLists[i][2];
            prefsLists[i][7] = prefsLists[i][5];
            prefsLists[i][8] = prefsLists[i][6];

            // 변경 사항 발송
            Managers.Prefs.SetPrefsList((ENUM_BTNPREFS_TYPE)prefsLists[i][0], prefsLists[i]);
        }

        Managers.Prefs.SaveButtonPrefs();
        Close();
    }

    public void CheckIsUpdatable()
    {
        isUpdatable = setBtn.isUpdatable;
    }

    // setBtn TransForm move
    public void OnMoveBtnDown(string direction)
    {
        isPushMoveBtn = true;
        moveSpeed = 0.0f;
        StartCoroutine(MoveKeyPanelUI(direction));
    }

    public void OnMoveBtnUp(string direction)
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