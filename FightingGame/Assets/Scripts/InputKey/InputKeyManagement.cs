using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputKeyManagement : MonoBehaviour
{
    PlayerPrefsManagement playerPrefsManagement = null;
    KeySettingData[] keySettingData = null;
    InputPanel inputPanel = null;
    InputKey inputKey = null;
    RectTransform inputRectTr = null;
    Image inputImage = null;

    public void Init()
    {
        playerPrefsManagement = new PlayerPrefsManagement();
        keySettingData = new KeySettingData[(int)ENUM_KEYSETTING_NAME.Max];

        inputPanel = Managers.Resource.Instantiate("UI/InputPanel", this.transform).GetComponent<InputPanel>();
        // inputPanel.Init();

        for(int i = 0; i < keySettingData.Length; i++)
        {
            inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)i);
            inputRectTr = inputKey.GetComponent<RectTransform>();
            keySettingData[i] = playerPrefsManagement.Get_KeySettingData((ENUM_KEYSETTING_NAME)i);

            if(keySettingData[i] == null)
            {
                keySettingData[i] = new KeySettingData(50, 100, inputRectTr.position.x, inputRectTr.position.y);
                playerPrefsManagement.Set_KeySettingData(keySettingData[i], (ENUM_KEYSETTING_NAME)i);
            }

            Set_InputKey((ENUM_INPUTKEY_NAME)i, inputKey);
        }
    }

    public void Set_KeySettingData(KeySettingData keySettingData, ENUM_KEYSETTING_NAME keyName)
    {
        this.keySettingData[(int)keyName] = keySettingData;
    }

    public void Save_KeySettingData()
    {
        for(int i = 0; i < keySettingData.Length; i++)
            playerPrefsManagement.Set_KeySettingData(keySettingData[i], (ENUM_KEYSETTING_NAME)i);

        playerPrefsManagement.Save_KeySettingData();
    }

    public void Set_InputKey(ENUM_INPUTKEY_NAME operatingName, InputKey inputKey)
    {
        inputRectTr = inputKey.GetComponent<RectTransform>();
        inputImage = inputKey.GetComponent<Image>();

        inputRectTr.localScale *= (50 + keySettingData[(int)operatingName].size);

        Color changeColor = inputImage.color;
        changeColor.a = 0.5f + (keySettingData[(int)operatingName].opacity / 200);
        inputImage.color = changeColor;

        Vector2 changeVector = new Vector2(keySettingData[(int)operatingName].rectTrX, keySettingData[(int)operatingName].rectTrY);
        inputRectTr.position = changeVector;
    }
}
