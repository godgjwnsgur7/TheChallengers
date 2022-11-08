using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class InputKeyController : MonoBehaviour
{
    List<KeySettingData> keySettingDataList = null;

    public InputPanel inputPanel = null;
    private RectTransform panelTr = null;
    private InputKey inputKey = null;
    private RectTransform inputKeyRectTr = null;

    public bool isActive = false;

    public void Init()
    {
        if (inputPanel == null)
        {
            inputPanel = Managers.Resource.Instantiate("UI/InputPanel", this.transform).GetComponent<InputPanel>();
            inputPanel.Init(null, null);
            panelTr = inputPanel.GetComponent<RectTransform>();
        }

        Set_keySettingDataList();
        Set_PanelActive(true);
    }

    public void PointerDown(InputKey _inputkey)
    {

    }

    // PlayerPrefs 값 호출
    public void Set_keySettingDataList()
    {
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

                keySettingDataList.Insert(i, new KeySettingData(i, 50, 100, inputKeyRectTr.anchoredPosition.x, inputKeyRectTr.anchoredPosition.y));
            }
        }
        else
        {
            for (int i = 0; i < keySettingDataList.Count; i++)
            {
                inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)i);
                inputKeyRectTr = inputKey.GetComponent<RectTransform>();
                float ratio;

                // Set Size
                ratio = (50 + keySettingDataList[i].size) / 100;
                Vector3 changeVec = new Vector3(1, 1, 1) * ratio;
                inputKeyRectTr.localScale = changeVec;

                // Set Opacity
                Image inputKeyImage;
                ratio = 0.5f + (keySettingDataList[i].opacity / 200);
                Transform imageObjectTr = inputKey.transform.Find("SlotImage");
                if (imageObjectTr != null)
                {
                    inputKeyImage = imageObjectTr.GetComponent<Image>();
                    Set_ChangeColor(inputKeyImage, ratio);
                }

                imageObjectTr = inputKey.transform.Find("IconArea");
                if (imageObjectTr != null)
                {
                    inputKeyImage = imageObjectTr.GetChild(0).GetComponent<Image>();
                    Set_ChangeColor(inputKeyImage, ratio);
                }

                // Set Transform 
                changeVec = new Vector2(keySettingDataList[i].rectTrX, keySettingDataList[i].rectTrY);
                inputKeyRectTr.anchoredPosition = changeVec;
            }
        }
    }

    public void Set_ChangeColor(Image _inputKeyImage, float _opacityRatio)
    {
        Color changeColor = _inputKeyImage.color;
        changeColor.a = _opacityRatio;
        _inputKeyImage.color = changeColor;
    }

    public void Set_PanelActive(bool _binary)
    {
        this.isActive = _binary;
        this.inputPanel.gameObject.SetActive(_binary);
    }
}