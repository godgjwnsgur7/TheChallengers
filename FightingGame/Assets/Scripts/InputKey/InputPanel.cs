using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 해당 스크립트를 가진 오브젝트가 자식으로 가진 조작키의 오브젝트 이름과 같아야 함
/// </summary>
[Serializable]
public enum ENUM_INPUTKEY_NAME
{
    LeftArrow = 0, // 현재 Left는 이미지 회전을 시켜서 인식이 안됨. 이미지 다시 받을것임
    RightArrow = 1,
    Attack = 2,
    Skill1 = 3,
    Skill2 = 4,
    Skill3 = 5,
    Jump = 6,
}

public class InputPanel : MonoBehaviour
{
    InputKey[] inputKeys = new InputKey[Enum.GetValues(typeof(ENUM_INPUTKEY_NAME)).Length];

    public void Init(Action<InputKey> OnPointDownCallBack, Action<InputKey> OnPointUpCallBack)
    {
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

    // 추가로 필요한 기능이 있는 것 같다면, 요청하면 됨! 모르는건 물어보3!
}
