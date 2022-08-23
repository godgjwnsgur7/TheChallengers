using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class StatusWindowUI : UIElement
{
    public Slider hpBarSlider;
    [SerializeField] Image charFrameImage;

    public float maxHP;
    public float curHP;

    Coroutine hpBarCoroutine;

    public void Set_CharFrameImage(ENUM_CHARACTER_TYPE charType)
    {
        switch(charType)
        {
            case ENUM_CHARACTER_TYPE.Knight:
                Debug.Log("이미지 아직 없음 ㅋㅋ");
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
            hpBarSlider.value = curHP / maxHP; 
            return true;
        }

        hpBarSlider.value = 0f;
        return false;
    }

    protected IEnumerator IFadeHpBar()
    {

        yield return null;
    }
}

