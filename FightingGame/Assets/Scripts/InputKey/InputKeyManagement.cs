using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class InputKeyManagement : MonoBehaviour
{
    List<KeySettingData> keySettingDataList = null;

    private InputPanel inputPanel = null;
    private InputKey inputKey = null;
    private RectTransform inputKeyRectTr = null;

    private AreaPanel areaPanel = null;
    private KeyArea keyArea = null;
    private RectTransform areaRectTr = null;

    private SettingPanel settingPanel;

    public void Init()
    {
        settingPanel = this.transform.root.Find("SettingPanel").GetComponent<SettingPanel>();

        // AreaPanel Instantiate
        areaPanel = Managers.Resource.Instantiate("UI/AreaPanel", this.transform).GetComponent<AreaPanel>();
        areaPanel.Init();

        // InputPanel Instantiate
        inputPanel = Managers.Resource.Instantiate("UI/InputPanel", this.transform).GetComponent<InputPanel>();
        inputPanel.Init(Select_InputKey, Select_InputKey);

        // 설정된 PlayerPrefs 호출
        keySettingDataList = PlayerPrefsManagement.Load_KeySettingData();

        // 설정된 PlayerPrefs가 없으면 초기화
        if (keySettingDataList == null)
        {
            keySettingDataList = new List<KeySettingData>();
            for (int i = 0; i < (int)ENUM_INPUTKEY_NAME.Max; i++)
            {
                inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)i);
                inputKeyRectTr = inputKey.GetComponent<RectTransform>();

                keySettingDataList.Insert(i, new KeySettingData(i, 50, 100, inputKeyRectTr.position.x, inputKeyRectTr.position.y));
                Set_InputKey(i);
                Debug.Log((ENUM_INPUTKEY_NAME)i + "초기화");
            }
        }
    }

    public void Set_InputKey(int _inputkeyNum)
    {
        Set_InputKeySize(keySettingDataList[_inputkeyNum].size, _inputkeyNum);
        Set_InputKeyOpacity(keySettingDataList[_inputkeyNum].opacity, _inputkeyNum);
        Set_InputKeyTransForm(keySettingDataList[_inputkeyNum].rectTrX, keySettingDataList[_inputkeyNum].rectTrY, _inputkeyNum);
    }

    public void Set_InputKeySize(float size, int _inputkeyNum)
    {
        inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)_inputkeyNum);
        keyArea = areaPanel.Get_keyArea((ENUM_INPUTKEY_NAME)_inputkeyNum);

        float sizeRatio = (50 + size) / 100;
        Vector3 changeScale = new Vector3(1, 1, 1) * sizeRatio;

        areaRectTr = keyArea.GetComponent<RectTransform>();
        areaRectTr.localScale = changeScale;

        inputKeyRectTr = inputKey.GetComponent<RectTransform>();
        inputKeyRectTr.localScale = changeScale;

        keySettingDataList[_inputkeyNum].size = size;
    }

    public void Set_InputKeyOpacity(float opacity, int _inputkeyNum)
    {
        float opacityRatio = 0.5f + (opacity / 200);
        Color changeColor;
        Image inputKeyImage;

        inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)_inputkeyNum);
        Transform imageObjectTr = inputKey.transform.Find("SlotImage");
        if (imageObjectTr != null)
        {
            inputKeyImage = imageObjectTr.GetComponent<Image>();
            changeColor = inputKeyImage.color;
            changeColor.a = opacityRatio;
            inputKeyImage.color = changeColor;
        }

        imageObjectTr = inputKey.transform.Find("IconArea");
        if (imageObjectTr != null)
        {
            inputKeyImage = imageObjectTr.GetChild(0).GetComponent<Image>();
            changeColor = inputKeyImage.color;
            changeColor.a = opacityRatio;
            inputKeyImage.color = changeColor;
        }

        keySettingDataList[_inputkeyNum].opacity = opacity;
    }

    public void Set_InputKeyTransForm(float rectTrX, float rectTrY, int _inputkeyNum)
    {
        inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)_inputkeyNum);
        keyArea = areaPanel.Get_keyArea((ENUM_INPUTKEY_NAME)_inputkeyNum);

        Vector2 changeVector = new Vector2(rectTrX, rectTrY);
        areaRectTr = keyArea.GetComponent<RectTransform>();

        inputKeyRectTr = inputKey.GetComponent<RectTransform>();
        inputKeyRectTr.position = changeVector;
        areaRectTr.position = changeVector;

        keySettingDataList[_inputkeyNum].rectTrX = rectTrX;
        keySettingDataList[_inputkeyNum].rectTrY = rectTrY;
    }

    // InputPanel Init 테스트 용 임시
    public void Select_InputKey(InputKey _inputKey)
    {
        keyArea.isSelect = false;
        keyArea.Set_AreaColor();

        for (int i = 0; i < (int)ENUM_INPUTKEY_NAME.Max; i++)
        {
            if (((ENUM_INPUTKEY_NAME)i).ToString() == _inputKey.name)
            {
                keyArea = areaPanel.Get_keyArea((ENUM_INPUTKEY_NAME)i);
                settingPanel.OnClick_SetInputKey(i);
                keyArea.isSelect = true;
                keyArea.Set_AreaColor();
            }
        }
        Debug.Log($"{_inputKey.name}세팅");
    }

    public void Save_KeySettingData()
    {
        PlayerPrefsManagement.Save_KeySettingData(keySettingDataList);
    }
}
