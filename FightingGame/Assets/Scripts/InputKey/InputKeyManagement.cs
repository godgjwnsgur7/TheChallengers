using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputKeyManagement : MonoBehaviour
{
    private PlayerPrefsManagement playerPrefsManagement = null;
    private KeySettingData[] keySettingData = null;
    private InputPanel inputPanel = null;
    private InputKey inputKey = null;
    private RectTransform inputKeyRectTr = null;

    public void Init()
    {
        playerPrefsManagement = new PlayerPrefsManagement();
        keySettingData = new KeySettingData[(int)ENUM_KEYSETTING_NAME.Max];

        // InputPanel Instantiate
        inputPanel = Managers.Resource.Instantiate("UI/InputPanel", this.transform).GetComponent<InputPanel>();
        inputPanel.Init(Select_InputKey, Select_InputKey);

        // InputKey 세팅
        for(int i = 0; i < keySettingData.Length; i++)
        {
            inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)i);
            inputKeyRectTr = inputKey.GetComponent<RectTransform>();

            keySettingData[i] = playerPrefsManagement.Get_KeySettingData((ENUM_KEYSETTING_NAME)i);
            if (keySettingData[i] == null)
            {
                keySettingData[i] = new KeySettingData(50, 100, inputKeyRectTr.position.x, inputKeyRectTr.position.y);
                playerPrefsManagement.Set_KeySettingData(keySettingData[i], (ENUM_KEYSETTING_NAME)i);
                Debug.Log((ENUM_KEYSETTING_NAME)i + "초기화");
            }

            Set_InputKey(i, inputKey);
        }
    }

    public void Set_InputKey(int inputkeyNum, InputKey inputKey)
    {
        Set_InputKeySize(keySettingData[inputkeyNum].size, inputKey);
        Set_InputKeyOpacity(keySettingData[inputkeyNum].opacity, inputKey);
        Set_InputKeyTransForm(keySettingData[inputkeyNum].rectTrX, keySettingData[inputkeyNum].rectTrY, inputKey);
    }

    public void Set_InputKeySize(float size, InputKey inputKey)
    {
        float sizeRatio = (50 + size) / 100;

        inputKeyRectTr = inputKey.GetComponent<RectTransform>();
        inputKeyRectTr.localScale = new Vector3(1, 1, 1) * sizeRatio;
    }

    public void Set_InputKeyOpacity(float opacity, InputKey inputKey)
    {
        float opacityRatio = 0.5f + (opacity / 200);
        Color changeColor;

        for(int i = 0; i < inputKey.inputKeyImages.Length; i++)
        {
            changeColor = inputKey.inputKeyImages[i].color;
            changeColor.a = opacityRatio;
            inputKey.inputKeyImages[i].color = changeColor;
        }
    }

        public void Set_InputKeyTransForm(float rectTrX, float rectTrY, InputKey inputKey)
    {
        Vector2 changeVector = new Vector2(rectTrX, rectTrY);

        inputKeyRectTr = inputKey.GetComponent<RectTransform>();
        inputKeyRectTr.position = changeVector;
    }

    // InputPanel Init 테스트 용 임시
    public void Select_InputKey(InputKey inputKey)
    {
        Debug.Log("눌렀다.");
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
}
