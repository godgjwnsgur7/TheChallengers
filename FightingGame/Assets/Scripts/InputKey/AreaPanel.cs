using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public class AreaPanel : MonoBehaviour
{
    public bool isUpdate;
    bool isReset = false;
    AreaKey[] areaKeys = new AreaKey[(int)ENUM_INPUTKEY_NAME.Max];

    public void Init()
    {
        List<KeySettingData> keySettingDatas = PlayerPrefsManagement.Load_KeySettingData();

        for (int index = 0; index < areaKeys.Length; index++)
        {
            areaKeys[index] = gameObject.transform.Find(Enum.GetName(typeof(ENUM_INPUTKEY_NAME), index)).GetComponent<AreaKey>();

            if (areaKeys[index] == null)
            {
                Debug.LogError($"{Enum.GetName(typeof(ENUM_INPUTKEY_NAME), index)} 를 찾지 못했습니다.");
                return;
            }

            areaKeys[index].Init();

            if (keySettingDatas != null && !isReset)
                Set_AreaKey(areaKeys[index], keySettingDatas[index]);
        }

        if (isReset)
            Set_isReset(!isReset);
    }

    private void Set_AreaKey(AreaKey keyArea, KeySettingData keySettingData)
    {
        keyArea.rectTr.localScale = new Vector3(keySettingData.size, keySettingData.size, 1f);
        keyArea.rectTr.position = new Vector2(keySettingData.rectTrX, keySettingData.rectTrY);
    }

    public AreaKey Get_AreaKey(ENUM_INPUTKEY_NAME keyName)
    {
        return areaKeys[(int)keyName];
    }

    public AreaKey[] Get_AreaKeys()
    {
        return areaKeys;
    }

    public void Set_isReset(bool _isReset) => isReset = _isReset;
}
