using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public class InputPanel : MonoBehaviour
{
    InputKey[] inputKeys = new InputKey[(int)ENUM_INPUTKEY_NAME.Max];
    public RectTransform thisRectTr = null;

    public void Init(Action<InputKey> OnPointDownCallBack, Action<InputKey> OnPointUpCallBack)
    {
        if (thisRectTr == null)
            thisRectTr = GetComponent<RectTransform>();

        for(int index = 0; index < inputKeys.Length; index++)
        {
            inputKeys[index] = gameObject.transform.Find(Enum.GetName(typeof(ENUM_INPUTKEY_NAME), index)).GetComponent<InputKey>();
            
            if(inputKeys[index] == null)
            {
                Debug.LogError($"{Enum.GetName(typeof(ENUM_INPUTKEY_NAME), index)} 를 찾지 못했습니다.");
                return;
            }

            inputKeys[index].Init(OnPointDownCallBack, OnPointUpCallBack);
        }
    }
    
    public InputKey Get_InputKey(ENUM_INPUTKEY_NAME keyName)
    {
        return inputKeys[(int)keyName];
    }

    public void Set_InputKeyData(List<KeySettingData> _keySettingDataList)
    {
        if (_keySettingDataList == null)
            return;

        for (int i = 0; i < _keySettingDataList.Count; i++)
        {
            // size
            inputKeys[_keySettingDataList[i].key].inputKeyRectTr.localScale = new Vector3(1, 1, 1) * (50 + _keySettingDataList[i].size) / 100;
            
            // Opacity
            Color tempColor = inputKeys[_keySettingDataList[i].key].slotImage.color;
            tempColor.a = 0.5f + (_keySettingDataList[i].opacity / 200);

            if (inputKeys[_keySettingDataList[i].key].iconImage != null)
            {
                tempColor = inputKeys[_keySettingDataList[i].key].iconImage.color;
                tempColor.a = 0.5f + (_keySettingDataList[i].opacity / 200);
            }

            // Transform
            inputKeys[_keySettingDataList[i].key].inputKeyRectTr.position =
                new Vector2(_keySettingDataList[i].rectTrX, _keySettingDataList[i].rectTrY);
        }
    }

    public void Set_InputKeyIconImage(Sprite _iconImage, InputKey _inputKey) => _inputKey.iconImage.sprite = _iconImage;

    // 추가로 필요한 기능이 있는 것 같다면, 요청하면 됨! 모르는건 물어보3!
}
