using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public class AreaPanel : MonoBehaviour
{
    KeyArea[] keyArea = new KeyArea[(int)ENUM_INPUTKEY_NAME.Max];

    public void Init()
    {
        for (int index = 0; index < keyArea.Length; index++)
        {
            keyArea[index] = gameObject.transform.Find(Enum.GetName(typeof(ENUM_INPUTKEY_NAME), index)).GetComponent<KeyArea>();

            if (keyArea[index] == null)
            {
                Debug.LogError($"{Enum.GetName(typeof(ENUM_INPUTKEY_NAME), index)} 를 찾지 못했습니다.");
                return;
            }
            keyArea[index].Init();
        }
    }

    public KeyArea Get_keyArea(ENUM_INPUTKEY_NAME keyName)
    {
        return keyArea[(int)keyName];
    }
}
