using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public class Z_AreaPanel : MonoBehaviour
{
    public bool isUpdate;
    Z_AreaKey[] Z_AreaKeys = new Z_AreaKey[(int)ENUM_INPUTKEY_NAME.Max];

    public void Init(InputKey[] inputKeys)
    {
        for (int index = 0; index < Z_AreaKeys.Length; index++)
        {
            Z_AreaKeys[index] = gameObject.transform.Find(Enum.GetName(typeof(ENUM_INPUTKEY_NAME), index)).GetComponent<Z_AreaKey>();
            Z_AreaKeys[index].Init();
            
            Set_Z_AreaKey(Z_AreaKeys[index], inputKeys[index]);
        }
    }

    private void Set_Z_AreaKey(Z_AreaKey keyArea, InputKey inputkey)
    {
        keyArea.rectTr.localScale = inputkey.GetComponent<RectTransform>().localScale;
        keyArea.rectTr.position = inputkey.GetComponent<RectTransform>().transform.position;
    }

    public Z_AreaKey Get_Z_AreaKey(ENUM_INPUTKEY_NAME keyName)
    {
        return Z_AreaKeys[(int)keyName];
    }

    public Z_AreaKey[] Get_Z_AreaKeys()
    {
        return Z_AreaKeys;
    }
}
