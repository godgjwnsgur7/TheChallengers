using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class InputKeyManagement : MonoBehaviour
{
    List<KeySettingData> keySettingDataList = null;
    private InputPanel inputPanel = null;
    private InputKey inputKey = null;
    private RectTransform inputKeyRectTr = null;

    public void Init()
    {
        // InputPanel Instantiate
        inputPanel = Managers.Resource.Instantiate("UI/InputPanel", this.transform).GetComponent<InputPanel>();
        inputPanel.Init(Select_InputKey, Select_InputKey);

        if (PlayerPrefsManagement.Load_KeySettingData() == null)
        {
            keySettingDataList = new List<KeySettingData>();

            for (int i = 0; i < (int)ENUM_INPUTKEY_NAME.Max; i++)
            {
                inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)i);
                inputKeyRectTr = inputKey.GetComponent<RectTransform>();
                keySettingDataList.Insert(i, new KeySettingData(i, 50, 100, inputKeyRectTr.position.x, inputKeyRectTr.position.y));
                Debug.Log((ENUM_INPUTKEY_NAME)i + "초기화");
                Set_InputKey(i, inputKey);
            }
            PlayerPrefsManagement.Save_KeySettingData(keySettingDataList);
        }
        else
            keySettingDataList = PlayerPrefsManagement.Load_KeySettingData();
    }

    public void Set_InputKey(int inputkeyNum, InputKey inputKey)
    {
        Set_InputKeySize(keySettingDataList[inputkeyNum].size, inputKey);
        Set_InputKeyOpacity(keySettingDataList[inputkeyNum].opacity, inputKey);
        Set_InputKeyTransForm(keySettingDataList[inputkeyNum].rectTrX, keySettingDataList[inputkeyNum].rectTrY, inputKey);
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
        Image inputKeyImage;
        
        if (inputKey.transform.Find("SlotImage") != null)
        {
            inputKeyImage = inputKey.transform.Find("SlotImage").GetComponent<Image>();
            changeColor = inputKeyImage.color;
            changeColor.a = opacityRatio;
            inputKeyImage.color = changeColor;
        }
        
        if (inputKey.transform.Find("IconArea") != null)
        {
            inputKeyImage = inputKey.transform.Find("IconArea").GetChild(0).GetComponent<Image>();
            changeColor = inputKeyImage.color;
            changeColor.a = opacityRatio;
            inputKeyImage.color = changeColor;
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

    public void Save_KeySettingData()
    {
        PlayerPrefsManagement.Save_KeySettingData(keySettingDataList);
    }
}
