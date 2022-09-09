using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

[System.Serializable]
public class StatusData
{
    public float currHP;
    public float maxHP;

    public StatusData() { }

    public StatusData(float _maxHP)
    {
        this.currHP = _maxHP;
        this.maxHP = _maxHP;
    }

 }

public class StatusWindowUI : UIElement
{
    public Slider hpBarSlider;
    [SerializeField] Image charFrameImage;

    // 다시 꺼줘야함 처리안함 (임시)
    public static System.Func<float, bool> OnChangeHP = null;

    private float currHP
    {
        get
        {
            return NetworkDataHandler.Instance.CurrHP;
        }
        set
        {
            NetworkDataHandler.Instance.CurrHP = value;
        }
    }

    private float maxHP;

    Coroutine hpBarCoroutine;

    public void Set_StatusWindowUI(ENUM_CHARACTER_TYPE _charType, StatusData statusData)
    {
        Set_CharFrameImage(_charType);
        Set_MaxHP(statusData);

        OnChangeHP = Input_Damage;
    }

    public void Set_CharFrameImage(ENUM_CHARACTER_TYPE _charType)
    {
        switch(_charType)
        {
            case ENUM_CHARACTER_TYPE.Knight:
                // 이미지 아직 없음
                break;
            default:
                Debug.Log($"{_charType} 를 찾을 수 없음");
                break;
        }
    }

    public void Set_MaxHP(StatusData data)
    {
        NetworkDataHandler.Instance.CurrHP = data.currHP;
        maxHP = data.maxHP;
    }

    /// <summary>
    /// false 리턴 시 HP가 전부 닳은 것
    /// </summary>

    [BroadcastMethod]
    public bool Input_Damage(float _damege)
    {
        currHP -= _damege;

        if(hpBarCoroutine != null)
            StopCoroutine(hpBarCoroutine);

        if(currHP > 0)
        {
            hpBarCoroutine = StartCoroutine(IFadeHpBar(currHP / maxHP));
            return true;
        }

        hpBarCoroutine = StartCoroutine(IFadeHpBar(0f));
        return false;
    }

    protected IEnumerator IFadeHpBar(float _goalHPValue)
    {
        float _curHPValue = currHP / maxHP;
            
        while(_goalHPValue < hpBarSlider.value)
        {
            hpBarSlider.value -= 0.01f;
            yield return null;
        }

        hpBarSlider.value = _goalHPValue;
        hpBarCoroutine = null;
    }

    
}

