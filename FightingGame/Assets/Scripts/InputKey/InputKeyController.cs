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
            Set_keySettingDataList();
        }
        else
        {
            inputPanel.Set_PoniterEvent(null, null);
            panelTr = inputPanel.GetComponent<RectTransform>();
        }

        Set_PanelActive(true);
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
                Set_InputKey(i);
            }
        }
        else
            for (int i = 0; i < keySettingDataList.Count; i++)
                Set_InputKey(i);
    }

    public void Set_InputKey(int _inputkeyNum)
    {
        Set_InputKeySize(keySettingDataList[_inputkeyNum].size, _inputkeyNum);
        Set_InputKeyOpacity(keySettingDataList[_inputkeyNum].opacity, _inputkeyNum);
        Set_InputKeyTransForm(keySettingDataList[_inputkeyNum].rectTrX, keySettingDataList[_inputkeyNum].rectTrY, _inputkeyNum);
    }

    // size 조절
    public void Set_InputKeySize(float _size, int _inputkeyNum)
    {
        inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)_inputkeyNum);
        float sizeRatio = (50 + _size) / 100;
        Vector3 changeScale = new Vector3(1, 1, 1) * sizeRatio;

        // InputKey 크기 변경
        inputKeyRectTr = inputKey.GetComponent<RectTransform>();
        inputKeyRectTr.localScale = changeScale;
    }

    // opacity 조절
    public void Set_InputKeyOpacity(float _opacity, int _inputkeyNum)
    {
        Image inputKeyImage;
        float opacityRatio = 0.5f + (_opacity / 200);
        inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)_inputkeyNum);

        Transform imageObjectTr = inputKey.transform.Find("SlotImage");
        if (imageObjectTr != null)
        {
            inputKeyImage = imageObjectTr.GetComponent<Image>();
            Set_ChangeColor(inputKeyImage, opacityRatio);
        }

        imageObjectTr = inputKey.transform.Find("IconArea");
        if (imageObjectTr != null)
        {
            inputKeyImage = imageObjectTr.GetChild(0).GetComponent<Image>();
            Set_ChangeColor(inputKeyImage, opacityRatio);
        }
    }

    public void Set_ChangeColor(Image _inputKeyImage, float _opacityRatio)
    {
        Color changeColor = _inputKeyImage.color;
        changeColor.a = _opacityRatio;
        _inputKeyImage.color = changeColor;
    }

    // 위치 조절
    public void Set_InputKeyTransForm(float _rectTrX, float _rectTrY, int _inputkeyNum)
    {
        inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)_inputkeyNum);
        inputKeyRectTr = inputKey.GetComponent<RectTransform>();

        Vector2 movePos = CheckTransformRange(new Vector2(_rectTrX, _rectTrY));
        inputKeyRectTr.anchoredPosition = movePos;
    }

    // UI 이동 범위 체크
    public Vector2 CheckTransformRange(Vector2 changeVector)
    {
        Vector2 panelHalfSize = panelTr.sizeDelta / 2;

        // InputKey 가로,세로 길이의 반
        float scaleSizeX = (inputKeyRectTr.sizeDelta.x / 2) * inputKeyRectTr.localScale.x;
        float scaleSizeY = (inputKeyRectTr.sizeDelta.y / 2) * inputKeyRectTr.localScale.y;

        // 최대 이동범위 가로축 : InputPanel범위 안, 세로축 : InputPanel의 중심의 아래쪽
        float vecRangeX = Mathf.Clamp(changeVector.x, -panelHalfSize.x + scaleSizeX, panelHalfSize.x - scaleSizeX);
        float vecRangeY = Mathf.Clamp(changeVector.y, -panelHalfSize.y + scaleSizeY, panelTr.anchoredPosition.y - scaleSizeY);

        return new Vector2(vecRangeX, vecRangeY);
    }

    public void Set_PanelActive(bool _binary)
    {
        this.isActive = _binary;
        this.inputPanel.gameObject.SetActive(_binary);
    }
}