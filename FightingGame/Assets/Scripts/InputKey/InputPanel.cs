using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 해당 스크립트를 가진 오브젝트가 자식으로 가진 조작키의 오브젝트 이름과 같아야 함
/// </summary>
[Serializable]
public enum ENUM_OPERATINGKEY_NAME
{
    LeftArrow = 0,
    RightArrow = 1,
    Attack = 2,
    Skill1 = 3,
    Skill2 = 4,
    Skill3 = 5,
    Jump = 6,
}

public class InputPanel : MonoBehaviour
{
    InputKey[] operatingKeys = new InputKey[Enum.GetValues(typeof(ENUM_OPERATINGKEY_NAME)).Length];

    public void Init(Action<InputKey> OnPointDownCallBack, Action<InputKey> OnPointUpCallBack)
    {
        for(int index = 0; index < operatingKeys.Length; index++)
        {
            operatingKeys[index] = gameObject.transform.Find(Enum.GetName(typeof(ENUM_OPERATINGKEY_NAME), index)).GetComponent<InputKey>();
            
            if(operatingKeys[index] == null)
            {
                Debug.LogError($"{Enum.GetName(typeof(ENUM_OPERATINGKEY_NAME), index)} 를 찾지 못했습니다.");
                return;
            }
            
            operatingKeys[index].Init(OnPointDownCallBack, OnPointUpCallBack);
        }
    }
    
    public InputKey Get_InputKey(ENUM_OPERATINGKEY_NAME keyName)
    {
       return operatingKeys[(int)keyName];
    }

    // 추가로 필요한 기능이 있는 것 같다면, 요청하면 됨! 모르는건 물어보3!
}
