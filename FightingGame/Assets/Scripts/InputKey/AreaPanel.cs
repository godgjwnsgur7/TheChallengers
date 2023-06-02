using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public class AreaPanel : MonoBehaviour
{
    public bool isUpdate;
    AreaKey[] areaKeys = new AreaKey[(int)ENUM_INPUTKEY_NAME.Max];

    public void Init(InputKey[] inputKeys)
    {
        for (int index = 0; index < areaKeys.Length; index++)
        {
            areaKeys[index] = gameObject.transform.Find(Enum.GetName(typeof(ENUM_INPUTKEY_NAME), index)).GetComponent<AreaKey>();
            areaKeys[index].Init();
            
            Set_AreaKey(areaKeys[index], inputKeys[index]);
        }
    }

    private void Set_AreaKey(AreaKey keyArea, InputKey inputkey)
    {
        keyArea.rectTr.localScale = inputkey.GetComponent<RectTransform>().localScale;
        keyArea.rectTr.position = inputkey.GetComponent<RectTransform>().transform.position;
    }

    public AreaKey Get_AreaKey(ENUM_INPUTKEY_NAME keyName)
    {
        return areaKeys[(int)keyName];
    }

    public AreaKey[] Get_AreaKeys()
    {
        return areaKeys;
    }
}
