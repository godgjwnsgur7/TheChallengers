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
    RightArrow = 0,
    LeftArrow = 1,
    Jump = 2,
    Attack = 3,
    Skill1 = 4,
    Skill2 = 5,
    Skill3 = 6,
}

public class OperatingPanel : MonoBehaviour
{
    public OperatingKey[] operatingKeys = new OperatingKey[Enum.GetValues(typeof(ENUM_OPERATINGKEY_NAME)).Length];

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        for(int index = 0; index < operatingKeys.Length; index++)
        {
            operatingKeys[index] = gameObject.transform.Find(Enum.GetName(typeof(ENUM_OPERATINGKEY_NAME), index)).GetComponent<OperatingKey>();
            if(operatingKeys[index] == null)
                Debug.Log($"{Enum.GetName(typeof(ENUM_OPERATINGKEY_NAME), index)} 를 찾지 못했습니다.");
        }
    }

}
