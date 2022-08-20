using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class StatusWindowUI : UIElement
{
    public Slider slider;
    [SerializeField] Image charFrameImage;

    public float maxHP;
    public float curHP;

    public void Set_CharFrameImage(ENUM_CHARACTER_TYPE charType)
    {
        switch(charType)
        {
            case ENUM_CHARACTER_TYPE.Knight:

                break;
            default:
                Debug.Log($"{charType} 를 찾을 수 없음");
                break;
        }
    }

    public void Set_MaxHP(float _maxHP)
    {
        maxHP = _maxHP;
        curHP = _maxHP;
    }

    /// <summary>
    /// false 리턴 시 HP가 전부 닳은 것
    /// </summary>
    public bool Input_Damage(float _damege)
    {
        curHP -= _damege;

        if(curHP > 0)
        {
            return true;
        }

        return false;
    }

    // 체력 서서히 깎이는 코루틴 넣을 예1정
    // protected IEnumerator I
}

