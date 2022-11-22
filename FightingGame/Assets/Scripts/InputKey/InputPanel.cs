using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public class InputPanel : MonoBehaviour
{
    InputKey[] inputKeys = new InputKey[(int)ENUM_INPUTKEY_NAME.Max];

    public void Init(Action<ENUM_INPUTKEY_NAME> OnPointDownCallBack, Action<ENUM_INPUTKEY_NAME> OnPointUpCallBack)
    {
        List<KeySettingData> keySettingDatas = PlayerPrefsManagement.Load_KeySettingData();

        for (int index = 0; index < inputKeys.Length; index++)
        {
            inputKeys[index] = gameObject.transform.Find(Enum.GetName(typeof(ENUM_INPUTKEY_NAME), index)).GetComponent<InputKey>();
            
            if(inputKeys[index] == null)
            {
                Debug.LogError($"{Enum.GetName(typeof(ENUM_INPUTKEY_NAME), index)} 를 찾지 못했습니다.");
                return;
            }

            if(keySettingDatas != null)
                Set_InputKey(inputKeys[index], keySettingDatas[index]);

            inputKeys[index].Init(OnPointDownCallBack, OnPointUpCallBack);
        }
    }

    private void Set_InputKey(InputKey inputKey, KeySettingData keySettingData)
    {
        inputKey.rectTr.localScale = new Vector3(keySettingData.size, keySettingData.size, 1f);

        inputKey.slotImage.color = new Color(1, 1, 1, 0.5f + (keySettingData.opacity / 200));
        if (inputKey.iconImage != null)
            inputKey.iconImage.color = new Color(1, 1, 1, 0.5f + (keySettingData.opacity / 200));

        inputKey.rectTr.position = new Vector2(keySettingData.rectTrX, keySettingData.rectTrY);
    }

    public InputKey[] Get_InputKeys()
    {
        return inputKeys;
    }

    public InputKey Get_InputKey(ENUM_INPUTKEY_NAME inputKeyName)
    {
        return inputKeys[(int)inputKeyName];
    }
}
